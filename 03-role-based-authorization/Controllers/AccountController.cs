using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace _03_role_based_authorization.Controllers;

[ApiController]
public class AccountController : ControllerBase
{
    [HttpGet("/login")]
    public IActionResult Login() =>
        SignIn(new ClaimsPrincipal(
                new ClaimsIdentity(
                    [
                        new Claim(ClaimTypes.Name, Guid.NewGuid().ToString()),
                        new Claim("my_role", "admin")
                    ],
                    "cookie",
                    nameType: null,
                    roleType: "my_role"
                )
            ),
            authenticationScheme: "cookie"
        );
}