using AutoMapper;
using GR.Crm.Marketing.Abstractions.Models;
using GR.Crm.Marketing.Abstractions.ViewModels;
using GR.Crm.Marketing.Abstractions.ViewModels.CampaignsMarketingListsViewModels;
using GR.Crm.Marketing.Abstractions.ViewModels.CampaignsViewModels;


namespace GR.Crm.Marketing.Abstractions.Helpers
{
    public sealed class CampaignMapperProfile : Profile
    {
        public CampaignMapperProfile()
        {
            //Map create campaign
            CreateMap<Campaign, CampaignViewModel>()
                .ForMember(o => o.Name, m => m.MapFrom(x => x.Name))
                .ForMember(o => o.Description, m => m.MapFrom(x => x.Description))
                .ForMember(o => o.UserId, m => m.MapFrom(x => x.UserId))
                .ForMember(o => o.CampaignTypeId, m => m.MapFrom(x => x.CampaignTypeId))
                .ForMember(o => o.CampaignCost, m => m.MapFrom(x => x.CampaignCost))
                .ForMember(o => o.CampaignBudget, m => m.MapFrom(x => x.CampaignBudget))
                .ForMember(o => o.CurrencyCode, m => m.MapFrom(x => x.CurrencyCode))
                .ForMember(o => o.StartDate, m => m.MapFrom(x => x.StartDate))
                .ForMember(o => o.EndDate, m => m.MapFrom(x => x.EndDate))
                .ForMember(o => o.CampaignStatus, m => m.MapFrom(x => x.CampaignStatus))
                .ReverseMap();

            //Map campaign with get viewmodel
            CreateMap<Campaign, GetCampaignViewModel>()
                .IncludeAllDerived()
                .ReverseMap();

            //Map create campaignMarketingList
            CreateMap<CampaignMarketingList, CampaignMarketingListViewModel>()
                    .ForMember(o => o.CampaignId, m => m.MapFrom(x => x.CampaignId))
                    .ForMember(o => o.MarketingListId, m => m.MapFrom(x => x.MarketingListId))
                    .ReverseMap();

        }
    }
}
