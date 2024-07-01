namespace ICorteApi.Repositories;

public interface IEspecialidadeRepository : IBaseRepository
{
    Task<IEnumerable<EspecialidadeDto>> GetEspecialidades();
    Task<Especialidade> GetEspecialidadeById(int id);
}
