using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GR.Core.BaseControllers;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.Core.Helpers.Responses;
using GR.Crm.Organizations.Abstractions;
using GR.Crm.Organizations.Abstractions.ViewModels.HelpersViewModels;
using GR.Crm.Organizations.Abstractions.ViewModels.OrganizationStagesViewModels;
using GR.Crm.Organizations.Abstractions.ViewModels.OrganizationStatesViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GR.Crm.Organizations.Razor.Controllers
{
    [Authorize]
    public class OrganizationHelperController : BaseGearController
    {
        #region Injectable

        /// <summary>
        /// Industry service
        /// </summary>
        private readonly ICrmIndustryService _crmIndustryService;

        /// <summary>
        /// Employee service
        /// </summary>
        private readonly ICrmEmployeeService _crmEmployeeService;

        /// <summary>
        /// Employee service
        /// </summary>
        private readonly ICrmOrganizationHelperService _crmOrganizatioHelperService;

        #endregion

        public OrganizationHelperController(ICrmIndustryService crmIndustryService, 
            ICrmEmployeeService crmEmployeeService,
            ICrmOrganizationHelperService crmOrganizatioHelperService)
        {
            _crmIndustryService = crmIndustryService;
            _crmEmployeeService = crmEmployeeService;
            _crmOrganizatioHelperService = crmOrganizatioHelperService;
        }

        /// <summary>
        /// Get selectors Employees and Industries
        /// </summary>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<ListSelectorsForOrganization>))]
        public virtual async Task<JsonResult> GetSelectorsForOrganization(bool includeDeleted = false)
        {
            var listSelectors = new ListSelectorsForOrganization();

            var listIndustriesRequest = await _crmIndustryService.GetAllIndustriesAsync(includeDeleted);
            var lisEmployeesRequest = await _crmEmployeeService.GetAllEmployeesAsync();
           

            if (listIndustriesRequest.IsSuccess)
            {
                listSelectors.ListIndustry = listIndustriesRequest.Result.Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name
                });
            }

            if (lisEmployeesRequest.IsSuccess)
            {
                listSelectors.ListEmployees = lisEmployeesRequest.Result.Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Interval
                });
            }

            var result = new ResultModel<ListSelectorsForOrganization>
            {
                IsSuccess = true,
                Result = listSelectors
            };

            return Json(result, SerializerSettings);

        }

        /// <summary>
        /// Add new organization stage
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<Guid>))]
        public async Task<JsonResult> AddNewOrganizationStage([Required] AddOrganizationStageViewModel model)
        {
            if (!ModelState.IsValid) return Json(new InvalidParametersResultModel().AttachModelState(ModelState));
            return await JsonAsync(_crmOrganizatioHelperService.AddStageAsync(model));
        }

        /// <summary>
        /// Add new organization state
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<Guid>))]
        public async Task<JsonResult> AddNewOrganizationState([Required] AddOrganizationStateViewModel model)
        {
            if (!ModelState.IsValid) return Json(new InvalidParametersResultModel().AttachModelState(ModelState));
            return await JsonAsync(_crmOrganizatioHelperService.AddStateAsync(model));
        }

        /// <summary>
        /// Get all organization stages
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> GetAllOrganizationStages()
            => await JsonAsync(_crmOrganizatioHelperService.GetAllStagesAsync());

        /// <summary>
        /// Get all organization states
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> GetAllOrganizationStates()
            => await JsonAsync(_crmOrganizatioHelperService.GetAllStatesAsync());

        /// <summary>
        /// Get all organization states by stage
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> GetOrganizationStatesByStage(Guid stageId)
            => await JsonAsync(_crmOrganizatioHelperService.GetAllStatesByStageAsync(stageId));
    }
}
