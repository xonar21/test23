using GR.Core.Extensions;
using GR.Crm.Contracts.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gr.Crm.Comments.Abstractions.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add comments services
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>;

        public static IServiceCollection AddCrmCommentsModule<TService, TContext>(this IServiceCollection services)
            where TService : class, ICommentService
            where TContext : DbContext, ICommentContext
        {
            services.AddGearTransient<ICommentService, TService>();
            services.AddTransient<ICommentContext, TContext>();
            return services;
        }
    }
}
