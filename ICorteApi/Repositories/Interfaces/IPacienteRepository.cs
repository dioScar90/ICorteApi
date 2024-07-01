namespace ICorteApi.Repositories.Interfaces;

public interface IPacienteRepository : IBaseRepository
{
    Task<IEnumerable<PacienteDto>> GetPacientesAsync();
    Task<Paciente> GetPacientesByIdAsync(int id);
}
