using ICorteApi.Domain.Interfaces;
using ICorteApi.Domain.Entities;

namespace ICorteApi.Domain.Errors;

public sealed class AddressErrors : BaseErrors<Address>, IAddressErrors
{
}
