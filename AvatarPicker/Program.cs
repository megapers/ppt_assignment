var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IImageRetrievalStrategy, LastDigit6To9Strategy>();
builder.Services.AddScoped<IImageService, ImageService>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseDefaultFiles(new DefaultFilesOptions
{
    DefaultFileNames = new List<string> { "index.html" }
});
app.UseStaticFiles();


app.MapGet("/avatar", async (string userIdentifier, IImageService imageService) =>
{
    var imageUrl = await imageService.GetImageUrlAsync(userIdentifier);
    return Results.Ok(new { url = imageUrl });
});

app.MapFallbackToFile("index.html");

app.Run();