using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GR.CloudStorage.Abstractions;
using GR.CloudStorage.Abstractions.Enums;
using GR.CloudStorage.Abstractions.Models;
using GR.Core;
using GR.Core.Attributes.Documentation;
using GR.Core.BaseControllers;
using GR.Core.Helpers;
using GR.Core.Helpers.Global;
using GR.Core.Helpers.Pagination;
using GR.Crm.Abstractions;
using GR.Crm.Abstractions.ViewModels.MergeViewModels;
using GR.Crm.Leads.Abstractions;
using GR.Crm.Leads.Abstractions.Models;
using GR.Crm.Leads.Abstractions.ViewModels;
using GR.Crm.Organizations.Abstractions.ViewModels.ContactsViewModels;
using GR.Identity.Abstractions;
using GR.Identity.Abstractions.Helpers.Attributes;
using GR.Identity.Data.Permissions;
using GR.Identity.Permissions.Abstractions.Attributes;
using GR.TaskManager.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GR.Crm.Leads.Razor.Controllers
{
    [Authorize]
    [Author(Authors.LUPEI_NICOLAE, 1.1)]
    public sealed class LeadsController : BaseGearController
    {
        #region Injectable

        /// <summary>
        /// Inject lead service
        /// </summary>
        private readonly ILeadService<Lead> _leadService;


        /// <summary>
        /// Inject import service
        /// </summary>
        private readonly ICrmImportExportService _importExportService;

        /// <summary>
        /// Inject task service
        /// </summary>
        private readonly ITaskManager _taskService;

        /// <summary>
        /// Inject agreement service;
        /// </summary>
        private readonly IAgreementService _agreementService;


        /// <summary>
        /// Inject merge service;
        /// </summary>
        private readonly ICrmMergeService _mergeService;


        private readonly IUserTokenDataService _userTokenDataService;

        private readonly IUserManager<GearUser> _userManager;



        #endregion


        public async Task<IActionResult> Details(Guid id)
        {
            var leadRequest = await _leadService.FindLeadByIdAsync(id);
            if (!leadRequest.IsSuccess) return NotFound();
            ViewBag.UserHasToken = await _userTokenDataService.CheckUserToken(Guid.Parse((await _userManager.GetCurrentUserAsync()).Result.Id));
            ViewBag.CloudLoginUrl = Url.Action("LogInMicrosoft", "Home", null);
            return View(leadRequest.Result);
        }

        public IActionResult NoGoState(int nr = 0)
        {
            return View(nr);
        }

        

        public IActionResult LeadStates()
        {
            return View();
        }

        public async Task<IActionResult> LeadStatusDetails(Guid id)
        {
            var leadRequest = await _leadService.GetLeadStateByIdAsync(id);
            if (!leadRequest.IsSuccess) return NotFound();
            return View(leadRequest.Result);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="leadService"></param>
        /// <param name="taskService"></param>
        /// <param name="agreementService"></param>
        public LeadsController(ILeadService<Lead> leadService,
            ITaskManager taskService,
            IAgreementService agreementService,
            ICrmMergeService mergeService,
            ICrmImportExportService importExportService,
            IUserTokenDataService userTokenDataService,
            IUserManager<GearUser> userManager)
        {
            _leadService = leadService;
            _taskService = taskService;
            _mergeService = mergeService;
            _agreementService = agreementService;
            _importExportService = importExportService;
            _userTokenDataService = userTokenDataService;
            _userManager = userManager;
        }

        private string leadStateNewId { get; set; }

        #region Leads


        /// <summary>
        /// Get  lead by id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<GetLeadsViewModel>))]
        public async Task<JsonResult> GetLeadById([Required] Guid LeadId) => 
            await JsonAsync(_leadService.GetLeadByIdAsync(LeadId));


        /// <summary>
        /// Get all leads
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<IEnumerable<Lead>>))]
       
        public async Task<JsonResult> GetAllLeads(bool includeDeleted = false) => await JsonAsync(_leadService.GetAllLeadsAsync(includeDeleted));



        /// <summary>
        /// Get leads by pipeLine
        /// </summary>
        /// <param name="pipeLineId"></param>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<IEnumerable<Lead>>))]
      
        public async Task<JsonResult> GetLeadsByPipeLineId([Required]Guid? pipeLineId, bool includeDeleted = false)
            => await JsonAsync(_leadService.GetLeadsByPipeLineIdAsync(pipeLineId, includeDeleted));

        /// <summary>
        /// Get leads by organization id
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<IEnumerable<Lead>>))]
       
        public async Task<JsonResult> GetLeadsByOrganizationId([Required]Guid? organizationId, bool includeDeleted = false)
            => await JsonAsync(_leadService.GetLeadsByOrganizationIdAsync(organizationId, includeDeleted));

        /// <summary>
        /// Get leads by pipeline
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pipeLineId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(PagedResult<GetLeadsViewModel>))]
       
        public async Task<JsonResult> GetPaginatedLeadsByPipeLineId([Required]PageRequest request, [Required]Guid? pipeLineId)
            => await JsonAsync(_leadService.GetPaginatedLeadsByPipeLineIdAsync(request, pipeLineId));

        /// <summary>
        /// Get leads by pipeline
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pipeLineId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(PagedResult<IEnumerable<GetGridLeadsViewModel>>))]

        public async Task<JsonResult> GetGridLeadsByPipeLineId([Required] IEnumerable<PageRequestFilter> filters, [Required] Guid? pipeLineId)
            => await JsonAsync(_leadService.GetGridLeadsByPipeLineIdAsync(filters, pipeLineId));


        /// <summary>
        /// Get contacts by lead id
        /// </summary>
        /// <param name="leadId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<PagedResult<GetContactViewModel>>))]

        public async Task<JsonResult> GetAllContactsByLeadId(PageRequest request, [Required] Guid leadId)
            => await JsonAsync(_leadService.GetAllContactsByLeadIdAsync(request, leadId));

        /// <summary>
        /// Get paginated request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(PagedResult<Lead>))]
       
        public async Task<JsonResult> GetPaginatedLeads([Required] PageRequest request)
            => await JsonAsync(_leadService.GetPaginatedLeadsAsync(request));

        /// <summary>
        /// Get leads by organization id
        /// </summary>
        /// <param name="request"></param>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(PagedResult<Lead>))]
        
        public async Task<JsonResult> GetPaginatedLeadsByOrganizationId([Required] PageRequest request, [Required]Guid? organizationId)
            => await JsonAsync(_leadService.GetPaginatedLeadsByOrganizationIdAsync(request, organizationId));

        /// <summary>
        /// Add a lead
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<Guid>))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmLeadCreate)]
        public async Task<JsonResult> AddLead([Required]AddLeadViewModel model)
        {
            if (!ModelState.IsValid) return JsonModelStateErrors();
            return await JsonAsync(_leadService.AddLeadAsync(model));
        }

        /// <summary>
        /// Add productOrService 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmLeadCreate)]
        public async Task<JsonResult> AddProductOrServices(List<AddProductOrServiceViewModel> model)
        {
            if (!ModelState.IsValid) return JsonModelStateErrors();
            return await JsonAsync(_leadService.AddProductOrServicesAsync(model));
        }

        /// <summary>
        /// Update  productOrService 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmLeadCreate)]
        public async Task<JsonResult> UpdateProductOrServices(List<UpdateProductOrServiceViewModel> model)
        => await JsonAsync(_leadService.UpdateProductOrServicesAsync(model));
       

        /// <summary>
        /// Delete  productOrService 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmLeadCreate)]
        public async Task<JsonResult> DeleteProductOrServices(Guid id)
        {
            if (!ModelState.IsValid) return JsonModelStateErrors();
            return await JsonAsync(_leadService.DeleteProductOrServicesAsync(id));
        }

        /// <summary>
        /// Update lead
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmLeadUpdate)]
        public async Task<JsonResult> UpdateLead([Required]UpdateLeadViewModel model)
        {
            if (!ModelState.IsValid) return JsonModelStateErrors();
            return await JsonAsync(_leadService.UpdateLeadAsync(model, Url));
        }


        /// <summary>
        /// Add contact to lead
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmLeadUpdate)]
        public async Task<JsonResult> AddLeadContact([Required] LeadContactViewModel model)
        {
            if (!ModelState.IsValid) return JsonModelStateErrors();
            return await JsonAsync(_leadService.AddContactToLead(model));
        }

        /// <summary>
        /// Delete contact from lead 
        /// </summary>
        /// <param name="contactId"></param>
        /// <param name="leadId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmLeadDelete)]
        public async Task<JsonResult> DeleteLeadContact(Guid contactId, Guid leadId)
        => await JsonAsync(_leadService.DeleteContactFromLead(contactId, leadId));

        /// <summary>
        /// Activate lead
        /// </summary>
        /// <param name="leadId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmLeadDelete)]
        public async Task<JsonResult> ActivateLead(Guid? leadId)
            => await JsonAsync(_leadService.ActivateLeadAsync(leadId));

        /// <summary>
        /// Disable lead
        /// </summary>
        /// <param name="leadId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmLeadDelete)]
        public async Task<JsonResult> DisableLead(Guid? leadId)
        {
            var taskRequest = await _taskService.GetAllTaskByLeadIdAsync(leadId);
            var agreementRequest = await _agreementService.GetAllAgreementsByLeadIdAsync(leadId);

            if (taskRequest.IsSuccess && taskRequest.Result.Any())
                return Json(new ResultModel { IsSuccess = false, Errors = new List<IErrorModel> { new ErrorModel { Message = "Lead has task" } } });

            if (agreementRequest.IsSuccess && agreementRequest.Result.Any())
                return Json(new ResultModel { IsSuccess = false, Errors = new List<IErrorModel> { new ErrorModel { Message = "Lead has agreement" } } });

            return await JsonAsync(_leadService.DisableLeadAsync(leadId));
        }

        /// <summary>
        /// Disable lead
        /// </summary>
        /// <param name="leadId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmLeadDelete)]
        public async Task<JsonResult> DeleteLead(Guid? leadId)
            => await JsonAsync(_leadService.DeleteLeadAsync(leadId));

        /// <summary>
        /// Move lead to another stage
        /// </summary>
        /// <param name="leadId"></param>
        /// <param name="stageId"></param>
        /// <param name="stateId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmLeadUpdate)]
        public async Task<JsonResult> MoveLeadToStage([Required] Guid? leadId, [Required] Guid? stageId, Guid? stateId)
            => await JsonAsync(_leadService.MoveLeadToStageAsync(leadId, stageId, stateId, Url));


        /// <summary>
        /// Merge Leads
        /// </summary>
        /// <param name="LeadsToBeMerged"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> MergeLeads([FromBody] MergeLeadsViewModel LeadsToBeMerged)
             => await JsonAsync(_mergeService.MergeLeadsAsync(LeadsToBeMerged, Url), SerializerSettings);


        /// <summary>
        /// Import Leads
        /// </summary>
        /// <param name="leadsToImport"></param>
        /// <returns></returns
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> ImportLeads(IFormCollection leadsToImport)
            => await JsonAsync(_importExportService.ImportAsync(leadsToImport, Url), SerializerSettings);


        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> UploadFile(UploadFileViewModel model)
            => await JsonAsync(_leadService.UploadFile(model.LeadId, model.File));



        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<List<CloudMetaData>>))]
        public async Task<JsonResult> GetFiles(Guid LeadId)
            => await JsonAsync(_leadService.GetFiles(LeadId));


        [HttpDelete]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> DeleteFile(string id, string path, string fileName)
            => await JsonAsync(_leadService.DeleteFile(id, path, fileName));


        #endregion

        #region Lead states

        /// <summary>
        /// Add lead state
        /// </summary>
        /// <param name="name"></param>
        /// <param name="styleClass"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        [HttpPut]
        //[Roles(GlobalResources.Roles.ADMINISTRATOR)]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<Guid>))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmLeadStateCreate)]
        public async Task<JsonResult> AddLeadState([Required] [MinLength(2)] string name, string styleClass, string description)
        {
            if (!ModelState.IsValid) return JsonModelStateErrors();
            return await JsonAsync(_leadService.AddLeadStateAsync(name, styleClass, description));
        }

        /// <summary>
        /// Update states order
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        // [Roles(GlobalResources.Roles.ADMINISTRATOR)]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmLeadStateUpdate)]
        public async Task<JsonResult> OrderLeadStates([Required] IEnumerable<OrderLeadStatesViewModel> data)
        {
            if (!ModelState.IsValid) return JsonModelStateErrors();
            return await JsonAsync(_leadService.OrderLeadStateAsync(data));
        }

        /// <summary>
        /// Get lead state by id
        /// </summary>
        /// <param name="stateId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<LeadState>))]
        public async Task<JsonResult> GetLeadStateById([Required] Guid stateId)
            => await JsonAsync(_leadService.GetLeadStateByIdAsync(stateId));


        /// <summary>
        /// Get all lead states
        /// </summary>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> GetAllLeadStates(bool includeDeleted = false)
            => await JsonAsync(_leadService.GetAllLeadStatesAsync(includeDeleted));

        /// <summary>
        /// Get all lead states by stage
        /// </summary>
        /// <param name="includeDeleted"></param>
        /// <param name="stageId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> GetAllLeadStatesByStage([Required] Guid stageId, bool includeDeleted = false)
            => await JsonAsync(_leadService.GetAllLeadStatesByStageAsync(stageId, includeDeleted));

        /// <summary>
        /// Activate lead state
        /// </summary>
        /// <param name="leadStateId"></param>
        /// <returns></returns>
        [HttpDelete]
        //[Roles(GlobalResources.Roles.ADMINISTRATOR)]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmLeadStateDelete)]
        public async Task<JsonResult> ActivateLeadState(Guid? leadStateId)
            => await JsonAsync(_leadService.ActivateLeadStateAsync(leadStateId));

        /// <summary>
        /// Disable lead state
        /// </summary>
        /// <param name="leadStateId"></param>
        /// <returns></returns>
        [HttpDelete]
        // [Roles(GlobalResources.Roles.ADMINISTRATOR)]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmLeadStateDelete)]
        public async Task<JsonResult> DisableLeadState(Guid? leadStateId)
        {
            var leadRequest = await _leadService.GetAllLeadsAsync(true);

            if (leadRequest.IsSuccess && leadRequest.Result.FirstOrDefault(x => x.LeadState?.Id == leadStateId) != null)
                return Json(new ResultModel { IsSuccess = false, Errors = new List<IErrorModel> { new ErrorModel { Message = "Has lead in this state" } } });

            return await JsonAsync(_leadService.DisableLeadStateAsync(leadStateId));
        }

        /// <summary>
        /// Remove lead state
        /// </summary>
        /// <param name="leadStateId"></param>
        /// <returns></returns>
        [HttpDelete]
        // [Roles(GlobalResources.Roles.ADMINISTRATOR)]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmLeadStateDelete)]
        public async Task<JsonResult> RemoveLeadState(Guid? leadStateId)
            => await JsonAsync(_leadService.RemoveLeadStateAsync(leadStateId));

        /// <summary>
        /// Rename lead state
        /// </summary>
        /// <param name="leadStateId"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="styleClass"></param>
        /// <returns></returns>
        [HttpPost]
        //[Roles(GlobalResources.Roles.ADMINISTRATOR)]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmLeadStateUpdate)]
        public async Task<JsonResult> RenameLeadState([Required]Guid? leadStateId, [Required][MinLength(2)]string name, string description, string styleClass)
        {
            if (!ModelState.IsValid) return JsonModelStateErrors();
            return await JsonAsync(_leadService.UpdateLeadStateAsync(leadStateId, name, description, styleClass));
        }

        /// <summary>
        /// Change lead state
        /// </summary>
        /// <param name="leadId"></param>
        /// <param name="stateId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmLeadUpdate)]
        public async Task<JsonResult> ChangeLeadState([Required] Guid? leadId, [Required] Guid? stateId)
            => await JsonAsync(_leadService.ChangeLeadStateAsync(leadId, stateId));


       
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> SeedSystemLeadState()
        {
            await _leadService.SeedSystemLeadState();

            return Json("");
        }

        #endregion

        #region NoGoState
        /// <summary>
        /// Add NoGoState
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        [HttpPut]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<Guid>))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmLeadCreate)]
        public async Task<JsonResult> AddNoGoState([Required] AddNoGoStateViewModel model) =>
            await JsonAsync(_leadService.AddNoGoStateAsync(model));


        /// <summary>
        /// Update NoGoState
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmLeadUpdate)]
        public async Task<JsonResult> UpdateNoGoState([Required] UpdateNoGoStateViewModel model) =>
            await JsonAsync(_leadService.UpdateNoGoStateAsync(model));


        /// <summary>
        /// Get NoGoState by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<NoGoStateViewModel>))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmLeadDelete)]
        public async Task<JsonResult> GetNoGoStateById(Guid? Id) =>
            await JsonAsync(_leadService.GetNoGoStateByIdAsync(Id));

        /// <summary>
        /// Activate NoGoState by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmLeadDelete)]
        public async Task<JsonResult> ActivateNoGoState(Guid? Id) =>
            await JsonAsync(_leadService.ActivateNoGoStateAsync(Id));


        /// <summary>
        /// Disable NoGoState by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmLeadDelete)]
        public async Task<JsonResult> DisableNoGoState(Guid? Id) =>
            await JsonAsync(_leadService.DisableNoGoStateAsync(Id));

        /// <summary>
        /// Delete NoGoState by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmLeadDelete)]
        public async Task<JsonResult> DeleteNoGoState(Guid? Id) =>
            await JsonAsync(_leadService.DeleteNoGoStateAsync(Id));


        /// <summary>
        /// Get all NoGoStates
        /// </summary>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<IEnumerable<NoGoStateViewModel>>))]
        //[AuthorizePermission(PermissionsConstants.CorePermissions.BpmLeadDelete)]
        public async Task<JsonResult> GetAllNoGoStates() =>
            await JsonAsync(_leadService.GetAllNoGoStatesAsync());


        /// <summary>
        /// Get all pagged NoGoStates
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(PagedResult<NoGoStateViewModel>))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmLeadDelete)]
        public async Task<JsonResult> GetAllPaginatedNoGoStates(PageRequest request) =>
            await JsonAsync(_leadService.GetAllPaginatedNoGoStatesAsync(request));


        /*/// <summary>
        /// Add lead to no go state
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmLeadDelete)]
        public async Task<JsonResult> AddLeadToNoGoState(LeadToNoGoStateViewModel model) =>
            await JsonAsync(_leadService.AddLeadToNoGoStateAsync(model));*/

        /*/// <summary>
        /// Get leads by NoGoState
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<IEnumerable<GetLeadsViewModel>>))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmLeadDelete)]
        public async Task<JsonResult> GetLeadsByNoGoState(Guid? Id) =>
            await JsonAsync(_leadService.GetLeadsByNoGoStateIdAsync(Id));*/


        /*[HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmLeadDelete)]
        public async Task<JsonResult> RemoveLeadsFromNoGoState(List<Guid> LeadsIds) =>
            await JsonAsync(_leadService.RemoveLeadsFromNoGoStateAsync(LeadsIds));*/


        #endregion

        #region Team

        /// <summary>
        /// Set Lead owner
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="leadId"></param>
        /// <param name="listMembersId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<Guid>))]
        public async Task<JsonResult> SetLeadMembers([Required]Guid ownerId, [Required]Guid leadId, IEnumerable<Guid> listMembersId)
            => await JsonAsync(_leadService.SetLeadOwnerAsync(ownerId, leadId, listMembersId, Url));

        #endregion



        #region Contract


        //[HttpGet]
        //[Route(DefaultApiRouteTemplate)]
        //[Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<MemoryStream>))]
        //public async Task<JsonResult> GenerateContractForLead([Required] Guid leadId, [Required]Guid? templateId)
        //    => await JsonAsync(_leadService.GenerateContractForLeadAsync(leadId, templateId));



        //[HttpGet]
        //[Route(DefaultApiRouteTemplate)]
        //[Produces(ContentType.ApplicationJson, Type = typeof(FileResult))]
        //public async Task<FileResult> GenerateFileContractForLead([Required] Guid leadId, [Required] Guid? templateId)
        //{
        //    var resultRequest = await _leadService.GenerateContractForLeadAsync(leadId, templateId);
        //    return resultRequest.IsSuccess ? File(resultRequest.Result.ToArray(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "ABC.docx") : null;
        //}

        #endregion

    }
}
