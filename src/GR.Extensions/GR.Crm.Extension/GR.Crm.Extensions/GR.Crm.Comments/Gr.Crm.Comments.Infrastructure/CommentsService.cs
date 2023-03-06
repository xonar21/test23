using AutoMapper;
using Gr.Crm.Comments.Abstractions;
using Gr.Crm.Comments.Abstractions.Extensions;
using Gr.Crm.Comments.Abstractions.Models;
using Gr.Crm.Comments.Abstractions.ViewModels;
using Gr.Crm.Comments.Abstractions.ViewModels.Notifications;
using GR.Core.Abstractions;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.Core.Helpers.Responses;
using GR.Crm.Abstractions;
using GR.Crm.Leads.Abstractions;
using GR.Crm.Leads.Abstractions.Models;
using GR.Email.Abstractions;
using GR.Identity.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GR.Notifications.Abstractions;
using GR.Notifications.Abstractions.Models.Notifications;

namespace Gr.Crm.Comments.Infrastructure
{
    public class CommentsService : ICommentService
    {

        #region Injectable

        /// <summary>
        /// Inject context
        /// </summary>

        private readonly ICommentContext _context;

        /// <summary>
        /// Inject mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Inject usermanger
        /// </summary>
        private readonly IUserManager<GearUser> _userManager;

        /// <summary>
        /// Inject email sender
        /// </summary>
        private readonly IEmailSender _emailSender;

        /// <summary>
        /// Iject lead context
        /// </summary>
        private readonly ILeadContext<Lead> _leadContext;

        /// <summary>
        /// 
        /// </summary>
        private readonly INotify<GearRole> _notify;



        #endregion

        public CommentsService(ICommentContext context,
            IUserManager<GearUser> userManager,
            IMapper mapper,
            IEmailSender emailSender,
            INotify<GearRole> notify,
            ILeadContext<Lead> leadContext)
        {
            _emailSender = emailSender;
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
            _notify = notify;
            _leadContext = leadContext;
        }


        /// <summary>
        /// Add comment async
        /// </summary>
        public async  Task<ResultModel<Guid>> AddCommentAsync([Required]AddCommentViewModel model, IUrlHelper Url)
        {
            if (model == null)
                return new InvalidParametersResultModel<Guid>();

            var comment = _mapper.Map<Comment>(model);

            if (model.CommentId != null)
            {
                var parrenComment = await _context.Comments.FirstOrDefaultAsync(x => x.Id == model.CommentId);

                if (parrenComment == null)
                    return new InvalidParametersResultModel<Guid>();

                if (parrenComment.CommentId != null)
                    comment.CommentId = parrenComment.CommentId;
            }

            _context.Comments.Add(comment);

            var result = await _context.PushAsync();

            if(model.AssignedUsersIds != null)
            {
                var assigUsersResponse = await AssignUsersToCommentAsync(model.AssignedUsersIds, comment.Id);
                if (!assigUsersResponse.IsSuccess)
                {
                    result.IsSuccess = false;
                    result.Errors.Concat(assigUsersResponse.Errors);
                }
                else
                {
                    foreach (var userId in model.AssignedUsersIds)
                    {
                        var user = _userManager.UserManager.FindByIdAsync(userId.ToString());
                        string body;
                        if(model.CommentId == null)
                        {
                            body= $"Hi, {user.Result.UserFirstName}. You have been tagged by \"{comment.Author}\" to a comment , created on date: \"{comment.Changed}\". For details click the link: ";
                        }
                        else
                        {
                            var currentUser = await _userManager.GetCurrentUserAsync();
                            body = $"Hi, {user.Result.UserFirstName}. User {currentUser.Result.UserFirstName} have tagged you in his reply.For details click the link: ";
                        }
                            await SendEmailNotifications(user, Url, comment.LeadId.ToString(), body);
                    }
                }
            }
            var lead = await _leadContext.Leads
                            .Include(i => i.Team)
                            .ThenInclude(i => i.TeamMembers)
                            .FirstOrDefaultAsync(x => x.Id.ToString() == model.LeadId);
            if(lead != null)
            {
                var currenctUser = (await _userManager.GetCurrentUserAsync()).Result;
                var users = new List<Guid>();
                foreach(var member in lead.Team?.TeamMembers)
                {
                    if(member.UserId.ToString() != currenctUser.Id)
                        users.Add(member.UserId);
                }
                await SendInapNotificationsCreateComment(new AddCommentNotification
                { 
                    Entity = "Lead",
                    EntityName = lead.Name,
                    Users = users,
                    Url = "/leads/details?id=" + lead.Id,
                    AuthorName = currenctUser.UserName
                });
            }
            return new SuccessResultModel<Guid> { Result = comment.Id };
        }


        /// <summary>
        /// Delete comment async
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<ResultModel> DeleteCommentAsync(Guid? Id) =>
            await _context.RemovePermanentRecordAsync<Comment>(Id);

        /// <summary>
        /// Get all comments by lead Id async
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<ResultModel<IEnumerable<CommentViewModel>>> GetAllCommentByLeadIdAsync(Guid? LeadId)
        {
            if (LeadId == null)
                return new InvalidParametersResultModel<IEnumerable<CommentViewModel>>();

            var comments = _context.Comments
                .Include(i => i.AssignedUsers)
                .Include(i => i.ParrentComent)
                .Include(i => i.CommentReply)
                .ThenInclude(i => i.AssignedUsers)
                .Where(x => x.LeadId == LeadId && x.CommentId == null)
                .OrderByDescending(x => x.Changed);

            return new SuccessResultModel<IEnumerable<CommentViewModel>> { Result = _mapper.Map<IEnumerable<CommentViewModel>>(comments) };
        }

        /// <summary>
        /// Get all comments by lead Id async
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<ResultModel<IEnumerable<CommentViewModel>>> GetAllCommentsByOrganizationIdAsync(Guid? OrganizationId)
        {
            if (OrganizationId == null)
                return new InvalidParametersResultModel<IEnumerable<CommentViewModel>>();

            var comments = _context.Comments
                .Include(i => i.AssignedUsers)
                .Include(i => i.ParrentComent)
                .Include(i => i.CommentReply)
                .Where(x => x.OrganizationId == OrganizationId && x.CommentId == null)
                .OrderByDescending(x => x.Changed);

            return new SuccessResultModel<IEnumerable<CommentViewModel>> { Result = _mapper.Map<IEnumerable<CommentViewModel>>(comments) };
        }

        /// <summary>
        /// Get comment by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<ResultModel<CommentViewModel>> GetCommentByIdAsync(Guid? Id)
        {
            if (Id == null)
                return new InvalidParametersResultModel<CommentViewModel>();

            var comment = await _context.Comments
                .Include(i => i.AssignedUsers)
                .Include(i => i.ParrentComent)
                .Include(i => i.CommentReply)
                .FirstOrDefaultAsync(x => x.Id == Id);

            return new SuccessResultModel<CommentViewModel> { Result = _mapper.Map<CommentViewModel>(comment) };
        }


        /// <summary>
        /// Update comment async
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResultModel> UpdateCommentAsync(UpdateCommentViewModel model, IUrlHelper Url)
        {
            if (model == null)
                return new InvalidParametersResultModel();

            var comment = await _context.Comments
                .Include(i => i.AssignedUsers)
                .FirstOrDefaultAsync(x => x.Id == model.Comment.Id);

            if (comment == null)
                return new InvalidParametersResultModel();

            comment.Message = model.Comment.Message;

            _context.Comments.Update(comment);

            var result = await _context.PushAsync();
            if(model.UnassignedUsers != null)
            {
                var res = await RemoveCommentAssignedUserAsync(model.UnassignedUsers);
                if(!res.IsSuccess)
                {
                    result.IsSuccess = false;
                    result.Errors.Concat(res.Errors);
                }    
            }

            if(model.AssignedUsers != null)
            {
                var res = await AssignUsersToCommentAsync(model.AssignedUsers, model.Comment.Id);
                if (!res.IsSuccess)
                {
                    result.IsSuccess = false;
                    result.Errors.Concat(res.Errors);
                }
            }

            var assignedUsers = GetCommentAssignedUsersByCommentId(model.Comment.Id);
            foreach (var userId in assignedUsers.Result)
            {
                var user = _userManager.UserManager.FindByIdAsync(userId.UserId.ToString());
                string body = $"Hi, {user.Result.UserFirstName}. Comment to which you have been tagged by \"{comment.Author}\" to a comment , created on date: \"{comment.Changed}\". Has been updated. For details click the link: ";
                await SendEmailNotifications(user, Url, comment.LeadId.ToString(), body);
            }

            return result;
        }

        #region CommentAssignedUsers


        public async Task<ResultModel> RemoveCommentAssignedUserAsync(List<Guid> userIds)
        {
            foreach(var userId in userIds)
            {
                var assignedUser = await _context.CommentAssignedUsers.FirstOrDefaultAsync(x => x.UserId == userId);
                if(assignedUser != null)
                    _context.CommentAssignedUsers.Remove(assignedUser); ;
            }

            var ressult = await _context.PushAsync();

            return ressult;
        }

        public async Task<ResultModel> AssignUsersToCommentAsync(List<Guid> usersIds, Guid commentId)
        {
            var assignedUsers = new List<CommentAssignedUsers>();

            foreach(var userId in usersIds)
            {
                var user = await _userManager.UserManager.FindByIdAsync(userId.ToString());
                var commentUser = new CommentAssignedUsers
                {
                    UserId = userId,
                    CommentId = commentId,
                    UserEmail = user.Email
                };
                assignedUsers.Add(commentUser);
            }

            _context.CommentAssignedUsers.AddRange(assignedUsers);

            var result = await _context.PushAsync();

            return result;
        }


        public async Task<IEnumerable<CommentAssignedUsers>> GetCommentAssignedUsersByCommentId(Guid commentId)
        {
            var commentAssignedUsers = _context.CommentAssignedUsers.Where(x => x.CommentId == commentId);

            return commentAssignedUsers;
        }
        #endregion

        #region emailNotification 

        public async Task SendEmailNotifications(Task<GearUser> user, IUrlHelper Url, string leadId, string body)
        {
            var callbackUrl = Url.CommentNotificationCallBack("details", "leads", leadId);
            var mail = $"<p>{body}</p><a href='{callbackUrl}'>link</a>";
            var userEmail = _userManager.UserManager.GetEmailAsync(user.Result);
            await _emailSender.SendEmailAsync(userEmail.Result, "Task Notification!", mail);
        }

        #endregion

        #region InAppNotifications 

        public async Task SendInapNotificationsCreateComment(AddCommentNotification model)
        {
            
            if(model.Entity == "Lead")
            {
                var notification = new Notification
                {
                    Subject = "Info",
                    Content = $"A new comment has been added to {model.EntityName} by {model.AuthorName}",
                    SendEmail = false,
                    Url = model.Url
                };
                await _notify.SendNotificationAsync(model.Users, notification);
            }
        }

        #endregion
    }
}
