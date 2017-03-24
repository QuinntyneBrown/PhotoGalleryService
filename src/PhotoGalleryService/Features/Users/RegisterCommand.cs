using MediatR;
using PhotoGalleryService.Data;
using PhotoGalleryService.Features.Core;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;
using PhotoGalleryService.Data.Model;
using PhotoGalleryService.Security;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System;

namespace PhotoGalleryService.Features.Users
{
    public class RegisterCommand
    {
        public class RegisterRequest : IRequest<RegisterResponse>
        {
            public int? TenantId { get; set; }

            public string Secret { get; set; }

            public RegisterApiModel Register { get; set; }
        }

        public class RegisterResponse {
            public RegisterResponse(string token)
            {
                Token = token;
            }
            public string Token { get; set; }
        }
        

        public class RegisterHandler : IAsyncRequestHandler<RegisterRequest, RegisterResponse>
        {
            public RegisterHandler(PhotoGalleryServiceContext context, ICache cache, Lazy<IAuthConfiguration> lazyAuthConfiguration, IMediator mediator)
            {
                _context = context;
                _cache = cache;
                _mediator = mediator;
                _jwtWriterFormat = new JwtWriterFormat(lazyAuthConfiguration, new OAuthOptions(lazyAuthConfiguration, mediator));
            }

            public async Task<RegisterResponse> Handle(RegisterRequest request)
            {
                if ((await _context.Users.SingleOrDefaultAsync(x => x.Email.ToLower() == request.Register.Email.ToLower())) != null)
                    return null;

                var user = new User()
                {
                    TenantId = request.TenantId,
                    Firstname = request.Register.Firstname,
                    Lastname = request.Register.Lastname,
                    Email = request.Register.Email,
                    Username = request.Register.Email,
                    Password = new EncryptionService().TransformPassword(request.Register.Password)
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                var claims = await _mediator.Send(new GetClaimsForUserQuery.GetClaimsForUserRequest() { Username = user.Username });

                var authenticationTicket = new AuthenticationTicket(new ClaimsIdentity(claims.Claims), new AuthenticationProperties());
                return new RegisterResponse(_jwtWriterFormat.Protect(authenticationTicket));

            }

            private readonly PhotoGalleryServiceContext _context;
            private readonly ICache _cache;
            private readonly IMediator _mediator;
            protected readonly JwtWriterFormat _jwtWriterFormat;
        }

    }

}
