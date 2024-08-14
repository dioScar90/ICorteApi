using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public sealed class ServiceService(IServiceRepository repository)
    : BasePrimaryKeyService<Service, int>(repository), IServiceService
{
    public async Task<ISingleResponse<Service>> CreateAsync(IDtoRequest<Service> dtoRequest, int barberShopId)
    {
        if (dtoRequest is not ServiceDtoRequest dto)
            throw new ArgumentException("Tipo de DTO inválido", nameof(dtoRequest));

        var entity = new Service(dto, barberShopId);
        return await CreateByEntityAsync(entity);
    }
}
