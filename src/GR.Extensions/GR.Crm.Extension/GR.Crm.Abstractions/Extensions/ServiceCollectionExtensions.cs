using System;
using GR.Audit.Abstractions;
using GR.Audit.Abstractions.Helpers;
using GR.Core;
using GR.Core.Events;
using GR.Core.Extensions;
using GR.Core.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GR.Crm.Abstractions.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register Crm module
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCrmModule<TService>(this IServiceCollection services)
            where TService : class, ICrmService
        {
            services.AddTransient<ICrmService, TService>();
            return services;
        }

        /// <summary>
        /// Add Crm storage
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IServiceCollection AddCrmModuleStorage<TDbContext>(
            this IServiceCollection services, Action<DbContextOptionsBuilder> options)
            where TDbContext : DbContext, ICrmContext
        {
            Arg.NotNull(services, nameof(AddCrmModuleStorage));
            services.AddDbContext<TDbContext>(options, ServiceLifetime.Transient);
            services.AddGearTransient<ICrmContext, TDbContext>();
            SystemEvents.Database.OnMigrate += (sender, args) =>
            {
                GearApplication.GetHost<IWebHost>().MigrateDbContext<TDbContext>();
            };
            return services;
        }


        /// <summary>
        /// Add JobPosition module
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCrmJobPositionModule<TService, TContext>(this IServiceCollection services)
            where TService : class, IVocabulariesService
            where TContext : DbContext, ICrmContext
        {
            services.AddGearTransient<IVocabulariesService, TService>();
            services.AddTransient<ICrmContext, TContext>();
            return services;
        }

        public static IServiceCollection AddCrmAuditModule<TService, TContext>(this IServiceCollection services)
            where TService : class, ICrmService
            where TContext : DbContext, ITrackerDbContext
        {
            services.AddGearTransient<ICrmService, TService>();
            services.AddTransient<ITrackerDbContext, TContext>();
            return services;
        }

        /// <summary>
        /// Add Merge module
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCrmMergeModule<TService, TContext>(this IServiceCollection services)
            where TService : class, ICrmMergeService
            where TContext : DbContext, ICrmContext
        {
            services.AddGearTransient<ICrmMergeService, TService>();
            services.AddTransient<ICrmContext, TContext>();
            return services;
        }

        /// <summary>
        /// Add CampaignType module
        /// </summary>
        public static IServiceCollection AddCrmCampaignTypeModule<TService, TContext>(this IServiceCollection services)
            where TService : class, IVocabulariesService
            where TContext : DbContext, ICrmContext
        {
            services.AddGearTransient<IVocabulariesService, TService>();
                services.AddTransient<ICrmContext, TContext>();
            return services;
        }
        
        /// <summary>
        /// Add ImportExport Module
        /// </summary
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>

        public static IServiceCollection AddCrmImportExportModule<TService, TContext>(this IServiceCollection services)
            where TService : class, ICrmImportExportService
            where TContext : DbContext, ICrmContext
        {
            services.AddGearTransient<ICrmImportExportService, TService>();
            services.AddTransient<ICrmContext, TContext>();
            return services;
        }

        /// <summary>
        /// Add notification module
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCrmNotificationModule<TService, TContext>(this IServiceCollection services)
            where TService : class, ICrmNotificationService
            where TContext : DbContext, ICrmContext
        {
            services.AddGearTransient<ICrmNotificationService, TService>();
            services.AddTransient<ICrmContext, TContext>();
            return services;
        }
    }
}
