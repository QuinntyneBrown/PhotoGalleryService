using PhotoGalleryService.Features.DigitalAssets.UploadHandlers;
using PhotoGalleryService.Security;
using MediatR;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Net.Http.Headers;

using static PhotoGalleryService.Features.DigitalAssets.GetDigitalAssetByUniqueIdQuery;
using static PhotoGalleryService.Features.DigitalAssets.AzureBlobStorageDigitalAssetCommand;

namespace PhotoGalleryService.Features.DigitalAssets
{
    [Authorize]
    [RoutePrefix("api/digitalasset")]
    public class DigitalAssetController : ApiController
    {        
        public DigitalAssetController(IMediator mediator, IUserManager userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        [Route("add")]
        [HttpPost]
        [ResponseType(typeof(AddOrUpdateDigitalAssetCommand.AddOrUpdateDigitalAssetResponse))]
        public async Task<IHttpActionResult> Add(AddOrUpdateDigitalAssetCommand.AddOrUpdateDigitalAssetRequest request)
            => Ok(await _mediator.Send(request));

        [Route("update")]
        [HttpPut]
        [ResponseType(typeof(AddOrUpdateDigitalAssetCommand.AddOrUpdateDigitalAssetResponse))]
        public async Task<IHttpActionResult> Update(AddOrUpdateDigitalAssetCommand.AddOrUpdateDigitalAssetRequest request)
            => Ok(await _mediator.Send(request));
        
        [Route("get")]
        [AllowAnonymous]
        [HttpGet]
        [ResponseType(typeof(GetDigitalAssetsQuery.GetDigitalAssetsResponse))]
        public async Task<IHttpActionResult> Get()
            => Ok(await _mediator.Send(new GetDigitalAssetsQuery.GetDigitalAssetsRequest()));

        [Route("getById")]
        [HttpGet]
        [ResponseType(typeof(GetDigitalAssetByIdQuery.GetDigitalAssetByIdResponse))]
        public async Task<IHttpActionResult> GetById([FromUri]GetDigitalAssetByIdQuery.GetDigitalAssetByIdRequest request)
            => Ok(await _mediator.Send(request));

        [Route("remove")]
        [HttpDelete]
        [ResponseType(typeof(RemoveDigitalAssetCommand.RemoveDigitalAssetResponse))]
        public async Task<IHttpActionResult> Remove([FromUri]RemoveDigitalAssetCommand.RemoveDigitalAssetRequest request)
            => Ok(await _mediator.Send(request));

        [Route("serve")]
        [HttpGet]
        [ResponseType(typeof(GetDigitalAssetByUniqueIdResponse))]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> Serve([FromUri]GetDigitalAssetByUniqueIdRequest request)
        {
            var response = await _mediator.Send(request);
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
            var user = await _userManager.GetUserAsync(User);            
            var provider = await Request.Content.ReadAsMultipartAsync(new InMemoryMultipartFormDataStreamProvider());            
            return Ok(await _mediator.Send(new AzureBlobStorageDigitalAssetRequest() { Provider = provider, Folder = $"{user.Tenant.UniqueId}", TenantUniqueId = user.Tenant.UniqueId }));
        }

        protected readonly IMediator _mediator;
        protected readonly IUserManager _userManager;
    }
}