using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Infraestructure.Interfaces;

public interface ISpecialScheduleRepository
{
    Task<IResponse> CreateAsync(SpecialSchedule specialSchedules);
    Task<IResponse> CreateManyAsync(SpecialSchedule[] specialSchedules);
    Task<ISingleResponse<SpecialSchedule>> GetByIdAsync(int barberShopId, DateOnly date);
    Task<ICollectionResponse<SpecialSchedule>> GetAllAsync(int barberShopId, DateOnly startDate, DateOnly endDate);
    Task<IResponse> UpdateAsync(SpecialSchedule specialSchedules);
    Task<IResponse> UpdateManyAsync(SpecialSchedule[] specialSchedules);
    Task<IResponse> DeleteAsync(SpecialSchedule specialSchedules);
}
