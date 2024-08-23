using ICorteApi.Domain.Entities;
using ICorteApi.Infraestructure.Context;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Infraestructure.Repositories;

public sealed class ReportRepository(AppDbContext context)
    : BaseRepository<Report>(context), IReportRepository
{
}
