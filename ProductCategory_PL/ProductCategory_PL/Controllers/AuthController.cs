using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog_BLL.DTOs;
using ProductCatalog_BLL.Helpers.Error;
using ProductCatalog_BLL.IService;
using ProductCatalog_DAL.Models.IdentityModel;
using System.Security.Claims;

namespace ProductCatalog_PL.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IAuthServic _authServic;
        private readonly IMapper _mapper;
        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IAuthServic authServic, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authServic = authServic;
            _mapper = mapper;
        }

		public IActionResult Register() => View(new RegisterDto());


		[HttpPost]
		public async Task<IActionResult> Register(RegisterDto register)
		{
			if (!ModelState.IsValid)
			{
				return View(register);
			}

			var user = new ApplicationUser
			{
				Email = register.Email,
				UserName = register.Email.Split('@')[0],
				firstName = register.firstName,
				lastName = register.lastName,
				PhoneNumber = register.phoneNumber,
				country = register.country,
				city = register.city

			};

			var result = await _userManager.CreateAsync(user, register.Password);
			//await _userManager.AddToRoleAsync(user, register.Role ?? "Admine");
			if (!result.Succeeded)
			{
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
				return View(register);
			}

			await _signInManager.SignInAsync(user, isPersistent: false);

			return RedirectToAction("Index", "Home");
		}

		public IActionResult Login() => View();


        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return View(loginDto);
            }

            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Email Or Password Incorrect.");
                return View(loginDto);

                //return Unauthorized(new ApiResponse(401));
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.PassWord, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Email Or Password Incorrect.");
                return View(loginDto);
            }

            var userDto = new UserDto()
            {
                Name = user.firstName,
                Email = user.Email,
                Token = await _authServic.CreateTokenAsync(user, _userManager)
            };
            if(userDto == null)
            {
				return Unauthorized(new ApiResponse(400));
			}
			Response.Cookies.Append("AuthToken", userDto.Token, new CookieOptions
			{
				HttpOnly = true,
				Secure = true, 
				SameSite = SameSiteMode.Strict
			});

			return RedirectToAction("Index", "Home");
        }


		[HttpPost]
		public async Task<IActionResult> Logout()
		{
			Response.Cookies.Delete("AuthToken");
			return RedirectToAction("Login", "Auth");
		}

	}
}
