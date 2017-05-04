using MediatR;
using PhotoGalleryService.Data;
using PhotoGalleryService.Features.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;
using System;

namespace PhotoGalleryService.Features.PhotoGalleries
{
    public class GetPhotoGalleriesQuery
    {
        public class GetPhotoGalleriesRequest : IRequest<GetPhotoGalleriesResponse> { 
            public Guid TenantUniqueId { get; set; }
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
                    .Include(x => x.Tenant)
                    .Where(x => x.Tenant.UniqueId == request.TenantUniqueId)
                    .Include(x => x.Photos)
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