using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<IdentityDbContext>(options => options.UseInMemoryDatabase("my_db"));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(identityOptions =>
{
    identityOptions.User.RequireUniqueEmail = false;

    identityOptions.Password.RequireDigit = false;
    identityOptions.Password.RequiredLength = 4;
    identityOptions.Password.RequireLowercase = false;
    identityOptions.Password.RequireUppercase = false;
    identityOptions.Password.RequireNonAlphanumeric = false;
})
    .AddEntityFrameworkStores<IdentityDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllers();

var app = builder.Build();

using var scope = app.Services.CreateScope();

var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
await roleManager.CreateAsync(new IdentityRole("admin"));


var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

var user = new IdentityUser { UserName = "test@test.com", Email = "test@test.com" };

await userManager.CreateAsync(user, password: "password");
await userManager.AddToRoleAsync(user, "admin");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();