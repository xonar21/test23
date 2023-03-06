using AutoMapper;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Core.Helpers.Responses;
using GR.Crm.Abstractions;
using GR.Crm.Marketing.Abstractions;
using GR.Crm.Marketing.Abstractions.Models;
using GR.Crm.Marketing.Abstractions.ViewModels;
using GR.Crm.Marketing.Abstractions.ViewModels.CampaignsMarketingListsViewModels;
using GR.Crm.Marketing.Abstractions.ViewModels.CampaignsViewModels;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GR.Crm.Marketing.Infrastructure
{
    public class CampaignService : ICrmCampaignService
    {
        #region Injectable

        /// <summary>
        /// Inject campaign context
        /// </summary>
        private readonly ICrmCampaignContext _campaignContext;

        /// <summary>
        /// Inject mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Inject campaign context
        /// </summary>
        private readonly ICrmService _crmService;

        #endregion

        private Task<decimal> ConversionRate { get; set; }

        public CampaignService(ICrmCampaignContext campaignContext,
             IMapper mapper,
             ICrmService crmService)
        {
            _campaignContext = campaignContext;
            _mapper = mapper;
            _crmService = crmService;

        }

        /// <summary>
        /// Get paginated active campaigns
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ResultModel<PagedResult<GetCampaignViewModel>>> GetPaginatedCampaignAsync(PageRequest request)
        {
            if (request == null)
            {
                return new InvalidParametersResultModel<PagedResult<GetCampaignViewModel>>();
            }

            var query = _campaignContext.Campaigns
                .Include(x => x.CampaignType)
                .Include(x => x.MarketingLists)
                .ThenInclude(x => x.MarketingList)
                .Include(x => x.Currency)
                .Where(x => !x.IsDeleted || request.IncludeDeleted);

            var pagedResult = await query.GetPagedAsync(request);
            var map = pagedResult.Map(_mapper.Map<IEnumerable<GetCampaignViewModel>>(pagedResult.Result));

            return new SuccessResultModel<PagedResult<GetCampaignViewModel>>(map);
        }

        /// <summary>
        /// Get all active campaigns
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<GetCampaignViewModel>>> GetAllActiveCampaignsAsync(bool includeDeleted = false)
        {
            var marketingLists = await _campaignContext.Campaigns
                .Include(x => x.CampaignType)
                .Include(x => x.MarketingLists)
                .ThenInclude(x => x.MarketingList)
                .Include(x => x.Currency)
                .Where(x => !x.IsDeleted || includeDeleted)
                .ToListAsync();

            return new SuccessResultModel<IEnumerable<GetCampaignViewModel>>
                (_mapper.Map<IEnumerable<GetCampaignViewModel>>(marketingLists));
        }

        /// <summary>
        /// Add new campaign async
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<Guid>> AddNewCampaignAsync(CampaignViewModel model)
        {
            if (model == null)
                return new NotFoundResultModel<Guid>();

            var campaignBd =
                await _campaignContext.Campaigns
                    .FirstOrDefaultAsync(x => x.Name.Trim().ToLower().Equals(model.Name.Trim().ToLower()));


            if (campaignBd != null)
                return new ResultModel<Guid>
                {
                    IsSuccess = false,
                    Errors = new List<IErrorModel> { new ErrorModel { Message = "Campaign already exists!" } }
                };

            var newCampaign = _mapper.Map<Campaign>(model);
            newCampaign.CurrencyCode = "EUR";
            ConversionRate = _crmService.ConvertCurrencyToEURAsync(model.CurrencyCode);
            newCampaign.CampaignBudget = Math.Round(model.CampaignBudget * ConversionRate.Result, 2);
            newCampaign.CampaignCost = Math.Round(model.CampaignCost * ConversionRate.Result, 2);
            await _campaignContext.Campaigns.AddAsync(newCampaign);
            var result = await _campaignContext.PushAsync();

            foreach (var member in model.MarketingLists)
            {
                await AddMarketingListToCampaignAsync(new CampaignMarketingListViewModel

                { MarketingListId = member.MarketingListId, CampaignId = newCampaign.Id });
            }

            return result.Map(newCampaign.Id);
        }

        /// <summary>
        /// Disable campaign
        /// </summary>
        /// <param name="campaignId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DisableCampaignAsync(Guid? campaignId) =>
            await _campaignContext.DisableRecordAsync<Campaign>(campaignId);

        /// <summary>
        /// Activate campaign
        /// </summary>
        /// <param name="campaignId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> EnableCampaignAsync(Guid? campaignId) =>
            await _campaignContext.ActivateRecordAsync<Campaign>(campaignId);

        /// <summary>
        /// Delete campaign permanently
        /// </summary>
        /// <param name="campaignId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DeleteCampaignAsync(Guid? campaignId)
        {
            if (campaignId == null)
                return new InvalidParametersResultModel();

            var campaign = await _campaignContext.Campaigns.FirstOrDefaultAsync(x => x.Id == campaignId);

            if (campaign == null)
                return new NotFoundResultModel();

            _campaignContext.Campaigns.Remove(campaign);
            return await _campaignContext.PushAsync();

        }

        /// <summary>
        /// Get campaign by id
        /// </summary>
        /// <param name="campaignId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<GetCampaignViewModel>> GetCampaignByIdAsync(Guid? campaignId)
        {
            if (campaignId == null)
                return new InvalidParametersResultModel<GetCampaignViewModel>();

            var query = await _campaignContext.Campaigns
                .Include(x => x.CampaignType)
                .Include(x => x.MarketingLists)
                .ThenInclude(x => x.MarketingList)
                .Include(x => x.Currency)
                .FirstOrDefaultAsync(x => x.Id == campaignId);

            if (query == null)
                return new NotFoundResultModel<GetCampaignViewModel>();

            var campaign = _mapper.Map<GetCampaignViewModel>(query);

            return new SuccessResultModel<GetCampaignViewModel>(campaign);
        }

        /// <summary>
        /// Update campaign
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> UpdateCampaignAsync(CampaignViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel();

            var campaign = await _campaignContext.Campaigns
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            if (campaign == null)
                return new NotFoundResultModel();

            campaign.Name = model.Name;
            ConversionRate = _crmService.ConvertCurrencyToEURAsync(model.CurrencyCode);
            campaign.CampaignBudget = Math.Round(model.CampaignBudget * ConversionRate.Result, 2);
            campaign.CampaignCost = Math.Round(model.CampaignCost * ConversionRate.Result, 2);
            campaign.CurrencyCode = "EUR";
            campaign.StartDate = model.StartDate;
            campaign.EndDate = model.EndDate;
            campaign.CampaignTypeId = model.CampaignTypeId;
            campaign.Description = model.Description;

     
            var result = await _campaignContext.PushAsync();

            var listMembers = await _campaignContext.CampaignsMarketingLists.Where(x => x.CampaignId == model.Id).ToListAsync();
            _campaignContext.CampaignsMarketingLists.RemoveRange(listMembers);

            result.Result = campaign.Id;

            if (!result.IsSuccess || !model.MarketingLists.Any()) return result;

            foreach (var member in model.MarketingLists)
            {
                await AddMarketingListToCampaignAsync(new CampaignMarketingListViewModel
                { MarketingListId = member.MarketingListId, CampaignId = campaign.Id });
            }

            return result;
        }

        /// <summary>
        /// Add marketing list to campaign
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<Guid>> AddMarketingListToCampaignAsync(CampaignMarketingListViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel<Guid>();

            var existentMember = await _campaignContext.CampaignsMarketingLists
                .FirstOrDefaultAsync(x => x.CampaignId == model.CampaignId && x.MarketingListId == model.MarketingListId);

            if (existentMember != null)
                return new ResultModel<Guid>
                {
                    IsSuccess = false,
                    Errors = new List<IErrorModel> { new ErrorModel { Message = "Marketing list is already associated with the campaign" } }
                };

            var newMember = new CampaignMarketingList
            {
                CampaignId = model.CampaignId,
                MarketingListId = model.MarketingListId,
            };

            await _campaignContext.CampaignsMarketingLists.AddAsync(newMember);

            var result = await _campaignContext.PushAsync();
            return new ResultModel<Guid> { IsSuccess = result.IsSuccess, Errors = result.Errors };
        }

    }
}
