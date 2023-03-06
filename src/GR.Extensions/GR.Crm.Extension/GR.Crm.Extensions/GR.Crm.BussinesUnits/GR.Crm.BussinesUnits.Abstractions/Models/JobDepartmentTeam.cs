using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Crm.BussinesUnits.Abstractions.Models
{
    public class JobDepartmentTeam
    {
        public Guid DepartmentTeamId { get; set; }

        public DepartmentTeam DepartmentTeam { get; set; }

        public Guid JobPositionId { get; set; }

        public BusinessJobPosition JobPosition { get; set; }
    }
}
