using Microsoft.AspNetCore.Identity;

namespace ICorteApi.Domain.Interfaces;

public interface IAdminErrors : IBaseErrors
{
    void ThrowNotEqualPassphaseException();
    void ThrowNullPassphaseException();
    void ThrowNotEqualEmailException();
    void ThrowNullEmailException();
    void ThrowThereIsNobodyToBeDeletedException();
    void ThrowThereAreTooManyPeopleHereException();
    void ThrowThereIsNobodyHereToSetAppointmentsException();
    void ThrowThereAreTooManyAppointmentsHereException();
    void ThrowUserDoesNotExistException(string email);
    void ThrowResetPasswordException(string email, params IdentityError[] identityErrors);
}
