using GR.Core.Helpers;
using GR.Crm.Emails.Abstractions.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GR.Crm.Emails.Abstractions
{
    public interface IEmailService
    {
        /// <summary>
        /// Get all emails
        /// </summary>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<EmailViewModel>>> GetAllEmailsAsync(bool includeDeleted);

        /// <summary>
        /// Get email by id
        /// </summary>
        /// <param name="emailId"></param>
        /// <returns></returns>
        Task<ResultModel<EmailViewModel>> GetEmailByIdAsync(Guid? emailId);

        /// <summary>
        /// Get emails by contactId
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<EmailViewModel>>> GetEmailsByContactIdAsync(Guid? contactId, bool includeDeleted = false);

        /// <summary>
        /// Get emails by organizationId
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<EmailViewModel>>> GetEmailsByOrganizationIdAsync(Guid? organizationId, bool includeDeleted = false);

        /// <summary>
        /// Add new email
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<Guid>> AddNewEmailAsync(AddEmailViewModel model);

        /// <summary>
        /// Update email
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel> UpdateEmailAsync(EmailViewModel model);

        /// <summary>
        /// Delete permanently email async
        /// </summary>
        /// <param name="emailId"></param>
        /// <returns></returns>
        Task<ResultModel> DeleteEmailAsync(Guid? emailId);

        /// <summary>
        /// Add email range
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel> AddEmailRangeAsync(List<AddEmailViewModel> model);

        /// <summary>
        /// Update range email contact
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel> UpdateRangeEmailAsync(List<EmailViewModel> model);

        /// <summary>
        /// Get all email labels
        /// </summary>
        /// <returns></returns>
        Task<ResultModel<List<string>>> GetAllEmailLabelsAsync();
    }
}
