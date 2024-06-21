namespace ICorteApi.Entities;

public class Payment : BaseEntity
{
    public override int Id { get; set; }
    public int AppointmentId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string PaymentMethod { get; set; }

    // Navigation Properties
    public Appointment Appointment { get; set; }
}

/*
CHAT GPT

Payment: Representa os pagamentos.

Id (int)
AppointmentId (int) - Foreign Key
Amount (decimal)
PaymentDate (DateTime)
PaymentMethod (string)
CreatedAt (DateTime)
UpdatedAt (DateTime)
IsActive (bool)
Navigation Properties: Appointment
*/