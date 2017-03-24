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
    public class AddOrUpdatePhotoGallerySlideCommand
    {
        public class AddOrUpdatePhotoGallerySlideRequest : IRequest<AddOrUpdatePhotoGallerySlideResponse>
        {
            public PhotoGallerySlideApiModel PhotoGallerySlide { get; set; }
            public int? TenantId { get; set; }
        }

        public class AddOrUpdatePhotoGallerySlideResponse { }

        public class AddOrUpdatePhotoGallerySlideHandler : IAsyncRequestHandler<AddOrUpdatePhotoGallerySlideRequest, AddOrUpdatePhotoGallerySlideResponse>
        {
            public AddOrUpdatePhotoGallerySlideHandler(PhotoGalleryServiceContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<AddOrUpdatePhotoGallerySlideResponse> Handle(AddOrUpdatePhotoGallerySlideRequest request)
            {
                var entity = await _context.PhotoGallerySlides
                    .SingleOrDefaultAsync(x => x.Id == request.PhotoGallerySlide.Id && x.TenantId == request.TenantId);
                if (entity == null) _context.PhotoGallerySlides.Add(entity = new PhotoGallerySlide());
                entity.Name = request.PhotoGallerySlide.Name;
                entity.TenantId = request.TenantId;

                await _context.SaveChangesAsync();

                return new AddOrUpdatePhotoGallerySlideResponse();
            }

            private readonly PhotoGalleryServiceContext _context;
            private readonly ICache _cache;
        }

    }

}
