using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Domain.Errors;

public sealed class PersonErrors : BaseErrors<Person>, IPersonErrors
{
}
