using ICorteApi.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ICorteApi.Infraestructure.Context;
using ICorteApi.Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Configuração do banco de dados
// builder.Services.AddDbContext<AppDbContext>(options =>
//     // Alternativas de banco de dados descomentadas conforme necessidade
//     options
//         //.UseInMemoryDatabase("AppDb")
//         // .UseSqlite(
//         //     builder.Configuration.GetConnectionString("SqliteConnection"),
//         //     assembly => assembly.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName))
//         .UseSqlServer(
//             builder.Configuration.GetConnectionString("SqlServerConnection"),
//             assembly => assembly.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName))
// );

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("SqlServerConnection"),
        sqlServerOptionsAction: sqlOptions =>
        {
            sqlOptions.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5, // Número máximo de tentativas
                maxRetryDelay: TimeSpan.FromSeconds(10), // Tempo máximo entre as tentativas
                errorNumbersToAdd: null); // Tipos de erro adicionais para retentativa (pode ser null)
        }));

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
    // .AddAntiCsrfConfiguration()
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
    
    app.Environment.WebRootPath = app.Configuration.GetValue<string>("ImagesPath")!;
}

app.DefineCultureLocalization("pt-BR");

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider;

    await RoleSeeder.SeedRoles(roleManager);
    await DataSeeder.SeedData(roleManager);
}

app.UseRouting();

// This call must be between `UseRouting` and `UseEndpoints`.
// app.UseAntiforgery();

// After .NET 8 it isn't necessary to use `AddAuthentication` or `UseAuthentication`
// when `AddAuthorization` or `UseAuthorization` is also present.
app.UseAuthorization();

// Configuring all application endpoints.
app.ConfigureMyEndpoints();

app.UseExceptionHandler("/error");

app.UseStaticFiles();

// Regenera o token de sessão na inicialização
SessionTokenManager.RegenerateToken();

app.Run();
