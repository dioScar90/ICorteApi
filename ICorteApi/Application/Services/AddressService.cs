using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Base;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Errors;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public sealed class AddressService(IAddressRepository repository)
    : BaseService<Address>(repository), IAddressService
{
    public async Task<ISingleResponse<Address>> CreateAsync(IDtoRequest<Address> dtoRequest, int barberShopId)
    {
        if (dtoRequest is not AddressDtoCreate dto)
            throw new ArgumentException("Tipo de DTO inválido", nameof(dtoRequest));

        var entity = new Address(dto, barberShopId);
        return await CreateAsync(entity);
    }

    public async Task<ISingleResponse<Address>> GetByIdAsync(int id, int barberShopId)
    {
        var resp = await GetByIdAsync(id);
        
        if (!resp.IsSuccess)
            return resp;

        if (resp.Value!.BarberShopId != barberShopId)
            return Response.Failure<Address>(Error.TEntityNotFound);

        return resp;
    }
    
    public async Task<IResponse> UpdateAsync(IDtoRequest<Address> dtoRequest, int id, int barberShopId)
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
