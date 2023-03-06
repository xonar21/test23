using System;

namespace GR.Crm.Marketing.Abstractions.ViewModels.CampaignsMarketingListsViewModels
{
    public class CampaignMarketingListViewModel
    {
        public virtual Guid? CampaignId { get; set; }
        public virtual Guid? MarketingListId { get; set; }
    }
}
