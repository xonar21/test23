using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GR.Core.BaseControllers;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Crm.Dashboard.Abstractions;
using GR.Crm.Dashboard.Abstractions.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace GR.Crm.Dashboard.Razor.Controller
{
    public class CrmDashboardController : BaseGearController
    {

        #region Injected

        /// <summary>
        /// Inject dashboard service
        /// </summary>
        private readonly ICrmDashboardService _dashboardService;

        

        #endregion
        public IActionResult Index()
        {
            return View();
        }

        public CrmDashboardController(ICrmDashboardService dashboardService,
            IConfiguration configuration)
        {
            _dashboardService = dashboardService;
            GlobalCurrency = configuration.GetSection("GeneralConfigurations").GetSection("CurrencyCode").Value;
        }

        private string GlobalCurrency { get; set; }


        /// <summary>
        /// Get list lead indices
        /// </summary>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<IEnumerable<LeadDashboardViewModel>>))]
        public async Task<JsonResult> GetLeadDashboardIndices(IEnumerable<PageRequestFilter> filters)
             => await JsonAsync(_dashboardService.GetLeadDashboardIndicesAsync(filters), SerializerSettings);
        
        /// <summary>
        /// Get list task indices
        /// </summary>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<IEnumerable<TaskDashboardViewModel>>))]
        public async Task<JsonResult> GetTaskDashboardIndices(IEnumerable<PageRequestFilter> filters)
             => await JsonAsync(_dashboardService.GetTaskDashboardIndicesAsync(filters), SerializerSettings);


        /// <summary>
        /// Get list organization indices
        /// </summary>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<IEnumerable<OrganizationDashboardViewModel>>))]
        public async Task<JsonResult> GetOrganizationDashboardIndices(IEnumerable<PageRequestFilter> filters)
             => await JsonAsync(_dashboardService.GetOrganizationDashboardIndicesAsync(filters), SerializerSettings);

        /// <summary>
        /// Get list organization indices
        /// </summary>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<IEnumerable<GetLeadAnalysisReportByYearViewModel>>))]
        public async Task<JsonResult> GetLeadsAnalysisReport(IEnumerable<PageRequestFilter> filters)
             => await JsonAsync(_dashboardService.GetLeadsAnalysisReport(filters), SerializerSettings);

        /// <summary>
        /// Get list organization indices
        /// </summary>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<IEnumerable<LeadByStatusAndValueResultViewModel>>))]
        public async Task<JsonResult> GetLeadsByStatusAndValue(IEnumerable<PageRequestFilter> filters)
             => await JsonAsync(_dashboardService.GetLeadsByStatusAndValueAsync(filters), SerializerSettings);
    }

    
}
