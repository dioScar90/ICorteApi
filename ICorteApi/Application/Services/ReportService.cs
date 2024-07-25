using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public class ReportService(IReportRepository reportRepository)
    : BasePrimaryKeyService<Report, int>(reportRepository), IReportService
{
}
