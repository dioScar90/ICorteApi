namespace ICorteApi.Application.Interfaces;

public interface IAdminService : IService
{
    Task RemoveAllRows(string passphrase, string userEmail);
}
