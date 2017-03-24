using MediatR;
using PhotoGalleryService.Data;
using PhotoGalleryService.Features.Core;
using PhotoGalleryService.Features.Users;
using PhotoGalleryService.Security;
using System.Threading.Tasks;
using static Newtonsoft.Json.JsonConvert;   

namespace PhotoGalleryService.Features.Notifications
{
    public class SendRegistrationConfirmationCommand
    {
        public class SendRegistrationConfirmationRequest : IRequest<SendRegistrationConfirmationResponse> {
            public ConfirmRegistrationApiModel ConfirmRegistration { get; set; }
        }

        public class SendRegistrationConfirmationResponse { }

        public class SendRegistrationConfirmationHandler : IAsyncRequestHandler<SendRegistrationConfirmationRequest, SendRegistrationConfirmationResponse>
        {
            public SendRegistrationConfirmationHandler(PhotoGalleryServiceContext context, ICache cache, INotificationService notificationService, IEncryptionService encryptionService)
            {
                _context = context;
                _cache = cache;
                _notificationService = notificationService;
                _encryptionService = encryptionService;
            }

            public async Task<SendRegistrationConfirmationResponse> Handle(SendRegistrationConfirmationRequest request)
            {
                var confirmationRegistration = SerializeObject(request.ConfirmRegistration);
                var encryptedConfirmationRegistration = _encryptionService.EncryptUri(confirmationRegistration);
                var mailMessage = _notificationService.BuildMessage();
                _notificationService.ResolveRecipients(ref mailMessage);
                var result = await _notificationService.SendAsync(mailMessage);
                return new SendRegistrationConfirmationResponse();
            }

            private readonly PhotoGalleryServiceContext _context;
            private readonly ICache _cache;
            private readonly INotificationService _notificationService;
            private readonly IEncryptionService _encryptionService;
        }
    }
}