namespace ICorteApi.Application.Interfaces;

public interface IBarberShopService : IService<BarberShop>
{
    Task<BarberShopDto> CreateAsync(BarberShopDto dto, int ownerId);
    Task<BarberShopDto> GetByIdAsync(int id);
    Task<PaginationResponse<AppointmentsByBarberShopDto>> GetAppointmentsByBarberShopAsync(int barberShopId, int ownerId, int? page, int? pageSize);
    Task<bool> UpdateAsync(BarberShopDto dto, int id, int ownerId);
    Task<bool> DeleteAsync(int id, int ownerId);
}
