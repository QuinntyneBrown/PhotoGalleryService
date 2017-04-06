using MediatR;
using PhotoGalleryService.Data;
using PhotoGalleryService.Features.Core;
using System.Threading.Tasks;
using System.Data.Entity;
using System;

namespace PhotoGalleryService.Features.PhotoGalleries
{
    public class RemovePhotoGalleryCommand
    {
        public class RemovePhotoGalleryRequest : IRequest<RemovePhotoGalleryResponse>
        {
            public int Id { get; set; }
            public Guid TenantUniqueId { get; set; }
        }

        public class RemovePhotoGalleryResponse { }

        public class RemovePhotoGalleryHandler : IAsyncRequestHandler<RemovePhotoGalleryRequest, RemovePhotoGalleryResponse>
        {
            public RemovePhotoGalleryHandler(PhotoGalleryServiceContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<RemovePhotoGalleryResponse> Handle(RemovePhotoGalleryRequest request)
            {
                var photoGallery = await _context.PhotoGalleries
                    .Include(x=>x.Tenant)
                    .SingleAsync(x=>x.Id == request.Id);
                photoGallery.IsDeleted = true;
                await _context.SaveChangesAsync();
                return new RemovePhotoGalleryResponse();
            }

            private readonly PhotoGalleryServiceContext _context;
            private readonly ICache _cache;
        }
    }
}
