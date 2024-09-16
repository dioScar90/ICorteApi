using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ICorteApi.Domain.Interfaces;
using FluentValidation;

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

        // This endpoint was written using both inspiration of Chat GPT and real Microsoft ASP.NET Core documentation,
        // that you can find in: https://github.com/dotnet/aspnetcore/blob/main/src/Identity/Core/src/IdentityApiEndpointRouteBuilderExtensions.cs
            
        group.MapPost("login", LoginAsync)
            .AllowAnonymous();
        
        group.MapPost("logout", LogoutUserAsync)
            .RequireAuthorization(nameof(PolicyUserRole.FreeIfAuthenticated));

        return app;
    }

    public static async Task<IResult> LoginAsync(
        UserDtoLoginRequest dto,
        IValidator<UserDtoLoginRequest> validator,
        SignInManager<User> signInManager,
        IUserErrors errors)
    {
        dto.CheckAndThrowExceptionIfInvalid(validator, errors);

        const bool USE_COOKIES = true;
        var result = await signInManager.PasswordSignInAsync(dto.Email, dto.Password, USE_COOKIES, lockoutOnFailure: true);

        if (!result.Succeeded)
            return Results.Unauthorized();

        return Results.Ok();
    }
    
    public static async Task<IResult> LogoutUserAsync(SignInManager<User> signInManager, [FromBody] object? empty)
    {
        if (empty is null)
            return Results.Unauthorized();

        await signInManager.SignOutAsync();
        return Results.StatusCode(StatusCodes.Status205ResetContent);
    }
}
