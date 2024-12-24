using WebAPI.Domain.Enum;

namespace WebAPI.Domain.Services
{
    public interface IS3Service
    {
        Task<bool> CreateBucketAsync(string _bucketName);
        Task ListBucketsAsync();
        Task<bool> PutObjectAsync(string bucketName, string objectName, string objectPath);
    }
}
