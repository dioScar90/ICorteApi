namespace ICorteApi.Application.Interfaces;

public interface IAdminService : IService
{
    Task RemoveAllRows(string passphrase, string userEmail, bool? evenMasterAdmin);
    Task PopulateAllInitialTables(string passphrase, string userEmail);
    Task PopulateWithAppointments(string passphrase, string userEmail);
    Task ResetPasswordForSomeUser(string passphrase, string userEmail, string emailToBeReseted);
}
