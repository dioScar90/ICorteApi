using ICorteApi.Domain.Errors;

namespace ICorteApi.Presentation.Exceptions;

public abstract class BaseException(string message, params Error[]? errors) : Exception(message)
{
    public IDictionary<string, string[]> Errors { get; } = GetErrosAsDictionary(errors);

    private static Dictionary<string, string[]> GetErrosAsDictionary(Error[]? errors = null)
    {
        if (errors is null)
            return [];
            
        if (errors!.Length == 0)
            return [];
            
        return errors
            .GroupBy(e => e.Code)
            .ToDictionary(
                e => GetCamelCaseFormat(e.Key),
                e => e.Select(e => e.Description).ToArray()
            );
    }
    
    private static string GetCamelCaseFormat(string value) =>
        System.Text.Json.JsonNamingPolicy.CamelCase.ConvertName(value);
}
