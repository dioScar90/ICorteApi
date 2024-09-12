using System.Globalization;
using ICorteApi.Domain.Entities;
using ICorteApi.Infraestructure.Context;
using ICorteApi.Infraestructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Infraestructure.Repositories;

public sealed class RecurringScheduleRepository(AppDbContext context)
    : BaseRepository<RecurringSchedule>(context), IRecurringScheduleRepository
{
}
