using ICorteApi.Presentation.Endpoints;

namespace ICorteApi.Settings;

public static class ConfigureEndpoints
{
    public static void MapMyEndpoints(WebApplication app)
    {
        app.MapGet("/", () => "Hello World!");

        AuthEndpoint.Map(app);
        
        AddressEndpoint.Map(app);
        AppointmentEndpoint.Map(app);
        BarberShopEndpoint.Map(app);
        MessageEndpoint.Map(app);
        PaymentEndpoint.Map(app);
        RecurringScheduleEndpoint.Map(app);
        ReportEndpoint.Map(app);
        ServiceEndpoint.Map(app);
        SpecialScheduleEndpoint.Map(app);
        UserEndpoint.Map(app);
    }
}
