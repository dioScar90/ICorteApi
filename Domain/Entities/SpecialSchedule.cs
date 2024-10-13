using ICorteApi.Domain.Base;

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

    public SpecialSchedule(SpecialScheduleDtoCreate dto, int? barberShopId = null)
    {
        Date = dto.Date;
        BarberShopId = barberShopId ?? default;
        
        DayOfWeek = Date.DayOfWeek;
        Notes = string.IsNullOrWhiteSpace(dto.Notes) ? null : dto.Notes;
        OpenTime = dto.OpenTime;
        CloseTime = dto.CloseTime;
        IsClosed = dto is { OpenTime: null, CloseTime: null };
    }
    
    private void UpdateBySpecialScheduleDto(SpecialScheduleDtoUpdate dto, DateTime? utcNow)
    {
        utcNow ??= DateTime.UtcNow;
        
        DayOfWeek = dto.Date.DayOfWeek;
        Notes = string.IsNullOrWhiteSpace(dto.Notes) ? null : dto.Notes;
        OpenTime = dto.OpenTime;
        CloseTime = dto.CloseTime;
        IsClosed = dto is { OpenTime: null, CloseTime: null };

        UpdatedAt = utcNow;
    }
    
    public override void UpdateEntityByDto(IDtoRequest<SpecialSchedule> requestDto, DateTime? utcNow = null)
    {
        switch (requestDto)
        {
            case SpecialScheduleDtoUpdate dto:
                UpdateBySpecialScheduleDto(dto, utcNow);
                break;
            default:
                throw new ArgumentException("Tipo de DTO inválido", nameof(requestDto));
        }
    }
    
    public override SpecialScheduleDtoResponse CreateDto() =>
        new(
            Date,
            BarberShopId,
            DayOfWeek,
            Notes,
            OpenTime,
            CloseTime,
            IsClosed
        );
}
