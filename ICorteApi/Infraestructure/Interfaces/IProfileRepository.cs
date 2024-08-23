using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Infraestructure.Interfaces;

public interface IProfileRepository
    : IBaseRepository<Profile>
{
    Task<ISingleResponse<Profile>> CreateAsync(Profile profile, string phoneNumber);
}
