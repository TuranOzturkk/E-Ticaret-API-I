using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using ETicaretAPI.Application.Abstractions.Storage;
using ETicaretAPI.Application.Abstractions.Storage.Aws;
using ETicaretAPI.Application.Dtos.AwsS3;
using ETicaretAPI.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace ETicaretAPI.Infrastructure.Services.Storage.Aws
{
    public class AwsStorage : Storage, IAwsStorage
    {
        readonly IConfiguration _configuration;
        public readonly IAmazonS3 _amazonS3;
       

        public AwsStorage(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task DeleteAsync(string bucketName, string fileName)
        {
            var credentials = new BasicAWSCredentials((_configuration["Storage:AWS:AccessKey"]), (_configuration["Storage:AWS:SecretKey"]));
            var config = new AmazonS3Config()
            {
                RegionEndpoint = Amazon.RegionEndpoint.EUNorth1
            };
            AmazonS3Client s3Client = new AmazonS3Client(credentials, config);
            await s3Client.DeleteObjectAsync(bucketName, fileName); 
        }

        public List<string> GetFiles(string bucketName)
        {
            var credentials = new BasicAWSCredentials((_configuration["Storage:AWS:AccessKey"]), (_configuration["Storage:AWS:SecretKey"]));
            var config = new AmazonS3Config()
            {
                RegionEndpoint = Amazon.RegionEndpoint.EUNorth1
            };
            AmazonS3Client s3Client = new AmazonS3Client(credentials, config);

            ListObjectsV2Request request = new()
            {
                BucketName = bucketName,
            };

            var response = s3Client.ListObjectsV2Async(request);
            return response.Result.S3Objects.Select(x => x.Key).ToList();

        }
        public async Task<List<AwsUrlDto>> GetFilesAsync(string bucketName , string? prefix)
        {
            var credentials = new BasicAWSCredentials((_configuration["Storage:AWS:AccessKey"]), (_configuration["Storage:AWS:SecretKey"]));
            var config = new AmazonS3Config()
            {
                RegionEndpoint = Amazon.RegionEndpoint.EUNorth1
            };
            AmazonS3Client s3Client = new AmazonS3Client(credentials, config);

            ListObjectsV2Request request = new()
            {
                BucketName = bucketName,
                Prefix = prefix

            };
            ListObjectsV2Response response = await s3Client.ListObjectsV2Async(request);

            List<AwsUrlDto> objectDatas = response.S3Objects.Select(@object =>
            {
                GetPreSignedUrlRequest urlRequest = new()
                {
                    BucketName = bucketName,
                    Key = @object.Key,
                    Expires = DateTime.UtcNow.AddMinutes(15)
                };
                return new AwsUrlDto
                {
                    Name = @object.Key,
                    Url = s3Client.GetPreSignedURL(urlRequest)
                };
            }).ToList();
            return objectDatas;
        }

        public bool HasFile(string bucketName, string fileName)
        {
            var credentials = new BasicAWSCredentials((_configuration["Storage:AWS:AccessKey"]), (_configuration["Storage:AWS:SecretKey"]));
            var config = new AmazonS3Config()
            {
                RegionEndpoint = Amazon.RegionEndpoint.EUNorth1
            };
            AmazonS3Client s3Client = new AmazonS3Client(credentials, config);
            var response = s3Client.GetObjectAsync(bucketName, fileName);
            try
            {
                var sonuc = response.Result?.Key.Any();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        async Task<List<(string fileName, string pathOrContainerName)>> IStorage.UploadAsync(string bucketName, IFormFileCollection files)
        {
            var credentials = new BasicAWSCredentials((_configuration["Storage:AWS:AccessKey"]), (_configuration["Storage:AWS:SecretKey"]));
            var config = new AmazonS3Config()
            {
                RegionEndpoint = Amazon.RegionEndpoint.EUNorth1
            };
            var response = new AwsS3ResponseDto();
            List<(string fileName, string pathOrContainerName)> datas = new();
            foreach (IFormFile file in files)
            {
                string fileNewName = await FileRenameAsync(bucketName, file.Name, HasFile);

                var uploadRequest = new TransferUtilityUploadRequest()
                {

                    InputStream = file.OpenReadStream(),
                    Key = fileNewName,
                    BucketName = bucketName,
                    CannedACL = S3CannedACL.NoACL

                };

                using var client = new AmazonS3Client(credentials, config);
                var transferUtiltiy = new TransferUtility(client);
                await transferUtiltiy.UploadAsync(uploadRequest);

                datas.Add((fileNewName, $"{bucketName}/{fileNewName}"));

                response.StatusCode = 200;
                response.Message = $"{fileNewName} Dosya Yüklendi...";

            }
            return datas;

        }
    }
}
