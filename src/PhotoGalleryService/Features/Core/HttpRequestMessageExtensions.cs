using System;
using System.Net.Http;

namespace PhotoGalleryService.Features.Core
{
    public static class HttpRequestMessageExtensions
    {
        public static Guid GetTenantUniqueId(this HttpRequestMessage request) 
            => new Guid($"{request.GetOwinContext().Environment["Tenant"]}");
    }
}
