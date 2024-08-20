namespace ICorteApi.Settings;

public static class SessionTokenManager
{
    private static string _currentToken = GenerateNewToken();

    public static string GetCurrentToken() => _currentToken;

    public static void RegenerateToken()
    {
        _currentToken = GenerateNewToken();
    }

    private static string GenerateNewToken()
    {
        return Guid.NewGuid().ToString(); // Gera um novo token único
    }
}
