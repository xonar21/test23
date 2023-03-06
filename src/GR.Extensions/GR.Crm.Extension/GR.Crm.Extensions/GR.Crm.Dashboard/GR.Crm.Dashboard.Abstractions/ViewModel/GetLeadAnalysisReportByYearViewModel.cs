using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Crm.Dashboard.Abstractions.ViewModel
{
    public class GetLeadAnalysisReportByYearViewModel
    {
        public string Year { get; set; }

        public string OwnerName { get; set; }

        public List<GetLeadsAnalysisReportViewModel> Values { get; set; }
    }
}
