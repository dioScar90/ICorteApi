namespace ICorteApi.Domain.Interfaces;

public interface IResponseDataModel<T> : IResponseModel where T : class
{
    public T? Data { get; }
}
