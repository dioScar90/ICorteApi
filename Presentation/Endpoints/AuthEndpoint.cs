using Microsoft.AspNetCore.Identity;
using ICorteApi.Domain.Interfaces;
using FluentValidation;

namespace ICorteApi.Presentation.Endpoints;

public static class AuthEndpoint
{
    public static IEndpointRouteBuilder MapAuthEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("auth").WithTags("Auth");
            
        group.MapPost("register", RegisterAsync)
            .WithSummary("Register")
            .AllowAnonymous();
            
        group.MapPost("login", LoginAsync)
            .WithSummary("Login")
            .AllowAnonymous();
        
        group.MapPost("logout", LogoutUserAsync)
            .WithSummary("Logout")
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
    public static async Task<IResult> RegisterAsync(
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
        
        return Results.Created("user/me", new { Message = "Usuário criado com sucesso" });
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
        await signInManager.SignOutAsync();
        return Results.StatusCode(StatusCodes.Status205ResetContent);
    }
}
