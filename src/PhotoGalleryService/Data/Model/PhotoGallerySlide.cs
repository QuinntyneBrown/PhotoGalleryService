using PhotoGalleryService.Data.Helpers;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoGalleryService.Data.Model
{
    [SoftDelete("IsDeleted")]
    public class PhotoGallerySlide: ILoggable
    {
        public int Id { get; set; }
        
		[ForeignKey("Tenant")]
        public int? TenantId { get; set; }

        [ForeignKey("PhotoGallery")]
        public int? PhotoGalleryId { get; set; }
        
		[Index("NameIndex", IsUnique = false)]
        [Column(TypeName = "VARCHAR")]        
		public string Name { get; set; }

        public string ImageUrl { get; set; }

        public int? OrderIndex { get; set; }

        public DateTime CreatedOn { get; set; }
        
		public DateTime LastModifiedOn { get; set; }
        
		public string CreatedBy { get; set; }
        
		public string LastModifiedBy { get; set; }
        
		public bool IsDeleted { get; set; }

        public virtual Tenant Tenant { get; set; }

        public virtual PhotoGallery PhotoGallery { get; set; }
    }
}