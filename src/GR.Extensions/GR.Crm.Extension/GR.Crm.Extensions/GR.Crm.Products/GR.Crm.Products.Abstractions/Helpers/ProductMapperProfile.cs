using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using GR.Crm.Products.Abstractions.Models;
using GR.Crm.Products.Abstractions.ViewModels.ProductsViewModels;

namespace GR.Crm.Products.Abstractions.Helpers
{
    public class ProductMapperProfile: Profile
    {
        public ProductMapperProfile()
        {
            // Add product
            CreateMap<ProductTemplate, AddProductViewModel>()
                .ForMember(o => o.Name, m => m.MapFrom(x => x.Name))
                .ForMember(o => o.ProductCode, m => m.MapFrom(x => x.ProductCode))
                .ForMember(o => o.Type, m => m.MapFrom(x => x.Type))
                .ForMember(o => o.CategoryId, m => m.MapFrom(x => x.Category.Id))
                .ForMember(o => o.Description, m => m.MapFrom(x => x.Description))
                .ForMember(o => o.EAN, m => m.MapFrom(x => x.EAN))
                .ForMember(o => o.Warranty, m => m.MapFrom(x => x.Warranty))
                .ForMember(o => o.CanBeSold, m => m.MapFrom(x => x.CanBeSold))
                .ForMember(o => o.ProductManufactoriesId, m => m.MapFrom(x => x.ProductManufactories.Id))
                .ForMember(o => o.SellingPrice, m => m.MapFrom(x => x.SellingPrice))
                .ForMember(o => o.CurrencyCode, m => m.MapFrom(x => x.Currency.Code))
                .ReverseMap();

            //Map agreement with get viewmodel
            CreateMap<ProductTemplate, GetProductViewModel>()
                .IncludeAllDerived()
                .ReverseMap();

            //Add product variation 
            CreateMap<ProductVariation, AddProductVariationViewModel>()
                .ForMember(o => o.TechnologyStack, m => m.MapFrom(x => x.TechnologyStack))
                .ForMember(o => o.ProductType, m => m.MapFrom(x => x.ProductType))
                .ForMember(o => o.ProductTemplateId, m => m.MapFrom(x => x.ProductTemplateId))
                .ReverseMap();

            CreateMap<ProductVariation, ProductVariationViewModel>()
                .IncludeAllDerived()
                .ReverseMap();

            //Add product deliverables
            CreateMap<ProductDeliverables, AddProductDeliverableViewModel>()
                .ForMember(o => o.ProductVariationId, m => m.MapFrom(x => x.ProductVariationId))
                .ForMember(o => o.Name, m => m.MapFrom(x => x.Name))
                .ReverseMap();

            CreateMap<ProductDeliverables, ProductDeliverableViewModel>()
                .IncludeAllDerived()
                .ReverseMap();
        }
    }
}
