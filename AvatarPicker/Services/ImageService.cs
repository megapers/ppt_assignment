public class ImageService : IImageService
{
    private readonly IEnumerable<IImageRetrievalStrategy> _strategies;

    public ImageService(IEnumerable<IImageRetrievalStrategy> strategies)
    {
        _strategies = strategies;
    }

    public async Task<string> GetImageUrlAsync(string userIdentifier)
    {
        foreach (var strategy in _strategies)
        {
            if (strategy.CanApply(userIdentifier))
            {
                return await strategy.GetImageUrlAsync(userIdentifier);
            }
        }
        return "https://api.dicebear.com/8.x/pixel-art/png?seed=default&size=150";
    }
}