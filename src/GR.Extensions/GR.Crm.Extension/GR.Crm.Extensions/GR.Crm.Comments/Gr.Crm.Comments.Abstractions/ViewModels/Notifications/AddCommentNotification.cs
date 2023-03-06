using System;
using System.Collections.Generic;
using System.Text;

namespace Gr.Crm.Comments.Abstractions.ViewModels.Notifications
{
    public class AddCommentNotification
    {
        public string Entity { get; set; }

        public string EntityName { get; set; }

        public List<Guid> Users { get; set; }

        public string AuthorName { get; set; }

        public string Url { get; set; }
    }
}
