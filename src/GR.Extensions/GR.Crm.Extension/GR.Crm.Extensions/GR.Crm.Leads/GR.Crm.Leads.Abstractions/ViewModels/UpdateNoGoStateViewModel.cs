using GR.Crm.Leads.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Crm.Leads.Abstractions.ViewModels
{
    public class UpdateNoGoStateViewModel
    {
        public NoGoState State { get; set; }

        public List<Guid> SelectedLeads { get; set; }
    }
}
