using MediatR;
using PhotoGalleryService.Data;
using PhotoGalleryService.Features.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;

namespace PhotoGalleryService.Features.PhotoGalleries
{
    public class GetPhotoGalleryByIdQuery
    {
        public class GetPhotoGalleryByIdRequest : IRequest<GetPhotoGalleryByIdResponse> { 
            public int Id { get; set; }
            public int? TenantId { get; set; }
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
                    PhotoGallery = PhotoGalleryApiModel.FromPhotoGallery(await _context.PhotoGalleries.SingleAsync(x=>x.Id == request.Id && x.TenantId == request.TenantId))
                };
            }

            private readonly PhotoGalleryServiceContext _context;
            private readonly ICache _cache;
        }

    }

}
