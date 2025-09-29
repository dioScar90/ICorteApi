using ICorteApi.Application.Services;
using ICorteApi.Domain.Errors;

namespace ICorteApi.Presentation.Exceptions;

public abstract class BaseException<TEntity>(
    ILogger<IService<TEntity>> logger,
    string message,
    params Error[]? errors)
    : Exception(message), ICustomException
        where TEntity : class, IBaseTableEntity
{
    private readonly ILogger<IService<TEntity>> _logger = logger;

    public abstract int HttpStatusCode { get; }
    public abstract string Title { get; }
    public IDictionary<string, string[]> Errors { get; } = GetErrosAsDictionary(errors);

    private static Dictionary<string, string[]> GetErrosAsDictionary(Error[]? errors = null)
    {
        errors ??= [];

        return errors
            .GroupBy(e => e.Code)
            .ToDictionary(
                e => e.Key.ToCamelCase(),
                e => e.Select(e => e.Description).ToArray()
            );
    }
    
    public static string GetTitle<TException>(TException exception)
        where TException : ICustomException
    {
        var nameWithoutException = exception.GetType().Name.Replace("Exception", "");
        
        var name = nameWithoutException.ToSnakeCase()
            .Split("_")
            .Select(text => char.ToUpper(text[0]) + text[1..])
            .Append("Error");

        return string.Join(" ", name);
    }

    public void LogWarning()
    {
        //
    }
}

public interface ICustomException
{
    IDictionary<string, string[]> Errors { get; }
    int HttpStatusCode { get; }
    string Title { get; }
    void LogWarning();
}
