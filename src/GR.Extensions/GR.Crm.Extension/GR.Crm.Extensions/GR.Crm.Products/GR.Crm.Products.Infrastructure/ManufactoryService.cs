using AutoMapper;
using Gr.Crm.Products.Abstractions;
using Gr.Crm.Products.Abstractions.Models;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Core.Helpers.Responses;
using GR.Crm.Leads.Abstractions;
using GR.Crm.Leads.Abstractions.Models;
using GR.Crm.Products.Abstractions;
using GR.Crm.Products.Abstractions.Models;
using GR.Crm.Products.Abstractions.ViewModels.ManufactoryViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Crm.Products.Infrastructure
{
    public class ManufactoryService : IManufactoryService
    {
        #region Injectable
        /// <summary>
        /// Inject context
        /// </summary>
        private readonly ILeadContext<Lead> _context;

        /// <summary>
        /// Inject mapper
        /// </summary>
        private readonly IMapper _mapper;
        #endregion

        public ManufactoryService(ILeadContext<Lead> context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Activate manufactory
        /// </summary>
        /// <param name="manufactoryId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> ActivateManufactoryAsync(Guid? manufactoryId) =>
            await _context.ActivateRecordAsync<ProductManufactories>(manufactoryId);

        /// <summary>
        /// Add manufactory
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<Guid>> AddManufactoryAsync(AddManufactoryViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel<Guid>();

            var manufactory = _mapper.Map<ProductManufactories>(model);

            _context.ProductManufactories.Add(manufactory);
            var result = await _context.PushAsync();

            return new ResultModel<Guid> { IsSuccess = result.IsSuccess, Errors = result.Errors, Result = manufactory.Id };
        }

        /// <summary>
        /// Delete manufactory
        /// </summary>
        /// <param name="manufactoryId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DeleteManufactoryAsync(Guid? manufactoryId) =>
            await _context.RemovePermanentRecordAsync<ProductManufactories>(manufactoryId);

        /// <summary>
        /// Disable manufactory
        /// </summary>
        /// <param name="manufactoryId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DisableManufactoryAsync(Guid? manufactoryId) =>
            await _context.DisableRecordAsync<ProductManufactories>(manufactoryId);

        /// <summary>
        /// Get all categories
        /// </summary>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<GetManufactoryViewModel>>> GetAllManufactoriesAsync(bool includeDeleted)
        {
            var manufactories = await _context.ProductManufactories
                .Where(x => !x.IsDeleted || includeDeleted)
                .ToListAsync();

            return new SuccessResultModel<IEnumerable<GetManufactoryViewModel>>(_mapper.Map<IEnumerable<GetManufactoryViewModel>>(manufactories));
        }

        /// <summary>
        /// Get all paginated manufactories
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<PagedResult<GetManufactoryViewModel>>> GetAllPaginatedManufactoriesAsync(PageRequest request)
        {
            var pagedResult = await _context.ProductManufactories
                .Where(x => !x.IsDeleted || request.IncludeDeleted)
                .GetPagedAsync(request);

            var map = pagedResult.Map(_mapper.Map<IEnumerable<GetManufactoryViewModel>>(pagedResult.Result));
            return new SuccessResultModel<PagedResult<GetManufactoryViewModel>>(map);
        }

        /// <summary>
        /// Get manufactory by id
        /// </summary>
        /// <param name="manufactoryId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<GetManufactoryViewModel>> GetManufactoryByIdAsync(Guid? manufactoryId)
        {
            if (manufactoryId == null)
                return new InvalidParametersResultModel<GetManufactoryViewModel>();

            var manufactory = await _context.ProductManufactories
                .FirstOrDefaultAsync(x => x.Id.Equals(manufactoryId));

            if (manufactory == null)
                return new NotFoundResultModel<GetManufactoryViewModel>();

            return new SuccessResultModel<GetManufactoryViewModel>(_mapper.Map<GetManufactoryViewModel>(manufactory));
        }

        /// <summary>
        /// Update manufactory
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> UpdateManufactoryAsync(AddManufactoryViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel();

            var manufactory = await _context.ProductManufactories
                .FirstOrDefaultAsync(x => x.Id.Equals(model.Id));

            if (manufactory == null)
                return new NotFoundResultModel();

            manufactory.Name = model.Name;
            manufactory.Description = model.Description;

            _context.ProductManufactories.Update(manufactory);
            return await _context.PushAsync();
        }
    }
}
