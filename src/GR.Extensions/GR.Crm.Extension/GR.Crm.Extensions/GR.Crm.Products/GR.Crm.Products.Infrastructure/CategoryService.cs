using AutoMapper;
using Gr.Crm.Products.Abstractions;
using Gr.Crm.Products.Abstractions.Models;
using Gr.Crm.Products.Abstractions.ViewModels.CategoryViewModels;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Core.Helpers.Responses;
using GR.Crm.Leads.Abstractions;
using GR.Crm.Leads.Abstractions.Models;
using GR.Crm.Products.Abstractions.Models;
using GR.Crm.Products.Abstractions.ViewModels.CategoryViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Crm.Products.Infrastructure
{
    public class CategoryService : ICategoryService
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

        public CategoryService(ILeadContext<Lead> context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Activate category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> ActivateCategoryAsync(Guid? categoryId) =>
            await _context.ActivateRecordAsync<Category>(categoryId);

        /// <summary>
        /// Add category
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<Guid>> AddCategoryAsync(AddCategoryViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel<Guid>();

            var category = _mapper.Map<Category>(model);

            _context.Categories.Add(category);
            var result = await _context.PushAsync();

            return new ResultModel<Guid> { IsSuccess = result.IsSuccess, Errors = result.Errors, Result = category.Id };
        }

        /// <summary>
        /// Delete product
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DeleteCategoryAsync(Guid? categoryId) =>
            await _context.RemovePermanentRecordAsync<Category>(categoryId);

        /// <summary>
        /// Disable category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DisableCategoryAsync(Guid? categoryId) =>
            await _context.DisableRecordAsync<Category>(categoryId);

        /// <summary>
        /// Get all categories
        /// </summary>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<GetCategoryViewModel>>> GetAllCategoriesAsync(bool includeDeleted)
        {
            var categories = await _context.Categories
                .Where(x => !x.IsDeleted || includeDeleted)
                .ToListAsync();

            return new SuccessResultModel<IEnumerable<GetCategoryViewModel>>(_mapper.Map<IEnumerable<GetCategoryViewModel>>(categories));
        }

        /// <summary>
        /// Get all paginated categories
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<PagedResult<GetCategoryViewModel>>> GetAllPaginatedCategoriesAsync(PageRequest request)
        {
            var pagedResult = await _context.Categories
                .Include(i => i.ParentCategory)
                .Where(x => !x.IsDeleted || request.IncludeDeleted)
                .GetPagedAsync(request);

            var map = pagedResult.Map(_mapper.Map<IEnumerable<GetCategoryViewModel>>(pagedResult.Result));
            return new SuccessResultModel<PagedResult<GetCategoryViewModel>>(map);
        }

        /// <summary>
        /// Get category by id
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public virtual async  Task<ResultModel<GetCategoryViewModel>> GetCategoryByIdAsync(Guid? categoryId)
        {
            if (categoryId == null)
                return new InvalidParametersResultModel<GetCategoryViewModel>();

            var category = await _context.Categories
                .FirstOrDefaultAsync(x => x.Id.Equals(categoryId));

            if (category == null)
                return new NotFoundResultModel<GetCategoryViewModel>();

            return new SuccessResultModel<GetCategoryViewModel>(_mapper.Map<GetCategoryViewModel>(category));
        }

        /// <summary>
        /// Update category
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> UpdateCategoryAsync(AddCategoryViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel();

            var category = await _context.Categories
                .FirstOrDefaultAsync(x => x.Id.Equals(model.Id));

            if (category == null)
                return new NotFoundResultModel();

            category.Name = model.Name;
            category.DisplayName = model.DisplayName;
            category.Description = model.Description;
            category.DisplayName = model.DisplayName;
            category.DisplayOrder = model.DisplayOrder;
            category.ParentCategoryId = model.ParentCategoryId;
            category.IsPublished = model.IsPublished;

            _context.Categories.Update(category);
            return await _context.PushAsync();
        }
    }
}
