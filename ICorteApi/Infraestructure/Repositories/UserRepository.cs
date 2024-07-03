using System.Security.Claims;
using ICorteApi.Domain.Base;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Context;
using ICorteApi.Infraestructure.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ICorteApi.Infraestructure.Repositories;

public class UserRepository(AppDbContext context, IHttpContextAccessor httpContextAccessor, UserManager<User> userManager) : IUserRepository
{
    private readonly AppDbContext _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly UserManager<User> _userManager = userManager;

    // private async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
    
    public async Task<IResponseDataModel<User>> GetAsync()
    {
        var user = _httpContextAccessor.HttpContext?.User;

        if (user is null)
            return new ResponseDataModel<User> { Success = false, Message = "Unauthorized" };
            
        if (!int.TryParse(_userManager.GetUserId(user), out int userId))
            return new ResponseDataModel<User> { Success = false, Message = "Unauthorized" };

        var userEntity = await _userManager.FindByIdAsync(userId.ToString());

        if (userEntity is null)
            return new ResponseDataModel<User> { Success = false, Message = "User not found" };

        return new ResponseDataModel<User> { Success = true, Data = userEntity };
    }

    public async Task<int?> GetUserIdAsync()
    {
        var user = _httpContextAccessor.HttpContext?.User;

        if (user is null)
            return null;
            
        if (!int.TryParse(_userManager.GetUserId(user), out int userId))
            return null;

        return userId;
    }
    
    // public async Task<IResponseModel> UpdateAsync(int id, BarberShopDtoRequest dto)
    // {
    //     try
    //     {
    //         var barberShop = await _context.BarberShops.SingleOrDefaultAsync(b => b.Id == id);

    //         if (barberShop is null)
    //             return new ResponseModel { Success = false };
            
    //         barberShop.Name = dto.Name;
    //         barberShop.Description = dto.Description;
    //         barberShop.PhoneNumber = dto.PhoneNumber;
    //         barberShop.ComercialNumber = dto.ComercialNumber;
    //         barberShop.ComercialEmail = dto.ComercialEmail;
    //         barberShop.OpeningHours = dto.OpeningHours;
    //         barberShop.ClosingHours = dto.ClosingHours;
    //         barberShop.DaysOpen = dto.DaysOpen;
            
    //         if (dto.Address is not null)
    //         {
    //             barberShop.Address.Street = dto.Address.Street;
    //             barberShop.Address.Number = dto.Address.Number;
    //             barberShop.Address.Complement = dto.Address.Complement;
    //             barberShop.Address.Neighborhood = dto.Address.Neighborhood;
    //             barberShop.Address.City = dto.Address.City;
    //             barberShop.Address.State = dto.Address.State;
    //             barberShop.Address.PostalCode = dto.Address.PostalCode;
    //             barberShop.Address.Country = dto.Address.Country;

    //             barberShop.Address.UpdatedAt = DateTime.UtcNow;
    //         }

    //         barberShop.UpdatedAt = DateTime.UtcNow;
            
    //         return new ResponseModel { Success = await SaveChangesAsync() };
    //     }
    //     catch (Exception)
    //     {
    //         throw;
    //     }
    // }
}
