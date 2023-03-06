using GR.Identity.LdapAuth.Abstractions.Exceptions;
using GR.Identity.LdapAuth.Abstractions.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GR.Identity.LdapAuth.Abstractions.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add Ldap module
        /// </summary>
        /// <typeparam name="TUser"></typeparam>
        /// <typeparam name="TLdapService"></typeparam>
        /// <typeparam name="TLdapUserManager"></typeparam>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddIdentityLdapModule<TUser, TLdapService, TLdapUserManager>(this IServiceCollection services, IConfiguration configuration)
            where TUser : LdapUser
            where TLdapService : class, ILdapService<TUser>
            where TLdapUserManager : BaseLdapUserManager<TUser>
        {
            //Add Active directory dependencies
            services.AddTransient<ILdapService<TUser>, TLdapService>();
            services.AddTransient<TLdapUserManager>();
            services.AddTransient<BaseLdapUserManager<TUser>, TLdapUserManager>();
            var config = configuration.GetSection(nameof(LdapSettings));
            if (!config.Exists())
            {
                throw new LdapSettingsNotFoundException();
            }
            services.Configure<LdapSettings>(config);
            return services;
        }

        /// <summary>
        /// Add ldap auth
        /// </summary>
        /// <typeparam name="TAuthorizeService"></typeparam>
        /// <param name="services"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IServiceCollection AddLdapAuthentication<TAuthorizeService>(this IServiceCollection services, Action<LdapConfiguration> config)
            where TAuthorizeService : class, ILdapAuthorizeService
        {
            services.AddTransient<ILdapAuthorizeService, TAuthorizeService>();

            var configuration = new LdapConfiguration();
            config(configuration);
            services.AddSingleton(configuration);
            return services;
        }
    }
}
