using ICorteApi.Domain.Base;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Errors;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Context;
using ICorteApi.Infraestructure.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ICorteApi.Infraestructure.Repositories;

public sealed class UserRepository(AppDbContext context, IHttpContextAccessor httpContextAccessor, UserManager<User> userManager) : IUserRepository
{
    private readonly AppDbContext _context = context;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly UserManager<User> _userManager = userManager;
    
    public async Task<ISingleResponse<User>> GetAsync()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        
        if (user is null)
            return Response.Failure<User>(Error.UserNotFound);

        if (!int.TryParse(_userManager.GetUserId(user), out int userId))
            return Response.Failure<User>(Error.Unauthorized);

        var userEntity = await _userManager.FindByIdAsync(userId.ToString());

        if (userEntity is null)
            return Response.Failure<User>(Error.UserNotFound);

        return Response.Success(userEntity);
    }

    public async Task<int?> GetUserIdAsync()
    {
        var user = _httpContextAccessor.HttpContext?.User;

        if (user is null || !int.TryParse(_userManager.GetUserId(user), out int userId))
            return null;
        
        return userId;
    }
}
