using ICorteApi.Settings;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.StartDatabaseWithAppropriateConnectionStrings();

// Most important applications services
// This order was suggested by Chat GPT
builder.Services
    .AddIdentityConfigurations()
    .AddRepositories()
    .AddServices()
    .AddErrors()
    .AddValidators()
    .AddCorsConfiguration()
    .AddAuthorizationRules()
    .AddCookieConfiguration(builder.Environment.IsDevelopment())
    .AddExceptionHandlers()
;

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BarberShop API", Version = "v1" });
    c.ResolveConflictingActions(x => x.First());
});

var app = builder.Build();

app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI();

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    /* DESCOMENTE ISSO APENAS EM CASO DE NECESSIDADE E DEPOIS COMENTE NOVAMENTE PELO AMOR DE DEUS */
    // await DataSeeder.ClearAllRowsBeforeSeedAsync(serviceProvider);
    /* DESCOMENTE ISSO APENAS EM CASO DE NECESSIDADE E DEPOIS COMENTE NOVAMENTE PELO AMOR DE DEUS */

    await MigrationApplier.ApplyMigration(serviceProvider);

    await RoleSeeder.SeedRoles(serviceProvider);
    await DataSeeder.SeedData(serviceProvider);
    // await DataSeeder.UpdateImageUrlsForFirstTime(serviceProvider);
}

app.UseRouting();

if (!app.Environment.IsDevelopment())
{
    Console.WriteLine("\n\n\nPRODUCTIOOOOOOOOOOOOON!\n\n\n");
    app.UseHttpsRedirection();
}

app.UseCors("AllowSpecificOrigin");
// app.UseCors("AllowAll");

// After .NET 8 it isn't necessary to use `AddAuthentication` or `UseAuthentication`
// when `AddAuthorization` or `UseAuthorization` is also present.
app.UseAuthorization();

// Configuring all application endpoints.
app.ConfigureMyEndpoints();

app.UseExceptionHandler("/error");

// Regenera o token de sessão na inicialização
SessionTokenManager.RegenerateToken();

app.Run();
