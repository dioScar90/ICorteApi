using ICorteApi.Domain.Base;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Domain.Entities;

public class User : BasePrimaryKeyUserEntity<int>
{
    public Person? Person { get; set; }
    
    // public int Key => Id;
}
