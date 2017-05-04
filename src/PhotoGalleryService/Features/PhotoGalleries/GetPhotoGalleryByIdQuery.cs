using MediatR;
using PhotoGalleryService.Data;
using PhotoGalleryService.Features.Core;
using System.Threading.Tasks;
using System.Data.Entity;
using System;

namespace PhotoGalleryService.Features.PhotoGalleries
{
    public class GetPhotoGalleryByIdQuery
    {
        public class GetPhotoGalleryByIdRequest : IRequest<GetPhotoGalleryByIdResponse> { 
            public int Id { get; set; }
            public Guid TenantUniqueId { get; set; }
        }

        public class GetPhotoGalleryByIdResponse
        {
            public PhotoGalleryApiModel PhotoGallery { get; set; } 
        }

        public class GetPhotoGalleryByIdHandler : IAsyncRequestHandler<GetPhotoGalleryByIdRequest, GetPhotoGalleryByIdResponse>
        {
            public GetPhotoGalleryByIdHandler(PhotoGalleryServiceContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<GetPhotoGalleryByIdResponse> Handle(GetPhotoGalleryByIdRequest request)
            {                
                return new GetPhotoGalleryByIdResponse()
                {
                    PhotoGallery = PhotoGalleryApiModel.FromPhotoGallery(await _context.PhotoGalleries
                    .Include( x => x.Photos)
                    .SingleAsync( x => x.Id == request.Id))
                };
            }

            private readonly PhotoGalleryServiceContext _context;
            private readonly ICache _cache;
        }
    }
}
