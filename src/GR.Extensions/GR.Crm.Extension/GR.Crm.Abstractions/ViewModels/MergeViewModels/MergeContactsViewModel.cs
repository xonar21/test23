using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Crm.Abstractions.ViewModels.MergeViewModels
{
    public class MergeContactsViewModel
    {
        /// <summary>
        /// Source Contact Id
        /// </summary>
        public virtual Guid SourceContact { get; set; }


        /// <summary>
        /// merge Source organization
        /// </summary>
        public virtual Guid SourceOrganization { get; set; }


        /// <summary>
        /// mergeSourceEmail
        /// </summary>
        public virtual string SourceEmail { get; set; }


        /// <summary>
        /// mergeSourcePhone
        /// </summary>
        public virtual string SourcePhone { get; set; }


        /// <summary>
        /// mergeSourceJobPosition
        /// </summary>
        public virtual Guid SourceJobPostion { get; set; }


        /// <summary>
        /// mergeRemainingContacts
        /// </summary>
        public virtual List<Guid> RemainingContacts { get; set; }
    }
}
