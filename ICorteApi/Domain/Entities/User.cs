using ICorteApi.Domain.Base;

namespace ICorteApi.Domain.Entities;

// Usuário
public class User : BaseUser
{
    public Person? Person { get; set; }
}
