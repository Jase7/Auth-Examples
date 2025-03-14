using System.ComponentModel.Design;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

const string authScheme = "cookie";
const string authScheme2 = "cookie2";

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddAuthentication(authScheme)
    .AddCookie(authScheme)
    .AddCookie(authScheme2);

builder.Services
    .AddAuthorization(authBuilder =>
    {
        authBuilder.AddPolicy("EU passport", policyBuilder =>
        {
            policyBuilder.RequireAuthenticatedUser()
                .AddAuthenticationSchemes(authScheme)
                .RequireClaim("passport_type", "eur");
        });
    });

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization(); // To avoid boilerplate and tons of headaches, we have this middleware to help us out

/*
 Let's think we have to build a middleware to check permissions
 It could be something like this middleware below 
 */

// app.Use((context, next) =>
// {
//
//     if (context.Request.Path.StartsWithSegments("/login"))
//     {
//         return next();
//     }
//     
//     if (context.User.Identities.All(identity => identity.AuthenticationType != authScheme))
//     {
//         context.Response.StatusCode = 401;
//         return Task.CompletedTask;
//     }
//     
//     if (!context.User.HasClaim("passport_type", "eur"))
//     {
//         context.Response.StatusCode = 403;
//         return Task.CompletedTask;        
//     }
//
//     return next();
// });

app.MapGet("/unsecure", (HttpContext context) => context.User.FindFirst("usr")?.Value ?? "empty");

app.MapGet("/sweden", (HttpContext context) =>
{
    // if (context.User.Identities.All(identity => identity.AuthenticationType != authScheme2))
    // {
    //     context.Response.StatusCode = 401;
    //     return "not-authenticated";
    // }
    //
    // if (!context.User.HasClaim("passport_type", "eur"))
    // {
    //     context.Response.StatusCode = 403;
    //     return "not-allowed";        
    // }
    
    return "allowed";
    
}).RequireAuthorization("EU passport");;

/*
 * As long as our endpoints grow, we are arriving to a problem with authorization
 */
app.MapGet("/norway", (HttpContext context) =>
{
    // if (context.User.Identities.All(identity => identity.AuthenticationType != authScheme2))
    // {
    //     context.Response.StatusCode = 401;
    //     return "not-authenticated";
    // }
    //
    // if (!context.User.HasClaim("passport_type", "NOR"))
    // {
    //     context.Response.StatusCode = 403;
    //     return "not-allowed";        
    // }
    
    return "allowed";
    
});

app.MapGet("/denmark", (HttpContext context) =>
{
    /*
     * What happens if we have several schemes? Things start to complicate for us...
    */
    
    // if (context.User.Identities.All(identity => identity.AuthenticationType != authScheme2))
    // {
    //     context.Response.StatusCode = 401;
    //     return "not-authenticated";
    // }
    //
    // if (!context.User.HasClaim("passport_type", "DEN"))
    // {
    //     context.Response.StatusCode = 403;
    //     return "not-allowed";        
    // }
    
    return "allowed";
    
});

app.MapGet("/login", async context =>
{
    List<Claim> claims = [
        new("usr", "Luismi"),
        new("passport_type", "eur")
    ];
    var identity = new ClaimsIdentity(claims, authScheme);
    var user = new ClaimsPrincipal(identity);
    
    await context.SignInAsync(authScheme, user);
}).AllowAnonymous();

app.Run();