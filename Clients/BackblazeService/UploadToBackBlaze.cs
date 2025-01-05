namespace WebApplication3.Clients.BackblazeService;

using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.IO;
using System.Threading.Tasks;

public interface IUploadToBackBlaze
{
    Task<string> Upload(IFormFile file);
}
public class UploadToBackBlaze: IUploadToBackBlaze
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;
    private readonly BackblazeB2Service _backblazeB2Service;

    public UploadToBackBlaze(IAmazonS3 s3Client, IOptions<AwsOptions> awsOptions, BackblazeB2Service backblazeB2Service)
    {
        _s3Client = s3Client;
        _bucketName = awsOptions.Value.BucketName;
        _backblazeB2Service = backblazeB2Service;
    }

    public async Task<string> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return "";
        }

        var fileTransferUtility = new TransferUtility(_s3Client);

        using (var newMemoryStream = new MemoryStream())
        {
            file.CopyTo(newMemoryStream);

            // Upload the file to B2 using S3-compatible API
            await fileTransferUtility.UploadAsync(newMemoryStream, _bucketName, file.FileName);
        }

        // Construct the file URL using the bucket's URL and the uploaded file's name
        var fileUrl = $"https://f004.backblazeb2.com/file/{_bucketName}/{file.FileName}";
        
        var authorizationToken = await _backblazeB2Service.GetAuthorizationToken();
        //for private bucket
        var signedUrlGenerator = new SignedUrlGenerator();
        string signedUrl = signedUrlGenerator.GenerateSignedUrl(fileUrl, authorizationToken);
        return signedUrl;
    }
}
