using MediatR;
using PhotoGalleryService.Data;
using PhotoGalleryService.Data.Model;
using PhotoGalleryService.Features.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;

namespace PhotoGalleryService.Features.PhotoGalleries
{
    public class AddOrUpdatePhotoGalleryCommand
    {
        public class AddOrUpdatePhotoGalleryRequest : IRequest<AddOrUpdatePhotoGalleryResponse>
        {
            public PhotoGalleryApiModel PhotoGallery { get; set; }
            public int? TenantId { get; set; }
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
                    .SingleOrDefaultAsync(x => x.Id == request.PhotoGallery.Id && x.TenantId == request.TenantId);
                if (entity == null) _context.PhotoGalleries.Add(entity = new PhotoGallery());
                entity.Name = request.PhotoGallery.Name;
                entity.TenantId = request.TenantId;

                await _context.SaveChangesAsync();

                return new AddOrUpdatePhotoGalleryResponse();
            }

            private readonly PhotoGalleryServiceContext _context;
            private readonly ICache _cache;
        }

    }

}
