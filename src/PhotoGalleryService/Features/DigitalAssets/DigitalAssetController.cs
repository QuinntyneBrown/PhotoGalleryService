using MediatR;
using PhotoGalleryService.Features.DigitalAssets.UploadHandlers;
using PhotoGalleryService.Features.Core;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Net.Http.Headers;

using static PhotoGalleryService.Features.DigitalAssets.GetDigitalAssetByUniqueIdQuery;
using static PhotoGalleryService.Features.DigitalAssets.AzureBlobStorageDigitalAssetCommand;
using static PhotoGalleryService.Features.DigitalAssets.GetDigitalAssetsQuery;
using static PhotoGalleryService.Features.DigitalAssets.GetDigitalAssetByIdQuery;
using static PhotoGalleryService.Features.DigitalAssets.RemoveDigitalAssetCommand;
using static PhotoGalleryService.Features.DigitalAssets.AddOrUpdateDigitalAssetCommand;

namespace PhotoGalleryService.Features.DigitalAssets
{
    [Authorize]
    [RoutePrefix("api/digitalasset")]
    public class DigitalAssetController : ApiController
    {
        public DigitalAssetController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("add")]
        [HttpPost]
        [ResponseType(typeof(AddOrUpdateDigitalAssetResponse))]
        public async Task<IHttpActionResult> Add(AddOrUpdateDigitalAssetRequest request)
        {
            request.TenantUniqueId = Request.GetTenantUniqueId();
            return Ok(await _mediator.Send(request));
        }

        [Route("update")]
        [HttpPut]
        [ResponseType(typeof(AddOrUpdateDigitalAssetResponse))]
        public async Task<IHttpActionResult> Update(AddOrUpdateDigitalAssetRequest request)
        {
            request.TenantUniqueId = Request.GetTenantUniqueId();
            return Ok(await _mediator.Send(request));
        }

        [Route("get")]
        [AllowAnonymous]
        [HttpGet]
        [ResponseType(typeof(GetDigitalAssetsResponse))]
        public async Task<IHttpActionResult> Get()
        {
            var request = new GetDigitalAssetsRequest();
            request.TenantUniqueId = Request.GetTenantUniqueId();
            return Ok(await _mediator.Send(request));
        }

        [Route("getById")]
        [HttpGet]
        [ResponseType(typeof(GetDigitalAssetByIdResponse))]
        public async Task<IHttpActionResult> GetById([FromUri]GetDigitalAssetByIdRequest request)
        {
            request.TenantUniqueId = Request.GetTenantUniqueId();
            return Ok(await _mediator.Send(request));
        }

        [Route("remove")]
        [HttpDelete]
        [ResponseType(typeof(RemoveDigitalAssetResponse))]
        public async Task<IHttpActionResult> Remove([FromUri]RemoveDigitalAssetRequest request)
        {
            request.TenantUniqueId = Request.GetTenantUniqueId();
            return Ok(await _mediator.Send(request));
        }

        [Route("serve")]
        [HttpGet]
        [ResponseType(typeof(GetDigitalAssetByUniqueIdResponse))]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> Serve([FromUri]GetDigitalAssetByUniqueIdRequest request)
        {
            var response = await _mediator.Send(request);
            request.TenantUniqueId = Request.GetTenantUniqueId();

            var result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new ByteArrayContent(response.DigitalAsset.Bytes);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue(response.DigitalAsset.ContentType);
            return result;
        }

        [Route("upload")]
        [HttpPost]
        public async Task<IHttpActionResult> Upload(HttpRequestMessage request)
        {
            if (!Request.Content.IsMimeMultipartContent("form-data"))
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            var provider = await Request.Content.ReadAsMultipartAsync(new InMemoryMultipartFormDataStreamProvider());

            return Ok(await _mediator.Send(new AzureBlobStorageDigitalAssetRequest()
            {
                Provider = provider,
                Folder = $"{Request.GetTenantUniqueId()}",
                TenantUniqueId = Request.GetTenantUniqueId()
            }));
        }

        protected readonly IMediator _mediator;
    }
}