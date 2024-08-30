using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Presentation.Exceptions;

namespace ICorteApi.Domain.Errors;

public sealed class ReportErrors : BaseErrors<Report>, IReportErrors
{
    public void ThrowReportNotBelongsToClientException(int clientId)
    {
        string message = $"{_entity} não pertence ao cliente \"{clientId}\" informado";
        throw new ConflictException(message);
    }
    
    public void ThrowReportNotBelongsToBarberShopException(int barberShopId)
    {
        string message = $"{_entity} não pertence à barbearia \"{barberShopId}\" informada";
        throw new ConflictException(message);
    }
}
