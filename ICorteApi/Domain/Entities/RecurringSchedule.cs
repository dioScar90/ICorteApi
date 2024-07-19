using ICorteApi.Domain.Base;

namespace ICorteApi.Domain.Entities;

public class RecurringSchedule : BaseHardCrudEntity
{
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly OpenTime { get; set; }
    public TimeOnly CloseTime { get; set; }

    public int BarberShopId { get; set; }
    public BarberShop BarberShop { get; set; }
}
