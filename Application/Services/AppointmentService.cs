using ICorteApi.Application.Validators;
using ICorteApi.Domain.Errors;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Application.Services;

public sealed class AppointmentService(
    AppDbContext context,
    UserService userService,
    AppointmentValidator validator,
    ServiceService serviceService,
    AppointmentErrors errors)
    : BaseService<Appointment, AppointmentDtoResponse, AppointmentDtoRequest>(context, userService)
{
    private readonly ServiceService _serviceService = serviceService;
    private readonly AppointmentValidator _validator = validator;
    private readonly AppointmentErrors _errors = errors;

    private static bool IsServicesFromUniqueBarberShopId(Service[] services)
    {
        var ids = services.Select(s => s.BarberShopId).ToHashSet();
        return ids.Count == 1;
    }
    
    public override async Task<AppointmentDtoResponse> CreateAsync(AppointmentDtoRequest dto)
    {
        dto.ThrowExceptionIfInvalid(_validator, _errors);

        if (dto.Services.Length == 0)
            _errors.ThrowEmptyServicesException();

        var services = await GetSpecificServicesByIdsAsync([.. dto.Services.Select(s => s.Id)]);

        if (!IsServicesFromUniqueBarberShopId(services))
            _errors.ThrowNotBarberShopIdsUniqueFromServicesException();
            
        dto = dto with { ClientId = await _userService.GetMyUserIdAsync() };
        var appointment = new Appointment(dto, services);

        _dbSet.Add(appointment);
        await _context.SaveChangesAsync();

        return await GetByIdAsync(appointment.Id);
    }
    
    private async Task<Service[]> GetSpecificServicesByIdsAsync(int[] ids)
    {
        return await _serviceService.GetSpecificServicesByIdsAsync(ids);
    }

    public record Includes(bool Collections = false);

    public async Task<AppointmentDtoResponse> GetByIdAsync(int id, Includes? includes = null)
    {
        includes ??= new();

        var query = _dbSet
            .AsNoTracking()
            .Where(a => a.Id == id);
            
        if (includes.Collections)
        {
            query = query
                .AsSplitQuery()
                .Include(a => a.Services);
        }

        var appointment = await query
            .Select(a => new AppointmentDtoResponse(
                a.Id,
                a.ClientId,
                a.BarberShopId,
                a.Date,
                a.StartTime,
                a.TotalDuration,
                a.Notes,
                a.PaymentType,
                a.TotalPrice,
                a.Services
                    .Where(_ => includes.Collections)
                    .Select(s => new ServiceDtoResponse(
                        s.Id,
                        s.BarberShopId,
                        s.BarberShop.Name,
                        s.Name,
                        s.Description,
                        s.Price,
                        s.Duration
                    ))
                    .ToArray(),
                a.Status
            ))
            .FirstOrDefaultAsync();

        if (appointment is null)
            _errors.ThrowNotFoundException();

        return appointment!;
    }

    public async Task<PaginationResponse<AppointmentDtoResponse>> GetAllAsync(
        int? page, int? pageSize)
    {
        var clientId = await _userService.GetMyUserIdAsync()!;

        return await GetAllAsync(
            new(
                page,
                pageSize,
                x => x.ClientId == clientId,
                new(x => x.Date),
                a => new AppointmentDtoResponse(
                    a.Id,
                    a.ClientId,
                    a.BarberShopId,
                    a.Date,
                    a.StartTime,
                    a.TotalDuration,
                    a.Notes,
                    a.PaymentType,
                    a.TotalPrice,
                    a.Services
                        .Select(s => new ServiceDtoResponse(
                            s.Id,
                            s.BarberShopId,
                            s.BarberShop.Name,
                            s.Name,
                            s.Description,
                            s.Price,
                            s.Duration
                        ))
                        .ToArray(),
                    a.Status
                )
            )
        );
    }

    private async Task UpdateAppointmentServicesAsync(Appointment appointment, AppointmentDtoRequest dto)
    {
        var currentServiceIds = appointment.Services.Select(s => s.Id).ToArray();
        int[] serviceIds = [.. dto.Services.Select(s => s.Id)];

        var serviceIdsToRemove = currentServiceIds.Except(serviceIds).ToArray();

        var serviceIdsToAdd = serviceIds.Except(currentServiceIds).ToArray();
        var servicesToAdd = await GetSpecificServicesByIdsAsync(serviceIdsToAdd);

        if (!IsServicesFromUniqueBarberShopId(servicesToAdd))
            _errors.ThrowNotBarberShopIdsUniqueFromServicesException();

        if (serviceIdsToRemove.Length > 0)
            appointment.RemoveServicesByIds(serviceIdsToRemove);

        if (serviceIdsToAdd.Length > 0)
            appointment.AddServices(servicesToAdd);
    }

    public async Task<bool> UpdateAsync(AppointmentDtoRequest dto, int id)
    {
        dto.ThrowExceptionIfInvalid(_validator, _errors);
        
        var appointment = await _dbSet
            .Include(a => a.Services)
            .Where(a => a.Id == id)
            .FirstAsync();

        if (appointment is null)
            _errors.ThrowNotFoundException();
            
        dto = dto with { ClientId = await _userService.GetMyUserIdAsync() };

        if (appointment!.ClientId != dto.ClientId)
            _errors.ThrowAppointmentNotBelongsToClientException(dto.ClientId);
            
        await UpdateAppointmentServicesAsync(appointment, dto);

        return await UpdateAsync(appointment, dto);
    }

    public async Task<bool> UpdatePaymentTypeAsync(AppointmentPaymentTypeDtoUpdateRequest dto, int id)
    {
        var appointment = await _dbSet.FindAsync(id);

        if (appointment is null)
            _errors.ThrowNotFoundException();
            
        dto = dto with { ClientId = await _userService.GetMyUserIdAsync() };
            
        if (appointment!.ClientId != dto.ClientId)
            _errors.ThrowAppointmentNotBelongsToClientException(dto.ClientId);
            
        appointment.UpdatePaymentType(dto);
        return await SaveChangesAsync();
    }
    
    public async Task<bool> DeleteAsync(int id)
    {
        var appointment = await _dbSet.FindAsync(id);

        if (appointment is null)
            _errors.ThrowNotFoundException();
            
        var clientId = await _userService.GetMyUserIdAsync()!;
        
        if (appointment!.ClientId != clientId)
            _errors.ThrowAppointmentNotBelongsToClientException(clientId);

        return await DeleteAsync(appointment);
    }
}
