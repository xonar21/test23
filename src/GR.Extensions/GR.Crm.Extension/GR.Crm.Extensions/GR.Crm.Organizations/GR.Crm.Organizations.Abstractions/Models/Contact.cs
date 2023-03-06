using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GR.Core;
using GR.Crm.Abstractions.Models;
using GR.Crm.Emails.Abstractions.Models;

namespace GR.Crm.Organizations.Abstractions.Models
{
    public class Contact: BaseModel
    {
        /// <summary>
        /// Organization Id
        /// </summary>
        [Required]
        public virtual Guid OrganizationId { get; set; }

        /// <summary>
        /// Organization 
        /// </summary>
        public virtual Organization Organization { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public virtual List<EmailList> EmailList { get; set; }

        /// <summary>
        /// PhoneLists
        /// </summary>
        public virtual IEnumerable<PhoneList> PhoneList { get; set; }

        /// <summary>
        /// First name
        /// </summary>
        [Required]
        [MinLength(2)]
        [MaxLength(128)]
        public virtual string FirstName { get; set; }

        /// <summary>
        /// Last name
        /// </summary>
        [Required]
        [MinLength(2)]
        [MaxLength(128)]
        public virtual string LastName { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Job position id
        /// </summary>
       
        public virtual Guid? JobPositionId { get; set; }

        /// <summary>
        /// Job position
        /// </summary>
        public virtual JobPosition JobPosition { get; set; }

        public bool NotAvailable { get; set; } = false;

        /// <summary>
        /// Contact web profile
        /// </summary>
        public virtual IEnumerable<ContactWebProfile> ContactWebProfiles { get; set; } = new List<ContactWebProfile>();
    }
}
