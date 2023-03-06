using GR.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Crm.BussinesUnits.Abstractions.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCrmBusinessUnitModule<TService, TContext>(this IServiceCollection services)
            where TService : class, IBusinessUnitService
            where TContext : DbContext, IBusinessUnitContext
        {
            services.AddGearTransient<IBusinessUnitService, TService>();
            services.AddTransient<IBusinessUnitContext, TContext>();
            return services;
        }
    }
}
