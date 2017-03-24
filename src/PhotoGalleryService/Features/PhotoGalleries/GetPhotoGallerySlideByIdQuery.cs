using MediatR;
using PhotoGalleryService.Data;
using PhotoGalleryService.Features.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;

namespace PhotoGalleryService.Features.PhotoGalleries
{
    public class GetPhotoGallerySlideByIdQuery
    {
        public class GetPhotoGallerySlideByIdRequest : IRequest<GetPhotoGallerySlideByIdResponse> { 
            public int Id { get; set; }
            public int? TenantId { get; set; }
        }

        public class GetPhotoGallerySlideByIdResponse
        {
            public PhotoGallerySlideApiModel PhotoGallerySlide { get; set; } 
        }

        public class GetPhotoGallerySlideByIdHandler : IAsyncRequestHandler<GetPhotoGallerySlideByIdRequest, GetPhotoGallerySlideByIdResponse>
        {
            public GetPhotoGallerySlideByIdHandler(PhotoGalleryServiceContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<GetPhotoGallerySlideByIdResponse> Handle(GetPhotoGallerySlideByIdRequest request)
            {                
                return new GetPhotoGallerySlideByIdResponse()
                {
                    PhotoGallerySlide = PhotoGallerySlideApiModel.FromPhotoGallerySlide(await _context.PhotoGallerySlides.SingleAsync(x=>x.Id == request.Id && x.TenantId == request.TenantId))
                };
            }

            private readonly PhotoGalleryServiceContext _context;
            private readonly ICache _cache;
        }

    }

}
