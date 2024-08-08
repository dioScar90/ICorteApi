using ICorteApi.Domain.Base;
using ICorteApi.Domain.Enums;

namespace ICorteApi.Domain.Entities;

public class Payment : BasePrimaryKeyEntity<int>
{
    public PaymentType PaymentType { get; set; }
    public decimal Amount { get; set; }
    
    public int AppointmentId { get; set; }
    public Appointment Appointment { get; set; }
}
