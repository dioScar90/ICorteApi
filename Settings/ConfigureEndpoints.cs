using ICorteApi.Presentation.Endpoints;

namespace ICorteApi.Settings;

public static class ConfigureEndpoints
{
    public static IEndpointRouteBuilder ConfigureMyEndpoints(this IEndpointRouteBuilder endpointBuilder)
    {
        endpointBuilder.MapGet("/", () => "Hello, world!");
        
        return endpointBuilder
            .MapAuthEndpoint()
            .MapAdminEndpoint()
            .MapAddressEndpoint()
            .MapAppointmentEndpoint()
            .MapBarberScheduleEndpoint()
            .MapBarberShopEndpoint()
            .MapMessageEndpoint()
            .MapProfileEndpoint()
            .MapRecurringScheduleEndpoint()
            .MapReportEndpoint()
            .MapServiceEndpoint()
            .MapSpecialScheduleEndpoint()
            .MapUserEndpoint();
    }
}
