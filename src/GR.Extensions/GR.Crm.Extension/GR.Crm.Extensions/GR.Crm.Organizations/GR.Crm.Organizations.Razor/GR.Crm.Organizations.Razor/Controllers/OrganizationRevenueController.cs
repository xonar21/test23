using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GR.Core.BaseControllers;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Core.Helpers.Responses;
using GR.Crm.Organizations.Abstractions;
using GR.Crm.Organizations.Abstractions.ViewModels.RevenueViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GR.Crm.Organizations.Razor.Controllers
{
    public class OrganizationRevenueController : BaseGearController
    {
        #region Injectable

        /// <summary>
        /// Revenue Service
        /// </summary>
        private readonly IOrganizationRevenueService _organizationRevenueService;

        #endregion

        public OrganizationRevenueController(IOrganizationRevenueService organizationRevenueService)
        {
            _organizationRevenueService = organizationRevenueService;
        }
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Get all paginated organization revenues 
        /// </summary>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<PagedResult<GetRevenueViewModel>>))]
        public async Task<JsonResult> GetAllPaginatedRevenuesByOrganization(PageRequest request, Guid organizationId)
            => await JsonAsync(_organizationRevenueService.GetAllRevenuesByOrganizationPaginatedAsync(request, organizationId));
        
        /// <summary>
        /// Get organization revenue by Id
        /// </summary>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<PagedResult<GetRevenueViewModel>>))]
        public async Task<JsonResult> GetRevenueById(Guid revenueId)
            => await JsonAsync(_organizationRevenueService.GetRevenueByIdAsync(revenueId)); 
        
        /// <summary>
        /// Get organization revenue by Id
        /// </summary>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<PagedResult<GetRevenueViewModel>>))]
        public async Task<JsonResult> GetAllRevenuesByOrganization( Guid organizationId, bool includeDeleted)
            => await JsonAsync(_organizationRevenueService.GetAllActiveRevenuesByOrganizationAsync(organizationId, includeDeleted));

        /// <summary>
        /// Add new organiation revenue
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<Guid>))]
        public async Task<JsonResult> AddNewOrganizationRevenue([Required] RevenueViewModel model)
        {
            if (!ModelState.IsValid) return Json(new InvalidParametersResultModel().AttachModelState(ModelState));
            return await JsonAsync(_organizationRevenueService.AddNewRevenue(model));
        }

        /// <summary>
        /// Delete organization addresses by id
        /// </summary>
        /// <param name="revenueId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(Task<ResultModel>))]
        public async Task<JsonResult> DeleteOrganizationRevenue([Required] Guid revenueId)
            => await JsonAsync(_organizationRevenueService.DeleteOrganizationRevenue(revenueId), SerializerSettings);

        /// <summary>
        /// Update organization revenue
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<Guid>))]
        public async Task<JsonResult> UpdateOrganizationRevenue([Required] RevenueViewModel model)
        {
            if (!ModelState.IsValid) return Json(new InvalidParametersResultModel().AttachModelState(ModelState));
            return await JsonAsync(_organizationRevenueService.UpdateOrganizationRevenue(model), SerializerSettings);
        }
    }
}
