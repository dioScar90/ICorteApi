using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;
using ICorteApi.Presentation.Extensions;

namespace ICorteApi.Application.Services;

public sealed class ReportService(IReportRepository repository)
    : BasePrimaryKeyService<Report, int>(repository), IReportService
{
    public async Task<ISingleResponse<Report>> CreateAsync(IDtoRequest<Report> dto, int clientId, int barberShopId)
    {
        var entity = dto.CreateEntity()!;
        
        entity.ClientId = clientId;
        entity.BarberShopId = barberShopId;
        
        return await CreateByEntityAsync(entity);
    }
}
