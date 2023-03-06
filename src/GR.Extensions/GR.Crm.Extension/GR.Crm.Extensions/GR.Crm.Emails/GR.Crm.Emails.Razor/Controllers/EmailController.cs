using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GR.Core.BaseControllers;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.Core.Helpers.Responses;
using GR.Crm.Emails.Abstractions;
using GR.Crm.Emails.Abstractions.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GR.Crm.Emails.Razor.Controllers
{
    public class EmailController : BaseGearController
    {
        #region Injectable
        private readonly IEmailService _emailService;
        #endregion

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }


        /// <summary>
        /// Get email by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<EmailViewModel>))]

        public async Task<JsonResult> GetEmailById([Required] Guid Id)
            => await JsonAsync(_emailService.GetEmailByIdAsync(Id));

        /// <summary>
        /// Get email by contact id
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<IEnumerable<EmailViewModel>>))]

        public async Task<JsonResult> GetEmailsByContactId([Required] Guid contactId)
            => await JsonAsync(_emailService.GetEmailsByContactIdAsync(contactId));

        /// <summary>
        /// Get email by contact id
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<IEnumerable<EmailViewModel>>))]

        public async Task<JsonResult> GetEmailsByOrganizationId([Required] Guid organizationId)
            => await JsonAsync(_emailService.GetEmailsByOrganizationIdAsync(organizationId));


        /// <summary>
        /// Get all emails labels
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<List<string>>))]

        public async Task<JsonResult> GetAllEmailLabel()
            => await JsonAsync(_emailService.GetAllEmailLabelsAsync());


        /// <summary>
        /// Add new phone
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<Guid>))]
        public async Task<JsonResult> AddNewEmail([Required] AddEmailViewModel model)
        {
            if (!ModelState.IsValid) return Json(new InvalidParametersResultModel().AttachModelState(ModelState));
            return await JsonAsync(_emailService.AddNewEmailAsync(model));
        }

        /// <summary>
        /// Add new phone list
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> AddEmailRange([Required] List<AddEmailViewModel> model)
            => await JsonAsync(_emailService.AddEmailRangeAsync(model));

        /// <summary>
        /// Update email
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> UpdateEmailAsync([Required] EmailViewModel model)
        {
            if (!ModelState.IsValid) return Json(new InvalidParametersResultModel().AttachModelState(ModelState));
            return await JsonAsync(_emailService.UpdateEmailAsync(model));
        }

        /// <summary>
        /// Update phone list
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> UpdateRangeEmail([Required] List<EmailViewModel> model)
            => await JsonAsync(_emailService.UpdateRangeEmailAsync(model));


        /// <summary>
        /// Delete email by id
        /// </summary>
        /// <param name="emailId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> DeleteEmailById([Required] Guid emailId)
            => await JsonAsync(_emailService.DeleteEmailAsync(emailId));
    }
}
