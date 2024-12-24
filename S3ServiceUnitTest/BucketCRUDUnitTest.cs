using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using WebAPI.Application.Services;
using WebAPI.Domain.Services;
using Xunit;

namespace S3ServiceUnitTest
{
    public class BucketCRUDUnitTest
    {
        private readonly IS3Service _s3service;
        private string bucketName = "test-zuzu-webapi-s3dotnet";

        public BucketCRUDUnitTest()
        {
            var serviceProvider = Startup.CreateServiceProvider("C:\\Users\\Gabriel Zuany\\Documents\\GitHub\\CSharpWebAPI\\S3ServiceUnitTest\\testsettings.json");
            // Get the service from the provider
            _s3service = serviceProvider.GetRequiredService<IS3Service>();
        }

        [Fact]
        public async Task CreateBucketAsync_Success_ReturnsTrue()
        {
            if (await _s3service.BucketAlreadyExists(bucketName)) return;
            bool bucketCreated = await _s3service.CreateBucketAsync(bucketName);
            Assert.True(bucketCreated);
        }

        [Fact]
        public async Task ListBucketsNameAsync_ShouldntReturnNullorEmpty()
        {
            List<string> list = await _s3service.ListBucketsNameAsync();
            Assert.NotNull(list);
            Assert.NotEmpty(list);
            Assert.Contains(bucketName, list);
        }
    }
}