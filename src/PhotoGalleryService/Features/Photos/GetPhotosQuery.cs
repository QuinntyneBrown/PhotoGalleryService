using MediatR;
using PhotoGalleryService.Data;
using PhotoGalleryService.Features.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;

namespace PhotoGalleryService.Features.Photos
{
    public class GetPhotosQuery
    {
        public class GetPhotosRequest : IRequest<GetPhotosResponse> { 
            public Guid TenantUniqueId { get; set; }       
        }

        public class GetPhotosResponse
        {
            public ICollection<PhotoApiModel> Photos { get; set; } = new HashSet<PhotoApiModel>();
        }

        public class GetPhotosHandler : IAsyncRequestHandler<GetPhotosRequest, GetPhotosResponse>
        {
            public GetPhotosHandler(PhotoGalleryServiceContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<GetPhotosResponse> Handle(GetPhotosRequest request)
            {
                var photos = await _context.Photos
                    .Include(x => x.Tenant)
                    .Where(x => x.Tenant.UniqueId == request.TenantUniqueId )
                    .ToListAsync();

                return new GetPhotosResponse()
                {
                    Photos = photos.Select(x => PhotoApiModel.FromPhoto(x)).ToList()
                };
            }

            private readonly PhotoGalleryServiceContext _context;
            private readonly ICache _cache;
        }

    }

}
