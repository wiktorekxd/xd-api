using DummyNetflixApi.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DummyNetflixApi.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _config;
        private readonly SignInManager<IdentityUser> _signInManager;

        public UsersController(UserManager<IdentityUser> userManager, IConfiguration config, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _config = config;
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewUser(UserCreateModel userCreateModel)
        {
            var existingUser = await _userManager.FindByEmailAsync(userCreateModel.Email);

            if (existingUser != null)
                return BadRequest();

            var identityUser = new IdentityUser();
            identityUser.Email = userCreateModel.Email;
            identityUser.UserName = userCreateModel.Email;

            var identityResult = await _userManager.CreateAsync(identityUser, userCreateModel.Password);
            if(identityResult.Succeeded)
                return Ok();
            else
                return BadRequest();
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginCreateModel userLoginModel)
        {
            var result = await _signInManager.PasswordSignInAsync(userLoginModel.Email, userLoginModel.Password, false, false);
            if (result.Succeeded)
            {
                var identityUser = await _userManager.FindByNameAsync(userLoginModel.Email);
                var jwtTokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_config["JWTOption:Secret"]);
                var subject = new ClaimsIdentity(new[]{
                            new Claim("Id", identityUser.Id),
                            new Claim(JwtRegisteredClaimNames.Email, identityUser.Email) });

                var jwtToken = jwtTokenHandler.CreateEncodedJwt(new SecurityTokenDescriptor()
                {
                    Subject = subject,
                    IssuedAt = DateTime.UtcNow,
                    Expires = DateTime.UtcNow.AddHours(6),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
                });

                return Ok(new { token = jwtToken });
            }
            else
            {
                return BadRequest();
            }
        }

    }
}
