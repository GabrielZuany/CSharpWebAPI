namespace WebAPI.Domain.Services
{
    public interface IS3Service
    {
        Task CreateBucketAsync(string _bucketName);
        Task ListBucketsAsync();
    }
}
