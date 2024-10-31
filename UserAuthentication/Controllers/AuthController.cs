using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using ECommerceWebMicroservices.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Shared;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace UserAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _config;
        public AuthController(UserManager<IdentityUser> userManager, 
                              SignInManager<IdentityUser> signInManager,
                              IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Register model)
        {
            if (model == null)
            {
                return BadRequest(ModelState);
            }
            var user = new IdentityUser { UserName = model.Username, Email = model.EmailAddress };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded) 
            {
                return Ok(new { message = "User registered successfully" });
            }
            return BadRequest();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            var user = await _userManager.FindByNameAsync(login.Username);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (user == null)
            {
                return Unauthorized();
            }
            var result = await _signInManager.PasswordSignInAsync(login.Username, login.Password, login.RememberMe, false);
            if (result.Succeeded)
            {
                var token = GenerateJwtToken(user);
                Console.WriteLine($"==Token: {token}");
                return Ok(new { message = "Login successful" });
            }
            return Unauthorized(new { message = "Invalid username or password" });
        }
        [HttpPut("updateprofile")]
        public async Task<IActionResult> UpdateProfie([FromBody] UpdateProfile userProfile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userManager.FindByNameAsync(userProfile.Username);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (userProfile.PhoneNumber != phoneNumber)
            {
                var setPhoneNumber = await _userManager.SetPhoneNumberAsync(user, phoneNumber);
                if (!setPhoneNumber.Succeeded)
                {
                    return BadRequest(new { message = "Unexpected error when trying to set phone number." });
                }
            }
            await _signInManager.RefreshSignInAsync(user);
            return Ok(new { message = "Your profile has been updated" });
        }
        [HttpPost("updatepassword")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePassword updatePassword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByNameAsync(updatePassword.Username);
            if (user == null) 
            { 
                return NotFound(new { message = "User not found" } );
            }
            var changePasswordResult = await _userManager.ChangePasswordAsync(user, updatePassword.OldPassword, updatePassword.NewPassword);
            if (changePasswordResult.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                return Ok(new { message = "Password Changed" });
            }
            return BadRequest(changePasswordResult);
        }

        [HttpGet("test")]
        public IActionResult GetTest()
        {
            Console.WriteLine("====>Test endpoint hit via Ocelot Gateway!");
            return Ok("====>Console output logged successfully");
        }
        private string GenerateJwtToken(IdentityUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Jwt:Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] 
                { 
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
