using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public sealed class ReportService(IReportRepository repository)
    : BasePrimaryKeyService<Report, int>(repository), IReportService
{
    public async Task<ISingleResponse<Report>> CreateAsync(IDtoRequest<Report> dtoRequest, int clientId, int barberShopId)
    {
        if (dtoRequest is not ReportDtoRequest dto)
            throw new ArgumentException("Tipo de DTO inválido", nameof(dtoRequest));

        var entity = new Report(dto, clientId, barberShopId);
        return await CreateByEntityAsync(entity);
    }
}
