using GR.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Crm.BussinesUnits.Abstractions.Models
{
    public class Department : BaseModel
    {
        public string Description { get; set; }

        public Guid? BusinessUnitId { get; set; }

        public Guid? DepartmentLeadId { get; set; }

        public string Abbreviation { get; set; }

        public int RowOrder { get; set; }

        public BusinessUnit BusinessUnit { get; set; }

        public ApplicationUser DepartmentLead { get; set; }

        public ICollection<DepartmentTeam> DepartmentTeams { get; set; }

        public string Name { get; set; }
    }
}
