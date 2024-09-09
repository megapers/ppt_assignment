namespace AvatarPicker.Services.Strategies;

public class DefaultStrategy : IImageRetrievalStrategy
{
    public bool CanApply(string userIdentifier)
    {
        return true; // This strategy always applies as a fallback
    }

    public Task<string> GetImageUrlAsync(string userIdentifier)
    {
        return Task.FromResult("https://api.dicebear.com/8.x/pixel-art/png?seed=default&size=150");
    }
}