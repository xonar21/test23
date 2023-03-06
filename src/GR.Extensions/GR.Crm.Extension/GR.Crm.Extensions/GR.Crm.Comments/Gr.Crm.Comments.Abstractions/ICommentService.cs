using Gr.Crm.Comments.Abstractions.ViewModels;
using GR.Core.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gr.Crm.Comments.Abstractions
{
    public interface ICommentService
    {
        /// <summary>
        /// Add Comment
        /// </summary>
        /// <param name="model"></param>
        /// <param name="Url"></param>
        /// <returns></returns>
        Task<ResultModel<Guid>> AddCommentAsync(AddCommentViewModel model, IUrlHelper Url);


        /// <summary>
        /// Update Comment
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel> UpdateCommentAsync(UpdateCommentViewModel model, IUrlHelper Url);


        /// <summary>
        /// Get Comment by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ResultModel<CommentViewModel>> GetCommentByIdAsync(Guid? Id);

        /// <summary>
        /// Delete DeleteComment by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ResultModel> DeleteCommentAsync(Guid? Id);


        /// <summary>
        /// Get all comments by lead id
        /// </summary>
        Task<ResultModel<IEnumerable<CommentViewModel>>> GetAllCommentByLeadIdAsync(Guid? LeadId);



        /// <summary>
        /// Get all comments by organization id
        /// </summary>
        Task<ResultModel<IEnumerable<CommentViewModel>>> GetAllCommentsByOrganizationIdAsync(Guid? OrganizationId);
    }
}
