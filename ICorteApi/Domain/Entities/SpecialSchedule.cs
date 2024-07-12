using ICorteApi.Domain.Base;

namespace ICorteApi.Domain.Entities;

public class SpecialSchedule : BaseEntity
{
    public int BarberShopId { get; set; }
    public BarberShop BarberShop { get; set; }
    public DateOnly Date { get; set; }
    public TimeSpan? OpenTime { get; set; }
    public TimeSpan? CloseTime { get; set; }
    public bool IsClosed { get; set; } = false;
    public string? Notes { get; set; }
}
