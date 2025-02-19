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



        //public async Task<string> CreateTokenAsync(ApplicationUser user, UserManager<ApplicationUser> userManager)
        //{
        //    // 1. Create claims
        //    var authClaims = new List<Claim>
        //    {
        //        new Claim(ClaimTypes.Name, user.firstName + " " + user.lastName),
        //        new Claim(ClaimTypes.Email, user.Email),
        //        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        //    };

        //    // 2. Add roles to claims
        //    var userRoles = await userManager.GetRolesAsync(user);
        //    foreach (var role in userRoles)
        //    {
        //        authClaims.Add(new Claim(ClaimTypes.Role, role));
        //    }

        //    // 3. Create symmetric key for signing
        //    var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:AuthKey"] ?? string.Empty));

        //    // 4. Initialize JwtSecurityTokenHandler
        //    var tokenHandler = new JwtSecurityTokenHandler();

        //    // 5. Create token descriptor
        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Audience = _configuration["JWT:audience"],
        //        Issuer = _configuration["JWT:issure"],
        //        Expires = DateTime.UtcNow.AddDays(3),
        //        Subject = new ClaimsIdentity(authClaims),
        //        SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature)
        //    };

        //    // 6. Generate the signed token
        //    var signedToken = tokenHandler.CreateToken(tokenDescriptor);

        //    // 7. Create encryption key
        //    var encryptionKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:EncryptionKey"] ?? string.Empty));
        //    var encryptingCredentials = new EncryptingCredentials(
        //        encryptionKey,
        //        SecurityAlgorithms.Aes128KW,
        //        SecurityAlgorithms.Aes128CbcHmacSha256);

        //    // 8. Create the encrypted token descriptor
        //    var encryptedTokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        TokenType = "JWT",
        //        EncryptingCredentials = encryptingCredentials,
        //        SigningCredentials = tokenDescriptor.SigningCredentials,
        //        Subject = tokenDescriptor.Subject,
        //        Expires = tokenDescriptor.Expires,
        //        Issuer = tokenDescriptor.Issuer,
        //        Audience = tokenDescriptor.Audience
        //    };

        //    // 9. Generate the encrypted token
        //    var encryptedToken = tokenHandler.CreateToken(encryptedTokenDescriptor);

        //    // 10. Return the encrypted token
        //    return tokenHandler.WriteToken(encryptedToken);
        //}
    }
}
