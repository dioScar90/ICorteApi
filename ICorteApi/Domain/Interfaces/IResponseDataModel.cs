namespace ICorteApi.Domain.Interfaces;

public interface IResponseDataModel<T> : IResponseModel
{
    public T Data { get; set; }
}
