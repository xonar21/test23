using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Gr.Crm.Products.Abstractions.ViewModels.CategoryViewModels;
using GR.Crm.Products.Abstractions.Models;
using GR.Crm.Products.Abstractions.ViewModels.CategoryViewModels;

namespace Gr.Crm.Products.Abstractions.Helpers
{
    public class CategoryMapperProfile : Profile
    {
        public CategoryMapperProfile()
        {
            // Add product
            CreateMap<Category, AddCategoryViewModel>()
                .ForMember(o => o.Name, m => m.MapFrom(x => x.Name))
                .ForMember(o => o.DisplayName, m => m.MapFrom(x => x.DisplayName))
                .ForMember(o => o.Description, m => m.MapFrom(x => x.Description))
                .ForMember(o => o.DisplayOrder, m => m.MapFrom(x => x.DisplayOrder))
                .ForMember(o => o.Description, m => m.MapFrom(x => x.Description))
                .ForMember(o => o.ParentCategoryId, m => m.MapFrom(x => x.ParentCategoryId))
                .ForMember(o => o.IsPublished, m => m.MapFrom(x => x.IsPublished))
                .ReverseMap();

            //Map agreement with get viewmodel
            CreateMap<Category, GetCategoryViewModel>()
                .IncludeAllDerived()
                .ReverseMap();
        }
    }
}
