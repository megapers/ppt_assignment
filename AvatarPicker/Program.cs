using Microsoft.EntityFrameworkCore;
using AvatarPicker.Data;
using AvatarPicker.Services.Strategies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AvatarDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient();
builder.Services.AddScoped<IImageRetrievalStrategy, NonAlphanumericStrategy>();
builder.Services.AddScoped<IImageRetrievalStrategy, ContainsVowelStrategy>();
builder.Services.AddScoped<IImageRetrievalStrategy, LastDigit6To9Strategy>();
builder.Services.AddScoped<IImageRetrievalStrategy, LastDigit1To5Strategy>();
builder.Services.AddScoped<IImageRetrievalStrategy, DefaultStrategy>(); // Add this line
builder.Services.AddScoped<IImageService, ImageService>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/avatar", async (string userIdentifier, IImageService imageService) =>
{
    var imageUrl = await imageService.GetImageUrlAsync(userIdentifier);
    return Results.Ok(new { url = imageUrl });
});

app.MapFallbackToFile("index.html");

app.Run();