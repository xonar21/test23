using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GR.Crm.Abstractions
{
    public interface ICrmNotificationService
    {
        /// <summary>
        /// Send opportunities clarification and task deadlines notification. 
        /// </summary>
        /// <returns></returns>
        Task DeadlinesSummaryNotificationAsync(IUrlHelper Url, IEnumerable<Guid> selectedUsers);

        /// <summary>
        /// Send notification about opportunities with no active tasks. 
        /// </summary>
        /// <returns></returns>
        Task NoTaskLeadsNotificationAsync(IUrlHelper Url, IEnumerable<Guid> selectedUsers);
    }
}
