using ICorteApi.Presentation.Endpoints;

namespace ICorteApi.Settings;

public static class ConfigureEndpoints
{
    public static IEndpointRouteBuilder ConfigureMyEndpoints(this IEndpointRouteBuilder endpointBuilder)
    {
        endpointBuilder.MapGet("/", () => "Hello World!");

        endpointBuilder
            .MapAuthEndpoint()
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

        return endpointBuilder;
    }
}
