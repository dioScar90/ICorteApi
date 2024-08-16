using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Infraestructure.Interfaces;

public interface IProfileRepository
    : IBasePrimaryKeyRepository<Profile, int>
{
    Task<ISingleResponse<Profile>> CreateAsync(Profile profile, string phoneNumber);
}
