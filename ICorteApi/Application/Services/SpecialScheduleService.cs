using FluentValidation;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Services;

public sealed class SpecialScheduleService(
    ISpecialScheduleRepository repository,
    IValidator<SpecialScheduleDtoCreate> createValidator,
    IValidator<SpecialScheduleDtoUpdate> updateValidator,
    ISpecialScheduleErrors errors)
    : BaseService<SpecialSchedule>(repository), ISpecialScheduleService
{
    private readonly IValidator<SpecialScheduleDtoCreate> _createValidator = createValidator;
    private readonly IValidator<SpecialScheduleDtoUpdate> _updateValidator = updateValidator;
    private readonly ISpecialScheduleErrors _errors = errors;

    public async Task<SpecialScheduleDtoResponse> CreateAsync(SpecialScheduleDtoCreate dto, int barberShopId)
    {
        dto.CheckAndThrowExceptionIfInvalid(_createValidator, _errors);
        var schedule = new SpecialSchedule(dto, barberShopId);
        return (await CreateAsync(schedule))!.CreateDto();
    }

    public async Task<SpecialScheduleDtoResponse> GetByIdAsync(DateOnly date, int barberShopId)
    {
        return (await base.GetByIdAsync(date, barberShopId))!.CreateDto();
    }
    
    public async Task<PaginationResponse<SpecialScheduleDtoResponse>> GetAllAsync(int? page, int? pageSize, int barberShopId)
    {
        var response = await GetAllAsync(new(page, pageSize, x => x.BarberShopId == barberShopId));
        
        return new(
            [..response.Data.Select(schedule => schedule.CreateDto())],
            response.TotalItems,
            response.TotalPages,
            response.Page,
            response.PageSize
        );
    }

    public async Task<bool> UpdateAsync(SpecialScheduleDtoUpdate dto, DateOnly date, int barberShopId)
    {
        dto.CheckAndThrowExceptionIfInvalid(_updateValidator, _errors);

        var schedule = await base.GetByIdAsync(date, barberShopId);

        if (schedule is null)
            _errors.ThrowNotFoundException();

        if (schedule!.BarberShopId != barberShopId)
            _errors.ThrowSpecialScheduleNotBelongsToBarberShopException(barberShopId);
        
        schedule.UpdateEntityByDto(dto);
        return await UpdateAsync(schedule);
    }

    public async Task<bool> DeleteAsync(DateOnly date, int barberShopId)
    {
        var schedule = await base.GetByIdAsync(date, barberShopId);

        if (schedule is null)
            _errors.ThrowNotFoundException();

        if (schedule!.BarberShopId != barberShopId)
            _errors.ThrowSpecialScheduleNotBelongsToBarberShopException(barberShopId);
        
        return await DeleteAsync(schedule);
    }
}
