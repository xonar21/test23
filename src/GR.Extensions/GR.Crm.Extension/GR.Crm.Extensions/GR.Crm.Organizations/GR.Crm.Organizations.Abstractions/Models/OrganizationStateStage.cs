using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Crm.Organizations.Abstractions.Models
{
    public class OrganizationStateStage
    {
        public virtual Guid StateId { get; set; }
        public OrganizationState State { get; set; }
        public virtual Guid StageId { get; set; }
        public OrganizationStage Stage { get; set; }
    }
}
