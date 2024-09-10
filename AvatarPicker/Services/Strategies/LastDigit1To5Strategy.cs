using AvatarPicker.Data;

namespace AvatarPicker.Services.Strategies;

public class LastDigit1To5Strategy : IImageRetrievalStrategy
{
    private readonly AvatarDbContext _dbContext;

    public LastDigit1To5Strategy(AvatarDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool CanApply(string userIdentifier)
    {
        return char.IsDigit(userIdentifier[^1]) && "12345".Contains(userIdentifier[^1]);
    }

    public async Task<string> GetImageUrlAsync(string userIdentifier)
    {
        var lastDigit = int.Parse(userIdentifier[^1].ToString());
        var image = await _dbContext.Images.FindAsync(lastDigit);
        return image?.Url ?? "https://api.dicebear.com/8.x/pixel-art/png?seed=default&size=150";
    }
}

