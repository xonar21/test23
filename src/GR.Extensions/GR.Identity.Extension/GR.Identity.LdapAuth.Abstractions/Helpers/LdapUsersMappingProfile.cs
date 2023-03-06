using AutoMapper;
using GR.Identity.LdapAuth.Abstractions.Models;
using GR.Identity.LdapAuth.Abstractions.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Identity.LdapAuth.Abstractions.Helpers
{
    public class LdapUsersMappingProfile : Profile
    {
        public LdapUsersMappingProfile()
        {
            CreateMap<LdapUser, GetLdapUsersViewModel>()
               .IncludeAllDerived()
               .ReverseMap();
        }
    }
}
