namespace ICorteApi.Application.Interfaces;

public interface ISpecialScheduleService : IService<SpecialSchedule>
{
    Task<SpecialScheduleDtoResponse> CreateAsync(SpecialScheduleDtoCreate dto, int barberShopId);
    Task<SpecialScheduleDtoResponse> GetByIdAsync(DateOnly date, int barberShopId);
    Task<SpecialScheduleDtoResponse[]> GetAllAsync(int? page, int? pageSize, int barberShopId);
    Task<bool> UpdateAsync(SpecialScheduleDtoUpdate dto, DateOnly date, int barberShopId);
    Task<bool> DeleteAsync(DateOnly date, int barberShopId);
}
