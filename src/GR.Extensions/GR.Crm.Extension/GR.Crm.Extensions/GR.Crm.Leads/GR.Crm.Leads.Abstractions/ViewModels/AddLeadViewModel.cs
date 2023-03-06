using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GR.Core.Extensions;
using GR.Crm.Leads.Abstractions.Models;

namespace GR.Crm.Leads.Abstractions.ViewModels
{
    public class AddLeadViewModel
    {
         
        /// <summary>
        /// Lead name
        /// </summary>
        [Required]
        [MinLength(2)]
        [MaxLength(128)]
        public virtual string Name { get; set; }

        /// <summary>
        /// Organization id 
        /// </summary>
        [Required]
        public virtual Guid? OrganizationId { get; set; }

        /// <summary>
        /// PipeLine id
        /// </summary>
        [Required]
        public virtual Guid? PipeLineId { get; set; }

        /// <summary>
        /// Stage id
        /// </summary>
        [Required]
        public virtual Guid? StageId { get; set; }


        /// <summary>
        /// Value
        /// </summary>
        public virtual decimal Value { get; set; } = 0;

        /// <summary>
        /// Currency
        /// </summary>
        public virtual string CurrencyCode { get; set; }
        
        [Required]
        public virtual DateTime Created { get; set; }

        public virtual DateTime? DeadLine { get; set; }

        /// <summary>
        /// Lead state id
        /// </summary>
        public virtual Guid? LeadStateId { get; set; }

        /// <summary>
        /// Clarification deadline
        /// </summary>
        public virtual DateTime? ClarificationDeadline { get; set; }

        /// <summary>
        /// Opportunity Contacts
        /// </summary>
        public virtual ICollection<Guid> ContactsIds { get; set; } = new List<Guid>();

        /// <summary>
        /// Source reference
        /// </summary>
        public virtual Guid? SourceId { get; set; }

        /// <summary>
        /// Description 
        /// </summary>
        public virtual string Description { get; set; }
    }
}
