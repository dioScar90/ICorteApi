using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Domain.Base;

public class ResponseModel : IResponseModel
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public int StatusCode { get; set; }
}
