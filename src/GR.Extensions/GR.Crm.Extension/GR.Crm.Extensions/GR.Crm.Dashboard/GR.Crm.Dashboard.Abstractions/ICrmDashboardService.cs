using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Crm.Dashboard.Abstractions.ViewModel;

namespace GR.Crm.Dashboard.Abstractions
{
    public interface ICrmDashboardService
    {

        /// <summary>
        /// Get list lead indices
        /// </summary>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<LeadDashboardViewModel>>> GetLeadDashboardIndicesAsync(IEnumerable<PageRequestFilter> filters);


        /// <summary>
        /// Get list task indices
        /// </summary>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<TaskDashboardViewModel>>> GetTaskDashboardIndicesAsync(IEnumerable<PageRequestFilter> filters);


        /// <summary>
        /// Get list organizations indices
        /// </summary>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<OrganizationDashboardViewModel>>> GetOrganizationDashboardIndicesAsync(IEnumerable<PageRequestFilter> filters);


        /// <summary>
        /// Get leads analysisi report
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<GetLeadAnalysisReportByYearViewModel>>> GetLeadsAnalysisReport(IEnumerable<PageRequestFilter> filters);

        /// <summary>
        /// Get leads by status and value
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<LeadByStatusAndValueResultViewModel>>> GetLeadsByStatusAndValueAsync(IEnumerable<PageRequestFilter> filters);

    }
}
