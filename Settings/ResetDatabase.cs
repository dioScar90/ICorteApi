using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Settings;

public static class ResetDatabase
{
    public static void ResetMyDatabase(this WebApplication app)
    {
        return;

        /* NÃO USAR, NÃO ESTÁ DANDO CERTO, APAGUE AS TABELAS NA UNHA OU CORRIJA ANTES DE USAR */

        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Remove apenas as tabelas
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        db.Database.Migrate(); // Reaplica a migration atual
    }
}
