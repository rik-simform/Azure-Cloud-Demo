// See https://aka.ms/new-console-template for more information
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using CopyFromOneContainerToAnother;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Reflection;

Console.WriteLine("Demo for Azure Upload and Download and Copy blob from one container to another!");

string sourceConnectionString = "DefaultEndpointsProtocol=https;AccountName=rikeshtestingpoc;AccountKey=8ciQvT4xn461vS+hmvrl59J5p4u7UX+zGUZXdvZpZQiHLTxwGH6g8dKUy7VMBemAoyoNxTmypQin+AStfkUgaA==;EndpointSuffix=core.windows.net";
string destinationConnectionString = sourceConnectionString;

string sourceContainerName = "testingpoc1";
string destinationContainerName = "testingpoc2";
string blobName = "file_example.jpg";

// Create BlobServiceClient instances using the connection strings
BlobServiceClient sourceServiceClient = new BlobServiceClient(sourceConnectionString);
BlobServiceClient destinationServiceClient = new BlobServiceClient(destinationConnectionString);

// Get a reference to the source blob container
BlobContainerClient sourceContainerClient = sourceServiceClient.GetBlobContainerClient(sourceContainerName);

// Get a reference to the destination blob container
BlobContainerClient destinationContainerClient = destinationServiceClient.GetBlobContainerClient(destinationContainerName);

// Get a reference to the source blob
BlobClient sourceBlobClient = sourceContainerClient.GetBlobClient(blobName);

// create an object of Azure utility class.
AzureUtility azureUtility = new AzureUtility();

await azureUtility.CopyFromContainer(sourceBlobClient, destinationContainerClient, blobName);

Console.WriteLine("---------------------------------Uploading and downloading of file----------------------------------");

string blobNameForUpload = "XYZ";
// Create a BlobServiceClient using the connection string
BlobServiceClient serviceClient = new BlobServiceClient(sourceConnectionString);

// Get a reference to the container
BlobContainerClient containerClient = serviceClient.GetBlobContainerClient(sourceContainerName);

// Path to the local image file for upload and download
string localImagePath = "C:/Users/rikesh/Downloads/illustration6.svg";

Console.WriteLine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
string currDirPath =  Path.GetDirectoryName(Environment.CurrentDirectory.Split(new String[] { "bin" }, StringSplitOptions.None)[0]);

string downloadDirPath = Path.Combine(currDirPath + "\\Downloads\\XYZ"); 

AzureUtility azureUtility1 = new AzureUtility();
await azureUtility1.UploadImage(containerClient, blobNameForUpload, localImagePath);
await azureUtility1.DownloadImage(containerClient, blobNameForUpload, downloadDirPath);

