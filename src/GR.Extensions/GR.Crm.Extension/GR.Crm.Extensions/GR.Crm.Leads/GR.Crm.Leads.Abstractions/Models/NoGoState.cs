using GR.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Crm.Leads.Abstractions.Models
{
    public class NoGoState : BaseModel
    {
        public virtual string Name { get; set; }

        public virtual IEnumerable<Lead> Leads { get; set; }
    }
}
