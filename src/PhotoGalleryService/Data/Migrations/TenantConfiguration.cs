using System.Data.Entity.Migrations;
using PhotoGalleryService.Data;
using PhotoGalleryService.Data.Model;

namespace PhotoGalleryService.Migrations
{
    public class TenantConfiguration
    {
        public static void Seed(PhotoGalleryServiceContext context) {

            context.Tenants.AddOrUpdate(x => x.Name, new Tenant()
            {
                Name = "Default"
            });

            context.SaveChanges();
        }
    }
}
