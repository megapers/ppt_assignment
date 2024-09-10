namespace AvatarPicker.Services.Strategies;

public class NonAlphanumericStrategy : IImageRetrievalStrategy
{
    private readonly Random _random = new Random();

    public bool CanApply(string userIdentifier)
    {
        return userIdentifier.Any(c => !char.IsLetterOrDigit(c));
    }

    public Task<string> GetImageUrlAsync(string userIdentifier)
    {
        int randomNumber = _random.Next(1, 6);
        return Task.FromResult($"https://api.dicebear.com/8.x/pixel-art/png?seed={randomNumber}&size=150");
    }
}