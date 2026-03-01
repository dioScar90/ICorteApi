namespace ICorteApi.Presentation.Extensions;

public static partial class StringExtensions
{
    extension(string value)
    {
        public string UcFirst() => char.ToUpper(value[0]) + value[..1].ToLower();

        public string UcWords() => string.Join("", WhitespacesRegex()
            .Split(value)
            .Select(txt => string.IsNullOrWhiteSpace(txt) ? txt : txt.UcFirst())
        );
        
        public string ToCamelCase() =>
            System.Text.Json.JsonNamingPolicy.CamelCase.ConvertName(value);

        public string ToSnakeCase(bool isUpper = false) => isUpper
            ? System.Text.Json.JsonNamingPolicy.SnakeCaseUpper.ConvertName(value)
            : System.Text.Json.JsonNamingPolicy.SnakeCaseLower.ConvertName(value);

        public bool IsVeryEquals(string valueToCompare) =>
            string.Equals(value, valueToCompare, StringComparison.OrdinalIgnoreCase);

        public IEnumerable<string> SplitByWhitespaces(bool emptyToo = false) =>
            WhitespacesRegex()
                .Split(value)
                .Where(txt => emptyToo || !string.IsNullOrWhiteSpace(txt));

        public string RemoveExtraWhitespaces() => string.Join("", WhitespacesRegex()
            .Split(value)
            .Where(txt => !string.IsNullOrWhiteSpace(txt))
        );
            
    }

    [System.Text.RegularExpressions.GeneratedRegex(@"\s+")]
    private static partial System.Text.RegularExpressions.Regex WhitespacesRegex();
}
