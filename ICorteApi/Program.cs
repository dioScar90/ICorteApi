using ICorteApi.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ICorteApi.Infraestructure.Context;
using ICorteApi.Presentation.Extensions;
using ICorteApi.Presentation.Exceptions;

var builder = WebApplication.CreateBuilder(args);

// Configuração do banco de dados
builder.Services.AddDbContext<AppDbContext>(options =>
    // Alternativas de banco de dados descomentadas conforme necessidade
    options
        //.UseInMemoryDatabase("AppDb")
        // .UseSqlite(
        //     builder.Configuration.GetConnectionString("SqliteConnection"),
        //     assembly => assembly.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName))
        // .UseNpgsql(
        //     builder.Configuration.GetConnectionString("PostgreSqlConnection"),
        //     assembly => assembly.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName))
        .UseSqlServer(
            builder.Configuration.GetConnectionString("SqlServerConnection"),
            assembly => assembly.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName))
);

builder.Services.AddHttpContextAccessor();

// Most important applications services
// This order was suggested by Chat GPT
builder.Services
    .AddIdentityConfigurations()
    .AddRepositories()
    .AddServices()
    .AddErrors()
    .AddValidators()
    .AddAuthorizationRules()
    .AddCookieConfiguration()
    .AddAntiCsrfConfiguration()
    .AddExceptionHandlers()
;

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BarberShop API", Version = "v1" });
    c.ResolveConflictingActions(x => x.First());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.DefineCultureLocalization("pt-BR");

using (var scope = app.Services.CreateScope())
{
    // var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
    var roleManager = scope.ServiceProvider;
    await RoleSeeder.SeedRoles(roleManager);
}

app.UseRouting();

// After .NET 8 it isn't necessary to use `AddAuthentication` or `UseAuthentication`
// when `AddAuthorization` or `UseAuthorization` is also present.
app.UseAuthorization();

// Configuring all application endpoints.
app.ConfigureMyEndpoints();

app.UseExceptionHandler("/error");

// Regenera o token de sessão na inicialização
SessionTokenManager.RegenerateToken();

// using (var scope = app.Services.CreateScope())
// {
//     var services = scope.ServiceProvider;
//     try
//     {
//         var context = services.GetRequiredService<AppDbContext>();
//         DbInitializer.Initialize(context);
//     }
//     catch (Exception ex)
//     {
//         var logger = services.GetRequiredService<ILogger<Program>>();
//         logger.LogError(ex, "An error occurred while seeding the database.");
//     }
// }

app.Run();
