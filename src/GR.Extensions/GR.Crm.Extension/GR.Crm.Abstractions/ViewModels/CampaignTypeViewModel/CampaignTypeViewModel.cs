using System;
using System.ComponentModel.DataAnnotations;


namespace GR.Crm.Abstractions.ViewModels.CampaignTypeViewModel
{
    public class CampaignTypeViewModel
    {
        public virtual Guid Id { get; set; }

        [Required]
        public virtual string Name { get; set; }
    }
}
