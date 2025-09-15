using ICorteApi.Domain.Base;

namespace ICorteApi.Domain.Entities;

public sealed class RecurringSchedule : CompositeKeyEntity<RecurringSchedule>
{
    public DayOfWeek DayOfWeek { get; init; }

    public TimeOnly OpenTime { get; private set; }
    public TimeOnly CloseTime { get; private set; }

    public int BarberShopId { get; init; }
    public BarberShop BarberShop { get; set; }

    private RecurringSchedule() {}

    public RecurringSchedule(RecurringScheduleDtoCreate dto, int? barberShopId = null)
    {
        DayOfWeek = dto.DayOfWeek;
        BarberShopId = barberShopId ?? default;
        
        OpenTime = dto.OpenTime;
        CloseTime = dto.CloseTime;
    }
    
    private void UpdateByRecurringScheduleDto(RecurringScheduleDtoUpdate dto, DateTime? utcNow)
    {
        utcNow ??= DateTime.UtcNow;

        OpenTime = dto.OpenTime;
        CloseTime = dto.CloseTime;
        IsActive = dto.IsActive;

        UpdatedAt = utcNow;
    }

    public override void UpdateEntityByDto(IDtoRequest<RecurringSchedule> requestDto, DateTime? utcNow = null)
    {
        Action action = requestDto switch
        {
            RecurringScheduleDtoUpdate dto => () => UpdateByRecurringScheduleDto(dto, utcNow),
            _ => () => throw new ArgumentException("Tipo de DTO inválido", nameof(requestDto))
        };

        action();
    }
    
    public override RecurringScheduleDtoResponse CreateDto() =>
        new(
            DayOfWeek,
            BarberShopId,
            OpenTime,
            CloseTime,
            IsActive
        );
}
