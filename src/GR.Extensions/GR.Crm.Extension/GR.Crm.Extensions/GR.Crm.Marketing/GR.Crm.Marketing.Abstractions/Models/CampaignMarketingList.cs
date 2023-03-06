using System;

namespace GR.Crm.Marketing.Abstractions.Models
{
    public class CampaignMarketingList
    {
        public virtual Guid? CampaignId { get; set; }
        public Campaign Campaign { get; set; }
        public virtual Guid? MarketingListId { get; set; }
        public MarketingList MarketingList { get; set; }
    }
}
