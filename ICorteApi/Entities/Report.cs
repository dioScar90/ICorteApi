namespace ICorteApi.Entities;

public class Report : BaseEntity
{
    public string Title { get; set; }
    public string Content { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; }
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