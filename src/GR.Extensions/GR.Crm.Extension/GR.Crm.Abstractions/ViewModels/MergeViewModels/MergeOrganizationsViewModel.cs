using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Crm.Abstractions.ViewModels.MergeViewModels
{
    public class MergeOrganizationsViewModel
    {
        /// <summary>
        /// Source Organization Id
        /// </summary>
        public virtual Guid TargetOrganization { get; set; }


        /// <summary>
        /// TargetAction
        /// </summary>
        public virtual string TargetAction { get; set; }

        /// <summary>
        /// SourceLeads
        /// </summary>
        public virtual IList<Guid> SourceLeads { get; set; }


        /// <summary>
        /// SourceContacts
        /// </summary>
        public virtual IList<Guid> SourceContacts { get; set; }


        /// <summary>
        /// SourceAddress
        /// </summary>
        public virtual IList<Guid> SourceAddrress { get; set; }


        /// <summary>
        /// RemainingOrgs
        /// </summary>
        public virtual IList<Guid> RemainingOrgs { get; set; }
    }
}
