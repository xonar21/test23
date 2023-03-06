using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Crm.BussinesUnits.Abstractions.Models
{
    public class UserDepartmentTeam
    {
        public Guid DeparmentTeamId { get; set; }

        public DepartmentTeam DepartmentTeam { get; set; }

        public Guid UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
