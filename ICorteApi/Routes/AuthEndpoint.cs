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
        // var group = app.MapGroup("auth").MapIdentityApi<User>();
        // var group = app.MapGroup("auth");
        var group = app.MapGroup("auth").RequireAuthorization();

        group.MapPost("register", CreateUser).AllowAnonymous();
        group.MapGet("me", GetUser);
        group.MapPut("update", UpdateUser);
        group.MapPut("resetpassword", ChangePassword);
        group.MapPost("logout", Logout);
        group.MapDelete("delete", DeleteUser);
    }

    // app.MapPost("/auth/login", async (UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration, LoginDto dto) =>
    public static async Task<IResult> Login(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration, UserLoginDtoRequest dto)
    {
        var user = await userManager.FindByNameAsync(dto.Email);

        if (user is null || !await userManager.CheckPasswordAsync(user, dto.Password))
            return Results.Unauthorized();

        var claims = new Claim[]
        {
            new(JwtRegisteredClaimNames.Sub, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, user.Id)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        return Results.Ok(new { Token = new JwtSecurityTokenHandler().WriteToken(token) });
    }
    
    public static async Task<IResult> CreateUser(UserDtoRequest dto, ICorteContext context, HttpContext httpContext, UserManager<User> userManager)
    {
        try
        {
            if (!httpContext.User.IsAuthorized(UserRole.Admin))
                return Results.Forbid();
            
            var newUser = dto.CreateEntity<User>()!;

            newUser.UserName = newUser.Email;
            newUser.EmailConfirmed = true;
            string password = dto.Password; // Fazer um hash cabuloso aqui.

            var userOperation = await userManager.CreateAsync(newUser, password);
            // await context.Users.AddAsync(newUser!);

            if (!userOperation.Succeeded)
                return Results.BadRequest();
            
            var roleOperation = await userManager.AddToRoleAsync(newUser, newUser.Role.ToString());

            if (!roleOperation.Succeeded)
                return Results.BadRequest();
            
            var id = await context.SaveChangesAsync();

            return Results.Created($"/user/{id}", new { Message = "Usuário criado com sucesso" });
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }

    // app.MapGet("/auth/me", async (UserManager<User> userManager, HttpContext httpContext) =>
    public static async Task<IResult> GetUser(UserManager<User> userManager, HttpContext httpContext)
    {
        var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await userManager.FindByIdAsync(userId);

        if (user is null)
            return Results.NotFound();

        var userDtoResponse = user.CreateDto<UserDtoResponse>();

        // var userDto = new UserDto
        // {
        //     UserName = user.UserName,
        //     Email = user.Email,
        //     FullName = user.FullName
        // };

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
            if (empty is not null)
            {
                // 'empty' must be passed and also must be empty, like => {}
                await signInManager.SignOutAsync();
                return Results.Ok();
            }

            return Results.Unauthorized();
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
