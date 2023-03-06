using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Crm.Abstractions.ViewModels.MergeViewModels
{
    public class MergeLeadsViewModel
    {
        /// <summary>
        /// Source Lead Id
        /// </summary>
        public virtual string SourceLeadId { get; set; }


        /// <summary>
        /// Organization Id
        /// </summary>
        public virtual string OrganizationId { get; set; }


        /// <summary>
        /// Contact Id
        /// </summary>
        public virtual string ContactId { get; set; }

        /// <summary>
        /// Lead Members
        /// </summary>
        public virtual IList<string> LeadMembersIds { get; set; }


        /// <summary>
        /// Owner
        /// </summary>
        public virtual string OwnerId { get; set; }


        /// <summary>
        /// Leaad State
        /// </summary>
        public virtual string LeadStateId { get; set; }


        /// <summary>
        /// Product Id
        /// </summary>
        public virtual string ProductId { get; set; }


        /// <summary>
        /// ServiceType Id
        /// </summary>
        public virtual string ServiceTypeId { get; set; }


        /// <summary>
        /// SolutionType
        /// </summary>
        public virtual string SolutionTypeId { get; set; }


        /// <summary>
        /// Source Id
        /// </summary>
        public virtual string SourceId { get; set; }


        /// <summary>
        /// Team Id
        /// </summary>
        public virtual string TeamId { get; set; }


        /// <summary>
        /// TechnologyType Id
        /// </summary>
        public virtual string TechnologyTypeId { get; set; }


        /// <summary>
        /// RemainingLeads
        /// </summary>
        public virtual IList<string> RemainingLeadsIds { get; set; }
    }
}