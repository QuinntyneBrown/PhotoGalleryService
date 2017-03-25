using MediatR;
using PhotoGalleryService.Data;
using PhotoGalleryService.Features.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;

namespace PhotoGalleryService.Features.PhotoGalleries
{
    public class GetPhotoGalleriesQuery
    {
        public class GetPhotoGalleriesRequest : IRequest<GetPhotoGalleriesResponse> { 
            public int? TenantId { get; set; }        
        }

        public class GetPhotoGalleriesResponse
        {
            public ICollection<PhotoGalleryApiModel> PhotoGalleries { get; set; } = new HashSet<PhotoGalleryApiModel>();
        }

        public class GetPhotoGalleriesHandler : IAsyncRequestHandler<GetPhotoGalleriesRequest, GetPhotoGalleriesResponse>
        {
            public GetPhotoGalleriesHandler(PhotoGalleryServiceContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<GetPhotoGalleriesResponse> Handle(GetPhotoGalleriesRequest request)
            {
                var photoGalleries = await _context.PhotoGalleries
                    .Where(x => x.TenantId == request.TenantId )
                    .Include(x => x.PhotoGallerySlides)
                    .ToListAsync();

                return new GetPhotoGalleriesResponse()
                {
                    PhotoGalleries = photoGalleries.Select(x => PhotoGalleryApiModel.FromPhotoGallery(x)).ToList()
                };
            }

            private readonly PhotoGalleryServiceContext _context;
            private readonly ICache _cache;
        }
    }
}