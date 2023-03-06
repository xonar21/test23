using GR.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Crm.Emails.Abstractions.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCrmEmailModule<TService, TContext>(this IServiceCollection services)
            where TService : class, IEmailService
            where TContext : DbContext, IEmailContext
        {
            services.AddGearTransient<IEmailService, TService>();
            services.AddTransient<IEmailContext, TContext>();
            return services;
        }
    }
}
