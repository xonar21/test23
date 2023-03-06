using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GR.Core;
using GR.Core.Attributes.Documentation;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.Core.Helpers.Global;
using GR.Core.Helpers.Responses;
using GR.Identity.Abstractions;
using GR.Notifications.Abstractions;
using GR.Notifications.Abstractions.Models.Notifications;
using GR.Notifications.Abstractions.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

// ReSharper disable UnusedTypeParameter
#pragma warning disable 1998

namespace GR.Notifications
{
    [Author(Authors.LUPEI_NICOLAE)]
    public class Notify<TContext, TRole, TUser> : INotify<TRole> where TContext : IdentityDbContext<TUser, TRole, string> where TRole : IdentityRole<string> where TUser : IdentityUser<string>
    {
        #region Injectable

        /// <summary>
        /// Inject data service
        /// </summary>
        private readonly INotificationsContext _notificationsContext;
        /// <summary>
        /// Context
        /// </summary>
        private readonly TContext _context;
        /// <summary>
        /// Notification hub
        /// </summary>
        private readonly ICommunicationHub _hub;
        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger<Notify<TContext, TRole, TUser>> _logger;

        /// <summary>
        /// Inject user manager
        /// </summary>
        private readonly IUserManager<GearUser> _userManager;

        /// <summary>
        /// Inject mapper
        /// </summary>
        private readonly IMapper _mapper;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="hub"></param>
        /// <param name="logger"></param>
        /// <param name="userManager"></param>
        /// <param name="mapper"></param>
        /// <param name="notificationsContext"></param>
        public Notify(TContext context, ICommunicationHub hub, ILogger<Notify<TContext, TRole, TUser>> logger, IUserManager<GearUser> userManager, IMapper mapper, INotificationsContext notificationsContext)
        {
            _context = context;
            _hub = hub;
            _logger = logger;
            _userManager = userManager;
            _mapper = mapper;
            _notificationsContext = notificationsContext;
        }

        /// <inheritdoc />
        /// <summary>
        /// Send Notification
        /// </summary>
        /// <param name="roles"></param>
        /// <param name="notification"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public virtual async Task SendNotificationAsync(IEnumerable<TRole> roles, Notification notification, Guid? tenantId = null)
        {
            var usersRequest = await _userManager.GetUsersInRolesAsync((IEnumerable<GearRole>)roles, tenantId);
            if (!usersRequest.IsSuccess) return;
            var usersUniques = usersRequest.Result.Select(x =>Guid.Parse(x.Id)).ToList();
            await SendNotificationAsync(usersUniques, notification);
        }

        /// <inheritdoc />
        /// <summary>
        /// Send notification
        /// </summary>
        /// <param name="users"></param>
        /// <param name="notificationType"></param>
        /// <param name="subject"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public virtual async Task SendNotificationAsync(IEnumerable<Guid> users, Guid notificationType, string subject,
            string content)
        {
            await SendNotificationAsync(users, new Notification
            {
                Content = content,
                Subject = subject,
                NotificationTypeId = notificationType
            });
        }
        /// <inheritdoc />
        /// <summary>
        /// Send notification to all users
        /// </summary>
        /// <param name="notification"></param>
        /// <returns></returns>
        public virtual async Task SendNotificationAsync(Notification notification)
        {
            var users = _context.Users.Select(x => Guid.Parse(x.Id)).ToList();
            await SendNotificationAsync(users, notification);
        }

        /// <inheritdoc />
        /// <summary>
        /// Send Notification
        /// </summary>
        /// <param name="usersIds"></param>
        /// <param name="notification"></param>
        /// <returns></returns>
        public virtual async Task SendNotificationAsync(IEnumerable<Guid> usersIds, Notification notification)
        {
            if (notification == null) throw new NullReferenceException();
            var users = usersIds.ToList();
            if (!users.Any()) return;
            foreach (var userId in users)
            {
                var user = await _userManager.UserManager.FindByIdAsync(userId.ToString());
                if (user == null) continue;
                notification.UserId = userId;

                await _notificationsContext.Notifications.AddAsync(notification);
            }

            var dbResponse = await _notificationsContext.PushAsync();
            if (!dbResponse.IsSuccess)
            {
                _logger.LogError("Fail to save notifications");
            }

            var mapped = _mapper.Map<SystemNotifications>(notification);
            _hub.SendNotification(users, mapped);
        }

        /// <inheritdoc />
        /// <summary>
        /// Send notification to system admins
        /// </summary>
        /// <param name="notification"></param>
        /// <returns></returns>
        public virtual async Task SendNotificationToSystemAdminsAsync(Notification notification)
        {
            if (!(await _context.Roles.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Name.Equals(GlobalResources.Roles.ADMINISTRATOR)) is GearRole role)) return;
            var usersRequest = await _userManager.GetUsersInRolesAsync(new List<GearRole> { role });
            if (!usersRequest.IsSuccess) return;
            var users = usersRequest.Result.Select(x => Guid.Parse((x.Id))).ToList();
            await SendNotificationAsync(users, notification);
        }

        /// <inheritdoc />
        /// <summary>
        /// Get notifications by user id
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="onlyUnread"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<Notification>>> GetNotificationsByUserIdAsync(Guid userId, bool onlyUnread = false)
        {
            var query = _notificationsContext.Notifications
                .Where(x => x.UserId.Equals(userId));

            if (onlyUnread) query = query.Where(x => !x.IsDeleted);

            var notifications = await query
                    .OrderByDescending(x => x.Created)
                .ToListAsync();
            return new SuccessResultModel<IEnumerable<Notification>>(notifications);
        }

        /// <summary>
        /// Get notifications with pagination
        /// </summary>
        /// <param name="page"></param>
        /// <param name="perPage"></param>
        /// <param name="onlyUnread"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<PaginatedNotificationsViewModel>> GetUserNotificationsWithPaginationAsync(uint page = 1, uint perPage = 10, bool onlyUnread = false)
        {
            var userRequest = _userManager.FindUserIdInClaims();
            if (!userRequest.IsSuccess) return userRequest.Map<PaginatedNotificationsViewModel>();
            var userId = userRequest.Result;
            var query = _notificationsContext.Notifications.Where(x => x.UserId.Equals(userId));

            if (onlyUnread) query = query.Where(x => !x.IsDeleted);

            var paginatedResult = await query
                    .OrderByDescending(x => x.Created)
                .GetPagedAsync((int)page, (int)perPage);

            var result = new PaginatedNotificationsViewModel
            {
                PerPage = perPage,
                Page = page,
                Total = (uint)await query.CountAsync(),
                Notifications = paginatedResult.Result
            };

            return new SuccessResultModel<PaginatedNotificationsViewModel>(result);
        }

        /// <inheritdoc />
        /// <summary>
        /// Get notification by id
        /// </summary>
        /// <param name="notificationId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<Notification>> GetNotificationByIdAsync(Guid? notificationId)
        {
            if (notificationId == null) return new NotFoundResultModel<Notification>();
            var notification = await _notificationsContext.Notifications.FirstOrDefaultAsync(x => x.Id == notificationId);
            if (notification == null)
                return new NotFoundResultModel<Notification>();
            return new SuccessResultModel<Notification> {Result = notification};
        }

        /// <inheritdoc />
        /// <summary>
        /// Mark notification as read 
        /// </summary>
        /// <param name="notificationId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<Guid>> MarkAsReadAsync(Guid notificationId)
        {
            if (notificationId == Guid.Empty) return new NotFoundResultModel<Guid>();
            var notification = await GetNotificationByIdAsync(notificationId);
            if (notification.IsSuccess)
            {
                notification.Result.IsDeleted = true;
                _notificationsContext.Notifications.Update(notification.Result);
            }
            var response = await _notificationsContext.PushAsync();
            return new ResultModel<Guid> { IsSuccess = true, Result = notificationId, Errors = response.Errors };
        }

        /// <inheritdoc />
        /// <summary>
        /// Delete notification
        /// </summary>
        /// <param name="notificationId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> PermanentlyDeleteNotificationAsync(Guid? notificationId)
            => await _notificationsContext.RemovePermanentRecordAsync<Notification>(notificationId);

        /// <summary>
        /// Clear all notifications
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> ClearAllUserNotificationsAsync(Guid userId)
        {
            if (userId == Guid.Empty) return new InvalidParametersResultModel();
            var notificationsRequest = await GetNotificationsByUserIdAsync(userId);
            if (!notificationsRequest.IsSuccess) return notificationsRequest.ToBase();
            var notifications = notificationsRequest.Result.ToList();
            _notificationsContext.Notifications.RemoveRange(notifications);
            var result = await _notificationsContext.PushAsync();

            return result;
        }

        /// <summary>
        /// Mark all user notifications as read
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ResultModel> MarkAllUserNotificationsAsReadAsync(Guid userId)
        {
            if (userId == Guid.Empty) return new InvalidParametersResultModel();
            var notificationsRequest = await GetNotificationsByUserIdAsync(userId);
            if (!notificationsRequest.IsSuccess) return notificationsRequest.ToBase();
            var notifications = notificationsRequest.Result.ToList();
            foreach (var notification in notifications)
            {
                notification.IsDeleted = true;
            }
            _notificationsContext.Notifications.UpdateRange(notifications);
            var result = await _notificationsContext.PushAsync();

            return result;
        }

        /// <inheritdoc />
        /// <summary>
        /// Check if user is online
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual bool IsUserOnline(Guid userId) => userId != Guid.Empty && _hub.GetUserOnlineStatus(userId);

        /// <summary>
        /// Send notification
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> SendAsync(string subject, string message, string to)
        {
            await SendNotificationAsync(new List<Guid> { to.ToGuid() }, new Notification
            {
                Subject = subject,
                Content = message
            });
            return new ResultModel
            {
                IsSuccess = true
            };
        }

        /// <summary>
        /// Send user real time notification
        /// </summary>
        /// <typeparam name="TUser1"></typeparam>
        /// <param name="user"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<ResultModel> SendAsync<TUser1>(TUser1 user, string subject, string message) where TUser1 : IdentityUser<Guid>
        {
            return await SendAsync(subject, message, user.Id.ToString());
        }

        /// <summary>
        /// Get provider
        /// </summary>
        /// <returns></returns>
        public virtual object GetProvider() => this;
    }
}
