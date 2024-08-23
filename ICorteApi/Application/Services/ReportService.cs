using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public sealed class ReportService(IReportRepository repository)
    : BaseService<Report>(repository), IReportService
{
    public async Task<ISingleResponse<Report>> CreateAsync(IDtoRequest<Report> dtoRequest, int clientId, int barberShopId)
    {
        if (dtoRequest is not ReportDtoRequest dto)
            throw new ArgumentException("Tipo de DTO inválido", nameof(dtoRequest));

        var entity = new Report(dto, clientId, barberShopId);
        return await CreateAsync(entity);
    }

    public async Task<ISingleResponse<Report>> GetByIdAsync(int id, int clientId, int barberShopId)
    {
        return await GetByIdAsync(x => x.Id == id && x.ClientId == clientId && x.BarberShopId == barberShopId);
    }
    
    public async Task<ICollectionResponse<Report>> GetAllAsync(int? page, int? pageSize, int barberShopId)
    {
        return await GetAllAsync(new(page, pageSize, x => x.BarberShopId == barberShopId));
    }
    
    public async Task<IResponse> UpdateAsync(IDtoRequest<Report> dtoRequest, int id, int clientId, int barberShopId)
    {
        var resp = await GetByIdAsync(id, clientId, barberShopId);

        if (!resp.IsSuccess)
            return resp;

        var entity = resp.Value!;
        entity.UpdateEntityByDto(dtoRequest);

        return await UpdateAsync(entity);
    }

    public async Task<IResponse> DeleteAsync(int id, int clientId, int barberShopId)
    {
        var resp = await GetByIdAsync(id, clientId, barberShopId);

        if (!resp.IsSuccess)
            return resp;

        var entity = resp.Value!;
        return await DeleteAsync(entity);
    }
}
