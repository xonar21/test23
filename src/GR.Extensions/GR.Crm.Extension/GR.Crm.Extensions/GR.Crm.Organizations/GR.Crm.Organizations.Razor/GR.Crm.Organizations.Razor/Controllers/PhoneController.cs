using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GR.Core.BaseControllers;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.Core.Helpers.Responses;
using GR.Crm.Organizations.Abstractions;
using GR.Crm.Organizations.Abstractions.ViewModels.PhoneListViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GR.Crm.Organizations.Razor.Controllers
{
    public class PhoneController : BaseGearController
    {
        #region Injectable
        /// <summary>
        /// inject crhPhoneListServie
        /// </summary>
        private readonly ICrmPhoneListService _crmPhoneListService;
        #endregion

        public PhoneController(ICrmPhoneListService crmPhoneListService)
        {
            _crmPhoneListService = crmPhoneListService;
        }


        /// <summary>
        /// Get phone by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<GetPhoneViewModel>))]

        public async Task<JsonResult> GetPhoneById([Required] Guid Id)
            => await JsonAsync(_crmPhoneListService.GetPhoneByIdAsync(Id));

        /// <summary>
        /// Get phone by contact id
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<GetPhoneViewModel>))]

        public async Task<JsonResult> GetPhonesByContactId([Required] Guid contactId)
            => await JsonAsync(_crmPhoneListService.GetPhonesByContactIdAsync(contactId));


        /// <summary>
        /// Get all phone labels
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<GetPhoneViewModel>))]

        public async Task<JsonResult> GetAllPhoneLabels()
            => await JsonAsync(_crmPhoneListService.GetAllPhoneLabelsAsync());


        /// <summary>
        /// Add new phone
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<Guid>))]
        public async Task<JsonResult> AddNewPhone([Required] AddPhoneViewModel model)
        {
            if (!ModelState.IsValid) return Json(new InvalidParametersResultModel().AttachModelState(ModelState));
            return await JsonAsync(_crmPhoneListService.AddNewPhoneAsync(model));
        }

        /// <summary>
        /// Add new phone list
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> AddPhoneRange([Required] List<AddPhoneViewModel> model)
            => await JsonAsync(_crmPhoneListService.AddPhoneRangeAsync(model));

        /// <summary>
        /// Update phone
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<Guid>))]
        public async Task<JsonResult> UpdatePhone([Required] PhoneViewModel model)
        {
            if (!ModelState.IsValid) return Json(new InvalidParametersResultModel().AttachModelState(ModelState));
            return await JsonAsync(_crmPhoneListService.UpdatePhoneAsync(model));
        }

        /// <summary>
        /// Update phone list
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> UpdateRangePhone([Required] List<PhoneViewModel> model)
        {
            //if (!ModelState.IsValid) return Json(new InvalidParametersResultModel().AttachModelState(ModelState));
            return await JsonAsync(_crmPhoneListService.UpdateRangePhoneAsync(model));
        }


        /// <summary>
        /// Delete phone by id
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> DeletePhoneById([Required] Guid phoneId)
            => await JsonAsync(_crmPhoneListService.DeletePhoneAsync(phoneId));
    }
}
