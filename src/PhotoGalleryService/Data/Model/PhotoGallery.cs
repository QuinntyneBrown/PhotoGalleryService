using PhotoGalleryService.Data.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static PhotoGalleryService.Constants;

namespace PhotoGalleryService.Data.Model
{
    [SoftDelete("IsDeleted")]
    public class PhotoGallery: ILoggable
    {
        public int Id { get; set; }
        
		[ForeignKey("Tenant")]
        public int? TenantId { get; set; }
        
		[Index("PhotoGalleryNameIndex", IsUnique = false)]
        [Column(TypeName = "VARCHAR")]
        [StringLength(MaxStringLength)]
        public string Name { get; set; }

        public string Slug { get; set; }

        public string Description { get; set; }
        
		public DateTime CreatedOn { get; set; }
        
		public DateTime LastModifiedOn { get; set; }
        
		public string CreatedBy { get; set; }
        
		public string LastModifiedBy { get; set; }
        
		public bool IsDeleted { get; set; }

        public ICollection<Photo> Photos { get; set; } = new HashSet<Photo>();

        public virtual Tenant Tenant { get; set; }
    }
}