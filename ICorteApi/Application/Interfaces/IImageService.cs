namespace ICorteApi.Application.Interfaces;

public interface IImageService //: IService<BarberShop>
{
    Task<string> SaveImageAsync(IFormFile image);
    Task<byte[]> GetImageAsync(string imageUrl);
    Task<string> UpdateImageAsync(IFormFile newImage, string currentImageUrl);
    bool DeleteImageAsync(string imageUrl);
}
