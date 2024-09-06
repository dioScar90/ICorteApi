using ICorteApi.Domain.Interfaces;
using ICorteApi.Presentation.Exceptions;

namespace ICorteApi.Domain.Errors;

public sealed class ImageErrors : IImageErrors
{
    public void ThrowFileIsNotImageException()
    {
        throw new BadRequestException("Arquivo enviado não é uma imagem");
    }

    public void ThrowNotAllowedImageExtensionException(HashSet<string> allowedExtensions)
    {
        var permitted = string.Join(", ", allowedExtensions);
        string message = $"Imagem deve estar num desses formatos: {permitted}";

        throw new BadRequestException(message);
    }

    private static (string, string) GetFormattedMaxSize(float maxSizeInBytes)
    {
        const int MAX_BYTES = 1_024;
        const int MAX_KYLO_BYTES = MAX_BYTES * MAX_BYTES;
        const int MAX_MEGA_BYTES = MAX_KYLO_BYTES * MAX_BYTES;

        const string BYTES = "B";
        const string KYLO_BYTES = "KB";
        const string MEGA_BYTES = "MB";

        string getMega() => (maxSizeInBytes / MAX_MEGA_BYTES).ToString("0.0");
        string getKylo() => (maxSizeInBytes / MAX_MEGA_BYTES).ToString("0.0");
        string getByte() => maxSizeInBytes.ToString("0");
        
        return maxSizeInBytes switch
        {
            > MAX_MEGA_BYTES => (getMega(), MEGA_BYTES),
            > MAX_KYLO_BYTES => (getKylo(), KYLO_BYTES),
            _ => (getByte(), BYTES)
        };
    }

    public void ThrowImageTooLargeException(int maxSizeInBytes)
    {
        var (formattedMax, formattedText) = GetFormattedMaxSize(maxSizeInBytes);
        string message = $"Imagem enviada é maior que o máximo permitido de {formattedMax} {formattedText}";
        
        throw new BadRequestException(message);
    }
}
