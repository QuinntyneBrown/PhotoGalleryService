using PhotoGalleryService.Security;
using MediatR;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using static PhotoGalleryService.Features.PhotoGalleries.AddOrUpdatePhotoGalleryCommand;
using static PhotoGalleryService.Features.PhotoGalleries.GetPhotoGalleriesQuery;
using static PhotoGalleryService.Features.PhotoGalleries.GetPhotoGalleryByIdQuery;
using static PhotoGalleryService.Features.PhotoGalleries.RemovePhotoGalleryCommand;

namespace PhotoGalleryService.Features.PhotoGalleries
{
    [Authorize]
    [RoutePrefix("api/photoGallery")]
    public class PhotoGalleryController : ApiController
    {
        public PhotoGalleryController(IMediator mediator, IUserManager userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        [Route("add")]
        [HttpPost]
        [ResponseType(typeof(AddOrUpdatePhotoGalleryResponse))]
        public async Task<IHttpActionResult> Add(AddOrUpdatePhotoGalleryRequest request)
        {
            request.TenantId = (await _userManager.GetUserAsync(User)).TenantId;
            return Ok(await _mediator.Send(request));
        }

        [Route("update")]
        [HttpPut]
        [ResponseType(typeof(AddOrUpdatePhotoGalleryResponse))]
        public async Task<IHttpActionResult> Update(AddOrUpdatePhotoGalleryRequest request)
        {
            request.TenantId = (await _userManager.GetUserAsync(User)).TenantId;
            return Ok(await _mediator.Send(request));
        }
        
        [Route("get")]
        [AllowAnonymous]
        [HttpGet]
        [ResponseType(typeof(GetPhotoGalleriesResponse))]
        public async Task<IHttpActionResult> Get()
        {
            var request = new GetPhotoGalleriesRequest();
            request.TenantId = (await _userManager.GetUserAsync(User)).TenantId;
            return Ok(await _mediator.Send(request));
        }

        [Route("getById")]
        [HttpGet]
        [ResponseType(typeof(GetPhotoGalleryByIdResponse))]
        public async Task<IHttpActionResult> GetById([FromUri]GetPhotoGalleryByIdRequest request)
        {
            request.TenantId = (await _userManager.GetUserAsync(User)).TenantId;
            return Ok(await _mediator.Send(request));
        }

        [Route("remove")]
        [HttpDelete]
        [ResponseType(typeof(RemovePhotoGalleryResponse))]
        public async Task<IHttpActionResult> Remove([FromUri]RemovePhotoGalleryRequest request)
        {
            request.TenantId = (await _userManager.GetUserAsync(User)).TenantId;
            return Ok(await _mediator.Send(request));
        }

        protected readonly IMediator _mediator;
        protected readonly IUserManager _userManager;
    }
}
