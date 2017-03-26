using PhotoGalleryService.Data.Model;
using System.Threading.Tasks;
using System.Security.Principal;
using PhotoGalleryService.Data;
using System.Data.Entity;

namespace PhotoGalleryService.Security
{
    public interface IUserManager
    {
        Task<User> GetUserAsync(IPrincipal user);
    }

    public class UserManager : IUserManager
    {
        public UserManager(IPhotoGalleryServiceContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserAsync(IPrincipal user) => await _context.Users
            .Include(x=>x.Tenant)
            .SingleAsync(x => x.Username == user.Identity.Name);

        protected readonly IPhotoGalleryServiceContext _context;
    }
}
