using AutoMapper;
using GR.Core.Extensions;
using GR.Crm.BussinesUnits.Abstractions.Models;
using GR.Crm.BussinesUnits.Abstractions.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GR.Crm.BussinesUnits.Abstractions.Helpers
{
    public class BusinessUnitMapperProfile : Profile
    {
        public BusinessUnitMapperProfile()
        {
            CreateMap<BusinessUnit, BusinessUnitDetailViewModel>()
            .IncludeAllDerived()
            .ReverseMap();

            CreateMap<Department, GetDepartmentViewModel>()
                .IncludeAllDerived()
                .ReverseMap();
        }
    }
}
