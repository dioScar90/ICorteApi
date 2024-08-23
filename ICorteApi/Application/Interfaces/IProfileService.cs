using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Interfaces;

public interface IProfileService : IService<Profile>
{
    Task<ISingleResponse<Profile>> CreateAsync(IDtoRequest<Profile> dtoRequest, int userId);
    Task<ISingleResponse<Profile>> GetByIdAsync(int id, int userId);
    Task<IResponse> UpdateAsync(IDtoRequest<Profile> dtoRequest, int id, int userId);
}
