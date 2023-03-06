using GR.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GR.Crm.Organizations.Abstractions.Models
{
    public class OrganizationStage : BaseModel
    {
        /// <summary>
        /// Name
        /// </summary>
        [Required]
        [MinLength(2)]
        [MaxLength(128)]
        public virtual string Name { get; set; }

        /// <summary>
        /// Display Order
        /// </summary>
        public virtual int DisplayOrder { get; set; }

        /// <summary>
        /// Reference to organization state
        /// </summary>
        public virtual ICollection<OrganizationStateStage> States { get; set; } = new List<OrganizationStateStage>();


    }
}
