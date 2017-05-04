using System;
using PhotoGalleryService.Data.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static PhotoGalleryService.Constants;

namespace PhotoGalleryService.Data.Model
{
    [SoftDelete("IsDeleted")]
    public class Photo: ILoggable
    {
        public int Id { get; set; }
        
		[ForeignKey("Tenant")]
        public int? TenantId { get; set; }

        [ForeignKey("PhotoGallery")]
        public int? PhotoGalleryId { get; set; }

        [Index("PhotoNameIndex", IsUnique = false)]
        [Column(TypeName = "VARCHAR")]     
        [StringLength(MaxStringLength)]		   
		public string Name { get; set; }

        public string ImageUrl { get; set; }

        public string Description { get; set; }

        public int? OrderIndex { get; set; }

        public DateTime CreatedOn { get; set; }
        
		public DateTime LastModifiedOn { get; set; }
        
		public string CreatedBy { get; set; }
        
		public string LastModifiedBy { get; set; }
        
		public bool IsDeleted { get; set; }

        public virtual Tenant Tenant { get; set; }

        public PhotoGallery PhotoGallery { get; set; }
    }
}
