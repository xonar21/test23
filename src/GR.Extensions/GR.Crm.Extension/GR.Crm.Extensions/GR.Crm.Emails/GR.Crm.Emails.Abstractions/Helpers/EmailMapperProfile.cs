using AutoMapper;
using GR.Crm.Emails.Abstractions.Models;
using GR.Crm.Emails.Abstractions.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Crm.Emails.Abstractions.Helpers
{
    public class EmailMapperProfile: Profile
    {
        public EmailMapperProfile()
        {
            CreateMap<EmailList, AddEmailViewModel>()
                .ForMember(o => o.Email, m => m.MapFrom(x => x.Email))
                .ForMember(o => o.OrganizationId, m => m.MapFrom(x => x.OrganizationId))
                .ForMember(o => o.ContactId, m => m.MapFrom(x => x.ContactId))
                .ReverseMap();

            CreateMap<EmailList, EmailViewModel>()
                .IncludeAllDerived()
                .ReverseMap();
        }
    }
}
