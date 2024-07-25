using ICorteApi.Domain.Entities;

namespace ICorteApi.Infraestructure.Interfaces;

public interface IAddressRepository
    : IBasePrimaryKeyRepository<Address, int>
{
}
