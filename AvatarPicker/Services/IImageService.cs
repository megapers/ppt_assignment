public interface IImageService
{
    Task<string> GetImageUrlAsync(string userIdentifier);
}