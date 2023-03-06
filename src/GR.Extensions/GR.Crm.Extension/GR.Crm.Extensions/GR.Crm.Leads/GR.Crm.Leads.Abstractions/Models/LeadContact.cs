using GR.Crm.Organizations.Abstractions.Models;
using System;

namespace GR.Crm.Leads.Abstractions.Models
{
    public class LeadContact
    {
        public virtual Guid ContactId { get; set; }
        public Contact Contact { get; set; }
        public virtual Guid LeadId { get; set; }
        public Lead Lead { get; set; }
    }
}
