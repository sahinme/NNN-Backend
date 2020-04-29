using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Microsoft.Nnn.ApplicationCore.Services.BlobService
{
    public class BlobService : IBlobService
    {
        private readonly IOptions<MyConfig> _config;
        public BlobService(IOptions<MyConfig> config)
        {
            _config = config;
        }
        private string GetImageName()
        {
            return Guid.NewGuid().ToString().Replace("-", "") + ".jpg";
        }
        public async Task<string> InsertFile(IFormFile asset)
        {
            string containerUrl = "nnncontainer";
            string url =
                "DefaultEndpointsProtocol=https;AccountName=edutrostorageaccount;AccountKey=i+39A6nr6OdF54X7z2CIlZvn9aN8j2Bi6Eo7g9HPH1zYj1wdgYKNC1n/QUPNke+ucnomaxsxd7pSFkN0JgdoXw==;EndpointSuffix=core.windows.net";
            try
            {
                var imageName = GetImageName();
                if (CloudStorageAccount.TryParse(url, out CloudStorageAccount storageAccount))
                {
                    CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                    CloudBlobContainer container = blobClient.GetContainerReference(containerUrl);

                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(imageName);

                    await blockBlob.UploadFromStreamAsync(asset.OpenReadStream());

                    return imageName;
                }
                else
                {
                    return "YUKLENNEMEDI";
                }
            }
            catch
            {
                return "YUKLENEMEDI";
            }
        }

        public static string GetImageUrl(string path)
        {
            return "https://edutrostorageaccount.blob.core.windows.net/nnncontainer/" + path;
        }
    }
}