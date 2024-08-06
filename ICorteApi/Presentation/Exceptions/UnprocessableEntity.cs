using ICorteApi.Domain.Errors;

namespace ICorteApi.Presentation.Exceptions;

public class UnprocessableEntity : Exception
{
    public UnprocessableEntity(string message, Error[]? errors = null)
        : base(message)
    {
        Errors = GetDictionaryByArrayOfErrors(errors);
    }
    
    public UnprocessableEntity(string message, IDictionary<string, string[]> errors)
        : base(message)
    {
        Errors = errors;
    }
    
    public IDictionary<string, string[]> Errors { get; }

    private static Dictionary<string, string[]> GetDictionaryByArrayOfErrors(Error[]? errors = null)
    {
        if (errors is null)
            return [];
        
        return errors
            .GroupBy(e => e.Code)
            .ToDictionary(
                e => e.Key,
                e => e.Select(e => e.Description).ToArray()
            );
    }
}
