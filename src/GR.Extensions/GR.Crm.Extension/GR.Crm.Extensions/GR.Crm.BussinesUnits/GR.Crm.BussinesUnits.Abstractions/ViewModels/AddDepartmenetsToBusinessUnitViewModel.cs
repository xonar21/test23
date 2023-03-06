using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GR.Crm.BussinesUnits.Abstractions.ViewModels
{
    public class AddDepartmenetsToBusinessUnitViewModel
    {
        public Guid BusinessUnitId { get; set; }

        [DisplayName("Department")]
        public List<Guid> DepartmentIds { get; set; }
    }
}
