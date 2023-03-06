using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Crm.Leads.Abstractions.ViewModels
{
    public class LeadToNoGoStateViewModel
    {
        public Guid? LeadId { get; set; }

        public Guid? NoGoStateId { get; set; }
    }
}
