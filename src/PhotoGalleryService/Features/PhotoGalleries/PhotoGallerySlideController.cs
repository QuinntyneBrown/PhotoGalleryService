using PhotoGalleryService.Security;
using MediatR;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using static PhotoGalleryService.Features.PhotoGalleries.AddOrUpdatePhotoGallerySlideCommand;
using static PhotoGalleryService.Features.PhotoGalleries.GetPhotoGallerySlidesQuery;
using static PhotoGalleryService.Features.PhotoGalleries.GetPhotoGallerySlideByIdQuery;
using static PhotoGalleryService.Features.PhotoGalleries.RemovePhotoGallerySlideCommand;

namespace PhotoGalleryService.Features.PhotoGalleries
{
    [Authorize]
    [RoutePrefix("api/photoGallerySlide")]
    public class PhotoGallerySlideController : ApiController
    {
        public PhotoGallerySlideController(IMediator mediator, IUserManager userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        [Route("add")]
        [HttpPost]
        [ResponseType(typeof(AddOrUpdatePhotoGallerySlideResponse))]
        public async Task<IHttpActionResult> Add(AddOrUpdatePhotoGallerySlideRequest request)
        {
            request.TenantId = (await _userManager.GetUserAsync(User)).TenantId;
            return Ok(await _mediator.Send(request));
        }

        [Route("update")]
        [HttpPut]
        [ResponseType(typeof(AddOrUpdatePhotoGallerySlideResponse))]
        public async Task<IHttpActionResult> Update(AddOrUpdatePhotoGallerySlideRequest request)
        {
            request.TenantId = (await _userManager.GetUserAsync(User)).TenantId;
            return Ok(await _mediator.Send(request));
        }
        
        [Route("get")]
        [AllowAnonymous]
        [HttpGet]
        [ResponseType(typeof(GetPhotoGallerySlidesResponse))]
        public async Task<IHttpActionResult> Get()
        {
            var request = new GetPhotoGallerySlidesRequest();
            request.TenantId = (await _userManager.GetUserAsync(User)).TenantId;
            return Ok(await _mediator.Send(request));
        }

        [Route("getById")]
        [HttpGet]
        [ResponseType(typeof(GetPhotoGallerySlideByIdResponse))]
        public async Task<IHttpActionResult> GetById([FromUri]GetPhotoGallerySlideByIdRequest request)
        {
            request.TenantId = (await _userManager.GetUserAsync(User)).TenantId;
            return Ok(await _mediator.Send(request));
        }

        [Route("remove")]
        [HttpDelete]
        [ResponseType(typeof(RemovePhotoGallerySlideResponse))]
        public async Task<IHttpActionResult> Remove([FromUri]RemovePhotoGallerySlideRequest request)
        {
            request.TenantId = (await _userManager.GetUserAsync(User)).TenantId;
            return Ok(await _mediator.Send(request));
        }

        protected readonly IMediator _mediator;
        protected readonly IUserManager _userManager;
    }
}
