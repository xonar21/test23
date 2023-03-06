using Gr.Crm.Comments.Abstractions.Models;
using GR.Crm.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gr.Crm.Comments.Abstractions
{
    public interface ICommentContext: ICrmContext
    {
        /// <summary>
        /// Comments
        /// </summary>
        DbSet<Comment> Comments { get; set; }

        /// <summary>
        /// CommentAssigned Users
        /// </summary>
        DbSet<CommentAssignedUsers> CommentAssignedUsers { get; set; }
    }
}
