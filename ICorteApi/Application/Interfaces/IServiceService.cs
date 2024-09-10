using ICorteApi.Application.Dtos;
using ICorteApi.Domain.Entities;

namespace ICorteApi.Application.Interfaces;

public interface IServiceService : IService<Service>
{
    Task<ServiceDtoResponse> CreateAsync(ServiceDtoCreate dto, int barberShopId);
    Task<ServiceDtoResponse> GetByIdAsync(int id, int barberShopId);
    Task<ServiceDtoResponse[]> GetAllAsync(int? page, int? pageSize, int barberShopId);
    Task<bool> UpdateAsync(ServiceDtoUpdate dto, int id, int barberShopId);
    Task<bool> DeleteAsync(int id, int barberShopId, bool forceDelete);
}
