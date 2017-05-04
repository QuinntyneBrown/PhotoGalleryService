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
    public class GetPhotoByIdQuery
    {
        public class GetPhotoByIdRequest : IRequest<GetPhotoByIdResponse> { 
            public int Id { get; set; }
            public Guid TenantUniqueId { get; set; }
        }

        public class GetPhotoByIdResponse
        {
            public PhotoApiModel Photo { get; set; } 
        }

        public class GetPhotoByIdHandler : IAsyncRequestHandler<GetPhotoByIdRequest, GetPhotoByIdResponse>
        {
            public GetPhotoByIdHandler(PhotoGalleryServiceContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<GetPhotoByIdResponse> Handle(GetPhotoByIdRequest request)
            {                
                return new GetPhotoByIdResponse()
                {
                    Photo = PhotoApiModel.FromPhoto(await _context.Photos
                    .Include(x => x.Tenant)				
					.SingleAsync(x=>x.Id == request.Id &&  x.Tenant.UniqueId == request.TenantUniqueId))
                };
            }

            private readonly PhotoGalleryServiceContext _context;
            private readonly ICache _cache;
        }

    }

}
