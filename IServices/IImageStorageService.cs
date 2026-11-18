using Microsoft.AspNetCore.Http;

namespace IndurFarmsAPI.Services; // adjust namespace

public interface IImageStorageService
{
    /// <summary>Uploads an image and returns its public URL.</summary>
    Task<string> UploadAsync(IFormFile file, string folder = "products", CancellationToken ct = default);

    /// <summary>Deletes an image by its URL. Ignores null, legacy local paths and foreign URLs.</summary>
    Task DeleteAsync(string? imageUrl, CancellationToken ct = default);
}