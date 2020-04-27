using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;

namespace IdentityServer4.Quickstart.UI
{
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEventService _events;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEventService events)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _events = events;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest();
            }

            var result = await _signInManager.PasswordSignInAsync(
                model.Username,
                model.Password,
                model.RememberLogin,
                lockoutOnFailure: true);

            if (result.Succeeded == false)
            {
                await _events.RaiseAsync(new UserLoginFailureEvent(model.Username, "invalid credentials"));
                return Unauthorized();
            }

            var user = await _userManager.FindByNameAsync(model.Username);
            await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName));

            var userResult = new UserDTO{
                Email = user.Email,
                Username = user.UserName,
            };

            return Ok();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            if (User?.Identity.IsAuthenticated == false)
            {
                return BadRequest();
            }

            await _signInManager.SignOutAsync();

            await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));

            return Ok();
        }

        [HttpPost("sign-up")]
        public async Task<IActionResult> Signup(SignUpDTO model)
        {
            var user = new ApplicationUser{
                Email = model.Email,
                UserName = model.UserName ?? model.Email.Split("@")[0]
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if(!result.Succeeded){
                return BadRequest(result.Errors);
            }

            return Ok();
        }

    }
}