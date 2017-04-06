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
    public class GetLatestGalleriesQuery
    {
        public class GetLatestGalleriesRequest : IRequest<GetLatestGalleriesResponse> {
            public Guid TenantUniqueId { get; set; }
        }

        public class GetLatestGalleriesResponse
        {
            public ICollection<PhotoGalleryApiModel> PhotoGalleries { get; set; }
        }

        public class GetLatestGalleriesHandler : IAsyncRequestHandler<GetLatestGalleriesRequest, GetLatestGalleriesResponse>
        {
            public GetLatestGalleriesHandler(PhotoGalleryServiceContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<GetLatestGalleriesResponse> Handle(GetLatestGalleriesRequest request)
            {
                var galleries = await _context.PhotoGalleries
                    .Include(x => x.PhotoGallerySlides)
                    .OrderByDescending(x=>x.CreatedOn)
                    .Take(10)
                    .ToListAsync();

                return new GetLatestGalleriesResponse()
                {
                    PhotoGalleries = galleries.Select(x => PhotoGalleryApiModel.FromPhotoGallery(x)).ToList()
                };
            }

            private readonly PhotoGalleryServiceContext _context;
            private readonly ICache _cache;
        }
    }
}