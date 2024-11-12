using ICorteApi.Domain.Errors;

namespace ICorteApi.Presentation.Exceptions;

public abstract class BaseException(string message, params Error[]? errors) : Exception(message)
{
    public IDictionary<string, string[]> Errors { get; } = GetDictionaryByArrayOfErrors(errors);

    private static Dictionary<string, string[]> GetDictionaryByArrayOfErrors(Error[]? errors = null)
    {
        if (errors is not { Length: > 0 })
            return [];
            
        return errors
            .GroupBy(e => e.Code)
            .ToDictionary(
                e => GetCamelCaseFormat(e.Key),
                e => e.Select(e => e.Description).ToArray()
            );
    }
    
    private static string GetCamelCaseFormat(string value) =>
        string.Join(".", value.Split(".").Select(sub => char.ToLower(sub[0]) + sub[1..]));
}
