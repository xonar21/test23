using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Crm.Dashboard.Abstractions.ViewModel
{
    public class GetLeadsAnalysisReportViewModel
    {
        public int Cases { get; set; }

        public decimal ExpectedRevenue { get; set; }

        public string CurrencyCode { get; set; }

        public string Month { get; set; }

        public string Year { get; set; }

        public string OwnerName { get; set; }
    }
}
