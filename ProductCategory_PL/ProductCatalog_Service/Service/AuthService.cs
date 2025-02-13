using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProductCatalog_BLL.IService;
using ProductCatalog_DAL.Models.IdentityModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProductCatalog_Service.ServiceRepo
{
    public class AuthService : IAuthServic
    {
        private readonly IConfiguration _configuration;
        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> CreateTokenAsync(ApplicationUser user, UserManager<ApplicationUser> userManager)
        {
            // Start Create Token By Claim 
            //1- Create New Claim 

            var authCliam = new List<Claim>
            {
                new Claim(ClaimTypes.Name , user.firstName + " " + user.lastName),
                new Claim(ClaimTypes.Email , user.Email),
				new Claim(ClaimTypes.NameIdentifier,user.Id.ToString())

			};

            // 2- Create Roles For Users 
            var userRoles = await userManager.GetRolesAsync(user);

            // 3- Get Roles With By Forech For Add Roles For Claim 
            foreach (var Role in userRoles)
            {
                authCliam.Add(new Claim(ClaimTypes.Role, Role));
            }


            // 4- Create SymmtricKey 
            //var SymmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("dsfsfda6666adf6daffafadddddd6fad45afd"));
            //Or We Can create it in appsetting 
            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:AuthKey"] ?? string.Empty));


            // 5- Create RegisterClaim 

            var token = new JwtSecurityToken(

                audience: _configuration["JWT:audience"],
                issuer: _configuration["JWT:issure"],
                expires: DateTime.UtcNow.AddDays(3),
                claims: authCliam,
                signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256Signature)
                );

            // 6- Return 
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
