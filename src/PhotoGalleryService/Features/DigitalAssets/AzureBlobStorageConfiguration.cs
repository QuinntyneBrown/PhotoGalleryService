using System;
using System.Configuration;

namespace PhotoGalleryService.Features.DigitalAssets
{
    public interface IAzureBlobStorageConfiguration
    {
        string AccountName { get; set; }
        string KeyValue { get; set; }
        string KeyName { get; set; }
    }

    public class AzureBlobStorageConfiguration: ConfigurationSection, IAzureBlobStorageConfiguration
    {

        [ConfigurationProperty("accountName")]
        public string AccountName
        {
            get { return (string)this["accountName"]; }
            set { this["accountName"] = value; }
        }

        [ConfigurationProperty("keyName")]
        public string KeyName
        {
            get { return (string)this["keyName"]; }
            set { this["keyName"] = value; }
        }


        [ConfigurationProperty("keyValue")]
        public string KeyValue
        {
            get { return (string)this["keyValue"]; }
            set { this["keyValue"] = value; }
        }

        public static IAzureBlobStorageConfiguration Config
        {
            get { return ConfigurationManager.GetSection("azureBlobStorageConfiguration") as IAzureBlobStorageConfiguration; }
        }

        public static readonly Lazy<IAzureBlobStorageConfiguration> LazyConfig = new Lazy<IAzureBlobStorageConfiguration>(() =>
        {
            var section = ConfigurationManager.GetSection("azureBlobStorageConfiguration") as IAzureBlobStorageConfiguration;
            if (section == null)
            {
                throw new ConfigurationErrorsException("azureBlobStorageConfiguration");
            }

            return section;
        }, true);
    }
}
