using GR.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Crm.BussinesUnits.Abstractions.Models
{
    public class BusinessJobPosition : BaseModel
    {
        public int? HourlySalary { get; set; }

        public string Abbreviation { get; set; }

        public string Description { get; set; }

        public int RowOrder { get; set; }

        public ICollection<JobDepartmentTeam> JobDepartmentTeams { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }

        public ICollection<JobPositionGrading> JobPositionGradings { get; set; }
    }
}
