using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.Identity.Abstractions;
using GR.TaskManager.Abstractions;
using GR.TaskManager.Abstractions.Enums;
using TaskStatus = GR.TaskManager.Abstractions.Enums.TaskStatus;
using GR.Email.Abstractions;
using GR.Core.Helpers.Templates;
using Microsoft.AspNetCore.Mvc;
using GR.TaskManager.Abstractions.Extensions;
using GR.Notifications.Abstractions;
using GR.Notifications.Abstractions.Models.Notifications;

namespace GR.TaskManager.Services
{

    public sealed class TaskManagerNotificationService : ITaskManagerNotificationService
    {
        #region Injectable
        /// <summary>
        /// Inject Email Sender
        /// </summary>
        private readonly IEmailSender _emailSender;
        #endregion

        private readonly INotify<GearRole> _notify;
        private readonly IUserManager<GearUser> _identity;
        private readonly ITaskManagerContext _context;

        private const string TaskCreated = "Task #{0}  has been created for you by {1}.";
        private const string TaskUpdated = "Task #{0} has been updated by {1}.";
        private const string TaskCompleted = "Task #{0} has been completed by {1}.";
        private const string TaskChangeStatus = "Task #{0} change state {1} to {2} by {3}.";
        private const string TaskChangePriority = "Task #{0} change priority {1} to {2} by {3}.";
        private const string TaskRemoved = "Task #{0} has been removed.";
        private const string TaskTitle = "Task Notification";
        private const string TaskExpires = "Task #{0} expires tomorrow.";

        public TaskManagerNotificationService(IUserManager<GearUser> identity,
                                IEmailSender emailSender)
        {
            _notify = IoC.Resolve<INotify<GearRole>>();
            _identity = identity;
            _emailSender = emailSender;
            _context = IoC.Resolve<ITaskManagerContext>();
        }

        internal async Task AddTaskNotificationAsync(Abstractions.Models.Task task,IUrlHelper Url)
        {
            var listAssignedUserId = task.AssignedUsers.Select(s => s.UserId).ToList();

            await _notify.SendNotificationAsync(listAssignedUserId, new Notification
            {
                Subject = "Info",
                Content = string.Format(TaskCreated, task.TaskNumber, task.Author),
                SendEmail = false,
                Url = "/TaskManager/details?id=" + task?.Id
            });
            foreach (var userId in listAssignedUserId)
            {
                var user = _identity.UserManager.FindByIdAsync(userId.ToString());
                string body = $"Hi, {user.Result.UserFirstName}. You have been assigned to <b>{task.Name}</b> by {task.Author}.";
                await SendEmailNotifications(user, Url, task.Id.ToString(), "", body, "New task has been assigned");
            }
        }

        internal async Task UpdateTaskNotificationAsync(Abstractions.Models.Task task,
            IUrlHelper Url)
        {
            var listAssignedUserId = task.AssignedUsers.Select(s => s.UserId).ToList();
            var recipients = new List<Guid>();
            var taskUserRequest = await _identity.UserManager.FindByNameAsync(task.Author);
            if (taskUserRequest != null)
            {
                listAssignedUserId.Add(Guid.Parse(taskUserRequest.Id.ToString()));
                recipients.Add(taskUserRequest.Id.ToGuid());
            }
            recipients.AddRange(task.AssignedUsers.Select(s => s.UserId));
            recipients = recipients.Distinct().ToList();
            await _notify.SendNotificationAsync(recipients,
                new Notification
                {
                    Content = string.Format(TaskUpdated, task.TaskNumber, task.ModifiedBy),
                    Subject = TaskTitle,
                    NotificationTypeId = NotificationType.Info
                });
            foreach (var userId in listAssignedUserId)
            {
                var user = _identity.UserManager.FindByIdAsync(userId.ToString());
                string body = $"Hi, {user.Result.UserFirstName}. Task \"{task.Name}\", has been updated. On date: \"{task.Changed}\".";
                await SendEmailNotifications(user, Url, task.Id.ToString(), "Activity Modified", body, "Task update");
            }
        }

        internal async Task ChangeStatusTaskNotificationAsync(Abstractions.Models.Task task, TaskStatus lastStatus,
            IUrlHelper Url)
        {
            var listAssignedUserId = task.AssignedUsers.Select(s => s.UserId).ToList();
            var recipients = task.AssignedUsers.Select(s => s.UserId).ToList();
            var taskUserRequest = await _identity.UserManager.FindByNameAsync(task.Author);
            if (taskUserRequest != null)
            {
                listAssignedUserId.Add(Guid.Parse(taskUserRequest.Id.ToString()));
                recipients.Add(taskUserRequest.Id.ToGuid());
            }
            recipients = recipients.Distinct().ToList();
            await _notify.SendNotificationAsync(listAssignedUserId, new Notification
            {

                Subject = TaskTitle,
                Content = string.Format(TaskChangeStatus, task.TaskNumber, lastStatus.ToString(), task.Status.ToString(), task.Author),
                SendEmail = false,
                Url = "/TaskManager/Details?id=" + task.Id
            });

            await _notify.SendNotificationAsync(recipients,
                new Notification
                {
                    Content = string.Format(TaskChangeStatus, task.TaskNumber, lastStatus.ToString(), task.Status.ToString(), task.ModifiedBy),
                    Subject = TaskTitle,
                    NotificationTypeId = NotificationType.Info
                });
            
            foreach (var userId in listAssignedUserId)
            {
                var user = _identity.UserManager.FindByIdAsync(userId.ToString());
                string body = $"Hi, {user.Result.UserFirstName}. Task \"{task.Name}\", has changed status from \"{lastStatus}\" to  \"{task.Status}\". On date: \"{task.Changed}\".";
                await SendEmailNotifications(user, Url, task.Id.ToString(), "Activity Status Modified", body, "Task status changed");
            }
        }
        internal async Task ChangePriorityTaskNotificationAsync(Abstractions.Models.Task task, TaskPriority lastPriority,
            IUrlHelper Url)
        {
            var listAssignedUserId = task.AssignedUsers.Select(s => s.UserId).ToList();
            var recipients = task.AssignedUsers.Select(s => s.UserId).ToList();
            var taskUserRequest = await _identity.UserManager.FindByNameAsync(task.Author);
            if (taskUserRequest != null)
            {
                listAssignedUserId.Add(Guid.Parse(taskUserRequest.Id.ToString()));
                recipients.Add(taskUserRequest.Id.ToGuid());
            }
            recipients = recipients.Distinct().ToList();

            await _notify.SendNotificationAsync(recipients,
                new Notification
                {
                    Content = string.Format(TaskChangePriority, task.TaskNumber, lastPriority.ToString(), task.TaskPriority.ToString(), task.ModifiedBy),
                    Subject = TaskTitle,
                    SendEmail = false,
                    Url = "/TaskManager/Details?id=" + task.Id
                });
            foreach (var userId in listAssignedUserId)
            {
                var user = _identity.UserManager.FindByIdAsync(userId.ToString());
                string body = $"Hi, {user.Result.UserFirstName}. Task \"{task.Name}\", has changed priority from \"{lastPriority}\" to  \"{task.TaskPriority}\". On date: \"{task.Changed}\".";
                await SendEmailNotifications(user, Url, task.Id.ToString(), "Activity Modified", body, "Task priority changed");
            }
        }

        internal async Task DeleteTaskNotificationAsync(Abstractions.Models.Task task)
        {
            var listAssignedUserId = task.AssignedUsers.Select(s => s.UserId).ToList();
            var userRequest = await _identity.UserManager.FindByNameAsync(task.Author);
            if (userRequest != null)
                listAssignedUserId.Add(userRequest.Id.ToGuid());

            listAssignedUserId = listAssignedUserId.Distinct().ToList();

            await _notify.SendNotificationAsync(listAssignedUserId,
                new Notification
                {
                    Content = string.Format(TaskRemoved, task.TaskNumber),
                    Subject = TaskTitle,
                    SendEmail = false,
                });
        }

        public async Task TaskExpirationNotificationAsync()
        {
            var notificationItems = await _context.Tasks
                .Include(i => i.AssignedUsers)
                .Where(x => x.EndDate.Date == DateTime.Now.Date.AddDays(1) && x.Status != TaskStatus.Completed).ToListAsync();


            foreach (var item in notificationItems)
            {
                var listAssignedUsersId = item.AssignedUsers.Select(s => s.UserId).ToList();
                var userRequest = await _identity.UserManager.FindByNameAsync(item.Author);
                if (userRequest != null)
                    listAssignedUsersId.Add(userRequest.Id.ToGuid());

                listAssignedUsersId = listAssignedUsersId.Distinct().ToList();

                await _notify.SendNotificationAsync(listAssignedUsersId, new Notification
                {
                    Content = string.Format(TaskExpires, item.TaskNumber),
                    Subject = TaskTitle,
                    SendEmail = false,
                    Url = "/TaskManager/Details?id=" + item.Id
                });
            }
        }

        #region Helpers
        public async Task SendEmailNotifications(Task<GearUser> user, IUrlHelper Url, string taskId,
            string Header, string Body, string subject)
        {
            var callbackUrl = Url.TaskNotificationCallBack("details", "taskmanager", taskId);
            var mail = $"<a href='{callbackUrl}'>link</a>";
            var userEmail = _identity.UserManager.GetEmailAsync(user.Result);
            var templateRequest = TemplateManager.GetTemplateBody("Notification");
            if (templateRequest.IsSuccess)
            {
                mail = templateRequest.Result?.Inject(new Dictionary<string, string>
                {
                    {"Header", Header},
                    {"Body",  Body},
                    {"Link", callbackUrl }
                });
            }
            await _emailSender.SendEmailAsync(userEmail.Result, subject, mail);
        }

        #endregion
    }
}
