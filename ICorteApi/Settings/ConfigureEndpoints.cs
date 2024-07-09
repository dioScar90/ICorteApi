using ICorteApi.Presentation.Endpoints;
using ICorteApi.Domain.Entities;

namespace ICorteApi.Settings;

public static class ConfigureEndpoints
{
    public static void MapMyEndpoints(WebApplication app)
    {
        // app.UseEndpoints(endpoints =>
        // {
        //     endpoints.MapGet("/", () => "Hello World!");
        //     endpoints.MapIdentityApi<User>();
        //     // endpoints.MapAuthEndpoint();
        //     endpoints.MapPersonEndpoint();
        //     endpoints.MapBarberShopEndpoint();
        //     // endpoints.MapOperatingScheduleEndpoint();
        //     endpoints.MapAddressEndpoint();
        // });

        // app.MapGet("/", () => "Hello World!");
        PersonEndpoint.Map(app);
        BarberShopEndpoint.Map(app);
        OperatingScheduleEndpoint.Map(app);
        AddressEndpoint.Map(app);
        app.MapIdentityApi<User>();
        AuthEndpoint.Map(app);
    }
}
