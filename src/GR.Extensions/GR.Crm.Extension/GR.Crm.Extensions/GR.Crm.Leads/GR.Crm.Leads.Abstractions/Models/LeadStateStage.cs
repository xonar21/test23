using GR.Crm.PipeLines.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Crm.Leads.Abstractions.Models
{
    public class LeadStateStage
    {
        public virtual Guid StateId { get; set; }
        public LeadState State { get; set; }
        public virtual Guid StageId { get; set; }
        public Stage Stage { get; set; }
    }
}
