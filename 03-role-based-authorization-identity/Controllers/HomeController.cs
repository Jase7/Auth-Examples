using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _03_role_based_authorization_identity.Controllers;

[ApiController]
public class HomeController : ControllerBase
{
    [HttpGet("/")]
    public string Index() => "Index route";
    
    [HttpGet("/secret")]
    [Authorize(Roles = "admin")]
    public string Secret() => "Secret route";
}