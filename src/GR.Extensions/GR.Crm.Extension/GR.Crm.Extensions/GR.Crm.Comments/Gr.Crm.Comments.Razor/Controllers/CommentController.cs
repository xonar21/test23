using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gr.Crm.Comments.Abstractions;
using Gr.Crm.Comments.Abstractions.ViewModels;
using GR.Core.BaseControllers;
using GR.Core.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Gr.Crm.Comments.Razor.Controllers
{
    public class CommentController : BaseGearController
    {
        #region Injectable 

        ///<summary>
        ///Inject comment service
        /// </summary> 
        private readonly ICommentService _commentService;

        #endregion

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        /// <summary>
        /// Add Comment
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<Guid>))]
        public async Task<JsonResult> AddCommentAsync(AddCommentViewModel model)
        {
            if(!ModelState.IsValid) return JsonModelStateErrors();
            return await JsonAsync(_commentService.AddCommentAsync(model, Url));
        }

        /// <summary>
        /// Update Comment
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> UpdateComment(UpdateCommentViewModel model) =>
            await JsonAsync(_commentService.UpdateCommentAsync(model, Url));


        /// <summary>
        /// Get Comment by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<CommentViewModel>))]
        public async Task<JsonResult> GetCommentByIdAsync(Guid? Id) =>
            await JsonAsync(_commentService.GetCommentByIdAsync(Id));

        /// <summary>
        /// Delete DeleteComment by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> DeleteCommentAsync(Guid? Id) =>
            await JsonAsync(_commentService.DeleteCommentAsync(Id));


        /// <summary>
        /// Get all comments by lead id
        /// </summary>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<IEnumerable<CommentViewModel>>))]
        public async Task<JsonResult> GetAllCommentByLeadId(Guid? LeadId) =>
            await JsonAsync(_commentService.GetAllCommentByLeadIdAsync(LeadId));

        /// <summary>
        /// Get all comments by organization id
        /// </summary>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<IEnumerable<CommentViewModel>>))]
        public async Task<JsonResult> GetAllCommentsByOrganizationId(Guid? OrganizationId) =>
            await JsonAsync(_commentService.GetAllCommentsByOrganizationIdAsync(OrganizationId));
    }
}
