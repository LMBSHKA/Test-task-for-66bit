using Application;
using Data;

namespace API
{
    public class ConfigureServices
    {
        public static void Configure(IServiceCollection services)
        {
            StartupApplication.Configure(services);
            StartupData.Configure(services);
        }
    }
}
