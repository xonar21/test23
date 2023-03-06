using System.ComponentModel.DataAnnotations;
using GR.Core;


namespace GR.Crm.Abstractions.Models
{
    public class CampaignType : BaseModel
    {
        /// <summary>
        /// Campaign type name 
        /// </summary>
        [Required]
        public virtual string Name { get; set; }
    }
}
