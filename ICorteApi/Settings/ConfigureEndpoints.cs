using ICorteApi.Presentation.Endpoints;

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
        //     // endpoints.MapRecurringScheduleEndpoint();
        //     endpoints.MapAddressEndpoint();
        // });

        app.MapGet("/", () => "Hello World!");

        PersonEndpoint.Map(app);

        BarberShopEndpoint.Map(app);

        RecurringScheduleEndpoint.Map(app);

        AddressEndpoint.Map(app);

        AuthEndpoint.Map(app);
    }
}
