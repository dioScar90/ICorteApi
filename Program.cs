using ICorteApi.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("AppDb"));

var pgHost = Environment.GetEnvironmentVariable("PGHOST");
var pgPort = Environment.GetEnvironmentVariable("PGPORT");
var pgDb = Environment.GetEnvironmentVariable("PGDATABASE");
var pgUser = Environment.GetEnvironmentVariable("PGUSER");
var pgPass = Environment.GetEnvironmentVariable("PGPASSWORD");

Console.WriteLine("pgHost => " + pgHost);
Console.WriteLine("pgPort => " + pgPort);
Console.WriteLine("pgDb => " + pgDb);
Console.WriteLine("pgUser => " + pgUser);
Console.WriteLine("pgPass => " + pgPass);

var connectionString = $"Host={pgHost};Port={pgPort};Database={pgDb};Username={pgUser};Password={pgPass};";
Console.WriteLine("connectionString => " + connectionString);

// var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
ArgumentNullException.ThrowIfNullOrEmpty(pgHost, nameof(pgHost));
ArgumentNullException.ThrowIfNullOrEmpty(pgPort, nameof(pgPort));
ArgumentNullException.ThrowIfNullOrEmpty(pgDb, nameof(pgDb));
ArgumentNullException.ThrowIfNullOrEmpty(pgUser, nameof(pgUser));
ArgumentNullException.ThrowIfNullOrEmpty(pgPass, nameof(pgPass));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql( // PostgreSQL
    // options.UseSqlServer( // SLQ Server
    // options.UseSqlite( // SQLite
        builder.Configuration.GetConnectionString(connectionString),
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

app.DefineCultureLocalization();

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    /* DESCOMENTE ISSO APENAS EM CASO DE NECESSIDADE E DEPOIS COMENTE NOVAMENTE PELO AMOR DE DEUS */
    // await DataSeeder.ClearAllRowsBeforeSeedAsync(serviceProvider);
    /* DESCOMENTE ISSO APENAS EM CASO DE NECESSIDADE E DEPOIS COMENTE NOVAMENTE PELO AMOR DE DEUS */

    await MigrationApplier.ApplyMigration(serviceProvider);

    await RoleSeeder.SeedRoles(serviceProvider);
    await DataSeeder.SeedData(serviceProvider);
}

app.UseRouting();

// This call must be between `UseRouting` and `UseEndpoints`.
// app.UseAntiforgery();

app.UseHttpsRedirection();

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
