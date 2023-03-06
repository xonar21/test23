using System;
using System.ComponentModel.DataAnnotations;
using GR.Core;

namespace GR.Crm.Organizations.Abstractions.Models
{
    public class City : BaseModel
    {
        /// <summary>
        /// City name
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Region id
        /// </summary>
        public virtual Guid? RegionId { get; set; }

        /// <summary>
        /// Region
        /// </summary>
        public virtual Region Region { get; set; }

        [Required]
        public virtual Guid? CrmCountryId { get; set; }

        
        public virtual CrmCountry CrmCountry { get; set; }
    }
}
