using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Crm.Products.Abstractions.Enums;
using GR.Crm.Products.Abstractions.ViewModels.ProductsViewModels;

namespace GR.Crm.Products.Abstractions
{
    public interface IProductService
    {

        /// <summary>
        /// Add product 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<Guid>> AddProductAsync(AddProductViewModel model);

        /// <summary>
        /// Update product
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel> UpdateProductAsync(AddProductViewModel model);

        /// <summary>
        /// Get product by id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        Task<ResultModel<GetProductViewModel>> GetProductByIdAsync(Guid? productId);

        /// <summary>
        /// Get products by product type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<GetProductViewModel>>> GetProductsByProductTypeAsync(TypeOfProduct typeOfProduct, bool includeDeleted);

        /// <summary>
        /// Get all product
        /// </summary>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<GetProductViewModel>>> GetAllProductsAsync(bool includeDeleted);

        /// <summary>
        /// Get all paginated product
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ResultModel<PagedResult<GetProductViewModel>>> GetAllPaginatedProductsAsync(PageRequest request);

        /// <summary>
        /// Disable product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        Task<ResultModel> DisableProductAsync(Guid? productId);

        /// <summary>
        /// Activate product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        Task<ResultModel> ActivateProductAsync(Guid? productId);

        /// <summary>
        /// Delete product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        Task<ResultModel> DeleteProductAsync(Guid? productId);

        Task<ResultModel> ActivateProductVariationAsync(Guid Id);

        Task<ResultModel> DisableProductVariationAsync(Guid Id);

        Task<ResultModel<Guid>> AddProductVariationAsync(AddProductVariationViewModel model);

        Task<ResultModel> UpdateProductVariationAsync(UpdateProductVariationViewModel model);

        Task<ResultModel> DeleteProductVariationAsync(Guid? Id);

        Task<ResultModel<ProductVariationViewModel>> GetProductVariationByIdAsync(Guid? productVariationId);

        Task<ResultModel<IEnumerable<ProductVariationViewModel>>> GetProductVariationByProductTemplateId(Guid Id);


        Task<ResultModel<IEnumerable<ProductVariationViewModel>>> GetAllProductVariationsAsync();


        Task<ResultModel<Guid>> AddProductDeliverableAsync(AddProductDeliverableViewModel model);

        Task<ResultModel> UpdateProductDeliverableAsync(ProductDeliverableViewModel model);

        Task<ResultModel> DeleteProductDeliverableAsync(Guid? Id);

        Task<ResultModel<IEnumerable<ProductDeliverableViewModel>>> GetProductDeliverableByProductVariationId(Guid Id);

        Task<ResultModel<ProductDeliverableViewModel>> GetProductDeliverableByIdAsync(Guid? productDeliverableId);

        Task<ResultModel> RemoveDeliverableFromVariationAsync(List<Guid> deliverables);

        Task<ResultModel<PagedResult<ProductDeliverableViewModel>>> GetAllPaginatedProductDeliverablesAsync(PageRequest request);

        Task<ResultModel> DisableProductDeliverableAsync(Guid? Id);

        Task<ResultModel> ActivateProductDeliverableAsync(Guid? Id);


        Task<ResultModel<IEnumerable<ProductDeliverableViewModel>>> GetProductDeliverablesWithNoVariationAsync();
    }
}
 