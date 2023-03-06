using GR.Crm.BussinesUnits.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Crm.BussinesUnits.Abstractions.ViewModels
{
    public class BusinessUnitListViewModel
    {
        public IList<BusinessUnitLookupModel> BusinessUnits { get; set; }
    }
}
