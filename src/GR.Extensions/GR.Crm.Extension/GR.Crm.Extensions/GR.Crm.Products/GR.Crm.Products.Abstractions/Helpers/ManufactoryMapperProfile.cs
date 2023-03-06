using AutoMapper;
using GR.Crm.Products.Abstractions.Models;
using GR.Crm.Products.Abstractions.ViewModels.ManufactoryViewModels;
using System;
using System.Collections.Generic;
using System.Text;
namespace GR.Crm.Products.Abstractions.Helpers

{
    public class ManufactoryMapperProfile : Profile
    {
        public ManufactoryMapperProfile()
        {
            // Add manufactory
            CreateMap<ProductManufactories, AddManufactoryViewModel>()
                .ForMember(o => o.Name, m => m.MapFrom(x => x.Name))
                .ForMember(o => o.Description, m => m.MapFrom(x => x.Description))
                .ReverseMap();

            //Map agreement with get viewmodel
            CreateMap<ProductManufactories, GetManufactoryViewModel>()
                .IncludeAllDerived()
                .ReverseMap();
        }
    }
}
