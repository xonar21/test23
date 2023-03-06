using System;
using System.ComponentModel.DataAnnotations;
using GR.Core;
using GR.Crm.Organizations.Abstractions.Models;

namespace GR.Crm.Organizations.Abstractions.ViewModels.OrganizationAddressViewModels
{
    public class CityViewModel : BaseModel
    {
        /// <summary>
        /// City name
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Region id
        /// </summary>
        [Required]
        public virtual Guid? RegionId { get; set; }

        /// <summary>
        /// Region
        /// </summary>
        public virtual RegionViewModel Region { get; set; }

        
        [Required]
        public virtual Guid CrmCountryId { get; set; }


        public virtual CrmCountry CrmCountry { get; set; }
    }
}
