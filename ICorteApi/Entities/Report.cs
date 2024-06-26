namespace ICorteApi.Entities;

public class Report : BaseEntity
{
    public string? Title { get; set; }
    public string? Content { get; set; }
    public int Rating { get; set; }
    
    public int PersonId { get; set; }
    public Person Person { get; set; }

    public int BarberId { get; set; }
    public Barber Barber { get; set; }
}

/*
CHAT GPT

Report: Representa os relatórios.

Id (int)
Title (string)
Content (string)
CreatedAt (DateTime)
UpdatedAt (DateTime)
IsActive (bool)
*/