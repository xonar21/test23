using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Crm.Dashboard.Abstractions.ViewModel
{
    public class LeadByStatusAndValueViewModel
    {
        public string StateName { get; set; }

        public string LeadName { get; set; }

        public decimal Value { get; set; }
    }
}
