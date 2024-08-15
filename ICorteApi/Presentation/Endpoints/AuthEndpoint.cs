using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ICorteApi.Domain.Entities;
using ICorteApi.Presentation.Enums;
using ICorteApi.Application.Dtos;
using ICorteApi.Domain.Interfaces;
using FluentValidation;
using ICorteApi.Presentation.Extensions;

namespace ICorteApi.Presentation.Endpoints;

public static class AuthEndpoint
{
    private static readonly string INDEX = "";
    private static readonly string ENDPOINT_PREFIX = EndpointPrefixes.Auth;
    private static readonly string ENDPOINT_NAME = EndpointNames.Auth;

    public static IEndpointRouteBuilder MapAuthEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(ENDPOINT_PREFIX)
            .WithTags(ENDPOINT_NAME);

        group.MapIdentityApi<User>();

        group.MapPost("logout", LogoutUser);

        return app;
    }

    public static async Task<IResult> LogoutUser(SignInManager<User> signInManager, [FromBody] object? empty)
    {
        try
        {
            if (empty is null)
                return Results.Unauthorized();

            await signInManager.SignOutAsync();
            return Results.StatusCode(StatusCodes.Status205ResetContent);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(new { ex.Message });
        }
    }

}
