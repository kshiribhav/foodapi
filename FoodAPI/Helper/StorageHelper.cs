using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using System.Threading.Tasks;

namespace FoodAPI.Helper
{
    public class StorageHelper
    {
        private string blobConnectionString;
        CloudStorageAccount blobStorageAcc;
        CloudBlobClient blobClient;
        
        public string BlobConnectionString
        {
            get { return blobConnectionString; }
            set
            {
                blobConnectionString = value;
                blobStorageAcc = CloudStorageAccount.Parse(blobConnectionString);
            }
        }

        public async Task<string> UploadFileBlobAsync(string filePath, string containerName)
        {
            blobClient = blobStorageAcc.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(containerName);
            await container.CreateIfNotExistsAsync();

            BlobContainerPermissions containerPermissions = new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Container
            };
            await container.SetPermissionsAsync(containerPermissions);

            var fileName = Path.GetFileName(filePath);
            var blob = container.GetBlockBlobReference(fileName);
            await blob.DeleteIfExistsAsync();

            await blob.UploadFromFileAsync(filePath);
            return blob.Uri.AbsoluteUri;
        }
    }
}
