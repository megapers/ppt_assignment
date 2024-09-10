using AvatarPicker.Services.Strategies;

namespace AvatarPicker.Configuration;

public class ApplicationInstaller : IServiceinstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IImageRetrievalStrategy, NonAlphanumericStrategy>();
        services.AddScoped<IImageRetrievalStrategy, ContainsVowelStrategy>();
        services.AddScoped<IImageRetrievalStrategy, LastDigit6To9Strategy>();
        services.AddScoped<IImageRetrievalStrategy, LastDigit1To5Strategy>();
        services.AddScoped<IImageRetrievalStrategy, DefaultStrategy>();
        services.AddScoped<IImageService, ImageService>();
    }
}

