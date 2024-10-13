namespace ICorteApi.Domain.Interfaces;

public interface IReportErrors : IBaseErrors<Report>
{
    void ThrowReportNotBelongsToClientException(int clientId);
    void ThrowReportNotBelongsToBarberShopException(int barberShopId);
}
