using ICorteApi.Domain.Errors;

namespace ICorteApi.Presentation.Exceptions;

public sealed class AddressValidationException : BadRequestException
{
    public AddressValidationException(Error[] errors)
        : base("Address validation failed", errors)
    {}

    public AddressValidationException(IDictionary<string, string[]> errors)
        : base("Address validation failed", errors)
    {}
}
