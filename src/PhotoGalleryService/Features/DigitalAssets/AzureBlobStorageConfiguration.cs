using System;
using System.Configuration;

namespace PhotoGalleryService.Features.DigitalAssets
{
    public interface IAzureBlobStorageConfiguration
    {
        string AccountName { get; set; }
        string Key { get; set; }
    }

    public class AzureBlobStorageConfiguration: ConfigurationSection, IAzureBlobStorageConfiguration
    {

        [ConfigurationProperty("accountName")]
        public string AccountName
        {
            get { return (string)this["accountName"]; }
            set { this["accountName"] = value; }
        }

        [ConfigurationProperty("key")]
        public string Key
        {
            get { return (string)this["key"]; }
            set { this["key"] = value; }
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
