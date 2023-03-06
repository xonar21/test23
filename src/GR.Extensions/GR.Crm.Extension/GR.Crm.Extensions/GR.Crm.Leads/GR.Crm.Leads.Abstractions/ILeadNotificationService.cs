using System.Collections.Generic;
using GR.Crm.Leads.Abstractions.Models;
using GR.Identity.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GR.Crm.Leads.Abstractions
{
    public interface ILeadNotificationService
    {
        /// <summary>
        /// Sent notification expired lead
        /// </summary>
        /// <returns></returns>
        Task NotifyUsersOnLeadExpirationAsync();

        /// <summary>
        /// Sent notification about expired lead on email
        /// </summary>
        /// <returns></returns>
        Task NotifyUsersOnLeadExpirationByEmailAsync();

        Task SendEmailNotifications(Task<GearUser> user, IUrlHelper Url, string taskId,
            string Header, string Body, string subject);

        Task NotifyOnBudgetChange(Lead lead, decimal lastValue, IUrlHelper Url);

        Task NotifyOnStageChange(Lead lead, string lastStage, string newStage, IUrlHelper Url);

        Task NotifyOnDeadlineClosureAsync();

        Task NotifyOnLeadAdd(List<Guid> userIds, Lead lead, IUrlHelper Url);
    }
}
