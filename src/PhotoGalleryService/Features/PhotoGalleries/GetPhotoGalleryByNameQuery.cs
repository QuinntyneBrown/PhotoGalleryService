using MediatR;
using PhotoGalleryService.Data;
using PhotoGalleryService.Features.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;

namespace PhotoGalleryService.Features.PhotoGalleries
{
    public class GetPhotoGalleryByNameQuery
    {
        public class GetPhotoGalleryByNameRequest : IRequest<GetPhotoGalleryByNameResponse>
        {
            public string Name { get; set; }
            public Guid TenantUniqueId { get; set; }
        }

        public class GetPhotoGalleryByNameResponse
        {
            public PhotoGalleryApiModel PhotoGallery { get; set; }
        }

        public class GetPhotoGalleryByNameHandler : IAsyncRequestHandler<GetPhotoGalleryByNameRequest, GetPhotoGalleryByNameResponse>
        {
            public GetPhotoGalleryByNameHandler(PhotoGalleryServiceContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<GetPhotoGalleryByNameResponse> Handle(GetPhotoGalleryByNameRequest request)
            {
                return new GetPhotoGalleryByNameResponse()
                {
                    PhotoGallery = PhotoGalleryApiModel.FromPhotoGallery(await _context.PhotoGalleries
                    .Include(x => x.Photos)
                    .SingleAsync(x => x.Name == request.Name))
                };
            }

            private readonly PhotoGalleryServiceContext _context;
            private readonly ICache _cache;
        }

    }

}
