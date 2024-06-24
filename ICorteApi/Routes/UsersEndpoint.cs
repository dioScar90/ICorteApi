using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ICorteApi.Context;
using Microsoft.AspNetCore.Http.HttpResults;
using ICorteApi.Entities;
using ICorteApi.Repositories;
using ICorteApi.Dtos;
using ICorteApi.Extensions;
using ICorteApi.Enums;

namespace ICorteApi.Routes;

public static class UsersEndpoint
{
    public static void MapUsersEndpoint(this IEndpointRouteBuilder app)
    {
        const string INDEX = "";
        var group = app.MapGroup("user");
            // .RequireAuthorization();

        // group.MapGet(INDEX, GetAllBarbers);
        group.MapGet("{id}", GetUser);
        group.MapPost("create", CreateUser);
        // group.MapPut("{id}", UpdateBarber);
        // group.MapDelete("{id}", DeleteBarber);
    }

    public static async Task<IResult> GetUser(int id, ICorteContext context)
    {
        var user = await context.Users
            .SingleOrDefaultAsync(u => u.IsActive && u.Id == id);

        if (user is null)
            return Results.NotFound();

        var dto = user.CreateDto<UserDtoResponse>();
        return Results.Ok(dto);
    }
    
    public static async Task<IResult> CreateUser(UserDtoRequest dto, ICorteContext context, HttpContext httpContext)
    {
        try
        {
            if (!httpContext.User.IsAuthorized(UserRole.Admin))
                return Results.Forbid();
            
            var newUser = dto.CreateEntity<User>();

            await context.Users.AddAsync(newUser!);

            var id = await context.SaveChangesAsync();

            return Results.Created($"/user/{id}", new { Message = "Usuário criado com sucesso" });
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }
}
