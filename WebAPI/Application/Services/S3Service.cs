using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;
using WebAPI.Domain.Services;
using WebAPI.Domain.Enum;
using Amazon.S3.Util;

namespace WebAPI.Application.Services
{
    public class S3Service : IS3Service
    {
        public IAmazonS3 _client;
        private readonly ILogger<S3Service> _logger;

        public S3Service(string awsKey, string awsSecret, Amazon.RegionEndpoint region, ILogger<S3Service> _logger)
        {
            this._logger = _logger;
            var credentials = new BasicAWSCredentials(awsKey, awsSecret);
            this._client = new AmazonS3Client(credentials, region);
            _logger.LogInformation("S3Service initialized in region: {Region}", region.DisplayName);
        }

        public async Task<bool> CreateBucketAsync(string _bucketName)
        {
            try
            { 
                var request = new PutBucketRequest
                {
                    BucketName = _bucketName,
                    UseClientRegion = true
                };

                var response = await _client.PutBucketAsync(request);
                _logger.LogInformation("Bucket created successfully: {BucketName}, HTTP Status: {HttpStatus}", _bucketName, response.HttpStatusCode);
                return true;
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error creating bucket: {BucketName}", _bucketName);
                return false;
            }
        }

        public async Task<List<string>> ListBucketsNameAsync()
        {
            List<string> bucketNames = new List<string>();
            try
            {
                _logger.LogInformation("Fetching list of buckets...");

                var response = await _client.ListBucketsAsync();
                response.Buckets.ForEach(response => bucketNames.Add(response.BucketName));
                return bucketNames;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error listing buckets");
                throw;
            }
        }

        public async Task<List<S3Bucket>> ListBucketsMetadataAsync()
        {
            try
            {
                _logger.LogInformation("Fetching list of buckets...");

                var response = await _client.ListBucketsAsync();
                return response.Buckets;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error listing buckets");
                throw;
            }
        }

        [Obsolete]
        public async Task<bool> BucketAlreadyExists(string _bucketName)
        {
            return await _client.DoesS3BucketExistAsync(_bucketName);
        }

        public async Task<bool> PutObjectAsync(string bucketName, string objectName, string objectPath)
        {
            var request = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = objectName,
                FilePath = objectPath
            };

            var response = await _client.PutObjectAsync(request);
            if (!(response.HttpStatusCode == System.Net.HttpStatusCode.OK))
            {
                _logger.LogError($"Could not upload {objectName} to {bucketName}.");
                return false;
            }
            _logger.LogInformation($"Successfully uploaded {objectName} to {bucketName}");
            return true;
        }

    }
}
