using AvatarPicker.API;
using AvatarPicker.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.InstallServices(builder.Configuration, typeof(IServiceinstaller).Assembly);

var app = builder.Build();

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();
app.RegisterEndPoints();
app.MapFallbackToFile("index.html");

app.Run();