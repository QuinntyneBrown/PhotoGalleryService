using PhotoGalleryService.Data.Model;
using System.Collections.Generic;
using System.Linq;

namespace PhotoGalleryService.Features.PhotoGalleries
{
    public class PhotoGalleryApiModel
    {        
        public int Id { get; set; }

        public int? TenantId { get; set; }

        public string Name { get; set; }

        public ICollection<PhotoGallerySlideApiModel> PhotoGallerySlides { get; set; } = new HashSet<PhotoGallerySlideApiModel>();

        public static TModel FromPhotoGallery<TModel>(PhotoGallery photoGallery) where
            TModel : PhotoGalleryApiModel, new()
        {
            var model = new TModel();
            model.Id = photoGallery.Id;
            model.TenantId = photoGallery.TenantId;
            model.Name = photoGallery.Name;
            model.PhotoGallerySlides = photoGallery.PhotoGallerySlides
                .Select(x => PhotoGallerySlideApiModel.FromPhotoGallerySlide(x))
                .OrderBy(x=>x.OrderIndex)
                .ToList();

            return model;
        }

        public static PhotoGalleryApiModel FromPhotoGallery(PhotoGallery photoGallery)
            => FromPhotoGallery<PhotoGalleryApiModel>(photoGallery);

    }
}
