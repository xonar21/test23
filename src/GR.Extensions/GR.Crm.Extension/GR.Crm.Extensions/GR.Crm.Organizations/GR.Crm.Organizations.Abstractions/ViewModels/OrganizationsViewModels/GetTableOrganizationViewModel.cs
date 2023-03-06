using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GR.Crm.Organizations.Abstractions.Enums;
using GR.Crm.Organizations.Abstractions.Models;

namespace GR.Crm.Organizations.Abstractions.ViewModels.OrganizationsViewModels
{
    public class GetTableOrganizationViewModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public virtual Guid? Id { get; set; }

        /// <summary>
        /// Created
        /// </summary>
        public virtual DateTime Created { get; set; }

        /// <summary>
        /// Client Name
        /// </summary>
        [Required]
        [MinLength(2)]
        [MaxLength(128)]
        public virtual string Name { get; set; }


        /// <summary>
        /// Brand
        /// </summary>
        [MinLength(2)]
        [MaxLength(128)]
        public virtual string Brand { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public virtual List<string> Email { get; set; }

        /// <summary>
        /// Stage 
        /// </summary>
        public virtual string Stage { get; set; }

        /// <summary>
        /// State
        /// </summary>
        public virtual OrganizationState State { get; set; }

        /// <summary>
        /// List contacts
        /// </summary>
        public virtual IEnumerable<Contact> Contacts { get; set; } = new List<Contact>();


        /// <summary>
        /// Lead count
        /// </summary>
        public virtual int LeadCount { get; set; }

        /// <summary>
        /// Is disabled
        /// </summary>
        public virtual bool IsDeleted { get; set; }


        /// <summary>
        /// Created by
        /// </summary>
        public virtual string Author { get; set; }

        /// <summary>
        /// Tenant Id
        /// </summary>
        public virtual Guid? TenantId { get; set; }


    }
}
