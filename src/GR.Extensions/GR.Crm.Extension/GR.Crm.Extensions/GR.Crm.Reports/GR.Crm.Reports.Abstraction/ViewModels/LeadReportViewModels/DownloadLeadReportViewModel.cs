using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Crm.Reports.Abstraction.ViewModels.LeadReportViewModels
{
    public class DownloadLeadReportViewModel
    {
        public virtual decimal SumNumberOfUnits { get; set; }

        public virtual decimal SumValue { get; set; }

        public virtual decimal AverageCommission { get; set; }

        public virtual List<Guid> Id { get; set; }
    }
}
