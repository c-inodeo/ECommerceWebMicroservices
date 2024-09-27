using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using ECommerceWebMicroservices.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Shared;

namespace UserAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Register model)
        {
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _signInManager.PasswordSignInAsync(login.Username, login.Password, login.RememberMe, false);
            if (result.Succeeded)
            {
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
    }
}
