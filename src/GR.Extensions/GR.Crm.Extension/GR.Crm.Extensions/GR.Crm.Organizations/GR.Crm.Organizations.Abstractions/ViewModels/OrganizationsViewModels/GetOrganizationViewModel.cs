using System;
using System.Collections.Generic;
using GR.Crm.Organizations.Abstractions.Models;

namespace GR.Crm.Organizations.Abstractions.ViewModels.OrganizationsViewModels
{
   public  class GetOrganizationViewModel : Organization
    {

        /// <summary>
        /// Lead count
        /// </summary>
        public virtual int LeadCount { get; set; }

        /// <summary>
        /// Days in stage
        /// </summary>
        public int DaysInStage { get; set; }
    }
}
