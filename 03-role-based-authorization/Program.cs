var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
    
builder.Services.AddAuthentication()
    .AddCookie("cookie");

builder.Services.AddAuthorization(); // AddControllers (line 3) already adds calls this method

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();