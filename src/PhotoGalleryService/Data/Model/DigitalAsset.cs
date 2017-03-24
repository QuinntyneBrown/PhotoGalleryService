using PhotoGalleryService.Data.Helpers;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoGalleryService.Data.Model
{
    [SoftDelete("IsDeleted")]
    public class DigitalAsset: ILoggable
    {
        public int Id { get; set; }

        [ForeignKey("Tenant")]
        public int? TenantId { get; set; }

        public string Url { get; set; }

        public string Name { get; set; }

        public string Folder { get; set; }

        public string FileName { get; set; }

        public string Description { get; set; }

        public DateTime? Created { get; set; }

        public DateTime? FileModified { get; set; }

        public long? Size { get; set; }

        public string ContentType { get; set; }

        [NotMapped]
        public string RelativePath { get { return $"api/digitalasset/serve?uniqueid={UniqueId}"; } }

        public Byte[] Bytes { get; set; } = new byte[0];

        [Index("UniqueIdIndex", IsUnique = true)]
        [Column(TypeName = "UNIQUEIDENTIFIER")]
        public Guid? UniqueId { get; set; } = Guid.NewGuid();

        public DateTime CreatedOn { get; set; }

        public DateTime LastModifiedOn { get; set; }

        public string CreatedBy { get; set; }

        public string LastModifiedBy { get; set; }
        
        public bool IsDeleted { get; set; }

        public virtual Tenant Tenant { get; set; }
    }
}
