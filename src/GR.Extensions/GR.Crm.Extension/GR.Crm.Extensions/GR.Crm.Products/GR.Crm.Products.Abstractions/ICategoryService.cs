using Gr.Crm.Products.Abstractions.ViewModels.CategoryViewModels;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Crm.Products.Abstractions.ViewModels.CategoryViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gr.Crm.Products.Abstractions
{
    public interface ICategoryService
    {
        /// <summary>
        /// Add product 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<Guid>> AddCategoryAsync(AddCategoryViewModel model);

        /// <summary>
        /// Update product
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel> UpdateCategoryAsync(AddCategoryViewModel model);

        /// <summary>
        /// Get product by id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        Task<ResultModel<GetCategoryViewModel>> GetCategoryByIdAsync(Guid? categoryId);

        /// <summary>
        /// Get all product
        /// </summary>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<GetCategoryViewModel>>> GetAllCategoriesAsync(bool includeDeleted);

        /// <summary>
        /// Get all paginated product
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ResultModel<PagedResult<GetCategoryViewModel>>> GetAllPaginatedCategoriesAsync(PageRequest request);

        /// <summary>
        /// Disable product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        Task<ResultModel> DisableCategoryAsync(Guid? categoryId);

        /// <summary>
        /// Activate product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        Task<ResultModel> ActivateCategoryAsync(Guid? categoryId);

        /// <summary>
        /// Delete product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        Task<ResultModel> DeleteCategoryAsync(Guid? categoryId);

    }
}
