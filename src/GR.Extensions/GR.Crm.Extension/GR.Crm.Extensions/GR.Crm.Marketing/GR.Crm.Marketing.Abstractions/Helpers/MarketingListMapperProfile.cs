using GR.Crm.Marketing.Abstractions.Models;
using GR.Crm.Marketing.Abstractions.ViewModels.MarketingListsViewModels;
using AutoMapper;
using GR.Crm.Marketing.Abstractions.ViewModels.MarketingListOrganizationViewModel;

namespace GR.Crm.Marketing.Abstractions.Helpers
{
    public sealed class MarketingListMapperProfile : Profile
    {
        public MarketingListMapperProfile()
        {
            //Map create marketing list
            CreateMap<MarketingList, MarketingListViewModel>()
                    .ForMember(o => o.Name, m => m.MapFrom(x => x.Name))
                    .ReverseMap();

            //Map create marketinglistorganization
            CreateMap<MarketingListOrganization, MarketingListOrganizationViewModel>()
                    .ForMember(o => o.OrganizationId, m => m.MapFrom(x => x.OrganizationId))
                    .ForMember(o => o.MarketingListId, m => m.MapFrom(x => x.MarketingListId))
                    .ReverseMap();

            //Map marketing list with get viewmodel
            CreateMap<MarketingList, GetMarketingListViewModel>()
                    .IncludeAllDerived()
                    .ReverseMap();
        }
        
    }
}
