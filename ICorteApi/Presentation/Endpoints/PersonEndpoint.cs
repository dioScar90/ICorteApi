using ICorteApi.Application.Dtos;
using ICorteApi.Domain.Entities;
using ICorteApi.Presentation.Extensions;
using ICorteApi.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ICorteApi.Presentation.Enums;
using ICorteApi.Presentation.Exceptions;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Presentation.Endpoints;

public static class PersonEndpoint
{
    private static readonly string INDEX = "";
    private static readonly string ENDPOINT_PREFIX = EndpointPrefixes.Person;
    private static readonly string ENDPOINT_NAME = EndpointNames.Person;

    public static void Map(WebApplication app)
    {
        var group = app.MapGroup(ENDPOINT_PREFIX)
            .WithTags(ENDPOINT_NAME)
            .RequireAuthorization();

        // group.MapGet(INDEX, GetAll);
        group.MapGet("me", GetMe);
        group.MapPost(INDEX, CreatePerson);
        // group.MapPut("{id}", UpdatePerson);
        // group.MapDelete("{id}", DeletePerson);
    }

    // public static async Task<IResult> Login(SignInManager<User> signInManager, UserDtoLoginRequest dto)
    // {
    //     var result = await signInManager.PasswordSignInAsync(dto.Email, dto.Password, false, false);

    //     if (!result.Succeeded)
    //         return Results.Unauthorized();

    //     return Results.Ok("Login bem-sucedido");
    // }

    public static async Task<IResult> CreatePerson(
        PersonDtoRequest dto,
        IUserService userService,
        IPersonService personService,
        IPersonErrors errors)
    {
        var userResponse = await userService.GetAsync();

        if (!userResponse.IsSuccess)
            return Results.BadRequest(userResponse);

        var newPerson = dto.CreateEntity()!;
        newPerson.UserId = userResponse.Value!.Id;

        var personResponse = await personService.CreateAsync(userResponse.Value.Id, dto);

        if (!personResponse.IsSuccess)
            return Results.BadRequest(personResponse.Error);

        return Results.Created($"/{ENDPOINT_PREFIX}/{newPerson!.UserId}", new { Message = "Pessoa criada com sucesso" });
    }

    public static async Task<IResult> GetMe(
        IUserService userService,
        IPersonService personService,
        IPersonErrors errors)
    {
        // Obtendo o ID do usuário autenticado
        // var userId = userManager.GetUserId(user);
        var userResponse = await userService.GetAsync();

        if (!userResponse.IsSuccess)
            // throw new UserNotFoundException();
            return Results.BadRequest(userResponse);

        // if (!int.TryParse(userManager.GetUserId(user), out int userId))
        //     return Results.Unauthorized();
        
        var personResponse = await personService.GetByIdAsync(userResponse.Value!.Id);

        if (!personResponse.IsSuccess)
            errors.ThrowNotFoundException();
            
        var personDto = personResponse.Value!.CreateDto();
        return Results.Ok(personDto);
    }

    // public static async Task<IResult> UpdateProfile(
    //     UserManager<User> userManager, ClaimsPrincipal user, UserDtoUpdateProfileRequest request)
    // {
    //     var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
    //     var userToUpdate = await userManager.FindByIdAsync(userId);

    //     if (userToUpdate is null)
    //         return Results.NotFound(new { Message = "Usuário não encontrado." });

    //     // Atualize os campos do usuário
    //     userToUpdate.Person.FirstName = request.FirstName;
    //     userToUpdate.Person.LastName = request.LastName;
    //     // Atualize outros campos conforme necessário

    //     var result = await userManager.UpdateAsync(userToUpdate);

    //     if (!result.Succeeded)
    //         return Results.BadRequest(new { Message = "Erro ao atualizar perfil." });

    //     return Results.Ok(new { Message = "Perfil atualizado com sucesso." });
    // }

    // public static async Task<IResult> Logout(SignInManager<User> signInManager, [FromBody] object empty)
    // {
    //     try
    //     {
    //         // 'empty' must be passed and also must be empty, like => {}
    //         // It was written on documentation. I don't know why.
    //         // It's unnecessary, perhaps.
    //         if (empty is not {})
    //             return Results.Unauthorized();

    //         await signInManager.SignOutAsync();
    //         return Results.Ok();
    //     }
    //     catch (Exception ex)
    //     {
    //         return TypedResults.BadRequest(ex.Message);
    //     }
    // }

    // public static async Task<IResult> ForgotPassword(UserManager<User> userManager, UserDtoForgotPasswordRequest request)
    // {
    //     var user = await userManager.FindByEmailAsync(request.Email);
    //     string DEFAULT_MESSAGE = "Se o email existir, um link de redefinição de senha será enviado.";

    //     if (user is null) // Não revelar que o usuário não existe ou não está confirmado
    //         return Results.Ok(new { Message = DEFAULT_MESSAGE });

    //     var token = await userManager.GeneratePasswordResetTokenAsync(user);
    //     var resetLink = $"https://yourapp.com/resetPassword?email={Uri.EscapeDataString(request.Email)}&token={Uri.EscapeDataString(token)}";

    //     // Envie o link por email. Aqui você pode usar um serviço de email para enviar o link.
    //     Console.WriteLine($"Reset link: {resetLink}"); // Substitua isso pelo serviço de email.

    //     return Results.Ok(new { Message = DEFAULT_MESSAGE });
    // }

    // public static async Task<IResult> ResetPassword(UserManager<User> userManager, UserDtoResetPasswordRequest request)
    // {
    //     if (request.NewPassword != request.ConfirmNewPassword)
    //         return Results.BadRequest(new { Message = "As senhas não coincidem." });

    //     var user = await userManager.FindByEmailAsync(request.Email);

    //     if (user is null) // Não revelar que o usuário não existe
    //         return Results.BadRequest(new { Message = "Token inválido." });

    //     var result = await userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);

    //     if (!result.Succeeded)
    //         return Results.BadRequest(new { Message = "Token inválido ou a senha não atende aos requisitos de segurança." });

    //     return Results.Ok(new { Message = "Senha redefinida com sucesso." });
    // }

    // public static async Task<IResult> ChangePassword(
    //     UserManager<User> userManager, ClaimsPrincipal user, UserDtoChangePasswordRequest request)
    // {
    //     if (request.NewPassword != request.ConfirmNewPassword)
    //         return Results.BadRequest(new { Message = "As senhas não coincidem." });

    //     var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
    //     var userToUpdate = await userManager.FindByIdAsync(userId);

    //     if (userToUpdate is null)
    //         return Results.NotFound(new { Message = "Usuário não encontrado." });

    //     var result = await userManager.ChangePasswordAsync(userToUpdate, request.CurrentPassword, request.NewPassword);

    //     if (!result.Succeeded)
    //         return Results.BadRequest(new { Message = "Erro ao alterar senha." });

    //     return Results.Ok(new { Message = "Senha alterada com sucesso." });
    // }

    // public static async Task<IResult> ConfirmEmail(UserManager<User> userManager, UserDtoConfirmEmailRequest request)
    // {
    //     var user = await userManager.FindByEmailAsync(request.Email);

    //     if (user is null)
    //         return Results.BadRequest(new { Message = "Token inválido." });

    //     var result = await userManager.ConfirmEmailAsync(user, request.Token);

    //     if (!result.Succeeded)
    //         return Results.BadRequest(new { Message = "Erro ao confirmar email." });

    //     return Results.Ok(new { Message = "Email confirmado com sucesso." });
    // }

    // public static async Task<IResult> DeleteUser(UserManager<User> userManager, HttpContext httpContext)
    // {
    //     var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
    //     var user = await userManager.FindByIdAsync(userId);

    //     if (user is null)
    //         return Results.NotFound();

    //     var result = await userManager.DeleteAsync(user);

    //     if (!result.Succeeded)
    //         return Results.BadRequest(result.Errors);

    //     return Results.Ok(new { Message = "Usuário removido com sucesso" });
    // }
}
