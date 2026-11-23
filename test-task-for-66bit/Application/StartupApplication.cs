using Application.DTO.EquipmentItem;
using Application.Interfaces.ServiceInterface;
using Application.Services;
using Application.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public class StartupApplication
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddScoped<IEquipmentTypeServce, EquipmentTypeService>();
            services.AddScoped<IManufacturerService, ManufacturerService>();
            services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));
            services.AddScoped<IEquipmentItemService, EquipmentItemService>();
            services.AddScoped<IValidator<CreateEquipmentItemDto>, CreateEquipmentItemValidator>();
        }
    }
}
