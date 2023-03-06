using AutoMapper;
using GR.CloudStorage.Abstractions.Models;
using GR.Crm.Abstractions;
using GR.Crm.Leads.Abstractions.Models;
using GR.Crm.Leads.Abstractions.ViewModels;
using GR.Crm.Leads.Abstractions.ViewModels.LeadFileViewModels;
using System;

namespace GR.Crm.Leads.Abstractions.Helpers
{
    public sealed class LeadMapperProfile : Profile
    {

        public LeadMapperProfile()
        {
            //Map create lead 
            CreateMap<Lead, AddLeadViewModel>()
                .ForMember(o => o.Name, m => m.MapFrom(x => x.Name))
                .ForMember(o => o.CurrencyCode, m => m.MapFrom(x => x.CurrencyCode))
                .ForMember(o => o.OrganizationId, m => m.MapFrom(x => x.OrganizationId))
                .ForMember(o => o.PipeLineId, m => m.MapFrom(x => x.PipeLineId))
                .ForMember(o => o.StageId, m => m.MapFrom(x => x.StageId))
                .ForMember(o => o.Value, m => m.MapFrom(x => x.Value))
                .ForMember(o => o.DeadLine, m => m.MapFrom(x => x.DeadLine))
                .ForMember(o => o.LeadStateId, m => m.MapFrom(x => x.LeadStateId))
                .ForMember(o => o.ClarificationDeadline, m => m.MapFrom(x => x.ClarificationDeadline))
                .ForMember(o => o.SourceId, m => m.MapFrom(x => x.SourceId))
                .ForMember(o => o.Description, m => m.MapFrom(x => x.Description))
                .ReverseMap();

            //Map update lead 
            CreateMap<Lead, UpdateLeadViewModel>()
                .ForMember(o => o.Name, m => m.MapFrom(x => x.Name))
                .ForMember(o => o.CurrencyCode, m => m.MapFrom(x => x.CurrencyCode))
                .ForMember(o => o.OrganizationId, m => m.MapFrom(x => x.OrganizationId))
                .ForMember(o => o.Id, m => m.MapFrom(x => x.Id))
                .ForMember(o => o.Value, m => m.MapFrom(x => x.Value))
                .ForMember(o => o.DeadLine, m => m.MapFrom(x => x.DeadLine))
                .ForMember(o => o.LeadStateId, m => m.MapFrom(x => x.LeadStateId))
                .ForMember(o => o.ClarificationDeadline, m => m.MapFrom(x => x.ClarificationDeadline))
                .ForMember(o => o.SourceId, m => m.MapFrom(x => x.SourceId))
                .ForMember(o => o.Description, m => m.MapFrom(x => x.Description))
                .ReverseMap();

            //Map lead with get viewmodel
            CreateMap<Lead, GetLeadsViewModel>()
                .IncludeAllDerived()
                .ReverseMap();


            CreateMap<LeadFile, GetLeadFileViewModel>()
                .IncludeAllDerived()
                .ReverseMap();


            CreateMap<NoGoState, AddNoGoStateViewModel>()
               .ForMember(o => o.Name, m => m.MapFrom(x => x.Name))
               .ReverseMap();

            CreateMap<NoGoState, NoGoStateViewModel>()
                 .IncludeAllDerived()
                 .ReverseMap();
            CreateMap<CloudMetaData, FileViewModel>()
                .IncludeAllDerived();

            CreateMap<ProductOrServiceList, AddProductOrServiceViewModel>()
                .IncludeAllDerived()
                .ReverseMap();

            CreateMap<ProductOrServiceList, UpdateProductOrServiceViewModel>()
                .IncludeAllDerived()
                .ReverseMap();
        }
    }
}
