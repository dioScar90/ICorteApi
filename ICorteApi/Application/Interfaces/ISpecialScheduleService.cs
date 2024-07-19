using ICorteApi.Application.Dtos;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Interfaces;

public interface ISpecialScheduleService
{
       Task<IResponse> CreateAsync(int barberShopId, SpecialScheduleDtoRequest dto);
       Task<IResponse> CreateManyAsync(SpecialScheduleDtoRequest[] dtoArr);
       Task<ISingleResponse<SpecialSchedule>> GetByIdAsync(DayOfWeek dayOfWeek, int barberShopId);
       Task<ICollectionResponse<SpecialSchedule>> GetAllAsync(int barberShopId);
       Task<IResponse> UpdateAsync(int barberShopId, SpecialScheduleDtoRequest dto);
       Task<IResponse> UpdateManyAsync(int barberShopId, SpecialScheduleDtoRequest[] dtoArr);
       Task<IResponse> DeleteAsync(DayOfWeek dayOfWeek, int barberShopId);
}
