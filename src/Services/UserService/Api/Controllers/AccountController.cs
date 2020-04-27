using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.AspNetCore.Authentication;

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

        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
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

            await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("userName", user.UserName));
            await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("email", user.Email));

            return Ok();
        }

    }
}