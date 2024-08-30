using ICorteApi.Application.Dtos;
using ICorteApi.Domain.Entities;

namespace ICorteApi.Application.Interfaces;

public interface IServiceService : IService<Service>
{
    Task<Service?> CreateAsync(ServiceDtoRequest dto, int barberShopId);
    Task<Service?> GetByIdAsync(int id, int barberShopId);
    Task<Service[]> GetAllAsync(int? page, int? pageSize, int barberShopId);
    Task<bool> UpdateAsync(ServiceDtoRequest dto, int id, int barberShopId);
    Task<bool> DeleteAsync(int id, int barberShopId, bool forceDelete);
}
