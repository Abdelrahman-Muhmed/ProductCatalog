using Microsoft.AspNetCore.Identity;
using ProductCatalog_DAL.Models.IdentityModel;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalog_DAL.Prsistence.Data.UserDataSeeding
{
	public class UserSeeding
	{
		public static async Task<int?> UserDataSeeding(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<int>> roleManager)
		{
			if (!userManager.Users.Any())
			{
				var roleName = "Admin";

				if (!await roleManager.RoleExistsAsync(roleName))
				{
					var role = new IdentityRole<int> { Name = roleName };
					await roleManager.CreateAsync(role);
				}

				var user = new ApplicationUser()
				{
					firstName = "Abdelrahman",
					lastName = "Gomaa",
					Email = "abdogomaa98@gmail.com",
					UserName = "AbdoGomaa18",
					PhoneNumber = "+201142796388",
					country = "Egypt",
					city = "Cairo"
				};

				var result = await userManager.CreateAsync(user, "Abdo@1881998");

				if (result.Succeeded)
				{
					await userManager.AddToRoleAsync(user, roleName);
					return user.Id; 
				}
			}
			return null;
		}
	}
}