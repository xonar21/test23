using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GR.Audit.Abstractions.Attributes;
using GR.Audit.Abstractions.Enums;
using GR.Core;
using GR.Crm.Abstractions.Models;
using GR.Crm.Organizations.Abstractions.Models;
using GR.Crm.PipeLines.Abstractions.Models;
using GR.Crm.Teams.Abstractions.Models;

namespace GR.Crm.Leads.Abstractions.Models
{
    [TrackEntity(Option = TrackEntityOption.SelectedFields)]
    public class Lead : BaseModel
    {
        /// <summary>
        /// Lead name
        /// </summary>
        [Required]
        [MinLength(2)]
        [MaxLength(256)]
        [TrackField(Option = TrackFieldOption.Allow)]
        public virtual string Name { get; set; }

        /// <summary>
        /// Organization reference
        /// </summary>
        public virtual Organization Organization { get; set; }
        [Required]
        [TrackField(Option = TrackFieldOption.Allow)]
        public virtual Guid OrganizationId { get; set; }

        /// <summary>
        /// PipeLine reference
        /// </summary>
        public virtual PipeLine PipeLine { get; set; }
        [Required]
        public virtual Guid PipeLineId { get; set; }

        /// <summary>
        /// Stage reference
        /// </summary>
        public virtual Stage Stage { get; set; }
        [Required]
        [TrackField(Option = TrackFieldOption.Allow)]
        public virtual Guid StageId { get; set; }

        /// <summary>
        /// Stage reference
        /// </summary>
        public virtual LeadState LeadState { get; set; }
        [TrackField(Option = TrackFieldOption.Allow)]
        
        public virtual Guid? LeadStateId { get; set; }

        /// <summary>
        /// Value
        /// </summary>
        [TrackField(Option = TrackFieldOption.Allow)]
        public virtual decimal Value { get; set; } = 0;

        /// <summary>
        /// Currency reference
        /// </summary>
        public virtual Currency Currency { get; set; }
        [TrackField(Option = TrackFieldOption.Allow)]
        public virtual string CurrencyCode { get; set; }

        /// <summary>
        /// Lead sequence number
        /// </summary>
        public virtual int Number { get; set; }

        /// <summary>
        /// Start date
        /// </summary>
        [Required]
        [TrackField(Option = TrackFieldOption.Allow)]
        public virtual DateTime StartDate { get; set; }

        /// <summary>
        /// Offer deadline
        /// </summary>
        [TrackField(Option = TrackFieldOption.Allow)]
        public virtual DateTime? StageDeadLine { get; set; }

        /// <summary>
        /// Clarification deadline
        /// </summary>
        [TrackField(Option = TrackFieldOption.Allow)]
        public virtual DateTime? DeadLine { get; set; }

        /// <summary>
        /// Team reference
        /// </summary>
        public virtual Team Team { get; set; }

        [TrackField(Option = TrackFieldOption.Allow)]
        public virtual Guid? TeamId { get; set; }

        /// <summary>
        /// Product
        /// </summary>
        public virtual ICollection<ProductOrServiceList> ProductOrServiceList { get; set; }

        /// <summary>
        /// Check if has team
        /// </summary>
        /// <returns></returns>
        public bool HasTeam() => TeamId != null;

        /// <summary>
        /// Clarification deadline
        /// </summary>
        [TrackField(Option = TrackFieldOption.Allow)]
        public virtual DateTime? ClarificationDeadline { get; set; }


        /// <summary>
        /// Contact reference
        /// </summary>
        public virtual Contact Contact { get; set; }
        [TrackField(Option = TrackFieldOption.Allow)]
        public virtual Guid? ContactId { get; set; }

        /// <summary>
        /// List of Contacts
        /// </summary>
        public virtual ICollection<LeadContact> Contacts { get; set; } = new List<LeadContact>();

        /// <summary>
        /// Source reference
        /// </summary>
        public virtual Source Source { get; set; }
        [TrackField(Option = TrackFieldOption.Allow)]
        public virtual Guid? SourceId { get; set; }

        /// <summary>
        /// Description 
        /// </summary>
        public virtual string Description { get; set; }

        public DateTime StageChangeDate { get; set; } = DateTime.UtcNow;
    }
}
