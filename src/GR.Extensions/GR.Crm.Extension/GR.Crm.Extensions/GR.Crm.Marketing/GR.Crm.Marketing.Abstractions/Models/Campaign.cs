using GR.Core;
using GR.Crm.Abstractions.Models;
using GR.Crm.Leads.Abstractions.Models;
using GR.Crm.Marketing.Abstractions.Enum;
using GR.Crm.Marketing.Abstractions.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GR.Crm.Marketing.Abstractions.Models
{
    public class Campaign : BaseModel
    {
        /// <summary>
        /// Campaign Title
        /// </summary>
        [Required]
        public virtual string Name { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Campaign Owner User id
        /// </summary>
        public virtual Guid UserId { get; set; }

        /// <summary>
        /// Campaign reference
        /// </summary>
        [Required]
        public virtual CampaignType CampaignType { get; set; }
        public virtual Guid? CampaignTypeId { get; set; }

        /// <summary>
        /// Campaign Cost
        /// </summary>

        public virtual decimal CampaignCost { get; set; }

        /// <summary>
        /// Campaign Budget
        /// </summary>

        public virtual decimal CampaignBudget { get; set; }
        /// <summary>
        /// Currency reference
        /// </summary>
        public virtual Currency Currency { get; set; }
        public virtual string CurrencyCode { get; set; }

        /// <summary>
        /// Schedule Start Date
        /// </summary>
        [Required]
        public virtual DateTime StartDate { get; set; }

        /// <summary>
        /// Schedule End Date
        /// </summary>
        [Required]
        public virtual DateTime EndDate { get; set; }

        /// <summary>
        /// List of Marketing Lists
        /// </summary>
        public virtual ICollection<CampaignMarketingList> MarketingLists { get; set; } = new List<CampaignMarketingList>();

        /// <summary>
        /// Campaign Status
        /// </summary>
        [Required]
        public virtual CampaignStatus CampaignStatus { get; set; } = CampaignStatus.Planning;
    }
}
