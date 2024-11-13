using FluentValidation;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Services;

public sealed class RecurringScheduleService(
    IRecurringScheduleRepository repository,
    IValidator<RecurringScheduleDtoCreate> createValidator,
    IValidator<RecurringScheduleDtoUpdate> updateValidator,
    IRecurringScheduleErrors errors)
    : BaseService<RecurringSchedule>(repository), IRecurringScheduleService
{
    private readonly IValidator<RecurringScheduleDtoCreate> _createValidator = createValidator;
    private readonly IValidator<RecurringScheduleDtoUpdate> _updateValidator = updateValidator;
    private readonly IRecurringScheduleErrors _errors = errors;

    public async Task<RecurringScheduleDtoResponse> CreateAsync(RecurringScheduleDtoCreate dto, int barberShopId)
    {
        dto.CheckAndThrowExceptionIfInvalid(_createValidator, _errors);
        var schedule = new RecurringSchedule(dto, barberShopId);
        return (await CreateAsync(schedule))!.CreateDto();
    }

    public async Task<RecurringScheduleDtoResponse> GetByIdAsync(DayOfWeek dayOfWeek, int barberShopId)
    {
        var schedule = await base.GetByIdAsync(dayOfWeek, barberShopId);

        if (schedule is null)
            _errors.ThrowNotFoundException();
        
        return schedule!.CreateDto();
    }
    
    public async Task<PaginationResponse<RecurringScheduleDtoResponse>> GetAllAsync(int? page, int? pageSize, int barberShopId)
    {
        var response = await GetAllAsync(new(page, pageSize, x => x.BarberShopId == barberShopId, new(x => x.DayOfWeek)));
        
        return new(
            [..response.Items.Select(service => service.CreateDto())],
            response.TotalItems,
            response.TotalPages,
            response.Page,
            response.PageSize
        );
    }
    
    public async Task<bool> UpdateAsync(RecurringScheduleDtoUpdate dto, DayOfWeek dayOfWeek, int barberShopId)
    {
        dto.CheckAndThrowExceptionIfInvalid(_updateValidator, _errors);

        var schedule = await base.GetByIdAsync(dayOfWeek, barberShopId);

        if (schedule is null)
            _errors.ThrowNotFoundException();

        if (schedule!.BarberShopId != barberShopId)
            _errors.ThrowRecurringScheduleNotBelongsToBarberShopException(barberShopId);

        schedule.UpdateEntityByDto(dto);
        return await UpdateAsync(schedule);
    }

    public async Task<bool> DeleteAsync(DayOfWeek dayOfWeek, int barberShopId)
    {
        var schedule = await base.GetByIdAsync(dayOfWeek, barberShopId);

        if (schedule is null)
            _errors.ThrowNotFoundException();

        if (schedule!.BarberShopId != barberShopId)
            _errors.ThrowRecurringScheduleNotBelongsToBarberShopException(barberShopId);
        
        return await DeleteAsync(schedule);
    }
}
