using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Crm.Dashboard.Abstractions.ViewModel
{
    public class LeadByStatusAndValueResultViewModel
    {
        public List<LeadByStatusAndValueViewModel> Leads { get; set; }

        public string StateName { get; set; }

        public decimal TotalValue { get; set; }
    }
}
