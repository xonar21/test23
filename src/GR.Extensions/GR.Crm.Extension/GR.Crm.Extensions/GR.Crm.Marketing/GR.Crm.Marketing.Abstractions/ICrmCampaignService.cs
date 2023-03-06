using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Crm.Marketing.Abstractions.Models;
using GR.Crm.Marketing.Abstractions.ViewModels;
using GR.Crm.Marketing.Abstractions.ViewModels.CampaignsMarketingListsViewModels;
using GR.Crm.Marketing.Abstractions.ViewModels.CampaignsViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GR.Crm.Marketing.Abstractions
{
    public interface ICrmCampaignService
    {
        /// <summary>
        /// Get all paginated campaigns
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ResultModel<PagedResult<GetCampaignViewModel>>> GetPaginatedCampaignAsync(PageRequest request);

        /// <summary>
        /// Get all active campaigns
        /// </summary>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<GetCampaignViewModel>>> GetAllActiveCampaignsAsync(bool includeDeleted);

        /// <summary>
        /// Add new campaign
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<Guid>> AddNewCampaignAsync(CampaignViewModel model);

        /// <summary>
        /// Disable campaign
        /// </summary>
        /// <param name="campaignId"></param>
        /// <returns></returns>
        Task<ResultModel> DisableCampaignAsync(Guid? campaignId);

        /// <summary>
        /// Enable campagin by id
        /// </summary>
        /// <param name="campaignId"></param>
        /// <returns></returns>
        Task<ResultModel> EnableCampaignAsync(Guid? campaignId);

        /// <summary>
        /// Delete campaign permanently
        /// </summary>
        /// <param name="campaignId"></param>
        /// <returns></returns>
        Task<ResultModel> DeleteCampaignAsync(Guid? campaignId);

        /// <summary>
        /// Get campaign by id
        /// </summary>
        /// <param name="campaignId"></param>
        /// <returns></returns>
        Task<ResultModel<GetCampaignViewModel>> GetCampaignByIdAsync(Guid? campaignId);

        /// <summary>
        /// Update campaign
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel> UpdateCampaignAsync(CampaignViewModel model);

        /// <summary>
        /// Add marketing list to campaign
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<Guid>> AddMarketingListToCampaignAsync(CampaignMarketingListViewModel model);

    }
}
