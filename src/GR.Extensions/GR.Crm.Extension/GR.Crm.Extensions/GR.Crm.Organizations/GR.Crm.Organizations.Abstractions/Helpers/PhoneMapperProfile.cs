using AutoMapper;
using GR.Crm.Organizations.Abstractions.Models;
using GR.Crm.Organizations.Abstractions.ViewModels.PhoneListViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Crm.Organizations.Abstractions.Helpers
{
    public class PhoneMapperProfile : Profile
    {
        public PhoneMapperProfile()
        {
            CreateMap<PhoneList, AddPhoneViewModel>()
                .ForMember(o => o.Phone, m => m.MapFrom(x => x.Phone))
                .ForMember(o => o.ContactId, m => m.MapFrom(x => x.ContactId))
                .ForMember(o => o.DialCode, m => m.MapFrom(x => x.DialCode))
                .ForMember(o => o.CountryCode, m => m.MapFrom(x => x.CountryCode))
                .ForMember(o => o.Label, m => m.MapFrom(x => x.Label))
                .ReverseMap();


            //Map organization with get viewmodel
            CreateMap<PhoneList, GetPhoneViewModel>()
                .IncludeAllDerived()
                .ReverseMap();

            CreateMap<PhoneList, PhoneViewModel>()
                .IncludeAllDerived()
                .ReverseMap();

        }
    }
}
