using MediatR;
using PhotoGalleryService.Data;
using PhotoGalleryService.Data.Model;
using PhotoGalleryService.Features.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;

namespace PhotoGalleryService.Features.PhotoGalleries
{
    public class RemovePhotoGalleryCommand
    {
        public class RemovePhotoGalleryRequest : IRequest<RemovePhotoGalleryResponse>
        {
            public int Id { get; set; }
            public int? TenantId { get; set; }
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
                var photoGallery = await _context.PhotoGalleries.SingleAsync(x=>x.Id == request.Id && x.TenantId == request.TenantId);
                photoGallery.IsDeleted = true;
                await _context.SaveChangesAsync();
                return new RemovePhotoGalleryResponse();
            }

            private readonly PhotoGalleryServiceContext _context;
            private readonly ICache _cache;
        }
    }
}
