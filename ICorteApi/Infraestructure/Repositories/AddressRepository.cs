using ICorteApi.Domain.Entities;
using ICorteApi.Infraestructure.Context;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Infraestructure.Repositories;

public sealed class AddressRepository(AppDbContext context)
    : BaseRepository<Address>(context), IAddressRepository
{
}
