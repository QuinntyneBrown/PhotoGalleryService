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
    public class RemovePhotoGallerySlideCommand
    {
        public class RemovePhotoGallerySlideRequest : IRequest<RemovePhotoGallerySlideResponse>
        {
            public int Id { get; set; }
            public int? TenantId { get; set; }
        }

        public class RemovePhotoGallerySlideResponse { }

        public class RemovePhotoGallerySlideHandler : IAsyncRequestHandler<RemovePhotoGallerySlideRequest, RemovePhotoGallerySlideResponse>
        {
            public RemovePhotoGallerySlideHandler(PhotoGalleryServiceContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<RemovePhotoGallerySlideResponse> Handle(RemovePhotoGallerySlideRequest request)
            {
                var photoGallerySlide = await _context.PhotoGallerySlides.SingleAsync(x=>x.Id == request.Id && x.TenantId == request.TenantId);
                photoGallerySlide.IsDeleted = true;
                await _context.SaveChangesAsync();
                return new RemovePhotoGallerySlideResponse();
            }

            private readonly PhotoGalleryServiceContext _context;
            private readonly ICache _cache;
        }
    }
}
