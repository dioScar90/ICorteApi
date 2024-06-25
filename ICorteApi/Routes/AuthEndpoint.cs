using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ICorteApi.Context;
using Microsoft.AspNetCore.Http.HttpResults;
using ICorteApi.Entities;
using ICorteApi.Dtos;
using ICorteApi.Extensions;
using ICorteApi.Enums;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ICorteApi.Routes;

public static class AuthEndpoint
{
    public static void MapUsersEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("auth").RequireAuthorization();

        group.MapPost("register", CreateUser).AllowAnonymous();
        group.MapPost("login", Login);
        group.MapPost("forgotPassword", Logout);
        group.MapPut("resetPassword", ChangePassword);
        group.MapGet("me", GetUser);
        group.MapPut("update", UpdateUser);
        group.MapPost("logout", Logout);
        group.MapDelete("delete", DeleteUser);
    }
    
    public static async Task<IResult> CreateUser(
        RegisterDtoRequest dto,
        ICorteContext context,
        UserManager<User> userManager)
    {
        using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            var newUser = dto.CreateEntity<User>()!;

            newUser.UserName = newUser.Email;
            newUser.EmailConfirmed = true;

            var userOperation = await userManager.CreateAsync(newUser, dto.Password);

            if (!userOperation.Succeeded)
                return Results.BadRequest();
            
            var roleOperation = await userManager.AddToRoleAsync(newUser, nameof(UserRole.Client));

            if (!roleOperation.Succeeded)
                return Results.BadRequest();
            
            var id = await context.SaveChangesAsync();

            await transaction.CommitAsync();
            return Results.Created($"/user/{id}", new { Message = "Usuário criado com sucesso" });
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return Results.BadRequest(ex.Message);
        }
    }
    
    public static async Task<IResult> Login(SignInManager<User> signInManager, LoginDtoRequest dto)
    {
        var result = await signInManager.PasswordSignInAsync(dto.Email, dto.Password, false, false);

        if (!result.Succeeded)
            return Results.Unauthorized();
        
        return Results.Ok("Login successful");
    }
    
    public static async Task<IResult> GetUser(UserManager<User> userManager, ClaimsPrincipal userPrincipal)
    {
        var user = await userManager.GetUserAsync(userPrincipal);

        if (user is null)
            return Results.Unauthorized();
        
        var roles = await userManager.GetRolesAsync(user);
        var userDtoResponse = user.CreateDto<UserDtoResponse>();

        // var roles = rolesAsString
        //     .Select(role => Enum.TryParse<UserRole>(role, out var userRole) ? userRole : (UserRole?)null)
        //     .Where(role => role.HasValue)
        //     .Select(role => role.Value)
        //     .ToArray();
        
        return Results.Ok(userDtoResponse);
    }
    
    // app.MapPut("/auth/update", async (UserManager<User> userManager, HttpContext httpContext, UpdateUserDto dto) =>
    public static async Task<IResult> UpdateUser(UserManager<User> userManager, HttpContext httpContext, UserDtoRequest dto)
    {
        var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await userManager.FindByIdAsync(userId);

        if (user is null)
            return Results.NotFound();

        // user.FullName = dto.FullName;
        // user.Email = dto.Email;

        
        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        user.Email = dto.Email;
        user.PhoneNumber = dto.PhoneNumber;

        // user.Password = dto.Password;

        var result = await userManager.UpdateAsync(user);

        if (!result.Succeeded)
            return Results.BadRequest(result.Errors);

        return Results.Ok(new { Message = "Usuário atualizado com sucesso" });
    }

    // app.MapPost("/auth/resetpassword", async (UserManager<User> userManager, ResetPasswordDto dto) =>
    public static async Task<IResult> ChangePassword(UserManager<User> userManager, UserDtoRequest dto)
    {
        var user = await userManager.FindByEmailAsync(dto.Email);

        if (user is null)
            return Results.NotFound();

        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        var result = await userManager.ResetPasswordAsync(user, token, dto.Password);

        if (!result.Succeeded)
            return Results.BadRequest(result.Errors);

        var oie = new { Message = "Senha alterada com sucesso" };
        return Results.Ok(oie);
    }

    public static async Task<IResult> Logout(SignInManager<User> signInManager, [FromBody] object empty)
    {
        try
        {
            // 'empty' must be passed and also must be empty, like => {}
            // It was written on documentation. I don't know why.
            // It's unnecessary, perhaps.
            if (empty is not {})
                return Results.Unauthorized();
                
            await signInManager.SignOutAsync();
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }

    // app.MapDelete("/auth/delete", async (UserManager<User> userManager, HttpContext httpContext) =>
    public static async Task<IResult> DeleteUser(UserManager<User> userManager, HttpContext httpContext)
    {
        var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await userManager.FindByIdAsync(userId);

        if (user is null)
            return Results.NotFound();

        var result = await userManager.DeleteAsync(user);

        if (!result.Succeeded)
            return Results.BadRequest(result.Errors);

        return Results.Ok(new { Message = "Usuário removido com sucesso" });
    }
}
