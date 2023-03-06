using GR.Core.BaseControllers;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Core.Helpers.Responses;
using GR.Crm.Marketing.Abstractions;
using GR.Crm.Marketing.Abstractions.ViewModels;
using GR.Crm.Marketing.Abstractions.ViewModels.CampaignsMarketingListsViewModels;
using GR.Crm.Marketing.Abstractions.ViewModels.CampaignsViewModels;
using GR.Identity.Data.Permissions;
using GR.Identity.Permissions.Abstractions.Attributes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;


namespace GR.Crm.Marketing.Razor.Controllers
{
    public class CampaignController : BaseGearController 
    {

        #region Injectable

        /// <summary>
        /// Inject crm campaign
        /// </summary>
        private readonly ICrmCampaignService _campaignService;
        #endregion

        public CampaignController(ICrmCampaignService campaignService)
        {
            _campaignService = campaignService;
        }

        public IActionResult Index(int nr = 1)
        {
            return View(nr);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var contactRequest = await _campaignService.GetCampaignByIdAsync(id);
            if (!contactRequest.IsSuccess) return NotFound();
            return View(contactRequest.Result);
        }


        /// <summary>
        /// Get paginated campaign
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<PagedResult<GetCampaignViewModel>>))]

        public async Task<JsonResult> GetAllCampaignsPaginated(PageRequest request)
            => await JsonAsync(_campaignService.GetPaginatedCampaignAsync(request), SerializerSettings);

        /// <summary>
        /// Add new campaign
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<CampaignViewModel>))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmClientCreate)]
        public async Task<JsonResult> AddNewCampaign([Required] CampaignViewModel model)
        {
            if (!ModelState.IsValid) return Json(new InvalidParametersResultModel().AttachModelState(ModelState));
            return await JsonAsync(_campaignService.AddNewCampaignAsync(model), SerializerSettings);
        }


        /// <summary>
        /// Disable campaign by id
        /// </summary>
        /// <param name="campaignId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmClientDelete)]
        public async Task<JsonResult> DisableCampaignById([Required] Guid campaignId)
         => await JsonAsync(_campaignService.DisableCampaignAsync(campaignId));


        /// <summary>
        /// Enable campaign by id
        /// </summary>
        /// <param name="campaignId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmClientDelete)]
        public async Task<JsonResult> EnableCampaignById([Required] Guid campaignId)
            => await JsonAsync(_campaignService.EnableCampaignAsync(campaignId));

        /// <summary>
        /// Delete campaign by id
        /// </summary>
        /// <param name="campaignId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmClientDelete)]
        public async Task<JsonResult> DeleteCampaignById([Required] Guid campaignId)
            => await JsonAsync(_campaignService.DeleteCampaignAsync(campaignId));

        /// <summary>
        /// Get campaign by id
        /// </summary>
        /// <param name="campaignId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<GetCampaignViewModel>))]

        public async Task<JsonResult> GetCampaignById([Required] Guid campaignId)
            => await JsonAsync(_campaignService.GetCampaignByIdAsync(campaignId));

        /// <summary>
        /// Update campaign
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<Guid>))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmClientUpdate)]
        public async Task<JsonResult> UpdateCampaignById([Required] CampaignViewModel model)
        {
            if (!ModelState.IsValid) return Json(new InvalidParametersResultModel().AttachModelState(ModelState));
            return await JsonAsync(_campaignService.UpdateCampaignAsync(model), SerializerSettings);
        }

        /// <summary>
        /// Get all campaigns
        /// </summary>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<IEnumerable<GetCampaignViewModel>>))]
        public async Task<JsonResult> GetAllCampaigns(bool includeDeleted = false)
            => await JsonAsync(_campaignService.GetAllActiveCampaignsAsync(includeDeleted));

        /// <summary>
        /// Add marketing list to campaign
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmClientDelete)]
        public async Task<JsonResult> AddMarketingListToCampaign([Required] CampaignMarketingListViewModel model)
            => await JsonAsync(_campaignService.AddMarketingListToCampaignAsync(model));

    }
}
