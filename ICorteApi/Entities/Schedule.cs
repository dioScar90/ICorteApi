namespace ICorteApi.Entities;

public class Schedule : BaseEntity
{
    public DayOfWeek DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public bool IsAvailable { get; set; } // Indica se o horário está disponível

    public int BarberId { get; set; }
    public BarberShop BarberShop { get; set; }

    public Appointment Appointment { get; set; }
}

/*
CHAT GPT

Schedule: Representa os horários disponíveis dos barbeiros.

Id (int)
BarberId (int) - Foreign Key
DayOfWeek (int) // 0 = Sunday, 1 = Monday, etc.
StartTime (TimeSpan)
EndTime (TimeSpan)
CreatedAt (DateTime)
UpdatedAt (DateTime)
IsActive (bool)
Navigation Properties: BarberShop
*/
