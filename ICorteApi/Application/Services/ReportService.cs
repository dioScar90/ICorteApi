using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Base;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Errors;
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
        var resp = await GetByIdAsync(id);
        
        if (!resp.IsSuccess)
            return resp;
            
        if (resp.Value!.ClientId != clientId || resp.Value!.BarberShopId != barberShopId)
            return Response.Failure<Report>(Error.TEntityNotFound);

        return resp;
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
