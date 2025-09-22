namespace ICorteApi.Presentation.Extensions;

public static partial class StringExtensions
{
    public static string UcFirst(this string value) => char.ToUpper(value[0]) + value[..1].ToLower();

    public static string UcWords(this string value) => string.Join("", WhitespacesRegex()
        .Split(value)
        .Select(txt => string.IsNullOrWhiteSpace(txt) ? txt : txt.UcFirst())
    );
    
    public static string ToCamelCase(this string value) =>
        System.Text.Json.JsonNamingPolicy.CamelCase.ConvertName(value);

    public static string ToSnakeCase(this string value, bool isUpper = false) => isUpper
        ? System.Text.Json.JsonNamingPolicy.SnakeCaseUpper.ConvertName(value)
        : System.Text.Json.JsonNamingPolicy.SnakeCaseLower.ConvertName(value);

    public static bool IsVeryEquals(this string value, string valueToCompare) =>
        string.Equals(value, valueToCompare, StringComparison.OrdinalIgnoreCase);

    public static IEnumerable<string> SplitByWhitespaces(this string value, bool emptyToo = false) =>
        WhitespacesRegex().Split(value).Where(txt => emptyToo || !string.IsNullOrWhiteSpace(txt));
        
    [System.Text.RegularExpressions.GeneratedRegex(@"\s+")]
    private static partial System.Text.RegularExpressions.Regex WhitespacesRegex();
}
