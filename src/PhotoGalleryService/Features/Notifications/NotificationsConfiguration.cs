using System;
using System.Configuration;

namespace PhotoGalleryService.Features.Notifications
{
    public interface INotificationsConfiguration
    {
        string SendGridApiKey { get; set; }
    }

    public class NotificationsConfiguration : ConfigurationSection, INotificationsConfiguration
    {

        [ConfigurationProperty("sendGridApiKey", IsRequired = true)]
        public string SendGridApiKey
        {
            get { return (string)this["sendGridApiKey"]; }
            set { this["sendGridApiKey"] = value; }
        }

        public static readonly Lazy<INotificationsConfiguration> LazyConfig = new Lazy<INotificationsConfiguration>(() =>
        {
            var section = ConfigurationManager.GetSection("notificationsConfiguration") as INotificationsConfiguration;
            if (section == null)
            {
                throw new ConfigurationErrorsException("notificationsConfiguration");
            }

            return section;
        }, true);
    }
}
