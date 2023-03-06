using GR.Crm.Marketing.Abstractions.Models;

namespace GR.Crm.Marketing.Abstractions.ViewModels.MarketingListsViewModels
{
    public class GetMarketingListViewModel : MarketingList
    {
        /// <summary>
        /// Members count
        /// </summary>
        public virtual int MembersCount { get; set; }
    }
}
