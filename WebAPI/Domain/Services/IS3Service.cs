using Amazon.S3.Model;
using WebAPI.Domain.Enum;

namespace WebAPI.Domain.Services
{
    public interface IS3Service
    {
        Task<bool> CreateBucketAsync(string _bucketName);
        Task<List<string>> ListBucketsNameAsync();
        Task<List<S3Bucket>> ListBucketsMetadataAsync();
        Task<bool> BucketAlreadyExists(string _bucketName);
        Task<bool> PutObjectAsync(string bucketName, string objectName, string objectPath);
    }
}
