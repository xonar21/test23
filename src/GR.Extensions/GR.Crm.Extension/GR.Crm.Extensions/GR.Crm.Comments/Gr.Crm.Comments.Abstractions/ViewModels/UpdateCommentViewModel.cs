using System;
using System.Collections.Generic;
using System.Text;

namespace Gr.Crm.Comments.Abstractions.ViewModels
{
    public class UpdateCommentViewModel
    {
        public List<Guid> AssignedUsers { get; set; }

        public List<Guid> UnassignedUsers { get; set; }

        public CommentViewModel Comment { get; set; }
    }
}
