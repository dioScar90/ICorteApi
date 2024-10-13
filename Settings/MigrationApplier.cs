using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Settings;

public static class MigrationApplier
{
    public static async Task ApplyMigration(IServiceProvider serviceProvider)
    {
        var db = serviceProvider.GetRequiredService<AppDbContext>();

        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("db");
        Console.WriteLine(db);
        Console.WriteLine(db.Database);
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
        
        await db.Database.MigrateAsync();
    }
}
