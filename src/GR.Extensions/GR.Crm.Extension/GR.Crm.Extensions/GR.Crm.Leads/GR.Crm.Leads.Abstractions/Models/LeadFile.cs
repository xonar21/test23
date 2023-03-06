using System;
using System.Collections.Generic;
using System.Text;
using GR.Core;

namespace GR.Crm.Leads.Abstractions.Models
{
    public class LeadFile: BaseModel
    {
        /// <summary>
        /// File name
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// File
        /// </summary>
        public virtual byte[] File { get; set; }

        /// <summary>
        /// File location in directory
        /// </summary>
        public virtual string Location { get; set; }

        /// <summary>
        /// Lead reference
        /// </summary>
        public virtual Lead Lead { get; set; }
        public virtual Guid LeadId { get; set; }
    }
}
