using GR.Crm.Organizations.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Crm.Marketing.Abstractions.Models
{
    public class MarketingListOrganization
    {
        public virtual Guid? MarketingListId { get; set; }
        public MarketingList MarketingList { get; set; }
        public virtual Guid? OrganizationId { get; set; }
        public Organization Organization { get; set; }
    }
}
