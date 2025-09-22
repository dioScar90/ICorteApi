using ICorteApi.Application.Validators;
using ICorteApi.Domain.Base;
using ICorteApi.Domain.Errors;

namespace ICorteApi.Domain.Entities;

public sealed class RecurringSchedule : CompositeKeyEntity<RecurringSchedule, RecurringScheduleDto, DayOfWeek, int>
{
    public DayOfWeek DayOfWeek { get; init; }

    public TimeOnly OpenTime { get; private set; }
    public TimeOnly CloseTime { get; private set; }

    public int BarberShopId { get; init; }
    public BarberShop BarberShop { get; set; }

    private RecurringSchedule() {}

    public RecurringSchedule(RecurringScheduleDto dto, int? barberShopId = null)
    {
        dto.ThrowExceptionIfInvalid(new RecurringScheduleDtoValidator(), new RecurringScheduleErrors());

        DayOfWeek = dto.DayOfWeek;
        BarberShopId = barberShopId ?? default;
        
        OpenTime = dto.OpenTime;
        CloseTime = dto.CloseTime;
    }
    
    public override void UpdateEntityByDto(RecurringScheduleDto dto, DateTime? utcNow = null)
    {
        utcNow ??= DateTime.UtcNow;

        OpenTime = dto.OpenTime;
        CloseTime = dto.CloseTime;
        IsActive = dto.IsActive;

        UpdatedAt = utcNow;
    }
    
    public override RecurringScheduleDto CreateDto() => new(
        DayOfWeek,
        BarberShopId,
        OpenTime,
        CloseTime,
        IsActive
    );
}
