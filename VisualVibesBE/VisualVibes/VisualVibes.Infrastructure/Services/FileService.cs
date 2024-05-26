using Azure.Storage;
using Azure.Storage.Blobs;

namespace VisualVibes.Infrastructure.Services
{
    public class FileService
    {
        private readonly string _storageAccount = "visualvibes";
        private readonly string _key = "";
        private readonly BlobContainerClient _filesContainer;

        public FileService()
        {
            var credential = new StorageSharedKeyCredential(_storageAccount, _key);
            var blobUri = $"https://{_storageAccount}.blob.core.windows.net";
            var blobServiceClient = new BlobServiceClient(new Uri(blobUri), credential);
            _filesContainer = blobServiceClient.GetBlobContainerClient("files");
        }
    }
}
