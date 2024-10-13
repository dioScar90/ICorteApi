namespace ICorteApi.Infraestructure.Interfaces;

public interface IServiceRepository
    : IBaseRepository<Service>
{
    Task<Service[]> GetSpecificServicesByIdsAsync(int[] ids);
    Task<bool> CheckCorrelatedAppointmentsAsync(int id);
    Task<Appointment[]> GetCorrelatedAppointmentsAsync(int id);
}
