using PhotoGalleryService.Data.Model;

namespace PhotoGalleryService.Features.Photos
{
    public class PhotoApiModel
    {        
        public int Id { get; set; }

        public int? PhotoGalleryId { get; set; }

        public int? TenantId { get; set; }

        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public int? OrderIndex { get; set; }

        public string Description { get; set; }

        public static TModel FromPhoto<TModel>(Photo photo) where
            TModel : PhotoApiModel, new()
        {
            var model = new TModel();

            model.Id = photo.Id;
            
            model.TenantId = photo.TenantId;

            model.PhotoGalleryId = photo.PhotoGalleryId;

            model.Name = photo.Name;

            model.Description = photo.Description;

            model.ImageUrl = photo.ImageUrl;

            model.OrderIndex = photo.OrderIndex;

            return model;
        }

        public static PhotoApiModel FromPhoto(Photo photo)
            => FromPhoto<PhotoApiModel>(photo);

    }
}
