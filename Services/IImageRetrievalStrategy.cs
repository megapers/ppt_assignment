public interface IImageRetrievalStrategy
{
    bool CanApply(string userIdentifier);
    Task<string> GetImageUrlAsync(string userIdentifier);
}