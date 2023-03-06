using GR.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Crm.BussinesUnits.Abstractions.Models
{
    public class BusinessUnit : BaseModel
    {
        public string Description { get; set; }

        public string Name { get; set; }

        public Guid? BusinessUnitLeadId { get; set; }

        public string Address { get; set; }


        public ApplicationUser BusinessUnitLead { get; set; }

        public ICollection<Department> Departments { get; set; }
    }
}
