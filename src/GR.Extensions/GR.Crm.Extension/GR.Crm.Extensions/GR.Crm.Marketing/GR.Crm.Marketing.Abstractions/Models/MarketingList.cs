using GR.Core;
using GR.Crm.Organizations.Abstractions.Enums;
using GR.Crm.Organizations.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace GR.Crm.Marketing.Abstractions.Models
{
    public class MarketingList : BaseModel
    {
        /// <summary>
        /// Name
        /// </summary>
        [Required]
        public virtual string Name { get; set; }


        /// <summary>
        /// List of Member Organizations
        /// </summary>
        public virtual ICollection<MarketingListOrganization> MemberOrganizations { get; set; } = new List<MarketingListOrganization>();
    
    }
}
