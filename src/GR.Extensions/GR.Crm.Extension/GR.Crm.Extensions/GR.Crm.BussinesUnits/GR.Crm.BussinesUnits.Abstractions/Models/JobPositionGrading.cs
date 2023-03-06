using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Crm.BussinesUnits.Abstractions.Models
{
    public class JobPositionGrading
    {
        public Guid JobPositionId { get; set; }

        public Guid GradingId { get; set; }

        public double? ExternalHourlyGrade { get; set; }


        public BusinessJobPosition JobPosition { get; set; }

        public Grading Grading { get; set; }
    }
}
