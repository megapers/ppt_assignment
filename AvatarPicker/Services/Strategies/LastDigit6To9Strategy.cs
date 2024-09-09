using System.Text.Json;
using AvatarPicker.Models;
using System.Net.Http;

namespace AvatarPicker.Services.Strategies;

public class LastDigit6To9Strategy : IImageRetrievalStrategy
{
    private readonly HttpClient _httpClient;

    public LastDigit6To9Strategy(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public bool CanApply(string userIdentifier)
    {
        return char.IsDigit(userIdentifier[^1]) && "6789".Contains(userIdentifier[^1]);
    }

    public async Task<string> GetImageUrlAsync(string userIdentifier)
    {
        var lastDigit = userIdentifier[^1];
        var url = $"https://my-json-server.typicode.com/ck-pacificdev/tech-test/images/{lastDigit}";

        try
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var image = JsonSerializer.Deserialize<Image>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return image?.Url ?? "https://api.dicebear.com/8.x/pixel-art/png?seed=default&size=150";
        }
        catch (HttpRequestException)
        {
            return "https://api.dicebear.com/8.x/pixel-art/png?seed=default&size=150";
        }
    }
}