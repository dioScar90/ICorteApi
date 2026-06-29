# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

ICorteApi is the backend for a barbershop scheduling web app (TCC/Final Paper). It is a single-project ASP.NET Core 10 application using Minimal APIs. The frontend is a separate React repo at https://github.com/dioScar90/icorte-app.

---

## Commands

```bash
# Run the application (defaults to SQLite in development)
dotnet run

# Build
dotnet build

# Add a new EF Core migration
dotnet ef migrations add <MigrationName>

# Apply pending migrations manually (also runs automatically on startup)
dotnet ef database update

# Install/update EF CLI tool (global)
dotnet tool install --global dotnet-ef
# or
dotnet tool update --global dotnet-ef
```

To switch the local database, set `ConnectionStrings:databaseToConnect` in `appsettings.json`:
- Omit the key (or set to any other value) → **SQLite** (`sqlite.db`)
- `"SQL_SERVER"` → SQL Server (reads `developmentConnection` string)
- `"POSTGRES"` → PostgreSQL (reads `developmentConnection` string)

---

## Architecture

Single-project monolith with logical folder-based layer separation:

| Folder | Responsibility |
|---|---|
| `Domain/` | Entities, error classes, utils — no external dependencies |
| `Application/` | Services (business logic), DTOs, custom validation attributes |
| `Infraestructure/` | `AppDbContext`, EF Core entity type configuration maps |
| `Presentation/` | Minimal API endpoints, exception types, DI extension methods |
| `Settings/` | Startup wiring: DB connection, endpoint registration, seeding, migrations |

Global usings are declared in `GlobalUsings.cs` for the five most common namespaces.

---

## Key Patterns

### Minimal API Endpoints

Each entity has a static class in `Presentation/Endpoints/` following this exact structure:

```csharp
public static class AppointmentEndpoint
{
    public static IEndpointRouteBuilder MapAppointmentEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("appointment").WithTags("Appointment");
        group.MapPost("", CreateAppointmentAsync)
            .WithSummary("Create Appointment")
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));
        // ...
        return app;
    }

    // Inner record for structured logging (factory pattern)
    internal record LoggerActions { ... }

    // Handler methods are public static async Task<Results<...>> (union return types)
    public static async Task<Results<Created<AppointmentDtoResponse>, BadRequest<Error>>> CreateAppointmentAsync(
        AppointmentDtoRequest dto,          // body — bound automatically
        AppointmentService service,          // injected from DI
        AppointmentErrors errors,            // injected from DI
        ILoggerFactory loggerFactory,
        CancellationToken cancellationToken = default)
    { ... }
}
```

- Always use `TypedResults.*` (not `Results.*`) for responses.
- Return union types: `Results<Ok<T>, NotFound<Error>, BadRequest<Error>, ...>`.
- Services and error classes arrive as parameters (Minimal API parameter DI).
- Register every new endpoint group in `Settings/ConfigureEndpoints.cs`.
- Add `cancellationToken.ThrowIfCancellationRequested()` at the top of each handler.

### Authorization

Five policies defined in the `PolicyUserRole` enum (`Domain/Entities/UserRoles.cs`):

| Policy | Allowed roles |
|---|---|
| `FreeIfAuthenticated` | Guest, Client, BarberShop, Admin |
| `ClientOrHigh` | Client, BarberShop, Admin |
| `ClientOnly` | Client, Admin |
| `BarberShopOrHigh` | BarberShop, Admin |
| `AdminOnly` | Admin |

Usage: `.RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh))`

Auth is **cookie-based** (not JWT, despite the package being present). The session token is regenerated on every app restart, which invalidates all existing cookies intentionally.

### Services

Concrete services are `sealed` classes extending `BaseService<TEntity>`:

```csharp
public sealed class AppointmentService(
    AppDbContext context,
    ServiceService serviceService,
    UserService userService,
    ILogger<AppointmentService> logger)
    : BaseService<Appointment>(context)
{ ... }
```

- `BaseService<T>` exposes `dbSet` and `SaveChangesAsync()` / `BeginTransactionAsync()`.
- `GetAllAsync<TDtoResponse>(PaginationProperties<T, TDtoResponse>)` handles pagination, filtering, ordering and projection generically.
- Return `null` on failure; the endpoint checks for null and calls the relevant error method.
- All services are registered as **Scoped** in `ServiceCollectionExtensions`.

### DTOs

DTOs are immutable `record` types:

```csharp
public record AppointmentDtoResponse(...) : IDtoResponse<Appointment>;
public record AppointmentDtoRequest(...) : IDtoRequest<Appointment>;
```

Validation uses **DataAnnotations** on request DTO constructor parameters. Custom attributes live in `Application/Validators/` (e.g., `[Password]`, `[Email]`, `[GreaterThanOrEqualToday]`). FluentValidation was planned but not implemented — keep using DataAnnotations.

### Error Classes

Each entity has a `sealed` error class extending `BaseErrors<TEntity>` in `Domain/Errors/`:

```csharp
public sealed class AppointmentErrors : BaseErrors<Appointment>
{
    public UnprocessableEntity<Error> EmptyServices() =>
        Error.UnprocessableEntity("Selecione pelo menos um serviço");
}
```

- `BaseErrors<T>` provides pre-built: `NotFound()`, `Create()`, `Update()`, `Delete()`, `BadRequest()`.
- Error messages are in **Portuguese**.
- Return values are typed `TypedResults` wrappers directly usable as handler return values.
- Register every new error class as **Scoped** in `ServiceCollectionExtensions`.

### EF Core Maps

Maps extend `BaseMap<TEntity>` (`Infraestructure/Maps/Base/BaseMap.cs`) which automatically:
- Converts table names to `snake_case`
- Converts all primitive column names to `snake_case`
- Sets `decimal` precision to `(9, 4)`
- Stores enums as `string` (except `DayOfWeek`, which stays as int)
- Applies `HasQueryFilter(x => !x.IsDeleted)` to any entity implementing `IBaseEntity<T>`

Override `Configure()` in subclass maps only for relationships, indexes, or special constraints.

### Soft Delete

**Never hard-delete entities that implement `IBaseEntity`.** Calling `dbSet.Remove(entity)` on them is intercepted by `AppDbContext.HandleSoftDelete()`, which converts `EntityState.Deleted` → `EntityState.Modified` and calls `entity.DeleteEntity()`.

Cascading soft-delete rules (defined in `AppDbContext`):
- `User` deleted → soft-deletes their `BarberShop`
- `BarberShop` deleted → soft-deletes `Address`; hard-deletes `Services`, `SpecialSchedules`, `RecurringSchedules`
- `Service` deleted → removes from related `Appointments` (many-to-many join rows)
- `Appointment` deleted → removes all its service join rows

For join tables (composite key entities without `IBaseEntity`), hard delete is fine.

### Logging

Each endpoint class contains an `internal record LoggerActions` with a factory:

```csharp
internal static LoggerActions FactoryCreate(ILoggerFactory loggerFactory) => new()
{
    Entity = nameof(Appointment),
    Logger = loggerFactory.CreateLogger(nameof(AppointmentEndpoint))
};
```

Use structured logging placeholders: `{Entity}`, `{Id}`, `{@Dto}`.

### C# 14 Extension Member Syntax

`ServiceCollectionExtensions.cs` and `ConfigureHostExtensions.cs` use the **C# 14 preview** `extension()` block syntax:

```csharp
public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddServices() { ... }
    }
}
```

This is not a bug — the project targets .NET 10 with C# 14 preview features. Do not refactor these into traditional extension methods.

---

## Adding a New Entity (Full Vertical Slice)

1. **Domain entity** — `Domain/Entities/MyEntity.cs`, extend `BaseEntity<MyEntity>` (or `BaseUserEntity` if it's user-shaped)
2. **DbSet** — add `DbSet<MyEntity> MyEntities` to `AppDbContext`
3. **EF map** — `Infraestructure/Maps/MyEntityMap.cs`, extend `BaseMap<MyEntity>`
4. **DTOs** — `Application/Dtos/MyEntityDto.cs`, `record MyEntityDtoResponse : IDtoResponse<MyEntity>` and `record MyEntityDtoRequest : IDtoRequest<MyEntity>`
5. **Service** — `Application/Services/MyEntityService.cs`, `sealed class` extending `BaseService<MyEntity>`; register as Scoped in `ServiceCollectionExtensions.AddServices()`
6. **Error class** — `Domain/Errors/MyEntityErrors.cs`, `sealed class` extending `BaseErrors<MyEntity>`; register as Scoped in `ServiceCollectionExtensions.AddErrors()`
7. **Endpoint** — `Presentation/Endpoints/MyEntityEndpoint.cs`, static class with `MapMyEntityEndpoint()` method
8. **Register endpoint** — add `.MapMyEntityEndpoint()` chain in `Settings/ConfigureEndpoints.cs`
9. **Migration** — `dotnet ef migrations add AddMyEntity`

---

## Database / Deployment Notes

- **Development default**: SQLite (`sqlite.db` at project root).
- **Production**: Railway platform with PostgreSQL. Required env vars: `PG_HOST`, `PG_PORT`, `PG_DATABASE`, `PG_USER`, `PG_PASSWORD`, `API_HTTP_PORT`.
- Migrations and seeding run automatically on startup via `MigrationApplier`, `RoleSeeder`, and `DataSeeder`.
- **WARNING**: The line `await DataSeeder.ClearAllRowsBeforeSeedAsync(serviceProvider)` in `Program.cs` is commented out deliberately. Never uncomment it in production — it wipes all data.
