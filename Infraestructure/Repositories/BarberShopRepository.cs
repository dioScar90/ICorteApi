using ICorteApi.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Infraestructure.Repositories;

public sealed class BarberShopRepository(AppDbContext context, IUserRepository userRepository, IBarberShopErrors errors)
    : BaseRepository<BarberShop>(context), IBarberShopRepository
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IBarberShopErrors _errors = errors;

    public override async Task<BarberShop?> CreateAsync(BarberShop barberShop)
    {
        using var transaction = await BeginTransactionAsync();

        try
        {
            var newbarberShop = await base.CreateAsync(barberShop);

            if (newbarberShop is null)
                _errors.ThrowCreateException();

            await _userRepository.AddUserRoleAsync(UserRole.BarberShop);

            await CommitAsync(transaction);
            return newbarberShop;
        }
        catch (Exception)
        {
            await RollbackAsync(transaction);
            throw;
        }
    }
    
    public async Task<BarberShop?> GetByIdAsync(int barberShopId)
    {
        return await _dbSet.Where(x => x.Id == barberShopId)
            .Include(x => x.Address)
            .FirstOrDefaultAsync();
    }
    
    public async Task<PaginationResponse<AppointmentsByBarberShopDtoResponse>> GetAppointmentsByBarberShopAsync(int barberShopId, int ownerId)
    {
        var appointments = await _context.Appointments
            .AsNoTracking()
            .Where(a => a.BarberShopId == barberShopId && a.BarberShop.OwnerId == ownerId)
            .Select(a => new AppointmentsByBarberShopDtoResponse(
                a.Id,
                new(
                    a.ClientId,
                    a.Client.Profile.FirstName,
                    a.Client.Profile.LastName,
                    a.Client.Profile.FirstName + ' ' + a.Client.Profile.LastName
                ),
                a.BarberShopId,
                a.Date,
                a.StartTime,
                a.TotalDuration,
                a.Notes,
                a.PaymentType,
                a.TotalPrice,
                a.Services.Select(s =>
                    new ServiceDtoResponse(
                        s.Id,
                        s.BarberShopId,
                        s.Name,
                        s.Description,
                        s.Price,
                        s.Duration
                    )
                ).ToArray(),
                a.Status
            ))
            .ToArrayAsync();

        if (appointments?.Length == 0)
            return new([], 0, 0, 1, 0);
        
        return new(appointments!, appointments!.Length, 1, 1, appointments.Length);
    }
    
    public override async Task<bool> DeleteAsync(BarberShop barberShop)
    {
        using var transaction = await BeginTransactionAsync();

        try
        {
            var result = await base.DeleteAsync(barberShop);

            if (!result)
                _errors.ThrowDeleteException();

            await _userRepository.RemoveFromRoleAsync(UserRole.BarberShop);

            await CommitAsync(transaction);
            return result;
        }
        catch (Exception)
        {
            await RollbackAsync(transaction);
            throw;
        }
    }
}
