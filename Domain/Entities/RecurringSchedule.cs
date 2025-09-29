namespace ICorteApi.Domain.Entities;

public sealed class RecurringSchedule : CompositeKeyEntity<RecurringSchedule>
{
    public DayOfWeek DayOfWeek { get; init; }

    public TimeOnly OpenTime { get; private set; }
    public TimeOnly CloseTime { get; private set; }

    public int BarberShopId { get; init; }
    public BarberShop BarberShop { get; set; }

    private RecurringSchedule() {}

    public RecurringSchedule(RecurringScheduleDtoRequest dto, int? barberShopId = null)
    {
        DayOfWeek = dto.DayOfWeek;
        BarberShopId = barberShopId ?? default;
        
        OpenTime = dto.OpenTime;
        CloseTime = dto.CloseTime;
    }
    
    public void UpdateEntity(RecurringScheduleDtoRequest dto, DateTime? utcNow = null)
    {
        utcNow ??= DateTime.UtcNow;

        OpenTime = dto.OpenTime;
        CloseTime = dto.CloseTime;
        IsActive = dto.IsActive;

        UpdatedAt = utcNow;
    }
    
    public RecurringScheduleDtoResponse CreateDto() => new(
        DayOfWeek,
        BarberShopId,
        OpenTime,
        CloseTime,
        IsActive
    );
}
