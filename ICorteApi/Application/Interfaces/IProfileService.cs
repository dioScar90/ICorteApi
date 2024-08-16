using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Interfaces;

public interface IProfileService
    : IBasePrimaryKeyService<Profile, int>, IHasOneForeignKeyService<Profile, int>
{
    new Task<ISingleResponse<Profile>> CreateAsync(IDtoRequest<Profile> dtoRequest, int userId);
}
