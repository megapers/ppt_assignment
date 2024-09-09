namespace AvatarPicker.Services.Strategies;

public class ContainsVowelStrategy : IImageRetrievalStrategy
{
    public bool CanApply(string userIdentifier)
    {
        return userIdentifier.ToLower().Any(c => "aeiou".Contains(c));
    }

    public Task<string> GetImageUrlAsync(string userIdentifier)
    {
        return Task.FromResult("https://api.dicebear.com/8.x/pixel-art/png?seed=vowel&size=150");
    }
}