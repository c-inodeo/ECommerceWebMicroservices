using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ECommerceWebMicroservices.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using UserAuthentication.Services;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using UserNotificationMessages.Helpers;

namespace UserAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _config;
        private readonly IMessageBusClientRabbitMQProducer _producer;

        public AuthController(UserManager<IdentityUser> userManager, 
                              SignInManager<IdentityUser> signInManager,
                              IConfiguration config, 
                              IMessageBusClientRabbitMQProducer producer)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
            _producer = producer;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] ECommerceWebMicroservices.Models.Register model)
        {
            if (model == null)
            {
                return BadRequest(ModelState);
            }
            var user = new IdentityUser { UserName = model.Username, Email = model.EmailAddress };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded) 
            {
                _producer.SendMessage(new UserNotifModel { UserId = model.Username, Message = "welcome to E-commerce site!", TimeStamp = DateTime.Today });
                return Ok(new { message = "User registered successfully" });
            }
            return BadRequest();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] ECommerceWebMicroservices.Models.Login login)
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
                var cartRedirectUrl = $"{_config["ProductCatalog:BaseUrl"]}/api/cart";
                Console.WriteLine($"==Token: {token}");

                _producer.SendMessage(new UserNotifModel { UserId = login.Username, Message = "Welcome back!", TimeStamp = DateTime.Today});
                return Ok(new 
                { 
                    message = "Login successful!",
                    token = token,
                    redirectUrl = cartRedirectUrl
                });
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
            Console.WriteLine("TEEEEEEEESSTT--2");
            Console.WriteLine("====>Test endpoint hit via Ocelot Gateway!");
            return Ok("====>TEEEEEEEESSTT--2");
        }
        private string GenerateJwtToken(IdentityUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Jwt:Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] 
                { 
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
