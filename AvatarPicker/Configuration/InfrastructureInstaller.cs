using AvatarPicker.Configuration;
using AvatarPicker.Data;
using AvatarPicker.Services.Strategies;
using Microsoft.EntityFrameworkCore;

public class InfrastructureInstaller : IServiceinstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AvatarDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

        services.AddHttpClient();
    }
}