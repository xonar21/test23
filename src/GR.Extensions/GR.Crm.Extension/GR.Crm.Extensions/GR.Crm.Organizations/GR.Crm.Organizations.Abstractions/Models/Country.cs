using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GR.Core;

namespace GR.Crm.Organizations.Abstractions.Models
{
    public class CrmCountry: BaseModel
    {

        /// <summary>
        /// Country name
        /// </summary>
        [Required]
        public virtual string Name { get; set; }

        public virtual IEnumerable<City> Cities { get; set; }
    }
}
