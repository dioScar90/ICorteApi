using Microsoft.AspNetCore.Identity;
using ICorteApi.Application.Services;
using ICorteApi.Domain.Errors;
using Microsoft.AspNetCore.Http.HttpResults;

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
            .WithSummary("Logout");
        
        return app;
    }

    private static async Task<SignInResult> LoginHowItMustBe(string userName, string password, SignInManager<User> signInManager)
    {
        const bool USE_COOKIES = true;
        return await signInManager.PasswordSignInAsync(userName, password, USE_COOKIES, lockoutOnFailure: true);
    }
    
    // This method was written using both inspiration of Chat GPT and real Microsoft ASP.NET Core documentation,
    // that you can find in: https://github.com/dotnet/aspnetcore/blob/main/src/Identity/Core/src/IdentityApiEndpointRouteBuilderExtensions.cs
    public static async Task<Results<Created<UserRegisterDtoResponse>, BadRequest<Error>, UnauthorizedHttpResult>> RegisterAsync(
        UserDtoRegisterRequest dto,
        UserService service,
        SignInManager<User> signInManager,
        UserErrors errors)
    {
        var result = await service.CreateAsync(dto);

        if (result is null)
            return errors.Create();

        if (!result.Succeeded)
            return errors.Create([..result.Errors]);
            
        var signInResult = await LoginHowItMustBe(dto.Email, dto.Password, signInManager);
        
        if (!signInResult.Succeeded)
            return errors.Unauthorized();
            
        return TypedResults.Created("user/me", new UserRegisterDtoResponse("Usuário criado com sucesso"));
    }

    public record UserRegisterDtoResponse(string Message);
    
    public static async Task<Results<Ok, UnauthorizedHttpResult>> LoginAsync(
        UserDtoLoginRequest dto,
        SignInManager<User> signInManager,
        UserErrors errors)
    {
        var result = await LoginHowItMustBe(dto.Email, dto.Password, signInManager);
        
        if (!result.Succeeded)
            return errors.Unauthorized();

        return TypedResults.Ok();
    }
    
    public static async Task<NoContent> LogoutUserAsync(object? empty, SignInManager<User> signInManager)
    {
        await signInManager.SignOutAsync();
        return TypedResults.NoContent();
    }
}
