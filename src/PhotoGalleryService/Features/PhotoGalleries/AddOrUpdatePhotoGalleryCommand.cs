using MediatR;
using PhotoGalleryService.Data;
using PhotoGalleryService.Data.Model;
using PhotoGalleryService.Features.Core;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace PhotoGalleryService.Features.PhotoGalleries
{
    public class AddOrUpdatePhotoGalleryCommand
    {
        public class AddOrUpdatePhotoGalleryRequest : IRequest<AddOrUpdatePhotoGalleryResponse>
        {
            public PhotoGalleryApiModel PhotoGallery { get; set; }
            public Guid TenantUniqueId { get; set; }            
        }

        public class AddOrUpdatePhotoGalleryResponse { }

        public class AddOrUpdatePhotoGalleryHandler : IAsyncRequestHandler<AddOrUpdatePhotoGalleryRequest, AddOrUpdatePhotoGalleryResponse>
        {
            public AddOrUpdatePhotoGalleryHandler(PhotoGalleryServiceContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<AddOrUpdatePhotoGalleryResponse> Handle(AddOrUpdatePhotoGalleryRequest request)
            {
                var entity = await _context.PhotoGalleries
                    .Include(x => x.Photos)
                    .Include(x => x.Tenant)
                    .SingleOrDefaultAsync(x => x.Id == request.PhotoGallery.Id && x.Tenant.UniqueId == request.TenantUniqueId);

                var tenant = await _context.Tenants.SingleAsync(x => x.UniqueId == request.TenantUniqueId);

                if (entity == null)
                    _context.PhotoGalleries.Add(entity = new PhotoGallery() { TenantId = tenant.Id });
                
                entity.Name = request.PhotoGallery.Name;

                entity.Photos.Clear();

                foreach(var photoApiModel in request.PhotoGallery.Photos)
                {
                    var photo = await _context.Photos.SingleOrDefaultAsync(x => x.Id == photoApiModel.Id);

                    if (photo == null) { photo = new Photo(); }

                    photo.TenantId = tenant.Id;

                    photo.PhotoGalleryId = entity.Id;

                    photo.ImageUrl = photoApiModel.ImageUrl;

                    photo.OrderIndex = photoApiModel.OrderIndex;

                    entity.Photos.Add(photo);
                }

                await _context.SaveChangesAsync();

                return new AddOrUpdatePhotoGalleryResponse();
            }

            private readonly PhotoGalleryServiceContext _context;
            private readonly ICache _cache;
        }
    }
}