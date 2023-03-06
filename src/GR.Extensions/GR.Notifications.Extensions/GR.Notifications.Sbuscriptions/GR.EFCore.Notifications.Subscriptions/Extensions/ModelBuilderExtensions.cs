using GR.Notifications.Subscribe.Abstract.Models;
using GR.Notifications.Subscriptions.Data;
using Microsoft.EntityFrameworkCore;

namespace GR.Notifications.Subscriptions.Extensions

{
    internal static class ModelBuilderExtensions
    {
        /// <summary>
        /// Db configurations
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static ModelBuilder RegisterNotificationDbContextBuilder(this ModelBuilder builder)
        {
            //register schema
            builder.HasDefaultSchema(NotificationsSubscriptionDbContext.Schema);
            //register composite key
            builder.Entity<NotificationSubscription>().HasKey(p => new { p.NotificationEventId, p.RoleId });

            return builder;
        }
    }
}
