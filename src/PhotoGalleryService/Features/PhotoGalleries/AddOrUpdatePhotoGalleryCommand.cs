using MediatR;
using PhotoGalleryService.Data;
using PhotoGalleryService.Data.Model;
using PhotoGalleryService.Features.Core;
using System.Threading.Tasks;
using System.Data.Entity;
using System;

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
                    .Include(x => x.PhotoGallerySlides)
                    .Include(x => x.Tenant)
                    .SingleOrDefaultAsync(x => x.Id == request.PhotoGallery.Id && x.Tenant.UniqueId == request.TenantUniqueId);

                if (entity == null)
                {
                    var tenant = await _context.Tenants.SingleAsync(x => x.UniqueId == request.TenantUniqueId);
                    _context.PhotoGalleries.Add(entity = new PhotoGallery() { TenantId = tenant.Id });
                }
                
                entity.Name = request.PhotoGallery.Name;

                entity.PhotoGallerySlides.Clear();

                foreach(var photoGallerySlideApiModel in request.PhotoGallery.PhotoGallerySlides)
                {
                    var photoGallerySlide = await _context.PhotoGallerySlides.SingleOrDefaultAsync(x => x.Id == photoGallerySlideApiModel.Id);

                    if (photoGallerySlide == null) { photoGallerySlide = new PhotoGallerySlide(); }

                    photoGallerySlide.PhotoGalleryId = entity.Id;

                    photoGallerySlide.ImageUrl = photoGallerySlideApiModel.ImageUrl;

                    photoGallerySlide.OrderIndex = photoGallerySlideApiModel.OrderIndex;

                    entity.PhotoGallerySlides.Add(photoGallerySlide);
                }

                await _context.SaveChangesAsync();

                return new AddOrUpdatePhotoGalleryResponse();
            }

            private readonly PhotoGalleryServiceContext _context;
            private readonly ICache _cache;
        }
    }
}