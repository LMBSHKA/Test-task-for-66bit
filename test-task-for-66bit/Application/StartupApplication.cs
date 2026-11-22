using Application.Interfaces.ServiceInterface;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public class StartupApplication
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddScoped<IEquipmentTypeServce, EquipmentTypeService>();
        }
    }
}
