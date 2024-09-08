var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register new services
builder.Services.AddScoped<IImageRetrievalStrategy, LastDigit6To9Strategy>();
builder.Services.AddScoped<IImageService, ImageService>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseDefaultFiles(new DefaultFilesOptions
{
    DefaultFileNames = new List<string> { "index.html" }
});
app.UseStaticFiles();

// Add this Minimal API endpoint
app.MapGet("/avatar", async (string userIdentifier, IImageService imageService) =>
{
    var imageUrl = await imageService.GetImageUrlAsync(userIdentifier);
    return Results.Ok(new { url = imageUrl });
});

app.MapFallbackToFile("index.html");

app.Run();