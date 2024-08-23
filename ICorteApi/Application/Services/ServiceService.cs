using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public sealed class ServiceService(IServiceRepository repository)
    : BaseService<Service>(repository), IServiceService
{
    public async Task<ISingleResponse<Service>> CreateAsync(IDtoRequest<Service> dtoRequest, int barberShopId)
    {
        if (dtoRequest is not ServiceDtoRequest dto)
            throw new ArgumentException("Tipo de DTO inválido", nameof(dtoRequest));

        var entity = new Service(dto, barberShopId);
        return await CreateAsync(entity);
    }
    
    public async Task<ISingleResponse<Service>> GetByIdAsync(int id, int barberShopId)
    {
        return await GetByIdAsync(x => x.Id == id && x.BarberShopId == barberShopId);
    }
    
    public async Task<ICollectionResponse<Service>> GetAllAsync(int? page, int? pageSize, int barberShopId)
    {
        return await GetAllAsync(new(page, pageSize, x => x.BarberShopId == barberShopId));
    }
    
    public async Task<IResponse> UpdateAsync(IDtoRequest<Service> dtoRequest, int id, int barberShopId)
    {
        var resp = await GetByIdAsync(id, barberShopId);

        if (!resp.IsSuccess)
            return resp;

        var entity = resp.Value!;
        entity.UpdateEntityByDto(dtoRequest);

        return await UpdateAsync(entity);
    }
    
    public async Task<IResponse> DeleteAsync(int id, int barberShopId)
    {
        var resp = await GetByIdAsync(id, barberShopId);

        if (!resp.IsSuccess)
            return resp;

        var entity = resp.Value!;
        return await DeleteAsync(entity);
    }
}
