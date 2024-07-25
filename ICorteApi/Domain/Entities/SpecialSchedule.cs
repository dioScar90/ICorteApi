using ICorteApi.Domain.Base;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Domain.Entities;

public class SpecialSchedule : BaseHardCrudEntity, ICompositeKeyEntity<DateOnly, int>
{
    public DateOnly Date { get; set; }
    public string? Notes { get; set; }
    public TimeOnly? OpenTime { get; set; }
    public TimeOnly? CloseTime { get; set; }
    public bool IsClosed { get; set; } = false;

    public int BarberShopId { get; set; }
    public BarberShop BarberShop { get; set; }
    
    public DateOnly Key1 => Date;
    public int Key2 => BarberShopId;
}
