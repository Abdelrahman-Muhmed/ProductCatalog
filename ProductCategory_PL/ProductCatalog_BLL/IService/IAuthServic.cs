using Microsoft.AspNetCore.Identity;
using ProductCatalog_DAL.Models.IdentityModel;

namespace ProductCatalog_BLL.IService
{
    public interface IAuthServic
    {
        Task<string> CreateTokenAsync(ApplicationUser user, UserManager<ApplicationUser> userManager);
    }
}
