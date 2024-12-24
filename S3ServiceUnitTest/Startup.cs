using Amazon;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Runtime;
using WebAPI.Application.Services;
using WebAPI.Domain.Services;

namespace S3ServiceUnitTest
{
    public static class Startup
    {
        public static ServiceProvider CreateServiceProvider(string configFilePath)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(configFilePath)
                .Build();

            var services = new ServiceCollection();
            services.AddLogging(builder => builder.AddConsole());

            // Register your S3Service with DI
            services.AddTransient<IS3Service, S3Service>(provider =>
            {
                var s3Settings = configuration.GetSection("S3Settings").Get<S3Settings>();
                return new S3Service(
                    s3Settings.AwsKey,
                    s3Settings.AwsSecret,
                    RegionEndpoint.GetBySystemName(s3Settings.Region),
                    provider.GetRequiredService<ILogger<S3Service>>()
                );
            });

            return services.BuildServiceProvider();
        }


        public class S3Settings
        {
            public string AwsKey { get; set; }
            public string AwsSecret { get; set; }
            public string Region { get; set; }
        }
    }
}
