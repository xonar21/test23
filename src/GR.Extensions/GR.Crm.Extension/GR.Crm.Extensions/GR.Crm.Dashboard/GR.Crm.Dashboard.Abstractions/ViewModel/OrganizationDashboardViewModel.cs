using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Crm.Dashboard.Abstractions.ViewModel
{
    public class OrganizationDashboardViewModel
    {
        /// <summary>
        /// State
        /// </summary>
        public virtual string State { get; set; }

        /// <summary>
        /// Organizations count
        /// </summary>
        public virtual int OrganizationsCount { get; set; } = 0;

        /// <summary>
        /// Organizations count percentage
        /// </summary>
        public virtual double OrganizationsCountPercentage { get; set; } = 0;

    }
}
