
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Infraestructure.Repositories;

public sealed class AdminRepository(AppDbContext context) : IAdminRepository
{
    private readonly AppDbContext _context = context;

    public async Task RemoveAllRows(string userEmailToNotBeRemoved)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            await _context.Messages.IgnoreQueryFilters().ExecuteDeleteAsync();
            await _context.Reports.IgnoreQueryFilters().ExecuteDeleteAsync();

            await _context.Appointments.IgnoreQueryFilters().ExecuteDeleteAsync();
            await _context.Services.IgnoreQueryFilters().ExecuteDeleteAsync();
            
            await _context.SpecialSchedules.IgnoreQueryFilters().ExecuteDeleteAsync();
            await _context.RecurringSchedules.IgnoreQueryFilters().ExecuteDeleteAsync();
            await _context.Addresses.IgnoreQueryFilters().ExecuteDeleteAsync();
            await _context.BarberShops.IgnoreQueryFilters().ExecuteDeleteAsync();
            
            await _context.Profiles
                .IgnoreQueryFilters()
                .Where(p => p.User.Email != userEmailToNotBeRemoved)
                .ExecuteDeleteAsync();

            await _context.Users
                .IgnoreQueryFilters()
                .Where(u => u.Email != userEmailToNotBeRemoved)
                .ExecuteDeleteAsync();
                
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
