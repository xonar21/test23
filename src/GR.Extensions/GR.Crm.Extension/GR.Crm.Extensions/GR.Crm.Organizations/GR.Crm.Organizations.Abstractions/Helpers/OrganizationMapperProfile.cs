using AutoMapper;
using GR.Crm.Organizations.Abstractions.Models;
using GR.Crm.Organizations.Abstractions.ViewModels.OrganizationsViewModels;

namespace GR.Crm.Organizations.Abstractions.Helpers
{
    public sealed class OrganizationMapperProfile : Profile
    {
        public OrganizationMapperProfile()
        {
            //Map create organization 
            CreateMap<Organization, OrganizationViewModel>()
                .ForMember(o => o.Name, m => m.MapFrom(x => x.Name))
                .ForMember(o => o.Brand, m => m.MapFrom(x => x.Brand))
                .ForMember(o => o.StageId, m => m.MapFrom(x => x.StageId))
                .ForMember(o => o.StateId, m => m.MapFrom(x => x.StateId))
                .ForMember(o => o.Bank, m => m.MapFrom(x => x.Bank))
                .ForMember(o => o.Phone, m => m.MapFrom(x => x.Phone))
                .ForMember(o => o.WebSite, m => m.MapFrom(x => x.WebSite))
                .ForMember(o => o.FiscalCode, m => m.MapFrom(x => x.FiscalCode))
                .ForMember(o => o.IBANCode, m => m.MapFrom(x => x.IBANCode))
                .ForMember(o => o.CodSwift, m => m.MapFrom(x => x.CodSwift))
                .ForMember(o => o.VitCode, m => m.MapFrom(x => x.VitCode))
                .ForMember(o => o.Description, m => m.MapFrom(x => x.Description))
                .ForMember(o => o.IndustryId, m => m.MapFrom(x => x.IndustryId))
                .ForMember(o => o.EmployeeId, m => m.MapFrom(x => x.EmployeeId))
                .ForMember(o => o.DialCode, m => m.MapFrom(x => x.DialCode))
                .ReverseMap();


            //Map organization with get viewmodel
            CreateMap<Organization, GetOrganizationViewModel>()
                .IncludeAllDerived()
                .ReverseMap();

        }
    }
}
