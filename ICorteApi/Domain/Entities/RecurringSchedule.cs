using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Base;

namespace ICorteApi.Domain.Entities;

public sealed class RecurringSchedule : CompositeKeyEntity<RecurringSchedule, DayOfWeek, int>
{
    public DayOfWeek DayOfWeek { get => Id1; init => Id1 = value; }

    public TimeOnly OpenTime { get; private set; }
    public TimeOnly CloseTime { get; private set; }

    public int BarberShopId { get => Id2; init => Id2 = value; }
    public BarberShop BarberShop { get; set; }

    private RecurringSchedule() {}

    public RecurringSchedule(RecurringScheduleDtoRequest dto, int? barberShopId = null)
    {
        DayOfWeek = dto.DayOfWeek;
        BarberShopId = barberShopId ?? default;
        
        OpenTime = dto.OpenTime;
        CloseTime = dto.CloseTime;
    }
    
    private void UpdateByRecurringScheduleDto(RecurringScheduleDtoRequest dto, DateTime? utcNow)
    {
        utcNow ??= DateTime.UtcNow;

        OpenTime = dto.OpenTime;
        CloseTime = dto.CloseTime;

        UpdatedAt = utcNow;
    }
    
    public override void UpdateEntityByDto(IDtoRequest<RecurringSchedule> requestDto, DateTime? utcNow = null)
    {
        switch (requestDto)
        {
            case RecurringScheduleDtoRequest dto:
                UpdateByRecurringScheduleDto(dto, utcNow);
                break;
            default:
                throw new ArgumentException("Tipo de DTO inválido", nameof(requestDto));
        }
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
