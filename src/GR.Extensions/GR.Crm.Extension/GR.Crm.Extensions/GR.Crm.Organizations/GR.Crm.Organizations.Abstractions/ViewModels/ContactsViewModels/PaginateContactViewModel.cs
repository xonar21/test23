using GR.Crm.Emails.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Crm.Organizations.Abstractions.ViewModels.ContactsViewModels
{
    public class PaginateContactViewModel
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
        /// First name
        /// </summary>
        public virtual string FirstName { get; set; }

        /// <summary>
        /// Last name
        /// </summary>
        public virtual string LastName { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public virtual List<string> Email { get; set; }

        /// <summary>
        /// Phone
        /// </summary>
        public virtual List<string> DialCode { get; set; }

        /// <summary>
        /// Phone
        /// </summary>
        public virtual List<string> Phone { get; set; }

        /// <summary>
        /// Jon position 
        /// </summary>
        public virtual string JobPosition { get; set; }

        /// <summary>
        /// Organization Id
        /// </summary>
        public virtual Guid? OrganizationId { get; set; }
        /// <summary>
        /// Organization name 
        /// </summary>
        public virtual string Organization { get; set; }

        /// <summary>
        /// Is deleted
        /// </summary>
        public virtual bool IsDeleted { get; set; }
    }
}
