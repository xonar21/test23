using GR.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Crm.Marketing.Abstractions.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add campaign module
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCrmCampaignModule<TService, TContext>(this IServiceCollection services)
            where TService : class, ICrmCampaignService
            where TContext : DbContext, ICrmCampaignContext
        {
            services.AddGearTransient<ICrmCampaignService, TService>();
            services.AddTransient<ICrmCampaignContext, TContext>();
            return services;
        } 
        
        /// <summary>
        /// Add marketing list
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCrmMarketingListModule<TService, TContext>(this IServiceCollection services)
            where TService : class, ICrmMarketingListService
            where TContext : DbContext, ICrmMarketingListContext
        {
            services.AddGearTransient<ICrmMarketingListService, TService>();
            services.AddTransient<ICrmMarketingListContext, TContext>();
            return services;
        }
    }
}
