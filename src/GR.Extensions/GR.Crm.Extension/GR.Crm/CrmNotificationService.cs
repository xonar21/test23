using GR.Crm.Abstractions;
using GR.Identity.Abstractions;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using GR.Core.Extensions;
using GR.TaskManager.Abstractions;
using GR.Email.Abstractions;
using GR.Core.Helpers;
using GR.Crm.Leads.Abstractions;
using GR.Crm.Leads.Abstractions.Models;
using Microsoft.AspNetCore.Mvc;
using GR.Core.Helpers.Templates;
using GR.Crm.Abstractions.Extensions;
using GR.Crm.Teams.Abstractions.Helpers;
using GR.Crm.Teams.Abstractions;
using GR.Notifications.Abstractions;

namespace GR.Crm
{
    public class CrmNotificationService : ICrmNotificationService
    {
        #region Injectable
        /// <summary>
        /// Inject Email Sender
        /// </summary>
        private readonly IEmailSender _emailSender;
        private readonly INotify<GearRole> _notify;
        private readonly IUserManager<GearUser> _identity;
        private readonly ITaskManagerContext _taskManagerContext;
        /// <summary>
        /// Inject leadContext
        /// </summary>
        private readonly ILeadContext<Lead> _leadContext;
        /// <summary>
        /// Inject lead team service
        /// </summary>
        private readonly ICrmTeamService _teamService;
        #endregion

        public CrmNotificationService(IUserManager<GearUser> identity,
            IEmailSender emailSender,
            ILeadContext<Lead> leadContext,
            ICrmTeamService teamService)
        {
            _notify = IoC.Resolve<INotify<GearRole>>();
            _identity = identity;
            _emailSender = emailSender;
            _taskManagerContext = IoC.Resolve<ITaskManagerContext>();
            _leadContext = leadContext;
            _teamService = teamService;
        }

        public async Task DeadlinesSummaryNotificationAsync(IUrlHelper Url, IEnumerable<Guid> selectedUsers)
        {
            var users = await _identity.UserManager.Users.Where(x => !x.IsDeleted && selectedUsers.Contains(x.Id.ToGuid())).ToListAsync();

            foreach (var user in users)
            {
                //get all user assigned tasks for the next week
                var assignedTasks = await _taskManagerContext.Tasks
                .Include(i => i.AssignedUsers)
                .Where(x => !x.IsDeleted &&
                  x.EndDate.Date <= DateTime.Now.Date.AddDays(7) &&
                  x.EndDate.Date >= DateTime.Now &&
                  x.AssignedUsers.Select(y => y.UserId).Contains(user.Id.ToGuid()) &&
                  !x.IsDeleted)
                .ToListAsync();

                if (user.UserName.Equals("vitalie.tcaci")) {
                     assignedTasks = await _taskManagerContext.Tasks
                    .Include(i => i.AssignedUsers)
                    .Where(x => !x.IsDeleted &&
                     x.EndDate.Date <= DateTime.Now.Date.AddDays(7) &&
                     x.EndDate.Date >= DateTime.Now && !x.IsDeleted)
                    .ToListAsync();
                }

                string body = $"Hi, {user.UserFirstName}. Here is the summary of this week deadlines:<br><br> ";
                string header = "";
                string subject = "Summary of this week deadlines";
                //add each task to message html body
                foreach (var task in assignedTasks)
                {
                    var assignedUsersIds = task.AssignedUsers.Select(x => x.UserId).ToList();
                    var link = Url.NotificationCallBack("details", "taskmanager", task.Id.ToString());

                    body += $"<br><b>Task</b>: <a href=\"{link}\">{task.Name}</a>, <b>assignees</b>:";

                    //get assigned users first and last name
                    var assignees = _identity.UserManager.Users.Where(x => assignedUsersIds.Contains(x.Id.ToGuid())).Select(i => new { Name = i.UserFirstName + " " + i.UserLastName });
                    body += string.Join(", ", assignees.Select(x => x.Name));

                }

                //get all leads where user is owner or member with clarification deadline set for next week
                var leads = await _leadContext.Leads
                    .Include(x => x.Team)
                     .ThenInclude(x => x.TeamMembers)
                    .Where(x => !x.IsDeleted &&
                       x.ClarificationDeadline <= DateTime.Now.Date.AddDays(7) &&
                       x.ClarificationDeadline >= DateTime.Now &&
                       x.Team.TeamMembers.Select(i => i.UserId).Contains(user.Id.ToGuid())&& 
                       !x.IsDeleted)
                    .OrderByDescending(x => x.ClarificationDeadline)
                    .ToListAsync();

                if (user.UserName.Equals("vitalie.tcaci"))
                {
                    leads = await _leadContext.Leads
                    .Include(x => x.Team)
                     .ThenInclude(x => x.TeamMembers)
                    .Where(x => !x.IsDeleted &&
                       x.ClarificationDeadline <= DateTime.Now.Date.AddDays(7) &&
                       x.ClarificationDeadline >= DateTime.Now && !x.IsDeleted) 
                    .OrderByDescending(x => x.ClarificationDeadline)
                    .ToListAsync();
                }
                //add each lead to message html body
                foreach (var lead in leads)
                {
                    var link = Url.NotificationCallBack("details", "leads", lead.Id.ToString());

                    body += $"<br><b>Opportunity</b>: <a href=\"{link}\">{lead.Name}</a>";
                    if (!lead.TeamId.IsNull())
                    {

                        var teamRequest = await _teamService.GetTeamMembersByTeamIdAsync(lead.TeamId);

                        //get members first and last name
                        var leadMembers = teamRequest.Result.Where(x => x.TeamRoleId.Equals(TeamResources.Member)).Select(i => new { Name = i.FirstName + " " + i.LastName });
                        var owner = teamRequest.Result.FirstOrDefault(x => x.TeamRoleId.Equals(TeamResources.Owner));

                        body += $" <b>owner</b>: ";
                        body += owner.IsNull()? "none" : $"{owner.FirstName} {owner.LastName}, ";
                        body += $"<b>co-worker(s)</b>: ";
                        body += leadMembers.IsNull()? "none" : string.Join(", ", leadMembers.Select(x => x.Name));
                    }
                    else body+= $" <b>owner</b>: none, <b>co-worker(s)</b>: none";

                }

                await SendEmailNotifications(user, Url, header, body, subject);
            }

        }

        public async Task NoTaskLeadsNotificationAsync(IUrlHelper Url, IEnumerable<Guid> selectedUsers)
        {
            var users = await _identity.UserManager.Users.Where(x => !x.IsDeleted && selectedUsers.Contains(x.Id.ToGuid())).ToListAsync();

            var activeTasksAssignedToLead = await _taskManagerContext.Tasks
                                 .Where(x => !x.IsDeleted &&
                                     !x.LeadId.IsNull() &&
                                     x.Status == TaskManager.Abstractions.Enums.TaskStatus.NotStarted ||
                                     x.Status == TaskManager.Abstractions.Enums.TaskStatus.InProgress)
                                  .Select(i => i.LeadId)
                                  .ToListAsync();

            foreach (var user in users)
            {
                //get all leads with no active tasks
                var leads = _leadContext.Leads
                    .Include(x => x.Team)
                    .ThenInclude(x => x.TeamMembers)
                    .Where(x => x.Team.TeamMembers.Select(i => i.UserId).Contains(user.Id.ToGuid()) &&
                        !activeTasksAssignedToLead.Contains(x.Id) && !x.IsDeleted);

                if (user.UserName.Equals("vitalie.tcaci"))
                {
                    leads = _leadContext.Leads
                    .Include(x => x.Team)
                    .ThenInclude(x => x.TeamMembers)
                    .Where(x => !activeTasksAssignedToLead.Contains(x.Id) && !x.IsDeleted);
                }

                    string body = $"Hi, {user.UserFirstName}. Here is the summary of Opportunities with no active tasks:<br><br> ";
                string header = "";
                string subject = "Summary of Opportunities with no active tasks";
                //add each lead to message html body
                foreach (var lead in leads)
                {
                    var link = Url.NotificationCallBack("details", "leads", lead.Id.ToString());

                    body += $"<br><b>Opportunity</b>: <a href=\"{link}\">{lead.Name}</a>";
                    if (!lead.TeamId.IsNull())
                    {

                        var teamRequest = await _teamService.GetTeamMembersByTeamIdAsync(lead.TeamId);

                        //get members first and last name
                        var leadMembers = teamRequest.Result.Where(x => x.TeamRoleId.Equals(TeamResources.Member)).Select(i => new { Name = i.FirstName + " " + i.LastName });
                        var owner = teamRequest.Result.FirstOrDefault(x => x.TeamRoleId.Equals(TeamResources.Owner));

                        body += $" <b>owner</b>: ";
                        body += owner.IsNull() ? "none " : $"{owner.FirstName} {owner.LastName}, ";
                        body += $"<b>co-worker(s)</b>: ";
                        body += leadMembers.IsNull() ? "none " : string.Join(", ", leadMembers.Select(x => x.Name));
                    }
                    else body += $" <b>owner</b>: none, <b>co-worker(s)</b>: none";

                }

                await SendEmailNotifications(user, Url, header, body, subject);
            }

        }
        #region Helpers
        public async Task SendEmailNotifications(GearUser user, IUrlHelper Url,
            string Header, string Body, string subject)
        {

            var mail = $"<a href=''>link</a>";
            var userEmail = _identity.UserManager.GetEmailAsync(user);
            var templateRequest = TemplateManager.GetTemplateBody("DeadlinesNotification");
            if (templateRequest.IsSuccess)
            {
                mail = templateRequest.Result?.Inject(new Dictionary<string, string>
                {
                    {"Header", Header},
                    {"Body",  Body},
                });
            }
            await _emailSender.SendEmailAsync(userEmail.Result, subject, mail);
        }

        #endregion

    }
}
