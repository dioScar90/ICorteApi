using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Domain.Errors;

public sealed class AdminErrors : IAdminErrors
{
    public void ThrowNotEqualEmailException()
    {
        throw new ConflictException("mail");
    }

    public void ThrowNotEqualPassphaseException()
    {
        throw new ConflictException("assphr");
    }

    public void ThrowNullEmailException()
    {
        throw new ConflictException("numaill");
    }

    public void ThrowNullPassphaseException()
    {
        throw new ConflictException("nulassphr");
    }
}
