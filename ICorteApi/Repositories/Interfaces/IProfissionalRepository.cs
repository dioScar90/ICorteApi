namespace ICorteApi.Repositories;

public interface IProfissionalRepository : IBaseRepository
{
    Task<IEnumerable<ProfissionalDto>> GetProfissionais();
    Task<Profissional> GetProfissionalById(int id);
    Task<ProfissionalEspecialidade> GetProfissionalEspecialidade(int profissionalId, int especialidadeId);
}
