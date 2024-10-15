namespace ICorteApi.Domain.Interfaces;

public interface IAdminErrors : IBaseErrors
{
    void ThrowNotEqualPassphaseException();
    void ThrowNullPassphaseException();
    void ThrowNotEqualEmailException();
    void ThrowNullEmailException();
}
