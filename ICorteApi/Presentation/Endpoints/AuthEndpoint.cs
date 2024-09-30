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
            
        group.MapPost("register", RegisterUser)
            .AllowAnonymous();
            
        group.MapPost("login", LoginAsync)
            .AllowAnonymous();
        
        group.MapPost("logout", LogoutUserAsync)
            .RequireAuthorization(nameof(PolicyUserRole.FreeIfAuthenticated));

        return app;
    }

    private static async Task<SignInResult> LoginHowItMustBe(string userName, string password, SignInManager<User> signInManager)
    {
        const bool USE_COOKIES = true;
        return await signInManager.PasswordSignInAsync(userName, password, USE_COOKIES, lockoutOnFailure: true);
    }
    
    // This method was written using both inspiration of Chat GPT and real Microsoft ASP.NET Core documentation,
    // that you can find in: https://github.com/dotnet/aspnetcore/blob/main/src/Identity/Core/src/IdentityApiEndpointRouteBuilderExtensions.cs
    public static async Task<IResult> RegisterUser(
        UserDtoRegisterCreate dto,
        IValidator<UserDtoRegisterCreate> validator,
        IUserService service,
        SignInManager<User> signInManager,
        IUserErrors errors)
    {
        dto.CheckAndThrowExceptionIfInvalid(validator, errors);
        var user = await service.CreateAsync(dto);

        if (user is null)
            errors.ThrowCreateException();

        var result = await LoginHowItMustBe(user!.UserName!, dto.Password, signInManager);

        if (!result.Succeeded)
            return Results.Unauthorized();

        string uri = EndpointPrefixes.User + "/me";
        object value = new { Message = "Usuário criado com sucesso" };
        return Results.Created(uri, value);
    }

    public static async Task<IResult> LoginAsync(
        UserDtoLoginRequest dto,
        IValidator<UserDtoLoginRequest> validator,
        SignInManager<User> signInManager,
        IUserErrors errors)
    {
        dto.CheckAndThrowExceptionIfInvalid(validator, errors);
        var result = await LoginHowItMustBe(dto.Email, dto.Password, signInManager);
        
        if (!result.Succeeded)
            return Results.Unauthorized();

        return Results.Ok();
    }
    
    public static async Task<IResult> LogoutUserAsync(object? empty, SignInManager<User> signInManager)
    {
        if (empty is null)
            return Results.Unauthorized();

        await signInManager.SignOutAsync();
        return Results.StatusCode(StatusCodes.Status205ResetContent);
    }
}
