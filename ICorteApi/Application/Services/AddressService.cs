using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public class AddressService(IAddressRepository addressRepository)
    : BasePrimaryKeyService<Address, int>(addressRepository), IAddressService
{
}
