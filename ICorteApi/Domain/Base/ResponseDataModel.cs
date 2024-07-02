using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Domain.Base;

public class ResponseDataModel<T> : ResponseModel, IResponseDataModel<T> where T : class
{
    public T Data { get; set; } = null!;
}
