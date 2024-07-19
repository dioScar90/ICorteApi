using ICorteApi.Domain.Base;

namespace ICorteApi.Domain.Entities;

public class User : BaseUser
{
    public Person? Person { get; set; }
}
