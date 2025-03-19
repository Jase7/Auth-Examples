using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace _03_role_based_authorization_identity.Controllers;

[ApiController]
public class AccountController : ControllerBase
{
    [HttpGet("/login")]
    public async Task<IActionResult> Login(SignInManager<IdentityUser> signInManager)
    {
        await signInManager.PasswordSignInAsync("test@test.com", "password", isPersistent: false, lockoutOnFailure: false);

        return Ok();
    }
}