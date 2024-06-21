namespace BarberAppApi.Entities;

public class Schedule : BaseEntity
{
    public int Id { get; set; }
    public int BarberId { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }

    // Navigation Properties
    public Barber Barber { get; set; }
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
Navigation Properties: Barber
*/
