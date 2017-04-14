using PhotoGalleryService.Data.Helpers;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoGalleryService.Data.Model
{
    [SoftDelete("IsDeleted")]
    public class Profile: ILoggable
    {
        public int Id { get; set; }
        
		[ForeignKey("Tenant")]
        public int? TenantId { get; set; }

        [ForeignKey("Account")]
        public int? AccountId { get; set; }

        [Index("NameIndex", IsUnique = false)]
        [Column(TypeName = "VARCHAR")]        
		public string Name { get; set; }
        
		public DateTime CreatedOn { get; set; }
        
		public DateTime LastModifiedOn { get; set; }
        
		public string CreatedBy { get; set; }
        
		public string LastModifiedBy { get; set; }
        
		public bool IsDeleted { get; set; }

        public virtual Tenant Tenant { get; set; }

        public virtual Account Account { get; set; }
    }
}
