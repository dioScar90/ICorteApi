namespace ICorteApi.Infraestructure.Repositories;

public sealed class AddressRepository(AppDbContext context)
    : BaseRepository<Address>(context), IAddressRepository
{
}
