using GR.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Crm.BussinesUnits.Abstractions.Models
{
    public class Grading : BaseModel
    {
        public string Description { get; set; }

        public ICollection<JobPositionGrading> JobPositionGradings { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }
    }
}
