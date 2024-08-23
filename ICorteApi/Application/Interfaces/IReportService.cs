using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Interfaces;

public interface IReportService : IService<Report>
{
    Task<ISingleResponse<Report>> CreateAsync(IDtoRequest<Report> dtoRequest, int clientId, int barberShopId);
    Task<ISingleResponse<Report>> GetByIdAsync(int id, int clientId, int barberShopId);
    Task<ICollectionResponse<Report>> GetAllAsync(int? page, int? pageSize, int barberShopId);
    Task<IResponse> UpdateAsync(IDtoRequest<Report> dtoRequest, int id, int clientId, int barberShopId);
    Task<IResponse> DeleteAsync(int id, int clientId, int barberShopId);
}
