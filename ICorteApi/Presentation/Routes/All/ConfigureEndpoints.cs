using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICorteApi.Routes;

public static class ConfigureEndpoints
{
    public static void MapMyEndpoints(this IApplicationBuilder app)
    {
        app.UseEndpoints(endpoints =>
        {
            // endpoints.MapIdentityApi<User>();
            endpoints.MapAuthEndpoint();
            endpoints.MapPersonEndpoint();
            endpoints.MapBarberShopEndpoint();
            endpoints.MapAddressEndpoint();
            endpoints.MapGet("/", () => "Hello World!");
        });
    }
}
