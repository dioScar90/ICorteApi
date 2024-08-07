using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public sealed class ReportService(IReportRepository repository)
    : BasePrimaryKeyService<Report, int>(repository), IReportService
{
}
