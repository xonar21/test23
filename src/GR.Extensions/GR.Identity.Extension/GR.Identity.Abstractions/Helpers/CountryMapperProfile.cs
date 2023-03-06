using AutoMapper;
using GR.Identity.Abstractions.Models.AddressModels;
using GR.Identity.Abstractions.ViewModels.LocationViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Identity.Abstractions.Helpers
{
    public class CountryMapperProfile : Profile
    {
        public CountryMapperProfile()
        {
            CreateMap<Country, CountryViewModelIdentity>()
                .IncludeAllDerived()
                .ReverseMap();
        }
    }
}
