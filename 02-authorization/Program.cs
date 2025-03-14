var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/weatherforecast", () =>
{
    return "ok";
});

app.Run();