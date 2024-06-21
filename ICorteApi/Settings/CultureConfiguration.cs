using System.Globalization;
using Microsoft.AspNetCore.Localization;

namespace ICorteApi.Settings;

public static class CultureConfiguration
{
    public static void DefineCultureLocalization(this IApplicationBuilder app, string name)
    {
        var defaultCulture = new CultureInfo(name); // "pt-BR"
        var localizationOptions = new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture(defaultCulture),
            SupportedCultures = [defaultCulture], // new List<CultureInfo> { defaultCulture },
            SupportedUICultures = [defaultCulture], // new List<CultureInfo> { defaultCulture }
        };

        app.UseRequestLocalization(localizationOptions);
    }
}
