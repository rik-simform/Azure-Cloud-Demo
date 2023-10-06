using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyFromOneContainerToAnother
{
    public class AzureUtility
    {
        public async Task UploadImage(BlobContainerClient containerClient, string blobName, string localImagePath)
        {
            // Get a reference to the blob
            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            // Open the image file for reading
            using (FileStream fs = File.OpenRead(localImagePath))
            {
                // Upload the image to the blob
                await blobClient.UploadAsync(fs, true);
            }
        }

        public async Task DownloadImage(BlobContainerClient containerClient, string blobName, string destinationPath)
        {
            // Get a reference to the blob
            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            // Check if the blob exists
            if (await blobClient.ExistsAsync())
            {
                // Download the blob to the specified destination
                BlobDownloadInfo blobDownloadInfo = await blobClient.DownloadAsync();

                // Open the destination file for writing
                using (FileStream fs = File.OpenWrite(destinationPath))
                {
                    // Copy the blob content to the file
                    await blobDownloadInfo.Content.CopyToAsync(fs);
                }
            }
            else
            {
                Console.WriteLine("Blob does not exist.");
            }
        }
        public async Task CopyFromContainer(BlobClient sourceBlobClient, BlobContainerClient destinationContainerClient, string blobName)
        {
            if (await sourceBlobClient.ExistsAsync())
            {
                // Get a reference to the destination blob
                BlobClient destinationBlobClient = destinationContainerClient.GetBlobClient(blobName);

                // Start the blob copy operation
                var copyInfo = await destinationBlobClient.StartCopyFromUriAsync(sourceBlobClient.Uri);

                // Monitor the copy operation if needed
                Console.WriteLine($"Copying blob: {copyInfo.HasCompleted}");
            }
            else
            {
                Console.WriteLine("Source blob does not exist.");
            }
        }
    }

}
