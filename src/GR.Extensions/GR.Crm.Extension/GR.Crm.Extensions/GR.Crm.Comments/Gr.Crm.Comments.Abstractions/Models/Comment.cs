using GR.Core;
using GR.Crm.Leads.Abstractions.Models;
using GR.Crm.Organizations.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Gr.Crm.Comments.Abstractions.Models
{
    public class Comment : BaseModel
    {
        
        /// <summary>
        /// Reference to prent comment
        /// </summary>
        public Guid? CommentId { get; set; }

        public virtual Comment ParrentComent { get; set; }

        public virtual IEnumerable<Comment> CommentReply { get; set; }

        /// <summary>
        /// Message
        /// </summary>
        public virtual string Message { get; set; }


        /// <summary>
        /// Assigned users
        /// </summary>
        public virtual IEnumerable<CommentAssignedUsers> AssignedUsers { get; set; }

        
        /// <summary>
        /// Lead reference
        /// </summary>
        public virtual Guid? LeadId { get; set; }
        public virtual Lead Lead { get; set; }

        /// <summary>
        /// Organization reference
        /// </summary>
        public virtual Guid? OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }
    }
}
