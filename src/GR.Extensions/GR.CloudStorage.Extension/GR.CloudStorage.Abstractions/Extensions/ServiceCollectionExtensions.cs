using Microsoft.Extensions.Configuration;
using GR.CloudStorage.Abstractions.Models;
using GR.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace GR.CloudStorage.Abstractions.Extensions
{
    public static class ServiceCollectionExtensions
    {


        /// <summary>
        /// Add contact module
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddStorageBaseServiceModule<TService>(this IServiceCollection services)
            where TService : class, IStorageBaseService
        {
            services.AddGearTransient<IStorageBaseService, TService>();
            return services;
        }


        /// <summary>
        /// Add contact module
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddUserTokenDataServiceModule<TService>(this IServiceCollection services)
            where TService : class, IUserTokenDataService
        {
            services.AddGearTransient<IUserTokenDataService, TService>();
            return services;
        }

        /// <summary>
        /// Add contact module
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCloudUserAuthorizationServiceModule<TService>(this IServiceCollection services)
            where TService : class, ICloudUserAuthorizationService
        {
            services.AddGearTransient<ICloudUserAuthorizationService, TService>();
            return services;
        }


        /// <summary>
        /// Bind Email settings
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection OneDriveSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureWritable<CloudServiceSettings>(configuration.GetSection("OneDriveSettings"));
            return services;
        }

        /// <summary>
        /// Add contact module
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCloudStorage<TService>(this IServiceCollection services)
            where TService : class, ICloudStorageDb
        {
            services.AddGearTransient<ICloudStorageDb, TService>();
            return services;
        }
    }
}
