using Azure.Identity;
using Azure.Storage.Blobs;

namespace Just4Fit_WorkingStaff.Infrastructure.Services;

public class BlobContainerService
{
    private readonly BlobServiceClient blobServiceClient;
    private readonly BlobContainerClient blobContainerClient;

    public BlobContainerService()
    {
        this.blobServiceClient = new BlobServiceClient
        (
            new Uri("https://Just4Fitstorage.blob.core.windows.net"),
            new DefaultAzureCredential()
        );

        this.blobContainerClient = this.blobServiceClient.GetBlobContainerClient("images");
    }

    public async Task UploadAsync(Stream stream, string path)
    {
        BlobClient blobClient = this.blobContainerClient.GetBlobClient(path);

        using (Stream file = stream)
        {
            await blobClient.UploadAsync(file);
        }
    }
}