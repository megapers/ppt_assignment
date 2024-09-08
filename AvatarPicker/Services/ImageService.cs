public class ImageService : IImageService
{
    private readonly IImageRetrievalStrategy _strategy;

    public ImageService(IImageRetrievalStrategy strategy)
    {
        _strategy = strategy;
    }

    public async Task<string> GetImageUrlAsync(string userIdentifier)
    {
        if (_strategy.CanApply(userIdentifier))
        {
            return await _strategy.GetImageUrlAsync(userIdentifier);
        }
        return "https://api.dicebear.com/8.x/pixel-art/png?seed=default&size=150";
    }
}