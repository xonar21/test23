using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Gr.Crm.Comments.Abstractions.ViewModels
{
    public class AddCommentViewModel
    {
        public Guid? CommentId { get; set; }

        public List<Guid> AssignedUsersIds { get; set; }
     
        [Required]
        public string Message { get; set; }

        public string LeadId { get; set; }

        public string OrganizationId { get; set; }
    }
}
