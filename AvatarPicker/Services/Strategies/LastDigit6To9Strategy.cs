using System.Text.Json;
using AvatarPicker.Models;

namespace AvatarPicker.Services.Strategies;

public class LastDigit6To9Strategy : IImageRetrievalStrategy
{
    private readonly IWebHostEnvironment _environment;

    public LastDigit6To9Strategy(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public bool CanApply(string userIdentifier)
    {
        return char.IsDigit(userIdentifier[^1]) && "6789".Contains(userIdentifier[^1]);
    }

    public async Task<string> GetImageUrlAsync(string userIdentifier)
    {
        var lastDigit = int.Parse(userIdentifier[^1].ToString());
        var jsonPath = Path.Combine(_environment.ContentRootPath, "Data", "db.json");
        var jsonContent = await File.ReadAllTextAsync(jsonPath);
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var data = JsonSerializer.Deserialize<JsonData>(jsonContent, options);

        var image = data?.Images.FirstOrDefault(i => i.Id == lastDigit);
        return image?.Url ?? "https://api.dicebear.com/8.x/pixel-art/png?seed=default&size=150";
    }

    private class JsonData
    {
        public List<Image> Images { get; set; }
    }
}