using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Interfaces;

public interface IReportService
    : IBasePrimaryKeyService<Report, int>, IHasTwoForeignKeyService<Report, int, int>
{
    new Task<ISingleResponse<Report>> CreateAsync(IDtoRequest<Report> dtoRequest, int clientId, int barberShopId);
}
