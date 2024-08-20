using ICorteApi.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ICorteApi.Infraestructure.Context;
using ICorteApi.Presentation.Extensions;
using ICorteApi.Presentation.Exceptions;

var builder = WebApplication.CreateBuilder(args);

// Configuração do banco de dados
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("SqliteConnection"),
        assembly => assembly.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
    )
// Alternativas de banco de dados descomentadas conforme necessidade
//.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlConnection"), assembly => assembly.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName))
//.UseInMemoryDatabase("AppDb")
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

app.Run();
