using MessagingApp.Core.Entities;
using MessagingApp.DAL.DbContext;
using MessagingApp.Services.DTOs;
using MessagingApp.Services.TokenHandler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MessagingApp.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly MessagingAppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(MessagingAppDbContext context, UserManager<User> userManeger, IConfiguration configuration, SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManeger;
            _configuration = configuration;
            _signInManager = signInManager;
        }

        public IActionResult RegisterNewUser()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RegisterNewUser(UserRegistrationDTO userDto)
        {
            if(!ModelState.IsValid)
            {
                return View(userDto);
            }

            var result = _context.Users.SingleOrDefault(x => x.Email == userDto.Email);

            if (result == null)
            {
                User newUser = new User();

                newUser.Id = Guid.NewGuid().ToString();
                newUser.Email = userDto.Email;
                newUser.UserName = userDto.UserName;
                newUser.NormalizedUserName=userDto.UserName.ToUpper();
                newUser.NormalizedEmail = userDto.Email.ToUpper();
                newUser.EmailConfirmed = true;
                newUser.PasswordHash = userDto.Password;
                newUser.SecurityStamp = Guid.NewGuid().ToString();
                newUser.ConcurrencyStamp = Guid.NewGuid().ToString();
                newUser.PhoneNumberConfirmed = true;

                await _userManager.CreateAsync(newUser, userDto.Password);

                return Ok("You are registered");
            }
            else
            {
                return BadRequest("New user cannot be registered with an used email");
            }
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginDTO loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user != null && await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                //Create the JWT Token
                var tokenProvider = new TokenProvider(_configuration);
                var token = tokenProvider.CreateAccessToken(user);

                Response.Cookies.Append("jwt", token.AccessToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = token.Expiration
                });
                await _signInManager.SignInAsync(user, isPersistent: false);

                return RedirectToAction("Index", "Home");
            }
            return Unauthorized("Invalid login attempt");
        }

    }
}
