using GR.Notifications.Razor.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace GR.Notifications.Razor.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register ui module
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddNotificationRazorUiModule(this IServiceCollection services)
        {
            services.ConfigureOptions(typeof(NotificationRazorFileConfiguration));
            return services;
        }
    }
}
