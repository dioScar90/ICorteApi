using ICorteApi.Application.Dtos;
using ICorteApi.Domain.Entities;

namespace ICorteApi.Application.Interfaces;

public interface ISpecialScheduleService : IService<SpecialSchedule>
{
    Task<SpecialSchedule?> CreateAsync(SpecialScheduleDtoRequest dto, int barberShopId);
    Task<SpecialSchedule?> GetByIdAsync(DateOnly date, int barberShopId);
    Task<SpecialSchedule[]> GetAllAsync(int? page, int? pageSize, int barberShopId);
    Task<bool> UpdateAsync(SpecialScheduleDtoRequest dto, DateOnly date, int barberShopId);
    Task<bool> DeleteAsync(DateOnly date, int barberShopId);
}
