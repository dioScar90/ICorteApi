using ICorteApi.Presentation.Endpoints;

namespace ICorteApi.Settings;

public static class ConfigureEndpoints
{
    public static IEndpointRouteBuilder ConfigureMyEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/", () => "Hello World!");

        app.MapAuthEndpoint()
            .MapAddressEndpoint()
            .MapAppointmentEndpoint()
            .MapBarberShopEndpoint()
            .MapMessageEndpoint()
            .MapPaymentEndpoint()
            .MapRecurringScheduleEndpoint()
            .MapReportEndpoint()
            .MapServiceEndpoint()
            .MapSpecialScheduleEndpoint()
            .MapUserEndpoint();

        return app;
    }
}
