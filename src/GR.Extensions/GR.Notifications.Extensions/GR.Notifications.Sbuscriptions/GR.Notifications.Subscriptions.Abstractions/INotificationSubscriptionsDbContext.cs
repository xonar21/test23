using GR.Core.Abstractions;
using GR.Notifications.Subscribe.Abstract.Models;
using Microsoft.EntityFrameworkCore;

namespace GR.Notifications.Subscribe.Abstract
{
    public interface INotificationSubscriptionsDbContext : IDbContext
    {
        /// <summary>
        /// Notification events
        /// </summary>
        DbSet<NotificationEvent> NotificationEvents { get; set; }
        /// <summary>
        /// Role events subscriptions
        /// </summary>
        DbSet<NotificationSubscription> NotificationSubscriptions { get; set; }

        /// <summary>
        /// Notification templates
        /// </summary>
        DbSet<NotificationTemplate> NotificationTemplates { get; set; }
    }
}