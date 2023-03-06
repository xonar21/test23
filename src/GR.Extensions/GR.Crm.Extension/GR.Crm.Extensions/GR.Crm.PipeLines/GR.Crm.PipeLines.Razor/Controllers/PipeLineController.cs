using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GR.Core.Attributes.Documentation;
using GR.Core.BaseControllers;
using GR.Core.Helpers;
using GR.Core.Helpers.Global;
using GR.Core.Helpers.Pagination;
using GR.Crm.Abstractions;
using GR.Crm.Leads.Abstractions;
using GR.Crm.Leads.Abstractions.Models;
using GR.Crm.PipeLines.Abstractions;
using GR.Crm.PipeLines.Abstractions.Models;
using GR.Crm.PipeLines.Abstractions.ViewModels;
using GR.Identity.Abstractions;
using GR.Identity.Data.Permissions;
using GR.Identity.Permissions.Abstractions.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace GR.Crm.PipeLines.Razor.Controllers
{
    /// <summary>
    /// PipeLine Rest API
    /// </summary>
    [Author(Authors.LUPEI_NICOLAE, 1.1)]
    [Authorize]
    public sealed class PipeLineController : BaseGearController
    {
        #region Injectable

        /// <summary>
        /// Inject pipeline service
        /// </summary>
        private readonly ICrmPipeLineService _service;


        /// <summary>
        /// Lead service
        /// </summary>
        private readonly ILeadService<Lead> _leadService;

        /// <summary>
        /// User manager
        /// </summary>
        private readonly IUserManager<GearUser> _userManager;

        /// <summary>
        /// Inject pipeline service
        /// </summary>
        private readonly ICrmContext _crmContext;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service"></param>
        /// <param name="leadService"></param>
        public PipeLineController(ICrmPipeLineService service, 
            ILeadService<Lead> leadService,
            IUserManager<GearUser> userManager,
            IConfiguration configuration,
            ICrmContext crmContext)
        {
            _service = service;
            _leadService = leadService;
            _userManager = userManager;
            _crmContext = crmContext;
            GlobalCurrency = configuration.GetSection("GeneralConfigurations").GetSection("CurrencyCode").Value;
        }

        private string GlobalCurrency { get; set; }
        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index(int nr = 1)
        {
            return View(nr);
        }

       
        public async Task<IActionResult> PipeLineLeads(string name, int nr = 1)
        {
            ViewBag.nr = nr;
            name = name.Replace('_', ' ');
            var pipelineRequest = await _service.FindPipeLineByNameAsync(name);
            if (!pipelineRequest.IsSuccess) return NotFound();
            var currentUser = await _userManager.GetCurrentUserAsync();
            ViewBag.CurrentUserId = currentUser.Result.Id;
            ViewBag.GlobalCurrencySymbol = string.IsNullOrEmpty(GlobalCurrency) ? "€" :  _crmContext.Currencies.FirstOrDefault(x => x.Code == GlobalCurrency).Symbol;
            return View(pipelineRequest.Result);
        }

        public async Task<IActionResult> CreateLead(Guid pipeLineId)
        {
            var pipeLine = await _service.FindPipeLineByIdAsync(pipeLineId);
            if (!pipeLine.IsSuccess) return NotFound();
            return View(pipeLine.Result);
        }

        public async Task<IActionResult> Edit(Guid LeadId)
        {
            var lead = await _leadService.GetLeadByIdAsync(LeadId);
            return View(lead.Result);
        }

        /// <summary>
        /// Get  pipeline by id
        /// </summary>
        /// <param name="pipeLineId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<PipeLine>))]
        public async Task<JsonResult> GetPipeLineById([Required] Guid pipeLineId) =>
            await JsonAsync(_service.GetPipeLineByIdAsync(pipeLineId));


        /// <summary>
        /// Get all pipelines
        /// </summary>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<IEnumerable<PipeLine>>))]
        public async Task<JsonResult> GetAllPipeLines(bool includeDeleted = false) =>
            await JsonAsync(_service.GetAllPipeLinesAsync(includeDeleted));

        /// <summary>
        /// Get all paginated pipelines
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<PagedResult<PipeLine>>))]
        public async Task<JsonResult> GetAllPaginatedPipeLines(PageRequest request) =>
            await JsonAsync(_service.GetAllPaginatedPipeLinesAsync(request));

        /// <summary>
        /// Get pipeline stages
        /// </summary>
        /// <param name="pipeLineId"></param>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<IEnumerable<Stage>>))]
        public async Task<JsonResult> GetPipeLineStages([Required]Guid? pipeLineId, bool includeDeleted = false)
            => await JsonAsync(_service.GetPipeLineStagesAsync(pipeLineId, includeDeleted));

        /// <summary>
        /// Add pipeline
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        //[Roles(GlobalResources.Roles.ADMINISTRATOR)]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<Guid>))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmPipeLineCreate)]
        public async Task<JsonResult> AddPipeLine([Required]CreatePipeLineViewModel model)
        {
            if (!ModelState.IsValid) return JsonModelStateErrors();
            var addPipeLineRequest = await _service.AddPipeLineAsync(model);
            return Json(addPipeLineRequest);
        }

        /// <summary>
        /// Update pipeline
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        //[Roles(GlobalResources.Roles.ADMINISTRATOR)]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmPipeLineUpdate)]
        public async Task<JsonResult> UpdatePipeLine([Required] UpdatePipeLineViewModel model)
        {
            if (!ModelState.IsValid) return JsonModelStateErrors();
            return await JsonAsync(_service.UpdatePipeLineAsync(model));
        }



        /// <summary>
        /// Get stage by id
        /// </summary>
        /// <param name="stageId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<Stage>))]
        public async Task<JsonResult> FindStageById([Required] Guid stageId) =>
            await JsonAsync(_service.FindStageByIdAsync(stageId));


        /// <summary>
        /// Update stage
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
       // [Roles(GlobalResources.Roles.ADMINISTRATOR)]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmPipeLineStageUpdate)]
        public async Task<JsonResult> UpdateStage([Required] UpdateStageViewModel model)
        {
            if (!ModelState.IsValid) return JsonModelStateErrors();
            return await JsonAsync(_service.UpdateStageAsync(model));
        }

        /// <summary>
        /// Order stages
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        //[Roles(GlobalResources.Roles.ADMINISTRATOR)]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmPipeLineStageUpdate)]
        public async Task<JsonResult> OrderStages([Required] IEnumerable<OrderStagesViewModel> model)
        {
            if (!ModelState.IsValid) return JsonModelStateErrors();
            return await JsonAsync(_service.OrderStagesAsync(model));
        }

        /// <summary>
        /// Add stage to pipeline
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        //[Roles(GlobalResources.Roles.ADMINISTRATOR)]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<Guid>))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmPipeLineStageCreate)]
        public async Task<JsonResult> AddStageToPipeLine(AddStageViewModel model)
        {
            if (!ModelState.IsValid) return JsonModelStateErrors();
            return await JsonAsync(_service.AddStageToPipeLineAsync(model));
        }

        /// <summary>
        /// Disable stage
        /// </summary>
        /// <param name="stageId"></param>
        /// <returns></returns>
        [HttpDelete]
       // [Roles(GlobalResources.Roles.ADMINISTRATOR)]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmPipeLineStageDelete)]
        public async Task<JsonResult> DisableStage([Required] Guid? stageId)
        {

            var leadRequest = await _leadService.GetLeadsByStageIdAsync(stageId, false);

            if (!leadRequest.IsSuccess) return await JsonAsync(_service.DisableStageAsync(stageId));

            var leads = leadRequest.Result;

            if (leads.FirstOrDefault(x => x.IsDeleted == false) != null)
                return Json(new ResultModel { IsSuccess = false, Errors = new List<IErrorModel> { new ErrorModel { Message = "Stage has active lead" } } });

            return await JsonAsync(_service.DisableStageAsync(stageId));
        }

        /// <summary>
        /// Activate stage
        /// </summary>
        /// <param name="stageId"></param>
        /// <returns></returns>
        [HttpDelete]
        //[Roles(GlobalResources.Roles.ADMINISTRATOR)]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmPipeLineStageDelete)]
        public async Task<JsonResult> ActivateStage([Required]Guid? stageId)
            => await JsonAsync(_service.ActivateStageAsync(stageId));

        /// <summary>
        /// Remove stage permanently
        /// </summary>
        /// <param name="stageId"></param>
        /// <returns></returns>
        [HttpDelete]
        //[Roles(GlobalResources.Roles.ADMINISTRATOR)]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmPipeLineStageDelete)]
        public async Task<JsonResult> RemoveStagePermanently([Required]Guid? stageId)
            => await JsonAsync(_service.RemoveStagePermanentlyAsync(stageId));

        /// <summary>
        /// Disable pipeLine
        /// </summary>
        /// <param name="pipeLineId"></param>
        /// <returns></returns>
        [HttpDelete]
        //[Roles(GlobalResources.Roles.ADMINISTRATOR)]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmPipeLineDelete)]
        public async Task<JsonResult> DisablePipeLine([Required] Guid? pipeLineId)
        {
            var leadRequest = await _leadService.GetLeadsByPipeLineIdAsync(pipeLineId, false);

            if (!leadRequest.IsSuccess) return await JsonAsync(_service.DisablePipeLineAsync(pipeLineId));

            var leads = leadRequest.Result;

            if (leads.FirstOrDefault(x => x.IsDeleted == false) != null)
                return Json(new ResultModel { IsSuccess = false, Errors = new List<IErrorModel> { new ErrorModel { Message = "PipeLine has active lead" } } });

            return await JsonAsync(_service.DisablePipeLineAsync(pipeLineId));
        }

        /// <summary>
        /// Activate pipeLine
        /// </summary>
        /// <param name="pipeLineId"></param>
        /// <returns></returns>
        [HttpDelete]
       // [Roles(GlobalResources.Roles.ADMINISTRATOR)]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmPipeLineDelete)]
        public async Task<JsonResult> ActivatePipeLine([Required]Guid? pipeLineId)
            => await JsonAsync(_service.ActivatePipeLineAsync(pipeLineId));

        /// <summary>
        /// Remove pipeline permanently
        /// </summary>
        /// <param name="pipeLineId"></param>
        /// <returns></returns>
        [HttpDelete]
       // [Roles(GlobalResources.Roles.ADMINISTRATOR)]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmPipeLineDelete)]
        public async Task<JsonResult> RemovePipeLinePermanently([Required]Guid? pipeLineId)
            => await JsonAsync(_service.RemovePipeLinePermanently(pipeLineId));
    }
}