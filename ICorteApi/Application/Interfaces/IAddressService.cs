using ICorteApi.Domain.Entities;

namespace ICorteApi.Application.Interfaces;

public interface IAddressService
    : IBasePrimaryKeyService<Address, int>
{
}