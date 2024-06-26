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

        group.MapPost("login", Login).AllowAnonymous();
        group.MapPost("register", CreateUser).AllowAnonymous();

        group.MapPost("forgotPassword", ForgotPassword);
        group.MapPut("resetPassword", ResetPassword);
        group.MapPut("changePassword", ChangePassword);
        group.MapPut("confirmEmail", ConfirmEmail);
        group.MapGet("me", GetUser);
        group.MapPut("update", UpdateProfile);
        group.MapPost("logout", Logout);
        group.MapDelete("delete", DeleteUser);
    }

    public static async Task<IResult> Login(SignInManager<User> signInManager, UserDtoLoginRequest dto)
    {
        var result = await signInManager.PasswordSignInAsync(dto.Email, dto.Password, false, false);

        if (!result.Succeeded)
            return Results.Unauthorized();
        
        return Results.Ok("Login bem-sucedido");
    }
    
    public static async Task<IResult> CreateUser(
        UserDtoRegisterRequest dto,
        ICorteContext context,
        UserManager<User> userManager,
        SignInManager<User> signInManager)
    {
        // Maybe it isn't necessary. Do a future confirmation, because this is
        // the only reason why context is here.
        using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            var newUser = dto.CreateEntity<User>()!;

            newUser.UserName = newUser.Email;
            newUser.EmailConfirmed = true;

            var userOperation = await userManager.CreateAsync(newUser, dto.Password);

            if (!userOperation.Succeeded)
                throw new Exception();
            
            var roleOperation = await userManager.AddToRoleAsync(newUser, nameof(UserRole.Client));

            if (!roleOperation.Succeeded)
                throw new Exception();
            
            await context.SaveChangesAsync();
            await transaction.CommitAsync();

            await signInManager.SignInAsync(newUser, isPersistent: false);
            return Results.Created($"/person/{newUser.Person.Id}", new { Message = "Usuário criado com sucesso" });
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return Results.BadRequest(ex.Message);
        }
    }
    
    public static async Task<IResult> GetUser(ClaimsPrincipal user, UserManager<User> userManager)
    {
        var currentUser = await userManager.GetUserAsync(user);
        Console.WriteLine("***User***\n\n");
        Console.WriteLine(currentUser);
        Console.WriteLine("\n\n***User***");

        if (currentUser is null)
            return Results.Unauthorized();
        
        var roles = await userManager.GetRolesAsync(currentUser);
        var userDtoResponse = currentUser.CreateDto<UserDtoResponse>();

        // var roles = rolesAsString
        //     .Select(role => Enum.TryParse<UserRole>(role, out var userRole) ? userRole : (UserRole?)null)
        //     .Where(role => role.HasValue)
        //     .Select(role => role.Value)
        //     .ToArray();
        
        return Results.Ok(userDtoResponse);
        // return Results.Ok(currentUser);
    }

    public static async Task<IResult> UpdateProfile(
        UserManager<User> userManager, ClaimsPrincipal user, UserDtoUpdateProfileRequest request)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        var userToUpdate = await userManager.FindByIdAsync(userId);

        if (userToUpdate is null)
            return Results.NotFound(new { Message = "Usuário não encontrado." });

        // Atualize os campos do usuário
        userToUpdate.Person.FirstName = request.FirstName;
        userToUpdate.Person.LastName = request.LastName;
        // Atualize outros campos conforme necessário

        var result = await userManager.UpdateAsync(userToUpdate);

        if (!result.Succeeded)
            return Results.BadRequest(new { Message = "Erro ao atualizar perfil." });
        
        return Results.Ok(new { Message = "Perfil atualizado com sucesso." });
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
    
    public static async Task<IResult> ForgotPassword(UserManager<User> userManager, UserDtoForgotPasswordRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        string DEFAULT_MESSAGE = "Se o email existir, um link de redefinição de senha será enviado.";

        if (user is null) // Não revelar que o usuário não existe ou não está confirmado
            return Results.Ok(new { Message = DEFAULT_MESSAGE });

        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        var resetLink = $"https://yourapp.com/resetPassword?email={Uri.EscapeDataString(request.Email)}&token={Uri.EscapeDataString(token)}";

        // Envie o link por email. Aqui você pode usar um serviço de email para enviar o link.
        Console.WriteLine($"Reset link: {resetLink}"); // Substitua isso pelo serviço de email.

        return Results.Ok(new { Message = DEFAULT_MESSAGE });
    }

    public static async Task<IResult> ResetPassword(UserManager<User> userManager, UserDtoResetPasswordRequest request)
    {
        if (request.NewPassword != request.ConfirmNewPassword)
            return Results.BadRequest(new { Message = "As senhas não coincidem." });

        var user = await userManager.FindByEmailAsync(request.Email);

        if (user is null) // Não revelar que o usuário não existe
            return Results.BadRequest(new { Message = "Token inválido." });

        var result = await userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);

        if (!result.Succeeded)
            return Results.BadRequest(new { Message = "Token inválido ou a senha não atende aos requisitos de segurança." });
        
        return Results.Ok(new { Message = "Senha redefinida com sucesso." });
    }
    
    public static async Task<IResult> ChangePassword(
        UserManager<User> userManager, ClaimsPrincipal user, UserDtoChangePasswordRequest request)
    {
        if (request.NewPassword != request.ConfirmNewPassword)
            return Results.BadRequest(new { Message = "As senhas não coincidem." });

        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        var userToUpdate = await userManager.FindByIdAsync(userId);

        if (userToUpdate is null)
            return Results.NotFound(new { Message = "Usuário não encontrado." });

        var result = await userManager.ChangePasswordAsync(userToUpdate, request.CurrentPassword, request.NewPassword);

        if (!result.Succeeded)
            return Results.BadRequest(new { Message = "Erro ao alterar senha." });
        
        return Results.Ok(new { Message = "Senha alterada com sucesso." });
    }

    public static async Task<IResult> ConfirmEmail(UserManager<User> userManager, UserDtoConfirmEmailRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user is null)
            return Results.BadRequest(new { Message = "Token inválido." });

        var result = await userManager.ConfirmEmailAsync(user, request.Token);

        if (!result.Succeeded)
            return Results.BadRequest(new { Message = "Erro ao confirmar email." });
        
        return Results.Ok(new { Message = "Email confirmado com sucesso." });
    }
    
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
