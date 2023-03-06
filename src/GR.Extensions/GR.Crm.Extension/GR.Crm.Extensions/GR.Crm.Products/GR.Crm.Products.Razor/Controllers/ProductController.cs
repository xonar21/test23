using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Gr.Crm.Products.Abstractions;
using Gr.Crm.Products.Abstractions.ViewModels.CategoryViewModels;
using GR.Core.BaseControllers;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Crm.Products.Abstractions;
using GR.Crm.Products.Abstractions.Enums;
using GR.Crm.Products.Abstractions.ViewModels.CategoryViewModels;
using GR.Crm.Products.Abstractions.ViewModels.ManufactoryViewModels;
using GR.Crm.Products.Abstractions.ViewModels.ProductsViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GR.Crm.Products.Razor.Controllers
{

    public class ProductController : BaseGearController
    {

        #region Injectable

        /// <summary>
        /// Inject service
        /// </summary>
        private readonly IProductService _productService;

        /// <summary>
        /// Inject category service
        /// </summary>
        /// <returns></returns>
        private readonly ICategoryService _categoryService;

        /// <summary>
        /// Inject manufactory service
        /// </summary>
        /// <returns></returns>
        private readonly IManufactoryService _manufactoryService;
        #endregion

        public IActionResult Index(int nr = 1)
        {
            return View(nr);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var productRequest = await _productService.GetProductByIdAsync(id);
            if (!productRequest.IsSuccess) return NotFound();
            return View(productRequest.Result);
        }

        public IActionResult Deliverables(int nr = 1)
        {
            return View(nr);
        }


        public ProductController(IProductService productService,
            ICategoryService categoryService,
            IManufactoryService manufactoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _manufactoryService = manufactoryService;
        }

        /// <summary>
        /// Get all products
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<IEnumerable<GetProductViewModel>>))]
        public async Task<JsonResult> GetAllProducts(bool includeDeleted = false)
            =>await JsonAsync(_productService.GetAllProductsAsync(includeDeleted));


        /// <summary>
        /// Get all categories
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<IEnumerable<GetCategoryViewModel>>))]
        public async Task<JsonResult> GetAllCategories(bool includeDeleted = false)
            => await JsonAsync(_categoryService.GetAllCategoriesAsync(includeDeleted));


        /// <summary>
        /// Get all ProductManufactoriesAsync
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<IEnumerable<GetManufactoryViewModel>>))]
        public async Task<JsonResult> GetAllProductManufactories(bool includeDeleted = false)
            => await JsonAsync(_manufactoryService.GetAllManufactoriesAsync(includeDeleted));


        /// <summary>
        /// Get all paginated products
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<PagedResult<GetProductViewModel>>))]
        public async Task<JsonResult> GetAllPaginatedProduct(PageRequest request)
            => await JsonAsync(_productService.GetAllPaginatedProductsAsync(request));



        /// <summary>
        /// Get all products
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<GetProductViewModel>))]
        public async Task<JsonResult> GetProductById([Required] Guid productId)
            => await JsonAsync(_productService.GetProductByIdAsync(productId));


        /// <summary>
        /// Get all products by type
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<IEnumerable<GetProductViewModel>>))]
        public async Task<JsonResult> GetAllProductsOfTypeProduct(bool includeDeleted = false)
            => await JsonAsync(_productService.GetProductsByProductTypeAsync(TypeOfProduct.Product, includeDeleted));


        /// <summary>
        /// Get all products by type
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<IEnumerable<GetProductViewModel>>))]
        public async Task<JsonResult> GetAllProductsOfTypeService(bool includeDeleted = false)
            => await JsonAsync(_productService.GetProductsByProductTypeAsync(TypeOfProduct.Service, includeDeleted));
        /// <summary>
        /// Add product
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<Guid>))]
        public async Task<JsonResult> AddProduct([Required] AddProductViewModel model)
        {
            if (!ModelState.IsValid) return JsonModelStateErrors();
            return await JsonAsync(_productService.AddProductAsync(model));
        }


        /// <summary>
        /// Update product
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> UpdateProduct([Required] AddProductViewModel model)
        {
            if (!ModelState.IsValid) return JsonModelStateErrors();
            return await JsonAsync(_productService.UpdateProductAsync(model));
        }



        /// <summary>
        /// Disable product
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> DisableProduct([Required] Guid productId)
            => await JsonAsync(_productService.DisableProductAsync(productId));

        /// <summary>
        /// Activate product
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> ActivateProduct([Required] Guid productId)
            => await JsonAsync(_productService.ActivateProductAsync(productId));

        /// <summary>
        /// Activate product
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> DeleteProduct([Required] Guid productId)
            => await JsonAsync(_productService.DeleteProductAsync(productId));

        /// <summary>
        /// Disable product
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> DisableProductVariation([Required] Guid productId)
            => await JsonAsync(_productService.DisableProductVariationAsync(productId));

        /// <summary>
        /// Activate product
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> ActivateProductVariation([Required] Guid productId)
            => await JsonAsync(_productService.ActivateProductVariationAsync(productId));

        [HttpPut]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> AddProductVariation([Required] AddProductVariationViewModel model)
        {
            if (!ModelState.IsValid) return JsonModelStateErrors();
            return await JsonAsync(_productService.AddProductVariationAsync(model));
        }


        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> UpdateProductVariation([Required] UpdateProductVariationViewModel model)
        {
            return await JsonAsync(_productService.UpdateProductVariationAsync(model));
        }


        [HttpDelete]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> DeleteProductVariation([Required] Guid id)
        {
            if (!ModelState.IsValid) return JsonModelStateErrors();
            return await JsonAsync(_productService.DeleteProductVariationAsync(id));
        }


        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<IEnumerable<ProductVariationViewModel>>))]
        public async Task<JsonResult> GetProductVariationByProductTemplateId([Required] Guid id)
        {
            if (!ModelState.IsValid) return JsonModelStateErrors();
            return await JsonAsync(_productService.GetProductVariationByProductTemplateId(id));
        }


        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<IEnumerable<ProductVariationViewModel>>))]
        public async Task<JsonResult> GetAllProductVariations() =>
            await JsonAsync(_productService.GetAllProductVariationsAsync());


        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<ProductVariationViewModel>))]
        public async Task<JsonResult> GetProductVariationById([Required] Guid productVariatinoId)
            => await JsonAsync(_productService.GetProductVariationByIdAsync(productVariatinoId));


        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> ActivateProductDeliverable([Required] Guid deliverableId)
            => await JsonAsync(_productService.ActivateProductDeliverableAsync(deliverableId));


        [HttpPut]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> AddProductDeliverable([Required] AddProductDeliverableViewModel model)
        {
            
            if (!ModelState.IsValid) return JsonModelStateErrors();
            return await JsonAsync(_productService.AddProductDeliverableAsync(model));
        }


        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> UpdateProductDeliverable([Required] ProductDeliverableViewModel model)
        {
            if (!ModelState.IsValid) return JsonModelStateErrors();
            return await JsonAsync(_productService.UpdateProductDeliverableAsync(model));
        }

        [HttpDelete]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> DeleteProductDeliverable([Required] Guid deliverableId)
        {
            if (!ModelState.IsValid) return JsonModelStateErrors();
            return await JsonAsync(_productService.DeleteProductDeliverableAsync(deliverableId));
        }

        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<IEnumerable<ProductDeliverableViewModel>>))]
        public async Task<JsonResult> GetProductDeliverableByProductVariationId([Required] Guid id)
            => await JsonAsync(_productService.GetProductDeliverableByProductVariationId(id));

        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<PagedResult<ProductDeliverableViewModel>>))]
        public async Task<JsonResult> GetAllPaginatedProductDeliverable(PageRequest request)
            => await JsonAsync(_productService.GetAllPaginatedProductDeliverablesAsync(request));

        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<ProductVariationViewModel>))]
        public async Task<JsonResult> GetProductDeliverableById([Required] Guid deliverableId)
            => await JsonAsync(_productService.GetProductDeliverableByIdAsync(deliverableId));

        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<ProductVariationViewModel>))]
        public async Task<JsonResult> GetProductDeliverablesWithNoVariation()
            => await JsonAsync(_productService.GetProductDeliverablesWithNoVariationAsync());

        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> RemoveDeliverableFromVariation(List<Guid> deliverablesIds) =>
            await JsonAsync(_productService.RemoveDeliverableFromVariationAsync(deliverablesIds));

        /// <summary>
        /// Disable product
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> DisableProductDeliverable([Required] Guid deliverableId)
            => await JsonAsync(_productService.DisableProductDeliverableAsync(deliverableId));
    }
}
