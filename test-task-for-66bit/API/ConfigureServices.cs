using Application;
using Application.Interfaces.Storage;
using Data;
using Data.Storage;

namespace API
{
    public class ConfigureServices
    {
        public static void Configure(IServiceCollection services)
        {
            StartupApplication.Configure(services);
            StartupData.Configure(services);
            services.AddScoped<IEquipmentPhotoStorage>(provider => new LocalEquipmentPhotoStorage(
                provider.GetRequiredService<IWebHostEnvironment>().WebRootPath));
        }
    }
}
