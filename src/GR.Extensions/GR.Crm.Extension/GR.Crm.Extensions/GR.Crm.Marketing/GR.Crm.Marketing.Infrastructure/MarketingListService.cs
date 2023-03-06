using AutoMapper;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Core.Helpers.Responses;
using GR.Crm.Marketing.Abstractions;
using GR.Crm.Marketing.Abstractions.Models;
using GR.Crm.Marketing.Abstractions.ViewModels.MarketingListOrganizationViewModel;
using GR.Crm.Marketing.Abstractions.ViewModels.MarketingListsViewModels;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Crm.Marketing.Infrastructure
{
    public class MarketingListService : ICrmMarketingListService
    {
        #region Injectable

        /// <summary>
        /// Inject campaign context
        /// </summary>
        private readonly ICrmMarketingListContext _marketingListContext;

        /// <summary>
        /// Inject mapper
        /// </summary>
        private readonly IMapper _mapper;

        #endregion

        public MarketingListService(ICrmMarketingListContext marketingListContext,
             IMapper mapper)
        {
            _marketingListContext = marketingListContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Get paginated marketing lists
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ResultModel<PagedResult<GetMarketingListViewModel>>> GetPaginatedMarketingListAsync(PageRequest request)
        {
            if (request == null)
            {
                return new InvalidParametersResultModel<PagedResult<GetMarketingListViewModel>>();
            }

            var query = _marketingListContext.MarketingLists
                .Include(x => x.MemberOrganizations)
                .ThenInclude(x => x.Organization)
                .Where(x => !x.IsDeleted || request.IncludeDeleted);

            var pagedResult = await query.GetPagedAsync(request);
            var map = pagedResult.Map(_mapper.Map<IEnumerable<GetMarketingListViewModel>>(pagedResult.Result));

            return new SuccessResultModel<PagedResult<GetMarketingListViewModel>>(map);
        }

        /// <summary>
        /// Add new marketing list
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<Guid>> AddMarketingListAsync(MarketingListViewModel model)
        {
            if (model == null)
                return new NotFoundResultModel<Guid>();

            var marketingListBd =
                await _marketingListContext.MarketingLists
                    .FirstOrDefaultAsync(x => x.Name.Trim().ToLower().Equals(model.Name.Trim().ToLower()));

            if (marketingListBd != null)
                return new ResultModel<Guid>
                {
                    IsSuccess = false,
                    Errors = new List<IErrorModel> { new ErrorModel { Message = "Marketing list already exists!" } }
                };

            var newMarketingList = _mapper.Map<MarketingList>(model);
            await _marketingListContext.MarketingLists.AddAsync(newMarketingList);
            var result = await _marketingListContext.PushAsync();

            foreach (var member in model.Members)
            {
                await AddNewMemberOrganizationToListAsync(new MarketingListOrganizationViewModel
                { OrganizationId = member.OrganizationId, MarketingListId = newMarketingList.Id});
            }

            return result.Map(newMarketingList.Id);
        }

        /// <summary>
        /// Get all active marketing lists
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<GetMarketingListViewModel>>> GetAllActiveMarketingListsAsync(bool includeDeleted = false)
        {
            var marketingLists = await _marketingListContext.MarketingLists
                .Include(x => x.MemberOrganizations)
                .ThenInclude(x => x.Organization)
                .Where(x => !x.IsDeleted || includeDeleted)
                .ToListAsync();

            return new SuccessResultModel<IEnumerable<GetMarketingListViewModel>>
                (_mapper.Map<IEnumerable<GetMarketingListViewModel>>(marketingLists));
        }

        /// <summary>
        /// Get marketing list by id
        /// </summary>
        /// <param name="campaignId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<GetMarketingListViewModel>> GetMarketingListByIdAsync(Guid? marketingListId)
        {
            if (marketingListId == null)
                return new InvalidParametersResultModel<GetMarketingListViewModel>();


            var query = await _marketingListContext.MarketingLists
                .Include(x => x.MemberOrganizations)
                .ThenInclude(x => x.Organization)
                .FirstOrDefaultAsync(x => x.Id == marketingListId);

            if (query == null)
                return new NotFoundResultModel<GetMarketingListViewModel>();

            var marketingList = new GetMarketingListViewModel
            {
                Id = query.Id,
                Name = query.Name,
                MemberOrganizations = query.MemberOrganizations,
             
            };
            return new ResultModel<GetMarketingListViewModel>
            {
                IsSuccess = true,
                Result = marketingList
            };

        }

        /// <summary>
        /// Add new member organization to list 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<Guid>> AddNewMemberOrganizationToListAsync(MarketingListOrganizationViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel<Guid>();

            var existentMember = await _marketingListContext.MarketingListOrganizations
                .FirstOrDefaultAsync(x => x.OrganizationId == model.OrganizationId && x.MarketingListId == model.MarketingListId);

            if (existentMember != null)
                return new ResultModel<Guid>
                {
                    IsSuccess = false,
                    Errors = new List<IErrorModel> { new ErrorModel { Message = "Organization already in the list!" } }
                };

            var newMember = new MarketingListOrganization
            {
                OrganizationId = model.OrganizationId,
                MarketingListId = model.MarketingListId,

            };

            await _marketingListContext.MarketingListOrganizations.AddAsync(newMember);

            var result = await _marketingListContext.PushAsync();
            return new ResultModel<Guid> { IsSuccess = result.IsSuccess, Errors = result.Errors };
        }

        /// <summary>
        /// Update marketing list
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> UpdateMarketingListAsync(MarketingListViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel();

            var marketingList = await _marketingListContext.MarketingLists
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            if (marketingList == null)
                return new NotFoundResultModel();

            marketingList.Name = model.Name;

            var listMembers = await _marketingListContext.MarketingListOrganizations.Where(x => x.MarketingListId == model.Id).ToListAsync();
            _marketingListContext.MarketingListOrganizations.RemoveRange(listMembers);

            var result = await _marketingListContext.PushAsync();
            result.Result = marketingList.Id;

            if (!result.IsSuccess || !model.Members.Any()) return result;

            foreach (var member in model.Members)
            {
                await AddNewMemberOrganizationToListAsync(new MarketingListOrganizationViewModel
                { OrganizationId = member.OrganizationId, MarketingListId = marketingList.Id });
            }
            return result;
        }

        /// <summary>
        /// Disable marketing list
        /// </summary>
        /// <param name="marketingListId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DisableMarketingListAsync(Guid? marketingListId) =>
            await _marketingListContext.DisableRecordAsync<MarketingList>(marketingListId);


        /// <summary>
        /// Enable marketing list
        /// </summary>
        /// <param name="marketingListId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> EnableMarketingListAsync(Guid? marketingListId) =>
            await _marketingListContext.ActivateRecordAsync<MarketingList>(marketingListId);

        /// <summary>
        /// Delete marketing list permanently
        /// </summary>
        /// <param name="marketingListId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DeleteMarketingListAsync(Guid? marketingListId)
        {
            if (marketingListId == null)
                return new InvalidParametersResultModel();

            var marketingList = await _marketingListContext.MarketingLists.FirstOrDefaultAsync(x => x.Id == marketingListId);

            if (marketingList == null)
                return new NotFoundResultModel();

            _marketingListContext.MarketingLists.Remove(marketingList);
            return await _marketingListContext.PushAsync();

        }

        /// <summary>
        /// Get member organizations count by marketing list id
        /// </summary>
        /// <param name="marketingListId"></param>
        /// <returns></returns>
        public async Task<ResultModel<int>> GetMembersCountAsync(Guid? marketingListId)
        {
            if (marketingListId == null) return new InvalidParametersResultModel<int>();
            var count = await _marketingListContext.MarketingListOrganizations
                .CountAsync(x => x.MarketingListId.Equals(marketingListId));

            return new SuccessResultModel<int>(count);
        }
    }
}
