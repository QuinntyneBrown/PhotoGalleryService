using MediatR;
using PhotoGalleryService.Data;
using PhotoGalleryService.Data.Model;
using PhotoGalleryService.Features.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;

namespace PhotoGalleryService.Features.Users
{
    public class RemoveUserCommand
    {
        public class RemoveUserRequest : IRequest<RemoveUserResponse>
        {
            public int Id { get; set; }
            public int? TenantId { get; set; }
        }

        public class RemoveUserResponse { }

        public class RemoveUserHandler : IAsyncRequestHandler<RemoveUserRequest, RemoveUserResponse>
        {
            public RemoveUserHandler(PhotoGalleryServiceContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<RemoveUserResponse> Handle(RemoveUserRequest request)
            {
                var user = await _context.Users.SingleAsync(x=>x.Id == request.Id && x.TenantId == request.TenantId);
                user.IsDeleted = true;
                await _context.SaveChangesAsync();
                return new RemoveUserResponse();
            }

            private readonly PhotoGalleryServiceContext _context;
            private readonly ICache _cache;
        }
    }
}
