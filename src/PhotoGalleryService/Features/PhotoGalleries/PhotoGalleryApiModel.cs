using PhotoGalleryService.Data.Model;
using PhotoGalleryService.Features.Photos;
using System.Collections.Generic;
using System.Linq;

namespace PhotoGalleryService.Features.PhotoGalleries
{
    public class PhotoGalleryApiModel
    {        
        public int Id { get; set; }

        public int? TenantId { get; set; }

        public string Name { get; set; }

        public ICollection<PhotoApiModel> Photos { get; set; } = new HashSet<PhotoApiModel>();

        public static TModel FromPhotoGallery<TModel>(PhotoGallery photoGallery) where
            TModel : PhotoGalleryApiModel, new()
        {
            var model = new TModel();

            model.Id = photoGallery.Id;

            model.TenantId = photoGallery.TenantId;

            model.Name = photoGallery.Name;

            model.Photos = photoGallery.Photos
                .Select(x => PhotoApiModel.FromPhoto(x))
                .OrderBy(x => x.OrderIndex)
                .ToList();

            return model;
        }

        public static PhotoGalleryApiModel FromPhotoGallery(PhotoGallery photoGallery)
            => FromPhotoGallery<PhotoGalleryApiModel>(photoGallery);

    }
}
