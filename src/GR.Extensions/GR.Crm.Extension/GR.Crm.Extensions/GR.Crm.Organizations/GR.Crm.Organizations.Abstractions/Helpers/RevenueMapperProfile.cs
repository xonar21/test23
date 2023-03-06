using AutoMapper;
using GR.Crm.Organizations.Abstractions.Models;
using GR.Crm.Organizations.Abstractions.ViewModels.RevenueViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Crm.Organizations.Abstractions.Helpers
{
    class RevenueMapperProfile : Profile
    {
        public RevenueMapperProfile()
        {
            CreateMap<Revenue, RevenueViewModel>()
           .ForMember(o => o.OrganizationId, m => m.MapFrom(x => x.OrganizationId))
           .ForMember(o => o.Year, m => m.MapFrom(x => x.Year))
           .ForMember(o => o.CurrencyCode, m => m.MapFrom(x => x.CurrencyCode))
           .ReverseMap();

            CreateMap<RevenueViewModel, GetRevenueViewModel>()
                .IncludeAllDerived()
                .ReverseMap();

        }
    }
}
