using MediatR;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using PhotoGalleryService.Features.Core;
using static PhotoGalleryService.Features.Photos.AddOrUpdatePhotoCommand;
using static PhotoGalleryService.Features.Photos.GetPhotosQuery;
using static PhotoGalleryService.Features.Photos.GetPhotoByIdQuery;
using static PhotoGalleryService.Features.Photos.RemovePhotoCommand;

namespace PhotoGalleryService.Features.Photos
{
    [Authorize]
    [RoutePrefix("api/photo")]
    public class PhotoController : ApiController
    {
        public PhotoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("add")]
        [HttpPost]
        [ResponseType(typeof(AddOrUpdatePhotoResponse))]
        public async Task<IHttpActionResult> Add(AddOrUpdatePhotoRequest request)
        {
            request.TenantUniqueId = Request.GetTenantUniqueId();
            return Ok(await _mediator.Send(request));
        }

        [Route("update")]
        [HttpPut]
        [ResponseType(typeof(AddOrUpdatePhotoResponse))]
        public async Task<IHttpActionResult> Update(AddOrUpdatePhotoRequest request)
        {
            request.TenantUniqueId = Request.GetTenantUniqueId();
            return Ok(await _mediator.Send(request));
        }
        
        [Route("get")]
        [AllowAnonymous]
        [HttpGet]
        [ResponseType(typeof(GetPhotosResponse))]
        public async Task<IHttpActionResult> Get()
        {
            var request = new GetPhotosRequest();
            request.TenantUniqueId = Request.GetTenantUniqueId();
            return Ok(await _mediator.Send(request));
        }

        [Route("getById")]
        [HttpGet]
        [ResponseType(typeof(GetPhotoByIdResponse))]
        public async Task<IHttpActionResult> GetById([FromUri]GetPhotoByIdRequest request)
        {
            request.TenantUniqueId = Request.GetTenantUniqueId();
            return Ok(await _mediator.Send(request));
        }

        [Route("remove")]
        [HttpDelete]
        [ResponseType(typeof(RemovePhotoResponse))]
        public async Task<IHttpActionResult> Remove([FromUri]RemovePhotoRequest request)
        {
            request.TenantUniqueId = Request.GetTenantUniqueId();
            return Ok(await _mediator.Send(request));
        }

        protected readonly IMediator _mediator;
    }
}
