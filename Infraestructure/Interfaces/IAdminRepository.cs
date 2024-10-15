namespace ICorteApi.Infraestructure.Interfaces;

public interface IAdminRepository : IRepository
{
    Task RemoveAllRows(string userEmailToNotBeRemoved);
}
