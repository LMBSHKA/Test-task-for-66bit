using Application.Interfaces.RepoIntrefaces;
using Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Data
{
    public class StartupData
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddScoped<IEquipmentTypeRepo, EquipmentTypeRepo>();
            services.AddScoped<IWarehouseUnitOfWork, WarehouseUnitOfWork>();
            services.AddScoped<IManufacturerRepo, ManufacturerRepo>();
            services.AddScoped(typeof(IGenericRepo<>), typeof(GenericRepo<>));
            services.AddScoped<IEquipmentItemRepo, EquipmentItemRepo>();
            services.AddScoped<ICountryRepo, CountryRepo>();
        }
    }
}
