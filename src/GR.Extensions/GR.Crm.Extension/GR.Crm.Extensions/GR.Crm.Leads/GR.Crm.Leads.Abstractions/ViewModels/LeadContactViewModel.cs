using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Crm.Leads.Abstractions.ViewModels
{
    public class LeadContactViewModel
    {
        public Guid LeadId { get; set; }

        public Guid ContactId { get; set; }
    }
}
