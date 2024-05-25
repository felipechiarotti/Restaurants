using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.Extensions.Options;
using Restaurants.Domain.Interfaces;
using Restaurants.Infrastructure.Configuration;

namespace Restaurants.Infrastructure.Storage
{
    public class BlobStorageService(IOptions<BlobStorageSettings> options) : IBlobStorageService
    {
        private readonly BlobStorageSettings _blobStorageSettings = options.Value;

        public async Task<string> UploadToBlobAsync(Stream data, string fileName)
        {
            var blobServiceClient = new BlobServiceClient(_blobStorageSettings.ConnectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(_blobStorageSettings.LogosContainerName);

            var blobClient = containerClient.GetBlobClient(fileName);
            await blobClient.DeleteIfExistsAsync();
            await blobClient.UploadAsync(data);
            
            return blobClient.Uri.ToString();
        }

        public string? GetBlobSasUrl(string? blobUrl)
        {
            if (blobUrl is null)
                return null;

            var sasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = _blobStorageSettings.LogosContainerName,
                Resource = "b",
                StartsOn = DateTimeOffset.UtcNow,
                ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(_blobStorageSettings.SasExpiration),
                BlobName = GetBlobNameFromUrl(blobUrl)
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            var blobServiceClient = new BlobServiceClient(_blobStorageSettings.ConnectionString);

            var sasToken = sasBuilder.ToSasQueryParameters(new StorageSharedKeyCredential(blobServiceClient.AccountName, _blobStorageSettings.AccountKey))
                .ToString();

            return $"{blobUrl}?{sasToken}";
        }

        private string GetBlobNameFromUrl(string blobUrl)
        {
            var uri = new Uri(blobUrl);
            return uri.Segments.Last();
        }
    }
}
