namespace ICorteApi.Infraestructure.Interfaces;

public interface IProfileRepository
    : IBaseRepository<Profile>
{
    Task<Profile?> CreateAsync(Profile profile, string phoneNumber);
    Task<bool> UpdateAsync(Profile entity, string phoneNumber);
}
