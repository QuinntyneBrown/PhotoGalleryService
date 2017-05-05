using MediatR;
using PhotoGalleryService.Data;
using PhotoGalleryService.Features.Core;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace PhotoGalleryService.Features.PhotoGalleries
{
    public class GetPhotoGalleryBySlugQuery
    {
        public class GetPhotoGalleryBySlugRequest : IRequest<GetPhotoGalleryBySlugResponse>
        {
            public Guid TenantUniqueId { get; set; }
            public string Slug { get; set; }
        }

        public class GetPhotoGalleryBySlugResponse
        {
            public PhotoGalleryApiModel PhotoGallery { get; set; }
        }

        public class GetPhotoGalleryBySlugHandler : IAsyncRequestHandler<GetPhotoGalleryBySlugRequest, GetPhotoGalleryBySlugResponse>
        {
            public GetPhotoGalleryBySlugHandler(PhotoGalleryServiceContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<GetPhotoGalleryBySlugResponse> Handle(GetPhotoGalleryBySlugRequest request)
            {
                return new GetPhotoGalleryBySlugResponse()
                {
                    PhotoGallery = PhotoGalleryApiModel.FromPhotoGallery(await _context.PhotoGalleries
                    .Include(x => x.Photos)
                    .SingleAsync(x => x.Slug == request.Slug))
                };
            }

            private readonly PhotoGalleryServiceContext _context;
            private readonly ICache _cache;
        }
    }
}