using GR.Crm.Leads.Abstractions.Models;
using GR.Crm.Leads.Abstractions.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Crm.Reports.Abstraction.ViewModels.LeadReportViewModels
{
    public class ReportLeadViewModel
    {
        public virtual object GroupKeys { get; set; }

        public virtual int Count { get; set; }

        public virtual decimal SumNumberOfUnits { get; set; }

        public virtual decimal SumValue { get; set; }

        public virtual decimal AverageCommission { get; set; }

        public virtual IEnumerable<Guid> Id { get; set; } = new List<Guid>();

        public IEnumerable<ReportLeadMappedViewModel> Leads { get; set; }
    }
}
