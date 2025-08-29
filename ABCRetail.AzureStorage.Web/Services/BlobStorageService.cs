using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ABCRetail.AzureStorage.Web.Services
{
    public class BlobStorageService
    {
        private readonly BlobContainerClient _containerClient;

        public BlobStorageService(string connectionString)
        {
            // Container name must match the one in appsettings.json
            _containerClient = new BlobContainerClient(connectionString, "product-images");
            _containerClient.CreateIfNotExists();
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            var blobClient = _containerClient.GetBlobClient(fileName);

            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream);
            }

            // Return the full URL to the uploaded blob
            return blobClient.Uri.ToString();
        }
    }
}
