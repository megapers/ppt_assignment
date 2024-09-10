namespace AvatarPicker.API;

public static class AvatarModule
{
    public static void RegisterEndPoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/avatar");

        group.MapGet("/", async (string userIdentifier, IImageService imageService) =>
        {
            var imageUrl = await imageService.GetImageUrlAsync(userIdentifier);
            return Results.Ok(new { url = imageUrl });
        });
    }
}
