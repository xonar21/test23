using GR.Crm.Marketing.Abstractions.Enum;
using GR.Crm.Marketing.Abstractions.Extensions;
using GR.Crm.Marketing.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GR.Crm.Marketing.Abstractions.ViewModels
{
    public class CampaignViewModel
    {
        public virtual Guid? Id { get; set; }

        /// <summary>
        /// Campaign Name
        /// </summary>
        [Required]
        public virtual string Name { get; set; }

        /// <summary>
        /// Campaign description
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Campaign Owner User id
        /// </summary>
        public virtual Guid UserId { get; set; }

        /// <summary>
        /// CampaignType reference
        /// </summary>
        [Required]
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
        /// Currency
        /// </summary>
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
        [ValidationCampaignDateTimeAttributeExtensions("End date is less than Start date")]
        public virtual DateTime EndDate { get; set; }

        /// <summary>
        /// Campaign Status
        /// </summary>
        [Required]
        public virtual CampaignStatus CampaignStatus { get; set; } = CampaignStatus.Planning;

        /// <summary>
        /// Marketing Lists associated with Campaign
        /// </summary>
        public virtual ICollection<CampaignMarketingList> MarketingLists { get; set; } = new List<CampaignMarketingList>();

    }
}
