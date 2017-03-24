using PhotoGalleryService.Data;
using PhotoGalleryService.Features.Core;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace PhotoGalleryService.Features.Notifications
{
    public interface INotificationService
    {
        SendGridMessage BuildMessage();
        void ResolveRecipients(ref SendGridMessage mailMessage);
        Task<dynamic> SendAsync(SendGridMessage sendGridMessage);     
    }

    public class NotificationService: INotificationService
    {
        public NotificationService(Lazy<NotificationsConfiguration> lazyNotificationsConfiguration)
        {
            _sendGridClient = new SendGridClient(lazyNotificationsConfiguration.Value.SendGridApiKey);
        }

        public SendGridMessage BuildMessage()
        {
            var mailMessage = new SendGrid.Helpers.Mail.SendGridMessage();            
            var html = @"<html><body><h1>Test</h1></body></html>";
            mailMessage.HtmlContent = html;
            return mailMessage;
        }

        public void ResolveRecipients(ref SendGridMessage mailMessage)
        {
            mailMessage.AddTo(new EmailAddress("quinntynebrown@gmail.com"));            
        }

        public async Task<dynamic> SendAsync(SendGridMessage mailMessage)
        {
            return await _sendGridClient.SendEmailAsync(mailMessage);
        }

        private readonly PhotoGalleryServiceContext _context;
        private readonly ICache _cache;
        private readonly SendGridClient _sendGridClient;
    }
}
