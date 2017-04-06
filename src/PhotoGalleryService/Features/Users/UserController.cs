using PhotoGalleryService.Security;
using MediatR;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using static PhotoGalleryService.Features.Users.AddOrUpdateUserCommand;
using static PhotoGalleryService.Features.Users.GetUsersQuery;
using static PhotoGalleryService.Features.Users.GetUserByIdQuery;
using static PhotoGalleryService.Features.Users.RemoveUserCommand;
using static PhotoGalleryService.Features.Users.GetUserByUsernameQuery;
using static PhotoGalleryService.Features.Users.RegisterCommand;
using static PhotoGalleryService.Features.Users.ConfirmRegistrationCommand;

namespace PhotoGalleryService.Features.Users
{
    [Authorize]
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        public UserController(IMediator mediator, IUserManager userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        [Route("add")]
        [HttpPost]
        [ResponseType(typeof(AddOrUpdateUserResponse))]
        public async Task<IHttpActionResult> Add(AddOrUpdateUserRequest request)
        {
            request.TenantId = (await _userManager.GetUserAsync(User)).TenantId;
            return Ok(await _mediator.Send(request));
        }

        [Route("update")]
        [HttpPut]
        [ResponseType(typeof(AddOrUpdateUserResponse))]
        public async Task<IHttpActionResult> Update(AddOrUpdateUserRequest request)
        {
            request.TenantId = (await _userManager.GetUserAsync(User)).TenantId;
            return Ok(await _mediator.Send(request));
        }
        
        [Route("get")]
        [AllowAnonymous]
        [HttpGet]
        [ResponseType(typeof(GetUsersResponse))]
        public async Task<IHttpActionResult> Get([FromUri]GetUsersQuery.GetUsersRequest request)
        {
            request.TenantId = (await _userManager.GetUserAsync(User)).TenantId;
            return Ok(await _mediator.Send(request));
        }

        [Route("getById")]
        [HttpGet]
        [ResponseType(typeof(GetUserByIdResponse))]
        public async Task<IHttpActionResult> GetById([FromUri]GetUserByIdRequest request)
        {
            request.TenantId = (await _userManager.GetUserAsync(User)).TenantId;
            return Ok(await _mediator.Send(request));
        }

        [Route("remove")]
        [HttpDelete]
        [ResponseType(typeof(RemoveUserResponse))]
        public async Task<IHttpActionResult> Remove([FromUri]RemoveUserRequest request)
        {
            request.TenantId = (await _userManager.GetUserAsync(User)).TenantId;
            return Ok(await _mediator.Send(request));
        }

        [Route("register")]
        [AllowAnonymous]
        [HttpPost]
        [ResponseType(typeof(RegisterResponse))]
        public async Task<IHttpActionResult> Register([FromBody]RegisterRequest request)
        {
            if (request.Secret != "Vendor Evaluation")
                return null;

            request.TenantId = 1;
            return Ok(await _mediator.Send(request));
        }

        [Route("confirmregistration")]
        [AllowAnonymous]
        [HttpPost]
        [ResponseType(typeof(ConfirmRegistrationResponse))]
        public async Task<IHttpActionResult> ConfirmRegistration([FromUri]ConfirmRegistrationRequest request)
        {
            return Ok(await _mediator.Send(request));
        }

        [Route("current")]
        [HttpGet]
        [AllowAnonymous]
        [ResponseType(typeof(GetUserByUsernameResponse))]
        public async Task<IHttpActionResult> Current()
        {
            
            if (!User.Identity.IsAuthenticated)
                return Ok();

            var request = new GetUserByUsernameRequest();
            request.Username = User.Identity.Name;
            var user = await _userManager.GetUserAsync(User);
            request.TenantId = user.TenantId;
            
            return Ok(await _mediator.Send(request));
        }

        protected readonly IMediator _mediator;
        protected readonly IUserManager _userManager;
    }
}
