using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;

namespace CampusConnect.Web.Services
{
    public interface IImageStorageService
    {
        Task<string?> UploadImageAsync(IFormFile file, string containerName);
    }

    public class BlobImageStorageService : IImageStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IConfiguration _configuration;

        public BlobImageStorageService(IConfiguration configuration)
        {
            _configuration = configuration;
            var connectionString = configuration.GetConnectionString("AzureBlobStorage");
            if (!string.IsNullOrEmpty(connectionString))
            {
                _blobServiceClient = new BlobServiceClient(connectionString);
            }
        }

        public async Task<string?> UploadImageAsync(IFormFile file, string containerName)
        {
            if (file == null || file.Length == 0) return null;

            var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp", "image/gif" };
            if (!allowedTypes.Contains(file.ContentType.ToLower())) return null;

            if (file.Length > 5 * 1024 * 1024) return null;

            if (_blobServiceClient == null)
            {
                return await UploadLocalAsync(file, containerName);
            }

            var container = _blobServiceClient.GetBlobContainerClient(containerName);
            await container.CreateIfNotExistsAsync(PublicAccessType.Blob);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var blobClient = container.GetBlobClient(fileName);

            using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = file.ContentType });

            return blobClient.Uri.ToString();
        }

        private async Task<string> UploadLocalAsync(IFormFile file, string containerName)
        {
            var webRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", containerName);
            Directory.CreateDirectory(webRoot);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(webRoot, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/uploads/{containerName}/{fileName}";
        }
    }
}
