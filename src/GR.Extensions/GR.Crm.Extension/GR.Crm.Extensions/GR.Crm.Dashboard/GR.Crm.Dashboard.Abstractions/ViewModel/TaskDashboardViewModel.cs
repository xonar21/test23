using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Crm.Dashboard.Abstractions.ViewModel
{
    public class TaskDashboardViewModel
    {
        /// <summary>
        /// Task Type
        /// </summary>
        public virtual string TaskType { get; set; }


        /// <summary>
        /// Task count
        /// </summary>
        public virtual double CountPercentage { get; set; } = 0.0;

    }
}
