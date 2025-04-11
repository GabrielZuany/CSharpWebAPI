using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Domain.Services;

namespace S3ServiceUnitTest
{
    public class ObjectCRUDUnitTest
    {
        private readonly IS3Service _s3service;
        private string bucketName = "test-zuzu-webapi-s3dotnet";

        public ObjectCRUDUnitTest()
        {
            var serviceProvider = Startup.CreateServiceProvider("C:\\Users\\Gabriel Zuany\\Documents\\GitHub\\CSharpWebAPI\\S3ServiceUnitTest\\testsettings.json");
            // Get the service from the provider
            _s3service = serviceProvider.GetRequiredService<IS3Service>();
        }

        [Fact]
        public async Task PutObjectAsync_Success_ReturnsTrue()
        {
            if (await _s3service.BucketAlreadyExists(bucketName) == false){
                await _s3service.CreateBucketAsync(bucketName);
            }
            string objectName = "testsettings.example.json";
            string objectPath = "C:\\Users\\Gabriel Zuany\\Documents\\GitHub\\CSharpWebAPI\\S3ServiceUnitTest\\testsettings.json";

            bool result = await _s3service.PutObjectAsync(bucketName, objectName, objectPath);
            Assert.True(result);
        }

        [Fact]
        public async Task PutObjectAsync_Prefix_Success_ReturnsTrue()
        {
            if (await _s3service.BucketAlreadyExists(bucketName) == false)
            {
                await _s3service.CreateBucketAsync(bucketName);
            }
            string objectName = "someprefix/testsettings.example.json";
            string objectPath = "C:\\Users\\Gabriel Zuany\\Documents\\GitHub\\CSharpWebAPI\\S3ServiceUnitTest\\testsettings.json";

            bool result = await _s3service.PutObjectAsync(bucketName, objectName, objectPath);
            Assert.True(result);
        }
    }
}
