using AutoMapper;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Core.Helpers.Responses;
using GR.Crm.Organizations.Abstractions;
using GR.Crm.Organizations.Abstractions.Models;
using GR.Crm.Organizations.Abstractions.ViewModels.RevenueViewModels;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GR.Crm.Organizations
{
    public class OrganizationRevenueService : IOrganizationRevenueService
    {
        #region Injectable

        /// <summary>
        /// organization context
        /// </summary>
        private readonly ICrmOrganizationContext _organizationContext;

        /// <summary>
        /// Inject mapper
        /// </summary>
        private readonly IMapper _mapper;


        #endregion

        public OrganizationRevenueService(ICrmOrganizationContext organizationContext, IMapper mapper)
        {
            _organizationContext = organizationContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Get revenues by organization id
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<GetRevenueViewModel>>> GetAllActiveRevenuesByOrganizationAsync(Guid organizationId, bool includeDeleted = false)
        {
            if (organizationId == null)
                return new InvalidParametersResultModel<IEnumerable<GetRevenueViewModel>>();

            var listRevenues = await _organizationContext.Revenues
                .Where(x => x.OrganizationId == organizationId && (!x.IsDeleted || includeDeleted))
                .OrderBy(x => x.Year)
                .ToListAsync();

            return new SuccessResultModel<IEnumerable<GetRevenueViewModel>>(listRevenues.Adapt<IEnumerable<GetRevenueViewModel>>());
        }

        /// <summary>
        /// Get All paginated Industries
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ResultModel<PagedResult<GetRevenueViewModel>>> GetAllRevenuesByOrganizationPaginatedAsync(PageRequest request, Guid organizationId)
        {
            var listRevenues = await _organizationContext.Revenues
                .Where(x => !x.IsDeleted || request.IncludeDeleted && x.OrganizationId == organizationId)
                .GetPagedAsync(request);

            var map = listRevenues.Map(_mapper.Map<IEnumerable<GetRevenueViewModel>>(listRevenues.Result));

            return new SuccessResultModel<PagedResult<GetRevenueViewModel>>(map);
        }

        /// <summary>
        /// Get organization revenue by id
        /// </summary>
        /// <param name="addressId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<GetRevenueViewModel>> GetRevenueByIdAsync(Guid revenueId)
        {
            if (revenueId == null)
                return new InvalidParametersResultModel<GetRevenueViewModel>();

            var revenue = await _organizationContext.Revenues
                .FirstOrDefaultAsync(x => x.Id == revenueId);

            if (revenue == null)
                return new NotFoundResultModel<GetRevenueViewModel>();

            return new SuccessResultModel<GetRevenueViewModel>(revenue.Adapt<GetRevenueViewModel>());
        }

        /// <summary>
        /// Add new organization revenue
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<Guid>> AddNewRevenue(RevenueViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel<Guid>();

            var duplicateRevenue =
                await _organizationContext.Revenues
                    .FirstOrDefaultAsync(x => x.Year == model.Year);

            if (duplicateRevenue != null)
                return new ResultModel<Guid>
                {
                    IsSuccess = false,
                    Errors = new List<IErrorModel> { new ErrorModel { Message = "You already added organization revenue for this year!" } }
                };

            var newRevenue = _mapper.Map<Revenue>(model);

            await _organizationContext.Revenues.AddAsync(newRevenue);
            var result = await _organizationContext.PushAsync();

            return result.Map(newRevenue.Id);

        }

        /// <summary>
        /// Delete organization revenue by id
        /// </summary>
        /// <param name="revenueId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DeleteOrganizationRevenue(Guid? revenueId)
        {
            if (revenueId == null)
                return new InvalidParametersResultModel();

            var revenue = await _organizationContext.Revenues.FirstOrDefaultAsync(x => x.Id == revenueId);

            if (revenue == null)
                return new NotFoundResultModel();

            _organizationContext.Revenues.Remove(revenue);
            return await _organizationContext.PushAsync();
        }


        /// <summary>
        /// Update organization address
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<Guid>> UpdateOrganizationRevenue(RevenueViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel<Guid>();

            var revenue = await _organizationContext.Revenues.FirstOrDefaultAsync(x => x.Id == model.Id);

            if (revenue == null)
                return new NotFoundResultModel<Guid>();

            revenue.OrganizationId = model.OrganizationId;
            revenue.Year = model.Year;
            revenue.CurrencyCode = model.CurrencyCode;
            revenue.Amount = model.Amount;

            _organizationContext.Revenues.Update(revenue);
            var result = await _organizationContext.PushAsync();
            return new ResultModel<Guid> { IsSuccess = result.IsSuccess, Errors = result.Errors, Result = revenue.Id };
        }

    }
}
