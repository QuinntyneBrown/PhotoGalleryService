using System;
using System.Collections.Generic;
using PhotoGalleryService.Data.Helpers;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoGalleryService.Data.Model
{
    [SoftDelete("IsDeleted")]
    public class Account: ILoggable
    {
        public int Id { get; set; }
        
		[ForeignKey("Tenant")]
        public int? TenantId { get; set; }
        
		[Index("NameIndex", IsUnique = false)]
        [Column(TypeName = "VARCHAR")]        
		public string Name { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Email { get; set; }

        public ICollection<Profile> Profiles { get; set; } = new HashSet<Profile>();

        public DateTime CreatedOn { get; set; }
        
		public DateTime LastModifiedOn { get; set; }
        
		public string CreatedBy { get; set; }
        
		public string LastModifiedBy { get; set; }
        
		public bool IsDeleted { get; set; }

        public virtual Tenant Tenant { get; set; }
    }
}
