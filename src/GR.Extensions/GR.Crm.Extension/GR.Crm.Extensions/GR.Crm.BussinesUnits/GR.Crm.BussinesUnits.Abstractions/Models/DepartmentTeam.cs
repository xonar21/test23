using GR.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Crm.BussinesUnits.Abstractions.Models
{
    public class DepartmentTeam : BaseModel
    {
        public string Description { get; set; }

        public string Name { get; set; }

        public Guid? DepartmentId { get; set; }

        public Guid? DepartmentTeamLeadId { get; set; }

        public string Abbreviation { get; set; }

        public int RowOrder { get; set; }

        public Department Department { get; set; }

        public ApplicationUser DepartmentTeamLead { get; set; }

        public ICollection<UserDepartmentTeam> UserDepartmentTeams { get; set; }

        public ICollection<JobDepartmentTeam> JobDepartmentTeams { get; set; }
    }
}
