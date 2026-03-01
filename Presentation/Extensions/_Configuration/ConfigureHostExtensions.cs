using Serilog;

namespace ICorteApi.Presentation.Extensions;

public static class ConfigureHostExtensions
{
    extension(ConfigureHostBuilder host)
    {
        public ConfigureHostBuilder AddSerilog()
        {
            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console() // log in console (stdout)
                .WriteTo.File("logs/log-.txt",
                            rollingInterval: RollingInterval.Day,   // 1 file per day
                            retainedFileCountLimit: 7,              // hold only 7 days
                            shared: true)                           // allows multiple processes to save
                .Enrich.FromLogContext()                            // add context (ex: RequestId)
                .CreateLogger();

            // Replace defult logger for Serilog
            host.UseSerilog();

            return host;
        }
    }
}
