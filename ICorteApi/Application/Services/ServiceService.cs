using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Base;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Errors;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public sealed class ServiceService(IServiceRepository repository)
    : BaseService<Service>(repository), IServiceService
{
    new private readonly IServiceRepository _repository = repository;
    
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
    
    public async Task<IResponse> DeleteAsync(int id, int barberShopId, bool forceDelete = false)
    {
        var resp = await GetByIdAsync(id, barberShopId);

        if (!resp.IsSuccess)
            return resp;

        var entity = resp.Value!;

        var thereAreAppointments = !forceDelete && await _repository.CheckCorrelatedAppointmentsAsync(entity.Id);

        if (!thereAreAppointments)
            return await DeleteAsync(entity);
            
        var appointments = await _repository.GetCorrelatedAppointmentsAsync(entity.Id);
        var errorApointments = appointments.Select(a => new Error("Datas", a.Date.ToString())).ToArray();

        return Response.Failure([Error.RemoveError, ..errorApointments]);
    }
}
