using AutoMapper;
using GR.Crm.Organizations.Abstractions.Models;
using GR.Crm.Organizations.Abstractions.ViewModels.ContactsViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Crm.Organizations.Abstractions.Helpers
{
    public class ContactMapperProfile : Profile
    {
        public ContactMapperProfile()
        {
            CreateMap<Contact, GetContactViewModel>()
                .IncludeAllDerived()
                .ReverseMap();
        }
    }
}
