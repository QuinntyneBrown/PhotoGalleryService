using PhotoGalleryService.Data.Helpers;
using static PhotoGalleryService.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PhotoGalleryService.Data.Model
{
    [SoftDelete("IsDeleted")]
    public class Role: ILoggable
    {
        public int Id { get; set; }
        [ForeignKey("Tenant")]
        public int? TenantId { get; set; }
        [Index("NameIndex", IsUnique = true)]
        [Column(TypeName = "VARCHAR")]
        [StringLength(MaxStringLength)]
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastModifiedOn { get; set; }
        public string CreatedBy { get; set; }
        public string LastModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<User> Users { get; set; } = new HashSet<User>();

        public virtual Tenant Tenant { get; set; }
    }
}
