using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Interfaces;

public interface IServiceService : IService<Service>
{
    Task<ISingleResponse<Service>> CreateAsync(IDtoRequest<Service> dtoRequest, int barberShopId);
    Task<ISingleResponse<Service>> GetByIdAsync(int id, int barberShopId);
    Task<ICollectionResponse<Service>> GetAllAsync(int? page, int? pageSize, int barberShopId);
    Task<IResponse> UpdateAsync(IDtoRequest<Service> dtoRequest, int id, int barberShopId);
    Task<IResponse> DeleteAsync(int id, int barberShopId);
}
