using AutoMapper;
using WebVilla.Logging;
using WebVilla.MapModels.VillaMappers;
using WebVilla.Repozitories.RepozitoryServices;

namespace WebVilla.ExtensionMethods.AddDIService
{
    public static class AddDIService
    {
        public static IServiceCollection AddServicesToLifeTimeDI(this IServiceCollection services)
        {
            // Add services to the container

            services.AddScoped<IVillaRepozitory, VillaRepozitory>();
            services.AddScoped<IVillaNumberRepozitory, VillaNumberRepozitory>();
            services.AddScoped<IUserRepozitory, UserRepozitory>();

            // Add log services to the container

            services.AddSingleton<ILogging, Logging.Logging>();

            // Add automapper services to the container

            services.AddAutoMapper(typeof(MapperDto));


            return services;
        }
    }
}
