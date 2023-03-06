using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Crm.Organizations.Abstractions.ViewModels.RevenueViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GR.Crm.Organizations.Abstractions
{
    public interface IOrganizationRevenueService
    {
        /// <summary>
        /// Get all revenues by organization id 
        /// </summary>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<GetRevenueViewModel>>> GetAllActiveRevenuesByOrganizationAsync(Guid organizationId, bool includeDeleted);

        /// <summary>
        /// Get all revenues by organization id paginated
        /// </summary>
        /// <returns></returns>
        Task<ResultModel<PagedResult<GetRevenueViewModel>>> GetAllRevenuesByOrganizationPaginatedAsync(PageRequest request, Guid organizationId);

        /// <summary>
        /// Get organization revenue by id
        /// </summary>
        /// <param name="revenueId"></param>
        /// <returns></returns>
        Task<ResultModel<GetRevenueViewModel>> GetRevenueByIdAsync(Guid revenueId);
        /// <summary>
        /// Add new revenue
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<Guid>> AddNewRevenue(RevenueViewModel model);

        /// <summary>
        /// Delete organization revenue by id
        /// </summary>
        /// <param name="revenueId"></param>
        /// <returns></returns>
        Task<ResultModel> DeleteOrganizationRevenue(Guid? revenueId);

        /// <summary>
        /// Update organization revenue
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<Guid>> UpdateOrganizationRevenue(RevenueViewModel model);
    }
}
