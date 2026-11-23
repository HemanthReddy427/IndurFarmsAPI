using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace IndurFarmsAPI.Services; // adjust namespace

public class BlobImageStorageService : IImageStorageService
{
    private const long MaxBytes = 5 * 1024 * 1024; // 5 MB

    // SVG is deliberately not allowed: it can carry scripts.
    private static readonly Dictionary<string, string> AllowedTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        [".jpg"] = "image/jpeg",
        [".jpeg"] = "image/jpeg",
        [".png"] = "image/png",
        [".webp"] = "image/webp"
    };

    private readonly BlobContainerClient _container;

    public BlobImageStorageService(IConfiguration config)
    {
        var section = config.GetSection("BlobStorage");
        var containerName = section["ContainerName"] ?? "product-images";
        var connectionString = section["ConnectionString"];

        BlobServiceClient service;
        if (!string.IsNullOrWhiteSpace(connectionString))
        {
            // Simple option: access key / connection string
            service = new BlobServiceClient(connectionString);
        }
        else
        {
            // Recommended in Azure: managed identity (no secret stored anywhere).
            // Locally this uses your `az login` / Visual Studio sign-in.
            var accountUrl = section["AccountUrl"]
                ?? throw new InvalidOperationException("BlobStorage:AccountUrl or BlobStorage:ConnectionString is required.");
            service = new BlobServiceClient(new Uri(accountUrl), new DefaultAzureCredential());
        }

        _container = service.GetBlobContainerClient(containerName);
    }

    public async Task<string> UploadAsync(
    IFormFile file,
    string folder = "products",
    CancellationToken ct = default)
    {
        Log.Information("STEP 1: UploadAsync entered");

        if (file == null)
        {
            Log.Error("STEP 2: Uploaded file is NULL");
            throw new ArgumentException("Image is required.");
        }

        Log.Information("STEP 2: File received. Length: {Length}", file.Length);

        if (file.Length == 0)
            throw new ArgumentException("Image is empty.");

        if (file.Length > MaxBytes)
            throw new ArgumentException("Image exceeds 5 MB.");

        Log.Information("STEP 3: File validation passed");

        var extension = Path.GetExtension(file.FileName);

        Log.Information("STEP 4: File extension: {Extension}", extension);

        if (!AllowedTypes.TryGetValue(extension, out var contentType))
            throw new ArgumentException("Unsupported image format.");

        Log.Information("STEP 5: Image format validated");

        Log.Information(
            "STEP 6: Container initialized: {Initialized}",
            _container != null
        );

        var blobName = $"{folder}/{Guid.NewGuid():N}{extension.ToLowerInvariant()}";

        var blob = _container.GetBlobClient(blobName);

        Log.Information("STEP 7: Blob client created");

        await using var stream = file.OpenReadStream();

        Log.Information("STEP 8: File stream opened");

        await blob.UploadAsync(stream, new BlobUploadOptions
        {
            HttpHeaders = new BlobHttpHeaders
            {
                ContentType = contentType,
                CacheControl = "public, max-age=31536000"
            }
        }, ct);

        Log.Information("STEP 9: Blob uploaded successfully");

        return blob.Uri.ToString();
    }

    public async Task DeleteAsync(string? imageUrl, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(imageUrl) ||
            !Uri.TryCreate(imageUrl, UriKind.Absolute, out var uri))
        {
            return; // null or an old local path such as "/images/abc.jpg"
        }

        if (!string.Equals(uri.Host, _container.Uri.Host, StringComparison.OrdinalIgnoreCase))
            return; // not one of our blobs

        var builder = new BlobUriBuilder(uri);
        if (builder.BlobContainerName != _container.Name || string.IsNullOrEmpty(builder.BlobName))
            return;

        await _container.DeleteBlobIfExistsAsync(builder.BlobName, cancellationToken: ct);
    }
}