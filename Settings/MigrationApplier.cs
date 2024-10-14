using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Settings;

public static class MigrationApplier
{
    public static async Task ApplyMigration(IServiceProvider serviceProvider)
    {
        var db = serviceProvider.GetRequiredService<AppDbContext>();
        await db.Database.MigrateAsync();
    }
}
