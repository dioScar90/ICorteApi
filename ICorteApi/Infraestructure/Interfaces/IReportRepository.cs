namespace ICorteApi.Infraestructure.Interfaces;

public interface IReportRepository
    : IBaseRepository<Report>
{
    Task<Report?> GetReportWithBarberShopByIdAsync(int id);
}
