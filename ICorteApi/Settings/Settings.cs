using System.Text;

namespace ICorteApi.Settings;

public static class Settings
{
    public const string SECRET = "6339997ef80b4e82b6ee6bdb9bb0960d";
    public static byte[] GetEncodingKey() => Encoding.ASCII.GetBytes(SECRET);
}
