using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Crm.Marketing.Abstractions.ViewModels.MarketingListOrganizationViewModel;
using GR.Crm.Marketing.Abstractions.ViewModels.MarketingListsViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GR.Crm.Marketing.Abstractions
{
    public interface ICrmMarketingListService
    {
        /// <summary>
        /// Get all paginated marketing lists
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ResultModel<PagedResult<GetMarketingListViewModel>>> GetPaginatedMarketingListAsync(PageRequest request);

        /// <summary>
        /// Add new marketing list
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<Guid>> AddMarketingListAsync(MarketingListViewModel model);

        /// <summary>
        /// Get all active marketing lists
        /// </summary>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<GetMarketingListViewModel>>> GetAllActiveMarketingListsAsync(bool includeDeleted);


        /// <summary>
        /// Get marketing list by id
        /// </summary>
        /// <param name="marketingListId"></param>
        /// <returns></returns>
        Task<ResultModel<GetMarketingListViewModel>> GetMarketingListByIdAsync(Guid? marketingListId);

        /// <summary>
        /// Add new member organization to list
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<Guid>> AddNewMemberOrganizationToListAsync(MarketingListOrganizationViewModel model);

        /// <summary>
        /// Update marketing list
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel> UpdateMarketingListAsync(MarketingListViewModel model);

        /// <summary>
        /// Disable marketing list
        /// </summary>
        /// <param name="marketingListId"></param>
        /// <returns></returns>
        Task<ResultModel> DisableMarketingListAsync(Guid? marketingListId);

        /// <summary>
        /// Enable marketing list by id
        /// </summary>
        /// <param name="marketingListId"></param>
        /// <returns></returns>
        Task<ResultModel> EnableMarketingListAsync(Guid? marketingListId);

        /// <summary>
        /// Delete marketing list permanently
        /// </summary>
        /// <param name="marketingListId"></param>
        /// <returns></returns>
        Task<ResultModel> DeleteMarketingListAsync(Guid? marketingListId);

        /// <summary>
        ///  Get member organizations count by marketing list id
        /// </summary>
        /// <param name="marketingListId"></param>
        /// <returns></returns>
        Task<ResultModel<int>> GetMembersCountAsync(Guid? marketingListId);

    }
}
