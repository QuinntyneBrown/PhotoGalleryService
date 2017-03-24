using System;
using System.Configuration;

namespace PhotoGalleryService.Features.DigitalAssets
{    
    public interface IAmazonS3Configuration
    {
        string AccessKey { get; set; }
        string SecretKey { get; set; }
        string BucketName { get; set; }
        string BaseUrl { get; set; }
    }

    public class AmazonS3Configuration: ConfigurationSection, IAmazonS3Configuration
    {
        [ConfigurationProperty("accessKey")]
        public string AccessKey
        {
            get { return (string)this["accessKey"]; }
            set { this["accessKey"] = value; }
        }

        [ConfigurationProperty("secretKey")]
        public string SecretKey
        {
            get { return (string)this["secretKey"]; }
            set { this["secretKey"] = value; }
        }

        [ConfigurationProperty("bucketName")]
        public string BucketName
        {
            get { return (string)this["bucketName"]; }
            set { this["bucketName"] = value; }
        }

        [ConfigurationProperty("baseUrl")]
        public string BaseUrl
        {
            get { return (string)this["baseUrl"]; }
            set { this["baseUrl"] = value; }
        }

        public static readonly Lazy<IAmazonS3Configuration> LazyConfig = new Lazy<IAmazonS3Configuration>(() =>
        {
            var section = ConfigurationManager.GetSection("amazonS3Configuration") as IAmazonS3Configuration;
            if (section == null)
            {
                throw new ConfigurationErrorsException("amazonS3Configuration");
            }

            return section;
        }, true);
    }
}