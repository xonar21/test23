using GR.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GR.Crm.Organizations.Abstractions.Models
{
    public class OrganizationState : BaseModel
    {
        /// <summary>
        /// Name
        /// </summary>
        [Required]
        [MinLength(2)]
        [MaxLength(128)]
        public virtual string Name { get; set; }

        /// <summary>
        /// Reference to organization stage
        /// </summary>
        public virtual ICollection<OrganizationStateStage> Stages { get; set; } = new List<OrganizationStateStage>();

        /// <summary>
        /// State style class
        /// </summary>
        public virtual string StateStyleClass { get; set; }
    }
}
