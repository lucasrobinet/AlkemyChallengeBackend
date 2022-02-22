﻿using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace AlkemyChallenge.Services
{
    public class AzureFileStorage : IFileStorage
    {
        private readonly string connectionString;
        public AzureFileStorage(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("AzureStorage");
        }
        public async Task DeleteFile(string path, string container)
        {
            if (string.IsNullOrEmpty(path))
                return;



            var client = new BlobContainerClient(connectionString, container);
            await client.CreateIfNotExistsAsync();  
            var file = Path.GetFileName(path);
            var blob = client.GetBlobClient(file);
            await blob.DeleteIfExistsAsync();
        }

        public async Task<string> EditFile(byte[] data, string extension, string container, string path, string contentType)
        {
            await DeleteFile(path, container);
            return await SaveFile(data, extension, container, contentType);
        }
        

        public async Task<string> SaveFile(byte[] data, string extension, string container, string contentType)
        {
            var client = new BlobContainerClient(connectionString, container);
            await client.CreateIfNotExistsAsync();
            client.SetAccessPolicy(PublicAccessType.Blob);

            var fileName = $"{Guid.NewGuid()}{extension}";
            var blob = client.GetBlobClient(fileName); 

            var blobUploadOptions = new BlobUploadOptions();  // cargar el tipo del archivo para saber que se trabaja con una imagen
            var blobHttpHeader = new BlobHttpHeaders();
            blobHttpHeader.ContentType = contentType;
            blobUploadOptions.HttpHeaders = blobHttpHeader;

            await blob.UploadAsync(new BinaryData(data), blobUploadOptions);
            return blob.Uri.ToString();
        }
    }
}
