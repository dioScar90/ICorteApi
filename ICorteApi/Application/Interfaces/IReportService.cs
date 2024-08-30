using ICorteApi.Application.Dtos;
using ICorteApi.Domain.Entities;

namespace ICorteApi.Application.Interfaces;

public interface IReportService : IService<Report>
{
    Task<Report?> CreateAsync(ReportDtoRequest dto, int clientId, int barberShopId);
    Task<Report?> GetByIdAsync(int id, int clientId, int barberShopId);
    Task<Report[]> GetAllAsync(int? page, int? pageSize, int barberShopId);
    Task<bool> UpdateAsync(ReportDtoRequest dto, int id, int clientId, int barberShopId);
    Task<bool> DeleteAsync(int id, int clientId, int barberShopId);
}
