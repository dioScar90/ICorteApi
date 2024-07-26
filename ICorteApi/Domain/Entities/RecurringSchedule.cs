using ICorteApi.Domain.Base;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Domain.Entities;

public class RecurringSchedule : CompositeKeyEntity<DayOfWeek, int>
{
    // public DayOfWeek DayOfWeek => Id1;
    public DayOfWeek DayOfWeek { get => Id1; set => Id1 = value; }

    public TimeOnly OpenTime { get; set; }
    public TimeOnly CloseTime { get; set; }

    // public int BarberShopId => Id2;
    public int BarberShopId { get => Id2; set => Id2 = value; }
    public BarberShop BarberShop { get; set; }
    
    // public DayOfWeek Key1 => DayOfWeek;
    // public int Key2 => BarberShopId;
}
