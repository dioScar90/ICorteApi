using System.Globalization;
using Microsoft.AspNetCore.Localization;

namespace ICorteApi.Settings;

public static class CultureConfiguration
{
    public static void DefineCultureLocalization(this IApplicationBuilder app, string? name = "pt-BR")
    {
        var defaultCulture = new CultureInfo(name!); // "pt-BR"
        var localizationOptions = new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture(defaultCulture),
            SupportedCultures = [defaultCulture],
            SupportedUICultures = [defaultCulture],
        };

        app.UseRequestLocalization(localizationOptions);
    }
}
