using System.Text.Json.Serialization;
using ICorteApi.Domain.Base;
using ICorteApi.Domain.Utils;

namespace ICorteApi.Domain.Entities;

public class OperatingSchedule : BaseCrudEntity
{
    public DayOfWeek DayOfWeek { get; set; }

    public int BarberShopId { get; set; }
    public BarberShop BarberShop { get; set; }

    [JsonConverter(typeof(TimeSpanConverter))]
    public TimeSpan OpenTime { get; set; }

    [JsonConverter(typeof(TimeSpanConverter))]
    public TimeSpan CloseTime { get; set; }
}
