using ICorteApi.Application.Dtos;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Interfaces;

public interface IProfileService : IService<Profile>
{
    Task<Profile?> CreateAsync(ProfileDtoCreate dto, int userId);
    Task<Profile?> GetByIdAsync(int id, int userId);
    Task<bool> UpdateAsync(ProfileDtoUpdate dto, int id, int userId);
}
