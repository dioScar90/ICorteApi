using ICorteApi.Domain.Entities;

namespace ICorteApi.Domain.Interfaces;

public interface IUserErrors : IBaseErrors<User>
{
    void ThrowRegisterNotCompletedException();
    void ThrowUserAlreadyCreatedException();
}
