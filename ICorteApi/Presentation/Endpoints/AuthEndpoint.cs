using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ICorteApi.Domain.Entities;
using ICorteApi.Application.Dtos;
using ICorteApi.Presentation.Enums;

namespace ICorteApi.Presentation.Endpoints;

public static class AuthEndpoint
{
    private static readonly string INDEX = "";
    private static readonly string ENDPOINT_PREFIX = EndpointPrefixes.Auth;
    private static readonly string ENDPOINT_NAME = EndpointNames.Auth;

    public static void Map(WebApplication app)
    {
        var group = app.MapGroup(ENDPOINT_PREFIX)
            .WithTags(ENDPOINT_NAME);

        group.MapPost("logout", LogoutUser);
        group.MapIdentityApi<User>();
    }

    public static async Task<IResult> LogoutUser(SignInManager<User> signInManager, [FromBody] object? empty)
    {
        try
        {
            if (empty is null)
                return Results.Unauthorized();

            await signInManager.SignOutAsync();
            return Results.StatusCode(205);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(new { ex.Message });
        }
    }

    public static async Task<IResult> Login(SignInManager<User> signInManager, UserDtoLoginRequest dto)
    {
        var result = await signInManager.PasswordSignInAsync(dto.Email, dto.Password, false, false);

        if (!result.Succeeded)
            return Results.Unauthorized();

        return Results.Ok("Login bem-sucedido");
    }
}
