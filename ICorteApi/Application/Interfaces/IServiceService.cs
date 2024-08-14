using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Interfaces;

public interface IServiceService
    : IBasePrimaryKeyService<Service, int>, IHasOneForeignKeyService<Service, int>
{
    new Task<ISingleResponse<Service>> CreateAsync(IDtoRequest<Service> dtoRequest, int barberShopId);
}
