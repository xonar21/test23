using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GR.Crm.Organizations.Abstractions.ViewModels.OrganizationStagesViewModels
{
    public class AddOrganizationStageViewModel
    {
        /// <summary>
        /// Name 
        /// </summary>
        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public virtual string Name { get; set; }

        /// <summary>
        /// DisplayOrder
        /// </summary>
        [Required]
        public virtual int DisplayOrder { get; set; }

    }
}
