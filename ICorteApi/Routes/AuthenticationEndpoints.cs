using BarberAppApi.Context;
using BarberAppApi.Dtos;
using BarberAppApi.Repositories;
using BarberAppApi.Services;

namespace BarberAppApi.Routes;

public static class AuthenticationEndpoints
{
    public static void MapAuthenticationEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("auth");
        // var configs = app.builder.Configuration.getSection("JwtSettings").Get<string>();

        group.MapPost("login", Login);
        // group.MapGet("", CreateProduct);
        // group.MapPost("", CreateProduct);
        // group.MapPost("", CreateProduct);
    }

    public static async Task<IResult> Login(UserLoginDto login, BarberShopContext context)
    {
        var user = await UserRepository.Get(login.Username, login.Password);

        if (user is null)
            return Results.Unauthorized();
        
        user.Password = "";

        var token = TokenService.GenerateToken(user);
        return Results.Ok(new { token, user = new { username = user.Username, role = user.Role, rolestring = user.Role.ToString() } });
    }
    
// app.MapPost("/register", async (User user, BarberShopContext context) =>
// {
//     user.PasswordHash = user.PasswordHash; // You should hash the password
//     context.Users.Add(user);
//     await context.SaveChangesAsync();
//     return Results.Ok(user);
// });
}
