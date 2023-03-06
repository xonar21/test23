using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GR.Crm.Organizations.Abstractions.ViewModels.OrganizationStatesViewModels
{
    public class AddOrganizationStateViewModel
    {
        /// <summary>
        /// Name 
        /// </summary>
        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public virtual string Name { get; set; }

        /// <summary>
        /// Stage reference
        /// </summary>
        public virtual Guid StageId { get; set; }


        /// <summary>
        /// Badge style class
        /// </summary>
        public virtual string StateStyleClass { get; set; }


    }
}
