using FluentValidation;
using ICorteApi.Domain.Base;
using ICorteApi.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Application.Services;

public sealed class BarberShopService(
    AppDbContext context,
    IBarberShopErrors errors)
    : BaseService<BarberShop, BarberShopDto>(context), IBarberShopService
{
    private readonly IBarberShopErrors _errors = errors;

    public async Task<BarberShopDto> CreateAsync(BarberShopDto dto, int ownerId)
    {
        var barberShop = new BarberShop(dto, ownerId);
        return (await CreateAsync(barberShop))!.CreateDto();
    }
    
    public async Task<BarberShopDto> GetByIdAsync(int id)
    {
        var barberShop = await base.GetByIdAsync(id);

        if (barberShop is null)
            _errors.ThrowNotFoundException();
            
        return barberShop!.CreateDto();
    }
    
    private async Task<PaginationResponse<AppointmentsByBarberShopDto>> GetAppointmentsByBarberShopAsync(
        int barberShopId, int ownerId,
        PaginationProperties<AppointmentsByBarberShopDto> props)
    {
        var query = _context.Appointments
            .AsNoTracking()
            .Where(a => a.BarberShopId == barberShopId && a.BarberShop.OwnerId == ownerId)
            .OrderByDescending(a => a.CreatedAt)
            .Select(a => new AppointmentsByBarberShopDto(
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
                    new ServiceDto(
                        s.Id,
                        s.BarberShopId,
                        a.BarberShop.Name,
                        s.Name,
                        s.Description,
                        s.Price,
                        s.Duration
                    )
                ).ToArray(),
                a.Status
            ));

        var totalItems = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalItems / (double)props.PageSize);
        
        int page = props.Page > 0 && totalPages > 0 ? Math.Clamp(props.Page, 1, totalPages) : 0;
        
        if (totalItems == 0)
            return new([], totalItems, totalPages, page, props.PageSize);
        
        var entities = await query
            .Skip((page - 1) * props.PageSize)
            .Take(props.PageSize)
            .ToArrayAsync();
        
        return new(entities ?? [], totalItems, totalPages, page, props.PageSize);
    }
    
    public async Task<PaginationResponse<AppointmentsByBarberShopDto>> GetAppointmentsByBarberShopAsync(int barberShopId, int ownerId, int? page, int? pageSize)
    {
        return await GetAppointmentsByBarberShopAsync(
            barberShopId, ownerId,
            new(page, pageSize, x => 1 == 1, new(x => x.Id)));
    }
    
    public async Task<bool> UpdateAsync(BarberShopDto dto, int id, int ownerId)
    {
        var barberShop = await GetByIdAsync(x => x.Id == id, x => x.Address);

        if (barberShop is null)
            _errors.ThrowNotFoundException();

        if (barberShop!.OwnerId != ownerId)
            _errors.ThrowBarberShopNotBelongsToOwnerException(ownerId);
            
        barberShop!.UpdateEntityByDto(dto);
        return await SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id, int ownerId)
    {
        var barberShop = await GetByIdAsync(
            x => x.Id == id,
            x => new BarberShopDto(
                x.Id,
                x.OwnerId,
                x.Name,
                x.Description,
                x.ComercialNumber,
                x.ComercialEmail,
                x.Address == null ? null : new AddressDto(
                    x.Address.Id,
                    x.Address.BarberShopId,
                    x.Address.Street,
                    x.Address.Number,
                    x.Address.Complement,
                    x.Address.Neighborhood,
                    x.Address.City,
                    x.Address.State,
                    x.Address.PostalCode,
                    x.Address.Country
                ),
                x.RecurringSchedules?.Select(rs => new RecurringScheduleDto(
                    rs.DayOfWeek,
                    rs.BarberShopId,
                    rs.OpenTime,
                    rs.CloseTime,
                    rs.IsActive
                )).ToArray(),
                x.SpecialSchedules?.Select(ss => new SpecialScheduleDto(
                    ss.Date,
                    ss.BarberShopId,
                    ss.DayOfWeek,
                    ss.Notes,
                    ss.OpenTime,
                    ss.CloseTime,
                    ss.IsClosed
                )).ToArray(),
                x.Services?.Select(s => new ServiceDto(
                    s.Id,
                    s.BarberShopId,
                    s.BarberShop.Name,
                    s.Name,
                    s.Description,
                    s.Price,
                    s.Duration
                )).ToArray(),
                x.Reports?.Select(r => new ReportDto(
                    r.Id,
                    r.BarberShopId,
                    r.Title,
                    r.Content,
                    r.Rating
                )).ToArray()
            ),
            x => x.Address,
            x => x.RecurringSchedules,
            x => x.SpecialSchedules,
            x => x.Services);

        if (barberShop is null)
            _errors.ThrowNotFoundException();

        if (barberShop!.OwnerId != ownerId)
            _errors.ThrowBarberShopNotBelongsToOwnerException(ownerId);
            
        return await DeleteAsync(barberShop);
    }
}
