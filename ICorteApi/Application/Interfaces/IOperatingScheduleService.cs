using ICorteApi.Application.Dtos;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Interfaces;

public interface IOperatingScheduleService
{
       Task<IResponseDataModel<OperatingSchedule>> CreateAsync(int barberShopId, OperatingScheduleDtoRequest dto);
       Task<IResponseModel> CreateManyAsync(OperatingScheduleDtoRequest[] dtoArr);
       Task<IResponseDataModel<OperatingSchedule>> GetByIdAsync(DayOfWeek dayOfWeek, int barberShopId);
       Task<IResponseDataModel<IEnumerable<OperatingSchedule>>> GetAllAsync(int barberShopId);
       Task<IResponseModel> UpdateAsync(int barberShopId, OperatingScheduleDtoRequest dto);
       Task<IResponseModel> UpdateManyAsync(int barberShopId, OperatingScheduleDtoRequest[] dtoArr);
       Task<IResponseModel> DeleteAsync(DayOfWeek dayOfWeek, int barberShopId);
}
