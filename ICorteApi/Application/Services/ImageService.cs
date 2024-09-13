using ICorteApi.Domain.Interfaces;
using System.Drawing;
using System.Drawing.Imaging;

namespace ICorteApi.Application.Services;

public class ImageService : IImageService
{
    private readonly string _imageDirectory;
    private readonly ILogger<ImageService> _logger;
    private readonly IImageErrors _errors;

    public ImageService(IWebHostEnvironment environment, ILogger<ImageService> logger, IImageErrors errors)
    {
        _imageDirectory = Path.Combine(environment.WebRootPath, "uploads");
        _logger = logger;
        _errors = errors;

        if (!Directory.Exists(_imageDirectory))
            Directory.CreateDirectory(_imageDirectory);
    }

    private void CheckAndThrowIfFileIsNotImage(IFormFile file)
    {
        string mimeType = file.ContentType.ToLowerInvariant();

        if (!mimeType.StartsWith("image/"))
            _errors.ThrowFileIsNotImageException();
    }

    private void CheckAndThrowIfNotAllowedImageExtension(IFormFile file)
    {
        HashSet<string> allowedExtensions = [".jpg", ".jpeg", ".png", ".gif"];
        string extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!allowedExtensions.Contains(extension))
            _errors.ThrowNotAllowedImageExtensionException(allowedExtensions);
    }

    private async Task<byte[]> ResizeImageIfNecessaryAsync(byte[] imageBytes, int maxSizeInKb = 500)
    {
        const int maxDimension = 500; // Limite das dimensões (largura/altura) da imagem
        int maxBytes = maxSizeInKb * 1024; // Converter KB para Bytes

        using (var inputStream = new MemoryStream(imageBytes))
        {
            using (var originalImage = Image.FromStream(inputStream))
            {
                if (imageBytes.Length > maxBytes || originalImage.Width > maxDimension || originalImage.Height > maxDimension)
                {
                    var ratioX = (double)maxDimension / originalImage.Width;
                    var ratioY = (double)maxDimension / originalImage.Height;
                    var ratio = Math.Min(ratioX, ratioY);

                    int newWidth = (int)(originalImage.Width * ratio);
                    int newHeight = (int)(originalImage.Height * ratio);

                    using (var resizedImage = new Bitmap(newWidth, newHeight))
                    {
                        using (var graphics = Graphics.FromImage(resizedImage))
                        {
                            graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                            graphics.DrawImage(originalImage, 0, 0, newWidth, newHeight);

                            using (var outputStream = new MemoryStream())
                            {
                                var encoder = ImageCodecInfo.GetImageDecoders().FirstOrDefault(c => c.FormatID == ImageFormat.Jpeg.Guid);
                                var encoderParams = new EncoderParameters(1)
                                {
                                    // Param = new[] { new EncoderParameter(Encoder.Quality, 85L) } // 85L é uma boa qualidade para balancear tamanho e definição
                                    Param = [new(Encoder.Quality, 85L)] // 85L é uma boa qualidade para balancear tamanho e definição
                                };

                                resizedImage.Save(outputStream, encoder, encoderParams);
                                return outputStream.ToArray();
                            }
                        }
                    }
                }
                else
                {
                    return imageBytes; // A imagem já está dentro dos limites aceitáveis
                }
            }
        }
    }
    
    public async Task<string> SaveImageAsync(IFormFile image)
    {
        if (image == null || image.Length == 0)
            throw new ArgumentException("A imagem fornecida é inválida.");

        string fileName = GenerateUniqueFileName(image.FileName);
        string filePath = Path.Combine(_imageDirectory, fileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await image.CopyToAsync(fileStream);
        }

        string message = $"Imagem salva em: {filePath}";
        _logger.LogInformation(message);

        return $"/uploads/{fileName}"; // URL relativo da imagem
    }

    public async Task<byte[]> GetImageAsync(string imageUrl)
    {
        string filePath = Path.Combine(_imageDirectory, Path.GetFileName(imageUrl));

        if (!File.Exists(filePath))
            throw new FileNotFoundException("A imagem solicitada não foi encontrada.");

        return await File.ReadAllBytesAsync(filePath);
    }

    public async Task<string> UpdateImageAsync(IFormFile newImage, string currentImageUrl)
    {
        DeleteImageAsync(currentImageUrl);
        return await SaveImageAsync(newImage);
    }

    private static string GenerateUniqueFileName(string originalFileName)
    {
        string fileExtension = Path.GetExtension(originalFileName);
        return $"{Guid.NewGuid()}{fileExtension}";
    }

    public bool DeleteImageAsync(string imageUrl)
    {
        string filePath = Path.Combine(_imageDirectory, Path.GetFileName(imageUrl));

        if (!File.Exists(filePath))
            return false;
            
        File.Delete(filePath);
        _logger.LogInformation("Imagem deletada em: {FilePath}", filePath);
        return true;
    }
}
