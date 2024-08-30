using ICorteApi.Domain.Entities;

namespace ICorteApi.Infraestructure.Interfaces;

public interface IProfileRepository
    : IBaseRepository<Profile>
{
    Task<Profile?> CreateAsync(Profile profile, string phoneNumber);
}
