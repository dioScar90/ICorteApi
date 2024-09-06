using ICorteApi.Application.Dtos;
using ICorteApi.Domain.Entities;

namespace ICorteApi.Application.Interfaces;

public interface IProfileService : IService<Profile>
{
    Task<Profile?> CreateAsync(ProfileDtoCreate dto, int userId);
    Task<Profile?> GetByIdAsync(int id, int userId);
    Task<bool> UpdateAsync(ProfileDtoUpdate dto, int id, int userId);
    Task<bool> UpdateProfileImageAsync(int id, int userId, IFormFile image);
}
