using ICorteApi.Domain.Base;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Domain.Entities;

public class RecurringSchedule : BaseHardCrudEntity, ICompositeKeyEntity<DayOfWeek, int>
{
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly OpenTime { get; set; }
    public TimeOnly CloseTime { get; set; }

    public int BarberShopId { get; set; }
    public BarberShop BarberShop { get; set; }
    
    public DayOfWeek Key1 => DayOfWeek;
    public int Key2 => BarberShopId;
}
