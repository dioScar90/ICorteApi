using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Base;

namespace ICorteApi.Domain.Entities;

public sealed class SpecialSchedule : CompositeKeyEntity<SpecialSchedule>
{
    public DateOnly Date { get; init; }
    public string? Notes { get; set; }
    public TimeOnly? OpenTime { get; set; }
    public TimeOnly? CloseTime { get; set; }
    public bool IsClosed { get; set; } = false;

    public int BarberShopId { get; init; }
    public BarberShop BarberShop { get; set; }

    private SpecialSchedule() {}

    public SpecialSchedule(SpecialScheduleDtoRequest dto, int? barberShopId = null)
    {
        Date = dto.Date;
        BarberShopId = barberShopId ?? default;
        
        OpenTime = dto.OpenTime;
        CloseTime = dto.CloseTime;
    }
    
    private void UpdateBySpecialScheduleDto(SpecialScheduleDtoRequest dto, DateTime? utcNow)
    {
        utcNow ??= DateTime.UtcNow;

        OpenTime = dto.OpenTime;
        CloseTime = dto.CloseTime;

        UpdatedAt = utcNow;
    }
    
    public override void UpdateEntityByDto(IDtoRequest<SpecialSchedule> requestDto, DateTime? utcNow = null)
    {
        switch (requestDto)
        {
            case SpecialScheduleDtoRequest dto:
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
            Notes,
            OpenTime,
            CloseTime,
            IsClosed
        );
}
