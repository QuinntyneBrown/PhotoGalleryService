using MediatR;
using PhotoGalleryService.Data;
using PhotoGalleryService.Data.Model;
using PhotoGalleryService.Features.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;

namespace PhotoGalleryService.Features.Photos
{
    public class AddOrUpdatePhotoCommand
    {
        public class AddOrUpdatePhotoRequest : IRequest<AddOrUpdatePhotoResponse>
        {
            public PhotoApiModel Photo { get; set; }
            public Guid TenantUniqueId { get; set; }
        }

        public class AddOrUpdatePhotoResponse { }

        public class AddOrUpdatePhotoHandler : IAsyncRequestHandler<AddOrUpdatePhotoRequest, AddOrUpdatePhotoResponse>
        {
            public AddOrUpdatePhotoHandler(PhotoGalleryServiceContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<AddOrUpdatePhotoResponse> Handle(AddOrUpdatePhotoRequest request)
            {
                var entity = await _context.Photos
                    .Include(x => x.Tenant)
                    .Include(x => x.PhotoGallery)
                    .SingleOrDefaultAsync(x => x.Id == request.Photo.Id && x.Tenant.UniqueId == request.TenantUniqueId);
                
                if (entity == null) {
                    var tenant = await _context.Tenants.SingleAsync(x => x.UniqueId == request.TenantUniqueId);
                    _context.Photos.Add(entity = new Photo() { TenantId = tenant.Id });
                }

                entity.Name = request.Photo.Name;

                entity.PhotoGalleryId = request.Photo.PhotoGalleryId;

                entity.ImageUrl = request.Photo.ImageUrl;

                entity.OrderIndex = request.Photo.OrderIndex;

                entity.Description = request.Photo.Description;
                
                await _context.SaveChangesAsync();

                return new AddOrUpdatePhotoResponse();
            }

            private readonly PhotoGalleryServiceContext _context;
            private readonly ICache _cache;
        }

    }

}
