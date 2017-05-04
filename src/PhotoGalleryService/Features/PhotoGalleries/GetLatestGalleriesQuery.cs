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
            public int Skip { get; set; }
            public int Take { get; set; }
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
                    .Include(x => x.Photos)
                    .Include(x => x.Tenant)
                    .OrderByDescending(x => x.CreatedOn)
                    .Where(x => x.Tenant != null && x.Tenant.UniqueId == request.TenantUniqueId)
                    .Skip(request.Skip)
                    .Take(request.Take)
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