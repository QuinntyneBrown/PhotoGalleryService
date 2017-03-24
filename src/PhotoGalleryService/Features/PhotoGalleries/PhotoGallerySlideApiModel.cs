using PhotoGalleryService.Data.Model;

namespace PhotoGalleryService.Features.PhotoGalleries
{
    public class PhotoGallerySlideApiModel
    {        
        public int Id { get; set; }
        public int? TenantId { get; set; }
        public string Name { get; set; }

        public static TModel FromPhotoGallerySlide<TModel>(PhotoGallerySlide photoGallerySlide) where
            TModel : PhotoGallerySlideApiModel, new()
        {
            var model = new TModel();
            model.Id = photoGallerySlide.Id;
            model.TenantId = photoGallerySlide.TenantId;
            model.Name = photoGallerySlide.Name;
            return model;
        }

        public static PhotoGallerySlideApiModel FromPhotoGallerySlide(PhotoGallerySlide photoGallerySlide)
            => FromPhotoGallerySlide<PhotoGallerySlideApiModel>(photoGallerySlide);

    }
}
