namespace ICorteApi.Application.Interfaces;

public interface IAddressService : IService<Address>
{
    Task<AddressDto> CreateAsync(AddressDto dto, int barberShopId);
    Task<AddressDto> GetByIdAsync(int id, int barberShopId);
    Task<bool> UpdateAsync(AddressDto dto, int id, int barberShopId);
    Task<bool> DeleteAsync(int id, int barberShopId);
}
