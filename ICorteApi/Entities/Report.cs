namespace ICorteApi.Entities;

public class Report : BaseEntity
{
    public override int Id { get; set; }
    public int Title { get; set; }
    public string Content { get; set; }
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