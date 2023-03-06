using GR.Core.BaseControllers;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Core.Helpers.Responses;
using GR.Crm.Marketing.Abstractions;
using GR.Crm.Marketing.Abstractions.Models;
using GR.Crm.Marketing.Abstractions.ViewModels.MarketingListOrganizationViewModel;
using GR.Crm.Marketing.Abstractions.ViewModels.MarketingListsViewModels;
using GR.Identity.Data.Permissions;
using GR.Identity.Permissions.Abstractions.Attributes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace GR.Crm.Marketing.Razor.Controllers
{
    public class MarketingListController : BaseGearController
    {
        #region Injectable

        /// <summary>
        /// Inject crm campaign
        /// </summary>
        private readonly ICrmMarketingListService _marketingListService;
        #endregion

        public MarketingListController(ICrmMarketingListService marketingListService)
        {
            _marketingListService = marketingListService;
        }

        public IActionResult Index(int nr = 1)
        {
            return View(nr);
        }

        /// <summary>
        /// Get paginated marketing list
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<PagedResult<GetMarketingListViewModel>>))]

        public async Task<JsonResult> GetPaginatedMarketingList(PageRequest request)
            => await JsonAsync(_marketingListService.GetPaginatedMarketingListAsync(request), SerializerSettings);

        /// <summary>
        /// Get all organizations 
        /// </summary>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<IEnumerable<GetMarketingListViewModel>>))]

        public async Task<JsonResult> GetAllMarketingLists(bool includeDeleted = false)
            => await JsonAsync(_marketingListService.GetAllActiveMarketingListsAsync(includeDeleted), SerializerSettings);

        /// <summary>
        /// Get marketing list by id
        /// </summary>
        /// <param name="marketingListId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<GetMarketingListViewModel>))]

        public async Task<JsonResult> GetMarketingListById([Required] Guid marketingListId)
            => await JsonAsync(_marketingListService.GetMarketingListByIdAsync(marketingListId));

        /// <summary>
        /// Add a marketing list
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<GetMarketingListViewModel>))]
        public async Task<JsonResult> AddMarketingList([Required] MarketingListViewModel model)
        {
            if (!ModelState.IsValid) return JsonModelStateErrors();
            return await JsonAsync(_marketingListService.AddMarketingListAsync(model));
        }

        /// <summary>
        /// Update marketing list by id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<Guid>))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmClientUpdate)]
        public async Task<JsonResult> UpdateMarketingListById([Required] MarketingListViewModel model)
        {
            if (!ModelState.IsValid) return Json(new InvalidParametersResultModel().AttachModelState(ModelState));
            return await JsonAsync(_marketingListService.UpdateMarketingListAsync(model), SerializerSettings);
        }

        /// <summary>
        /// Disable marketing list by id
        /// </summary>
        /// <param name="marketingListId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmClientDelete)]
        public async Task<JsonResult> DisableMarketingListById([Required] Guid marketingListId)
         => await JsonAsync(_marketingListService.DisableMarketingListAsync(marketingListId));


        /// <summary>
        /// Disable marketing list by id
        /// </summary>
        /// <param name="marketingListId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmClientDelete)]
        public async Task<JsonResult> EnableMarketingListById([Required] Guid marketingListId)
            => await JsonAsync(_marketingListService.EnableMarketingListAsync(marketingListId));

        /// <summary>
        /// Delete marketing list by id
        /// </summary>
        /// <param name="marketingListId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmClientDelete)]
        public async Task<JsonResult> DeleteMarketingListById([Required] Guid marketingListId)
            => await JsonAsync(_marketingListService.DeleteMarketingListAsync(marketingListId));

        /// <summary>
        /// Add organization to marketing list
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmClientDelete)]
        public async Task<JsonResult> AddNewMemberOrganizationToList([Required] MarketingListOrganizationViewModel model)
            => await JsonAsync(_marketingListService.AddNewMemberOrganizationToListAsync(model));

    }
}
