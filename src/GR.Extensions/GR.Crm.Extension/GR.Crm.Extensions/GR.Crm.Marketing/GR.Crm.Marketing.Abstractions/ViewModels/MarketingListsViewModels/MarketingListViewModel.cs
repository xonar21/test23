using GR.Crm.Marketing.Abstractions.Models;
using GR.Crm.Organizations.Abstractions.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GR.Crm.Marketing.Abstractions.ViewModels.MarketingListsViewModels
{
    public class MarketingListViewModel
    {
        public virtual Guid? Id { get; set; }

        /// <summary>
        /// Marketing List Name
        /// </summary>
        [Required]
        public virtual string Name { get; set; }


        /// <summary>
        /// List of Member Organizations
        /// </summary>
        public virtual ICollection<MarketingListOrganization> Members { get; set; } = new List<MarketingListOrganization>();

    }
}
