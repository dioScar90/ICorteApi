namespace ICorteApi.Entities;

public class RecurringSchedule : BaseEntity
{
    public DayOfWeek DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }

    public int BarberShopId { get; set; }
    public BarberShop BarberShop { get; set; }

    public int BarberId { get; set; }
    public Person Barber { get; set; }
}
