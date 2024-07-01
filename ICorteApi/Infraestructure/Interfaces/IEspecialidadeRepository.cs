namespace ICorteApi.Infraestructure.Interfaces;

public interface IEspecialidadeRepository : IBaseRepository
{
    Task<IEnumerable<EspecialidadeDto>> GetEspecialidades();
    Task<Especialidade> GetEspecialidadeById(int id);
}
