using ICorteApi.Application.Validators;
using ICorteApi.Domain.Errors;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Application.Services;

public sealed class ServiceService(
    AppDbContext context,
    ILogger<ServiceService> logger,
    ServiceValidator validator,
    ServiceErrors errors)
    : BaseService<Service>(context, logger)
{
    private readonly ServiceValidator _validator = validator;
    private readonly ServiceErrors _errors = errors;

    public async Task<ServiceDtoResponse> CreateAsync(ServiceDtoRequest dto)
    {
        dto.ThrowExceptionIfInvalid(_validator, _errors, _logger);

        var service = new Service(dto, dto.BarberShopId);

        _dbSet.Add(service);
        await SaveChangesAsync();

        return await GetByIdAsync(service.Id, service.BarberShopId);
    }

    public record Includes(bool Collections = false);
    
    public async Task<ServiceDtoResponse> GetByIdAsync(int id, int barberShopId, Includes? includes = null)
    {
        includes ??= new();

        var query = _dbSet
            .AsNoTracking()
            .Where(s => s.Id == id);
            
        if (includes.Collections)
        {
            query = query
                .AsSplitQuery()
                .Include(s => s.Appointments);
        }

        var service = await query
            .Select(s => new ServiceDtoResponse(
                s.Id,
                s.BarberShopId,
                s.BarberShop.Name,
                s.Name,
                s.Description,
                s.Price,
                s.Duration
            ))
            .FirstOrDefaultAsync();

        if (service is null)
            _errors.ThrowNotFoundException();

        if (service!.BarberShopId != barberShopId)
            _errors.ThrowServiceNotBelongsToBarberShopException(barberShopId);

        return service;
    }
    
    public async Task<PaginationResponse<ServiceDtoResponse>> GetAllAsync(int? page, int? pageSize, int barberShopId)
    {
        return await GetAllAsync<ServiceDtoResponse>(
            new(
                page,
                pageSize,
                x => x.BarberShopId == barberShopId,
                new(x => x.Name),
                s => new(
                    s.Id,
                    s.BarberShopId,
                    s.BarberShop.Name,
                    s.Name,
                    s.Description,
                    s.Price,
                    s.Duration
                )
            )
        );
    }
    
    public async Task<bool> UpdateAsync(ServiceDtoRequest dto, int id, int barberShopId)
    {
        dto.ThrowExceptionIfInvalid(_validator, _errors, _logger);

        var service = await _dbSet.FindAsync(id);

        if (service is null)
            _errors.ThrowNotFoundException();

        if (service!.BarberShopId != barberShopId)
            _errors.ThrowServiceNotBelongsToBarberShopException(barberShopId);

        service.UpdateEntity(dto);
        return await SaveChangesAsync();
    }

    public async Task<Service[]> GetSpecificServicesByIdsAsync(int[] ids)
    {
        var hashIds = ids.ToHashSet();
        return await _dbSet.Where(x => hashIds.Contains(x.Id)).ToArrayAsync();
    }

    private async Task<bool> CheckCorrelatedAppointmentsAsync(int id) =>
        await _dbSet.AnyAsync(s => s.Id == id && s.Appointments.Any());

    private async Task<Appointment[]> GetCorrelatedAppointmentsAsync(int id) =>
        await _dbSet
            .Where(s => s.Id == id)
            .SelectMany(s => s.Appointments)
            .ToArrayAsync();
            
    public async Task<bool> DeleteAsync(int id, int barberShopId, bool forceDelete = false)
    {
        var service = await _dbSet.FindAsync(id);

        if (service is null)
            _errors.ThrowNotFoundException();

        if (service!.BarberShopId != barberShopId)
            _errors.ThrowServiceNotBelongsToBarberShopException(barberShopId);

        var thereAreAppointments = !forceDelete && await CheckCorrelatedAppointmentsAsync(service.Id);

        if (thereAreAppointments)
        {
            var appointments = await GetCorrelatedAppointmentsAsync(service.Id);
            var dates = appointments.Select(a => a.Date);

            _errors.ThrowThereAreStillAppointmentsException([.. dates]);
        }

        return await DeleteAsync(service);
    }
}
