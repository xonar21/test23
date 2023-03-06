using AutoMapper;
using GR.Audit.Abstractions.Models;
using GR.Crm.Abstractions.Models;
using GR.Crm.Abstractions.Models.ProductConfiguration.Services;
using GR.Crm.Abstractions.ViewModels.AuditViewModels;
using GR.Crm.Abstractions.ViewModels.CampaignTypeViewModel;
using GR.Crm.Abstractions.ViewModels.JobPositionViewModels;
using GR.Crm.Abstractions.ViewModels.ProductConfiguration;
using GR.Crm.Abstractions.ViewModels.ProductTypeViewModels;
using GR.Crm.Abstractions.ViewModels.ServiceTypeViewModels;
using GR.Crm.Abstractions.ViewModels.SolutionTypeViewModels;
using GR.Crm.Abstractions.ViewModels.SourceViewModels;
using GR.Crm.Abstractions.ViewModels.TechnologyTypeViewModels;

namespace GR.Crm.Abstractions.Helpers
{
    public class CrmVocabulariesMapperProfile : Profile
    {

        public CrmVocabulariesMapperProfile()
        {

            CreateMap<JobPosition, JobPositionViewModel>()
                .ForMember(o => o.Id, m => m.MapFrom(x => x.Id))
                .ForMember(o => o.IsDeleted, m => m.MapFrom(x => x.IsDeleted))
                .ForMember(o => o.Name, m => m.MapFrom(x => x.Name))
                .ReverseMap();


            // Source 

            CreateMap<Source, SourceViewModel>()
                .ForMember(o => o.Name, m => m.MapFrom(x => x.Name))
                .ReverseMap();

            CreateMap<Source, GetSourceViewModel>()
                .IncludeAllDerived()
                .ReverseMap();

            // SolutionType 

            CreateMap<SolutionType, SolutionTypeViewModel>()
                .ForMember(o => o.Name, m => m.MapFrom(x => x.Name))
                .ReverseMap();

            CreateMap<SolutionType, GetSolutionTypeViewModel>()
                .IncludeAllDerived()
                .ReverseMap();

            // TechnologyType 

            CreateMap<TechnologyType, TechnologyTypeViewModel>()
                .ForMember(o => o.Name, m => m.MapFrom(x => x.Name))
                .ReverseMap();

            CreateMap<TechnologyType, GetTechnologyTypeViewModel>()
                .IncludeAllDerived()
                .ReverseMap();


            // ServiceType 

            CreateMap<ServiceType, ServiceTypeViewModel>()
                .ForMember(o => o.Name, m => m.MapFrom(x => x.Name))
                .ReverseMap();

            CreateMap<ServiceType, GetServiceTypeViewModel>()
                .IncludeAllDerived()
                .ReverseMap();

            CreateMap<DevelopmentVariation, GetDevelopementServiceViewModel>()
                .IncludeAllDerived()
                .ReverseMap();
            CreateMap<DevelopmentVariation, AddServiceVariationViewModel>()
                .ForMember(x => x.Name, m => m.MapFrom(o => o.Name))
                .ReverseMap();

            CreateMap<DesignVariation, GetDesigneServiceViewModel>()
                .IncludeAllDerived()
                .ReverseMap();

            CreateMap<ConsultancyVariation, GetConsultancyServiceViewModel>()
                .IncludeAllDerived()
                .ReverseMap();

            CreateMap<QAVariation, GetQAServiceViewModel>()
                .IncludeAllDerived()
                .ReverseMap();

            CreateMap<PMFramework, AddFrameWorkViewModel>()
                .ForMember(x => x.Name, m => m.MapFrom(o => o.Name))
                .ReverseMap();

            CreateMap<PMFramework, GetPMFramewoksViewModel>()
                .IncludeAllDerived()
                .ReverseMap();

            CreateMap<DevelopementFramework, GetDevelopementFrameWorkViewModel>()
                .IncludeAllDerived()
                .ReverseMap();

            CreateMap<ProductOrService, GetProductOrServiceViewModel>()
                .IncludeAllDerived()
                .ReverseMap();

            // ProductType 

            CreateMap<ProductType, ProductTypeViewModel>()
                .ForMember(o => o.Name, m => m.MapFrom(x => x.Name))
                .ReverseMap();

            CreateMap<ProductType, GetProductTypeViewModel>()
                .IncludeAllDerived()
                .ReverseMap();

            // Campaign yType 

            CreateMap<CampaignType, CampaignTypeViewModel>()
                .ForMember(o => o.Name, m => m.MapFrom(x => x.Name))
                .ReverseMap();

            CreateMap<CampaignType, GetCampaignTypeViewModel>()
                .IncludeAllDerived()
                .ReverseMap();

            //Audit
            CreateMap<TrackAudit, GetPaginatedAuditViewModel>()
                .IncludeAllDerived()
                .ReverseMap();
        }

    }
}
