using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Domain.Errors;

public sealed class ServiceErrors : BaseErrors<Service>, IServiceErrors
{
}
