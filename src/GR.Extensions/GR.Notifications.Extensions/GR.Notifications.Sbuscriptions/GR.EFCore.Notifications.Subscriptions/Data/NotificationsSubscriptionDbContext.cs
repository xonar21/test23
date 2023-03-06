﻿using System;
using System.Threading.Tasks;
using GR.Audit.Contexts;
using GR.Core;
using GR.Core.Helpers;
using GR.Notifications.Abstractions;
using GR.Notifications.Subscribe.Abstract;
using GR.Notifications.Subscribe.Abstract.Models;
using GR.Notifications.Subscriptions.Extensions;
using Microsoft.EntityFrameworkCore;

namespace GR.Notifications.Subscriptions.Data

{
    public class NotificationsSubscriptionDbContext : TrackerDbContext, INotificationSubscriptionsDbContext
    {
        /// <summary>
        /// Context schema
        /// Do not remove this
        /// </summary>
        public const string Schema = "Notifications";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options"></param>
        public NotificationsSubscriptionDbContext(DbContextOptions<NotificationsSubscriptionDbContext> options) : base(options)
        {
        }

        #region Entities
        /// <summary>
        /// Notifications events
        /// </summary>
        public virtual DbSet<NotificationEvent> NotificationEvents { get; set; }

        /// <summary>
        /// Subscriptions
        /// </summary>
        public virtual DbSet<NotificationSubscription> NotificationSubscriptions { get; set; }

        /// <summary>
        /// Notification templates
        /// </summary>
        public virtual DbSet<NotificationTemplate> NotificationTemplates { get; set; }
        #endregion

        /// <summary>
        /// Configurations
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.RegisterNotificationDbContextBuilder();
        }


        /// <summary>
        /// Seed data
        /// </summary>
        /// <returns></returns>
        public override Task InvokeSeedAsync(IServiceProvider services)
        {
            GearApplication.BackgroundTaskQueue.PushBackgroundWorkItemInQueue(async (cancellationToken) =>
            {
                var service = IoC.Resolve<INotificationSeederService>();
                await service.SeedNotificationTypesAsync();
            });
            return Task.CompletedTask;
        }
    }
}