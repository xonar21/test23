using Microsoft.Extensions.Configuration;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.Core.Helpers.Templates;
using GR.Crm.Abstractions.Extensions;
using GR.Crm.Leads.Abstractions;
using GR.Crm.Leads.Abstractions.Models;
using GR.Email.Abstractions;
using GR.Identity.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GR.Notifications.Abstractions;
using GR.Notifications.Abstractions.Models.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GR.Crm.Leads.Infrastructure
{
    public class LeadNotificationService : ILeadNotificationService
    {
        private readonly INotify<GearRole> _notify;
        private readonly IEmailSender _emailSender;
        private readonly IUserManager<GearUser> _identity;
        private readonly IConfiguration _configuration;
        /// <summary>
        /// Inject context
        /// </summary>
        private readonly ILeadContext<Lead> _context;


        private const string LeadTitle = "Opportunity expires";
        private const string LeadExpires = "#{0} deadline is in 3 days.";

        public LeadNotificationService(
            ILeadContext<Lead> context,
            IUserManager<GearUser> identity,
            IEmailSender emailSender,
            IConfiguration iconfig)
        {
            _notify = IoC.Resolve<INotify<GearRole>>();
            _context = context;
            _identity = identity;
            _emailSender = emailSender;
            _configuration = iconfig;


        }

        public async Task NotifyUsersOnLeadExpirationAsync()
        {
            var notificationItems = await _context.Leads
                .Include(i => i.Team)
                .ThenInclude(i => i.TeamMembers)
                .Where(x => x.DeadLine.HasValue && x.DeadLine.Value.Date == DateTime.Now.AddDays(3).Date && x.HasTeam()).ToListAsync();

            foreach (var item in notificationItems)
            {
                var assignedUsersId = notificationItems.SelectMany(s => s.Team.TeamMembers).Select(s => s.UserId);
                
                    await _notify.SendNotificationAsync(assignedUsersId, new Notification
                    {
                        Subject = "Info",
                        Content = string.Format(LeadExpires, item.Name),
                        SendEmail = false,
                        Url = "/leads/details?id=" + item?.Id
                    });

            }

        }

        public async Task NotifyUsersOnLeadExpirationByEmailAsync()
        {
            var users = await _identity.UserManager.Users.ToListAsync();
            var callbackUrl = _configuration.GetSection("WebClients").GetSection("CORE").GetValue<string>("uri") + "/leads/details?id=";
            var leads = await _context.Leads
                .Include(i => i.Team)
                .ThenInclude(i => i.TeamMembers)
                .Where(x => x.DeadLine.HasValue && x.DeadLine.Value.Date == DateTime.Now.AddDays(3).Date && x.HasTeam()).ToListAsync();

            foreach (var user in users)
            {
                var assignedLeads = leads.Where(x => x.Team.TeamMembers.Select(s => s.UserId).Contains(user.Id.ToGuid()));

                if (!assignedLeads.Any()) continue;
                string body = $"Hi, {user.UserFirstName}. This opportunities' end dates are close:<br>";
                foreach (var lead in assignedLeads)
                {
                    body += $"<b><a href =\"{callbackUrl}{lead.Id}\">{lead.Name}</a></b> expires on {lead.DeadLine.Value.Date.ToShortDateString()}<br>";
                }
                await SendEmailNotifications(user, "", body, $"Opportunities end dates are close");
            }

        }

        public async Task NotifyOnBudgetChange(Lead lead, decimal lastValue, IUrlHelper Url)
        {
            var user = _identity.UserManager.FindByNameAsync("vitalie.tcaci");
            string body = $"Hi, {user.Result.UserFirstName}. <b>{lead.Name}</b> budget has been changed from {lastValue} to {lead.Value} by {lead.ModifiedBy}.";
            await SendEmailNotifications(user, Url, lead.Id.ToString(), "", body, "Opportunity budget has been changed");

        }
        public async Task NotifyOnStageChange(Lead lead, string lastStage, string newStage, IUrlHelper Url)
        {
            var user = _identity.UserManager.FindByNameAsync("vitalie.tcaci");
            string body = $"Hi, {user.Result.UserFirstName}. <b>{lead.Name}</b> stage was changed from {lastStage} to {newStage} by {lead.ModifiedBy}.";
            await SendEmailNotifications(user, Url, lead.Id.ToString(), "", body, $"Opportunity moved from {lastStage} to {newStage}");
        }
        public async Task NotifyOnLeadAdd(List<Guid> userIds, Lead lead, IUrlHelper Url)
        {
            foreach (var userId in userIds)
            {
                var user = _identity.UserManager.FindByIdAsync(userId.ToString());
                string body = $"Hi, {user.Result.UserFirstName}. A new Opportunity <b>{lead.Name}</b> has been assigned to you by {lead.Author}.";
                await SendEmailNotifications(user, Url, lead.Id.ToString(), "", body, $"A new Opportunity has been assigned to you by {lead.Author}");
            }

        }
        public async Task SendEmailNotifications(Task<GearUser> user, IUrlHelper Url, string leadId,
            string Header, string Body, string subject)
        {
            var callbackUrl = Url.NotificationCallBack("details", "leads", leadId);
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

        public async Task NotifyOnDeadlineClosureAsync()
        {
            var leads = await _context.Leads
                .Include(i => i.Team)
                .ThenInclude(i => i.TeamMembers)
                .Where(x => x.ClarificationDeadline > DateTime.UtcNow && !x.ClarificationDeadline.IsNull())
                .Where(x => (x.ClarificationDeadline.Value - DateTime.UtcNow).TotalDays <= 2).ToListAsync();
            foreach (var lead in leads)
            {
                var users = lead?.Team?.TeamMembers?.Select((x => x.UserId)).ToList() ?? new List<Guid>();
                if (users.Count > 0)
                {
                    await _notify.SendNotificationAsync(users, new Notification
                    {

                        Subject = "Info",
                        Content = $"{lead?.Name} clarification deadline is on {lead?.ClarificationDeadline}",
                        SendEmail = false,
                        Url = "/leads/details?id=" + lead?.Id
                    });
                }
            }
        }

        public async Task SendEmailNotifications(GearUser user,
          string Header, string Body, string subject)
        {
            var mail = $"";
            var userEmail = _identity.UserManager.GetEmailAsync(user);
            var templateRequest = TemplateManager.GetTemplateBody("NoLinkNotification");
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
    }
}
