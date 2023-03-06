using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Crm.Leads.Abstractions.ViewModels
{
    public class GetGridLeadsViewModel
    {
        public string StageName { get; set; }
        public Guid StageId { get; set; }
        public IEnumerable<GetLeadsViewModel> LeadsInStage { get; set; }
    }
}
