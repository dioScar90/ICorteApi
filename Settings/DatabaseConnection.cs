using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Settings;

public static class DatabaseConnection
{
    public static void StartDatabaseWithAppropriateConnectionStrings(this WebApplicationBuilder builder)
    {
        if (builder.Environment.IsDevelopment())
        {
            Console.WriteLine("IsDevelopment");
            builder.StartDesktopConnection();
        }
        else
        {
            Console.WriteLine("!IsDevelopment");
            builder.StartRailwayConnection();
        }

        builder.Services.AddHttpContextAccessor();
    }
    
    private static void StartDesktopConnection(this WebApplicationBuilder builder)
    {
        switch (builder.Configuration.GetConnectionString("databaseToConnect")) {
            case "SQL_SERVER":
                builder.StartWithSqlServer();
                break;
            case "POSTGRES":
                builder.StartWithPostgreSql();
                break;
            default:
                builder.StartWithSqlite();
                break;
        }
    }

    private static void StartRailwayConnection(this WebApplicationBuilder builder)
    {
        var apiHttpPort = Environment.GetEnvironmentVariable("API_HTTP_PORT");
        ArgumentNullException.ThrowIfNullOrEmpty(apiHttpPort, nameof(apiHttpPort));
        
        // Em ambientes de produção, como o Railway, geralmente o proxy (ou plataforma)
        // cuida do SSL/HTTPS externamente, e sua aplicação deve escutar apenas na porta 
        // TTP (sem configurar o HTTPS manualmente). GPT
        // Porém mantenha `app.UseHttpsRedirection()` ativo. GPT
        builder.WebHost.ConfigureKestrel(options => options.ListenAnyIP(int.Parse(apiHttpPort)));
        
        var pgHost = Environment.GetEnvironmentVariable("PG_HOST");
        var pgPort = Environment.GetEnvironmentVariable("PG_PORT");
        var pgDb = Environment.GetEnvironmentVariable("PG_DATABASE");
        var pgUser = Environment.GetEnvironmentVariable("PG_USER");
        var pgPass = Environment.GetEnvironmentVariable("PG_PASSWORD");

        ArgumentNullException.ThrowIfNullOrEmpty(pgHost, nameof(pgHost));
        ArgumentNullException.ThrowIfNullOrEmpty(pgPort, nameof(pgPort));
        ArgumentNullException.ThrowIfNullOrEmpty(pgDb, nameof(pgDb));
        ArgumentNullException.ThrowIfNullOrEmpty(pgUser, nameof(pgUser));
        ArgumentNullException.ThrowIfNullOrEmpty(pgPass, nameof(pgPass));
        
        string connectionString = $"Host={pgHost};Port={pgPort};Database={pgDb};Username={pgUser};Password={pgPass};";
        builder.StartWithPostgreSql(connectionString);
    }

    private static void StartWithSqlite(this WebApplicationBuilder builder, string? connectionString = null)
    {
        Console.WriteLine("StartWithSqlite");
        connectionString ??= "Data Source=sqlite.db";
        
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(
                connectionString,
                assembly => assembly.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName))
        );
    }

    private static void StartWithSqlServer(this WebApplicationBuilder builder, string? connectionString = null)
    {
        Console.WriteLine("StartWithSqlServer");
        connectionString ??= builder.Configuration.GetConnectionString("developmentConnection");

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                connectionString,
                assembly => assembly.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName))
        );
    }

    private static void StartWithPostgreSql(this WebApplicationBuilder builder, string? connectionString = null)
    {
        Console.WriteLine("StartWithPostgreSql");
        connectionString ??= builder.Configuration.GetConnectionString("developmentConnection");

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(
                connectionString,
                assembly => assembly.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName))
        );
    }
}
