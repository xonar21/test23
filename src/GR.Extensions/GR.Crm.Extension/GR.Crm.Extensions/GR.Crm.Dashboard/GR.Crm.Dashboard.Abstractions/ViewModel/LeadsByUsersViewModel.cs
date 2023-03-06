using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Crm.Dashboard.Abstractions.ViewModel
{
    public class LeadsByUsersViewModel
    {
        public virtual string Id { get; set; }


        public virtual string Name { get; set; }

        public virtual int NrLeads { get; set; }

        public virtual decimal TotalSum { get; set; }

        public virtual string CurrencyCode { get; set; }

        public virtual string Pipeline { get; set; }
    }
}
