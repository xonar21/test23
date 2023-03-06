using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GR.Core.BaseControllers;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.Core.Helpers.Filters.Enums;
using GR.Core.Helpers.Pagination;
using GR.Crm.Abstractions;
using GR.Crm.Abstractions.Models;
using GR.Crm.Abstractions.ViewModels.AuditViewModels;
using GR.Crm.Emails.Abstractions;
using GR.Crm.Leads.Abstractions;
using GR.Crm.Leads.Abstractions.Models;
using GR.Crm.Marketing.Abstractions;
using GR.Crm.Marketing.Abstractions.ViewModels.MarketingListsViewModels;
using GR.Crm.Marketing.Infrastructure;
using GR.Crm.Organizations.Abstractions;
using GR.Crm.Organizations.Abstractions.Enums;
using GR.Crm.Organizations.Abstractions.ViewModels.OrganizationsViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GR.Crm.Razor.Controllers
{
    [Authorize]
    public class CrmCommonController : BaseGearController
    {
        #region Injectable

        /// <summary>
        /// Inject crm service
        /// </summary>
        private readonly ICrmService _crmService;

        /// <summary>
        /// Inject crm notification service
        /// </summary>
        private readonly ICrmNotificationService _crmNotificationService;
        /// <summary>
        /// Inject crm organization
        /// </summary>
        private readonly ICrmOrganizationService _organizationService;

        /// <summary>
        /// Inject crm lead service
        /// </summary>
        private readonly ILeadService<Lead> _leadService;

        /// <summary>
        /// Inject mapper
        /// </summary>
        private readonly IMapper _mapper;

        private readonly IEmailContext _emailContext;

        #endregion

        public CrmCommonController(ICrmService crmService,
            ICrmOrganizationService organizationService,
            ILeadService<Lead> leadService,
            IMapper mapper,
            IEmailContext emailContext,
            ICrmNotificationService notificationService
            )
        {
            _crmService = crmService;
            _crmNotificationService = notificationService;
            _organizationService = organizationService;
            _leadService = leadService;
            _mapper = mapper;
            _emailContext = emailContext;
        }

        /// <summary>
        /// Get all currencies
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<IEnumerable<Currency>>))]
        public async Task<JsonResult> GetAllCurrencies() => await JsonAsync(_crmService.GetAllCurrenciesAsync());


        /// <summary>
        /// Get organization by filter
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<PagedResult<GetTableOrganizationViewModel>>))]
        public async Task<JsonResult> GetAllOrganizationPaginated(PageRequest request)
        {

            var organizationRequest = await _organizationService.GetPaginatedOrganizationAsync(request);
            var organization = organizationRequest.Result.Result.ToList();

            if (!organization.Any())
            { 
                return Json(new ResultModel<PagedResult<GetTableOrganizationViewModel>>
                {
                    IsSuccess = false,
                });
            }

            var listOrganization = organization
                .Select(async s => new GetTableOrganizationViewModel
                {
                    Contacts = s.Contacts,
                    Id = s.Id,
                    Author = s.Author,
                    TenantId = s.TenantId,
                    Created = s.Created,
                    Name = s.Name,
                    Stage = s.Stage?.Name,
                    State = s.State,
                    IsDeleted = s.IsDeleted,
                    LeadCount = (await _leadService.GetLeadsCountByOrganizationAsync(s.Id)).Result
                }).Select(s => s.Result).ToList()
                .OrderByWithDirection(x => x.GetPropertyValue(request.Attribute), request.Descending).ToList();
           
            foreach(var org in listOrganization)
            {
                org.Email = _emailContext.Emails.Where(x => x.OrganizationId == org.Id).Select(x =>x.Label.ToString() + ": " + x.Email).ToList();
            }

            var result = new ResultModel<PagedResult<GetTableOrganizationViewModel>>
            {
                IsSuccess = true,
                Result = new PagedResult<GetTableOrganizationViewModel>
                {
                    Result = listOrganization,
                    IsSuccess = organizationRequest.Result.IsSuccess,
                    CurrentPage = organizationRequest.Result.CurrentPage,
                    PageCount = organizationRequest.Result.PageCount,
                    PageSize = organizationRequest.Result.PageSize,
                    Errors = organizationRequest.Result.Errors,
                    RowCount = organizationRequest.Result.RowCount,
                    KeyEntity = organizationRequest.Result.KeyEntity
                },
            };

            return Json(result, SerializerSettings);
        }


        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<PagedResult<GetPaginatedAuditViewModel>>))]
        public async Task<JsonResult> GetPaginatedAudit(PageRequest request, Guid RecordId)
            => await JsonAsync(_crmService.GetPaginatedAuditAsync(request, RecordId));


        /// <summary>
        /// Notify users about deadlines
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        public async Task NotifyUsersAboutDeadlines(IEnumerable<Guid> users) => await _crmNotificationService.DeadlinesSummaryNotificationAsync(Url, users);

        /// Notify users about opportunities with no active tasks
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        public async Task NotifyUsersAboutLeadsWithNoActiveTasks(IEnumerable<Guid> users) => await _crmNotificationService.NoTaskLeadsNotificationAsync(Url, users);
    }
}
