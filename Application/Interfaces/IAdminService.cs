using ICorteApi.Application.Services;

namespace ICorteApi.Application.Interfaces;

public interface IAdminService : IService
{
    Task RemoveAllRows(string passphrase, string userEmail, bool? evenMasterAdmin);
    Task PopulateAllInitialTables(string passphrase, string userEmail);
    Task ResetPasswordForSomeUser(string passphrase, string userEmail, string emailToBeReseted);
    Task PopulateWithAppointments(string passphrase, string userEmail, DateOnly? firstDate, DateOnly? limitDate);
    Task DeleteServiceAndRemoveFromAllAppointments(string passphrase, string userEmail, int serviceId);
    Task<FoundUserByAdmin[]> SearchForUsersByName(string userEmail, string? name);
    Task<FoundUserByAdmin[]> GetLastUsers(string userEmail, int? take = null);
}
