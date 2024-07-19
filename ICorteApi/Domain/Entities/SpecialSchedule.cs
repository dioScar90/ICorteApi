using ICorteApi.Domain.Base;

namespace ICorteApi.Domain.Entities;

public class SpecialSchedule : BaseEntity
{
    public DateOnly Date { get; set; }
    public string? Notes { get; set; }
    public TimeOnly? OpenTime { get; set; }
    public TimeOnly? CloseTime { get; set; }
    public bool IsClosed { get; set; } = false;
    
    public int BarberShopId { get; set; }
    public BarberShop BarberShop { get; set; }
}
