using MediatR;
using PhotoGalleryService.Data;
using PhotoGalleryService.Features.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;

namespace PhotoGalleryService.Features.PhotoGalleries
{
    public class GetPhotoGallerySlidesQuery
    {
        public class GetPhotoGallerySlidesRequest : IRequest<GetPhotoGallerySlidesResponse> { 
            public int? TenantId { get; set; }        
        }

        public class GetPhotoGallerySlidesResponse
        {
            public ICollection<PhotoGallerySlideApiModel> PhotoGallerySlides { get; set; } = new HashSet<PhotoGallerySlideApiModel>();
        }

        public class GetPhotoGallerySlidesHandler : IAsyncRequestHandler<GetPhotoGallerySlidesRequest, GetPhotoGallerySlidesResponse>
        {
            public GetPhotoGallerySlidesHandler(PhotoGalleryServiceContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<GetPhotoGallerySlidesResponse> Handle(GetPhotoGallerySlidesRequest request)
            {
                var photoGallerySlides = await _context.PhotoGallerySlides
                    .Where( x => x.TenantId == request.TenantId )
                    .ToListAsync();

                return new GetPhotoGallerySlidesResponse()
                {
                    PhotoGallerySlides = photoGallerySlides.Select(x => PhotoGallerySlideApiModel.FromPhotoGallerySlide(x)).ToList()
                };
            }

            private readonly PhotoGalleryServiceContext _context;
            private readonly ICache _cache;
        }

    }

}
