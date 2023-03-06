using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Gr.Crm.Comments.Abstractions.Models
{
    public class CommentAssignedUsers 
    {
        /// <summary>
        /// User id
        /// </summary>
        public virtual Guid UserId { get; set; }

        public virtual string UserEmail { get; set; }

        /// <summary>
        /// Task reference
        /// </summary>
        [Required]
        public virtual Guid CommentId { get; set; }
        public virtual Comment Comment { get; set; }
    }
}
