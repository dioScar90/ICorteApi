using ICorteApi.Application.Validators;
using ICorteApi.Domain.Errors;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Application.Services;

public sealed class BarberShopService(
    AppDbContext context,
    ILogger<BarberShopService> logger,
    UserService userService,
    BarberShopValidator validator,
    BarberShopErrors errors)
    : BaseService<BarberShop>(context, logger)
{
    private readonly UserService _userService = userService;
    private readonly BarberShopValidator _validator = validator;
    private readonly BarberShopErrors _errors = errors;

    public async Task<BarberShopDtoResponse> CreateAsync(BarberShopDtoRequest dto)
    {
        dto.ThrowExceptionIfInvalid(_validator, _errors);

        var ownerId = await _userService.GetMyUserIdAsync()!;
        var barberShop = new BarberShop(dto, ownerId);

        _dbSet.Add(barberShop);
        await SaveChangesAsync();

        return await GetByIdAsync(barberShop.Id);
    }

    public record Includes(bool Address = false, bool Collections = false);
    
    public async Task<BarberShopDtoResponse> GetByIdAsync(int id, Includes? includes = null)
    {
        includes ??= new();

        var query = _dbSet
            .AsNoTracking()
            .Where(b => b.Id == id);

        if (includes.Address)
        {
            query = query
                .AsSplitQuery()
                .Include(b => b.Address);
        }

        if (includes.Collections)
        {
            query = query
                .AsSplitQuery()
                .Include(b => b.RecurringSchedules)
                .Include(b => b.SpecialSchedules)
                .Include(b => b.Services)
                .Include(b => b.Reports);
        }

        var barberShop = await query
            .Select(b => new BarberShopDtoResponse(
                b.Id,
                b.OwnerId,
                b.Name,
                b.Description,
                b.ComercialNumber,
                b.ComercialEmail,
                !includes.Address ? null : new(
                    b.Address.Id,
                    b.Address.BarberShopId,
                    b.Address.Street,
                    b.Address.Number,
                    b.Address.Complement,
                    b.Address.Neighborhood,
                    b.Address.City,
                    b.Address.State,
                    b.Address.PostalCode,
                    b.Address.Country
                ),
                b.RecurringSchedules
                    .Where(_ => includes.Collections)
                    .Select(rs => new RecurringScheduleDtoResponse(
                        rs.DayOfWeek,
                        rs.BarberShopId,
                        rs.OpenTime,
                        rs.CloseTime,
                        rs.IsActive
                    )).ToArray(),
                b.SpecialSchedules
                    .Where(_ => includes.Collections)
                    .Select(ss => new SpecialScheduleDtoResponse(
                        ss.Date,
                        ss.BarberShopId,
                        ss.DayOfWeek,
                        ss.Notes,
                        ss.OpenTime,
                        ss.CloseTime,
                        ss.IsClosed
                    )).ToArray(),
                b.Services
                    .Where(_ => includes.Collections)
                    .Select(s => new ServiceDtoResponse(
                        s.Id,
                        s.BarberShopId,
                        s.BarberShop.Name,
                        s.Name,
                        s.Description,
                        s.Price,
                        s.Duration
                    )).ToArray(),
                b.Reports
                    .Where(_ => includes.Collections)
                    .Select(r => new ReportDtoResponse(
                        r.Id,
                        r.BarberShopId,
                        r.Title,
                        r.Content,
                        r.Rating
                    )).ToArray()
            ))
            .FirstOrDefaultAsync();

        if (barberShop is null)
            _errors.ThrowNotFoundException();

        return barberShop!;
    }

    public async Task<PaginationResponse<AppointmentsByBarberShopDtoResponse>> GetAppointmentsByBarberShopAsync(
        int barberShopId, int page, int pageSize)
    {
        var ownerId = await _userService.GetMyUserIdAsync()!;
        
        var query = _context.Appointments
            .AsNoTracking()
            .AsSplitQuery()
            .Where(a => a.BarberShopId == barberShopId && a.BarberShop.OwnerId == ownerId)
            .OrderByDescending(a => a.CreatedAt)
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
                a.Services.Select(s => new ServiceDtoResponse(
                    s.Id,
                    s.BarberShopId,
                    a.BarberShop.Name,
                    s.Name,
                    s.Description,
                    s.Price,
                    s.Duration
                )).ToArray(),
                a.Status
            ));

        var totalItems = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        page = page > 0 && totalPages > 0 ? Math.Clamp(page, 1, totalPages) : 1;

        var entities = totalItems == 0 ? [] : await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToArrayAsync();

        return new(entities ?? [], totalItems, totalPages, page, pageSize);
    }
    
    public async Task<bool> UpdateAsync(BarberShopDtoRequest dto, int id)
    {
        var barberShop = await _dbSet.FindAsync(id);

        if (barberShop is null)
            _errors.ThrowNotFoundException();

        var ownerId = await _userService.GetMyUserIdAsync()!;
        
        if (barberShop!.OwnerId != ownerId)
            _errors.ThrowBarberShopNotBelongsToOwnerException(ownerId);

        barberShop.UpdateEntity(dto);
        return await SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var barberShop = await _dbSet.FindAsync(id);

        if (barberShop is null)
            _errors.ThrowNotFoundException();

        var ownerId = await _userService.GetMyUserIdAsync()!;

        if (barberShop!.OwnerId != ownerId)
            _errors.ThrowBarberShopNotBelongsToOwnerException(ownerId);
            
        return await DeleteAsync(barberShop);
    }
}
