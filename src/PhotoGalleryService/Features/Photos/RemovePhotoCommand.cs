using MediatR;
using PhotoGalleryService.Data;
using PhotoGalleryService.Features.Core;
using System;
using System.Threading.Tasks;
using System.Data.Entity;

namespace PhotoGalleryService.Features.Photos
{
    public class RemovePhotoCommand
    {
        public class RemovePhotoRequest : IRequest<RemovePhotoResponse>
        {
            public int Id { get; set; }
            public Guid TenantUniqueId { get; set; } 
        }

        public class RemovePhotoResponse { }

        public class RemovePhotoHandler : IAsyncRequestHandler<RemovePhotoRequest, RemovePhotoResponse>
        {
            public RemovePhotoHandler(PhotoGalleryServiceContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<RemovePhotoResponse> Handle(RemovePhotoRequest request)
            {
                var photo = await _context.Photos.SingleAsync(x=>x.Id == request.Id && x.Tenant.UniqueId == request.TenantUniqueId);
                photo.IsDeleted = true;
                await _context.SaveChangesAsync();
                return new RemovePhotoResponse();
            }

            private readonly PhotoGalleryServiceContext _context;
            private readonly ICache _cache;
        }
    }
}
