using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Interfaces;

public interface ISpecialScheduleService : IService<SpecialSchedule>
{
    Task<ISingleResponse<SpecialSchedule>> CreateAsync(IDtoRequest<SpecialSchedule> dtoRequest, int barberShopId);
    Task<ISingleResponse<SpecialSchedule>> GetByIdAsync(DateOnly date, int barberShopId);
    Task<ICollectionResponse<SpecialSchedule>> GetAllAsync(int? page, int? pageSize, int barberShopId);
    Task<IResponse> UpdateAsync(IDtoRequest<SpecialSchedule> dtoRequest, DateOnly date, int barberShopId);
    Task<IResponse> DeleteAsync(DateOnly date, int barberShopId);
}
