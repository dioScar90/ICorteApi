# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project overview

**ICorteApi** is a barber shop appointment scheduling backend built with ASP.NET Core Minimal APIs on .NET 10. It serves a React SPA frontend (`icorte-app`). The project is deployed on Railway (PostgreSQL) and uses SQLite locally by default.

## Commands

```bash
# Run the app (defaults to SQLite in development)
dotnet run

# Build
dotnet build

# EF Core migrations
dotnet ef migrations add <NomeDaMigracao>
dotnet ef database update

# Switch database locally — set `databaseToConnect` in appsettings.json:
# "sqlite" (default) | "SQL_SERVER" | "POSTGRES"
```

There are no test projects.

## Architecture

Single-project solution (`ICorteApi.csproj`) organized into four logical layers by folder:

```
Domain/          — Entities, errors, interfaces, utils (no framework dependencies)
Application/     — Services, DTOs, validators (depends on Domain + EF Core)
Infraestructure/ — AppDbContext, EF Fluent API maps (note: typo is intentional in the codebase)
Presentation/    — Minimal API endpoints, exception types, DI extension methods
Settings/        — Startup helpers (DB connection, seeding, migrations, CORS, session)
```

**GlobalUsings.cs** wires the most-used namespaces project-wide so explicit `using` statements are rarely needed.

## Key patterns

### Minimal APIs (no controllers)
Every route group lives in `Presentation/Endpoints/<Entity>Endpoint.cs`. Each file exports a static class with a `Map<Entity>Endpoint(this IEndpointRouteBuilder)` extension method. All endpoints are registered in `Settings/ConfigureEndpoints.cs`.

Endpoint handlers receive services and error classes via parameter-based DI (not constructor injection). Return types use `TypedResults.*` and `Results<T1, T2, ...>` for compile-time checked HTTP responses.

### Error handling
`Domain/Errors/<Entity>Errors.cs` classes (injected as Scoped) wrap `TypedResults.BadRequest<Error>` and `TypedResults.NotFound<Error>` with Portuguese-language messages. Error messages use grammatical gender (masculine/feminine) derived from entity type name via `BaseErrors<TEntity>`. Throw `Presentation/Exceptions/<Type>Exception.cs` for cross-cutting concerns — `GlobalExceptionHandler` maps them to ProblemDetails responses.

### Soft delete
All entities inherit `BaseEntity<T>` (or `BaseUserEntity` for `User`). Delete operations set `IsDeleted = true` — never physical deletes. `AppDbContext.HandleSoftDelete()` intercepts `EntityState.Deleted` entries and redirects them to `Modified`. EF query filters on `BaseMap<T>` automatically exclude soft-deleted rows. Use `.IgnoreQueryFilters()` for admin operations. Cascade rules are enforced in `AppDbContext`: deleting a User soft-deletes its BarberShop; deleting a BarberShop cascades to Address, Services, and Schedules.

### Services
`BaseService<TEntity>` provides `GetAllAsync` with pagination via `PaginationProperties<TEntity, TDtoResponse>`. Concrete services inherit from it and receive `AppDbContext` via primary constructor. All services are registered as **Scoped**.

### Authorization
Five policies are derived from the `PolicyUserRole` enum in `Domain/Entities/UserRoles.cs`:

| Policy | Allowed roles |
|---|---|
| `FreeIfAuthenticated` | All authenticated (default policy) |
| `ClientOrHigh` | Client, BarberShop, Admin |
| `ClientOnly` | Client, Admin |
| `BarberShopOrHigh` | BarberShop, Admin |
| `AdminOnly` | Admin |

Use `.RequireAuthorization("PolicyName")` on endpoint groups/routes.

### Authentication
Cookie-based via `SignInManager<User>`. Session tokens are rotated on each app startup (`SessionTokenManager`), which invalidates all existing sessions — useful in production to force re-login after deploys. Cookie config: `HttpOnly`, `SlidingExpiration` (60 min), `SameSite=Lax` in dev / `SameSite=None; Secure` in prod.

### Database / EF Core
`Infraestructure/Maps/` contains one `IEntityTypeConfiguration<T>` per entity. `BaseMap<TEntity>` applies snake_case column naming, decimal precision (9,4), enum-to-string conversion, and the global soft-delete query filter. `AppDbContext` extends `IdentityDbContext<User, ApplicationRole, int>`.

In development, the DB is selected via `appsettings.json`:
```json
"ConnectionStrings": {
  "databaseToConnect": "sqlite"
}
```

In production (Railway), the app reads `PG_HOST`, `PG_PORT`, `PG_DATABASE`, `PG_USER`, `PG_PASSWORD`, and `API_HTTP_PORT` from environment variables.

## Startup sequence

`Program.cs` → DB connection → Serilog → Identity → Services/Errors/Validators → CORS → Authorization → Cookie → ExceptionHandlers → Swagger (dev only) → EF migrations → Role seeding → Data seeding → Endpoints → SessionToken rotation.

> **WARNING:** `DataSeeder.ClearAllRowsBeforeSeedAsync()` is commented out in `Program.cs` — uncomment only intentionally and recomment immediately after. It drops all data.

## DI registration

Extension methods use the C# 13 `extension(IServiceCollection services)` block syntax (new in .NET 10) in `Presentation/Extensions/_Configuration/ServiceCollectionExtensions.cs`. When adding a new service or error class, register it in `AddServices()` / `AddErrors()` in that file.

## Language

All user-facing messages, error strings, and most code comments are in **Brazilian Portuguese**.
