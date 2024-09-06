namespace ICorteApi.Domain.Interfaces;

public interface IImageErrors : IBaseErrors
{
    void ThrowFileIsNotImageException();
    void ThrowNotAllowedImageExtensionException(HashSet<string> allowedExtensions);
    void ThrowImageTooLargeException(int maxSizeInBytes);
}
