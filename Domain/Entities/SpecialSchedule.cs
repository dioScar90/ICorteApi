namespace ICorteApi.Domain.Entities;

public sealed class SpecialSchedule : CompositeKeyEntity<SpecialSchedule>
{
    public DateOnly Date { get; init; }
    public DayOfWeek DayOfWeek { get; set; }
    public string? Notes { get; set; }
    public TimeOnly? OpenTime { get; set; }
    public TimeOnly? CloseTime { get; set; }
    public bool IsClosed { get; set; }

    public int BarberShopId { get; init; }
    public BarberShop BarberShop { get; set; }

    private SpecialSchedule() {}

    public SpecialSchedule(SpecialScheduleDtoRequest dto, int? barberShopId = null)
    {
        Date = dto.Date;
        BarberShopId = barberShopId ?? default;
        
        DayOfWeek = Date.DayOfWeek;
        Notes = string.IsNullOrWhiteSpace(dto.Notes) ? null : dto.Notes;
        OpenTime = dto.OpenTime;
        CloseTime = dto.CloseTime;
        IsClosed = dto is { OpenTime: null, CloseTime: null };
    }
    
    public void UpdateEntity(SpecialScheduleDtoRequest dto, DateTime? utcNow = null)
    {
        utcNow ??= DateTime.UtcNow;

        DayOfWeek = dto.Date.DayOfWeek;
        Notes = string.IsNullOrWhiteSpace(dto.Notes) ? null : dto.Notes;
        OpenTime = dto.OpenTime;
        CloseTime = dto.CloseTime;
        IsClosed = dto is { OpenTime: null, CloseTime: null };

        UpdatedAt = utcNow;
    }
    
    public SpecialScheduleDtoResponse CreateDto() => new(
        Date,
        BarberShopId,
        DayOfWeek,
        Notes,
        OpenTime,
        CloseTime,
        IsClosed
    );
}
