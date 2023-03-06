using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GR.Crm.BussinesUnits.Abstractions.ViewModels
{
    public class MoveDepartmentViewModel
    {
        [DisplayName("Business Unit")]
        public Guid? BusinessUnitId { get; set; }

        public List<Guid> DepartmentIds { get; set; }
    }
}
