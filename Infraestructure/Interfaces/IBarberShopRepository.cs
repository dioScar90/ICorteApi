namespace ICorteApi.Infraestructure.Interfaces;

public interface IBarberShopRepository
    : IBaseRepository<BarberShop>
{
    Task<BarberShop?> GetByIdAsync(int barberShopId);
    Task<PaginationResponse<AppointmentsByBarberShopDtoResponse>> GetAppointmentsByBarberShopAsync(int barberShopId, int ownerId);
}
