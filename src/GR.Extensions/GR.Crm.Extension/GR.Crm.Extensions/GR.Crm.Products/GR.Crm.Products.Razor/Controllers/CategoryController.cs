using Gr.Crm.Products.Abstractions;
using Gr.Crm.Products.Abstractions.ViewModels.CategoryViewModels;
using GR.Core.BaseControllers;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Crm.Products.Abstractions;
using GR.Crm.Products.Abstractions.ViewModels.CategoryViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace GR.Crm.Products.Razor.Controllers
{
    public class CategoryController: BaseGearController
    {
        #region Injectable

        /// <summary>
        /// Inject category service
        /// </summary>
        /// <returns></returns>
        private readonly ICategoryService _categoryService;
        #endregion

        public IActionResult Index()
        {
            return View();
        }

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

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
        /// Get all paginated category
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<PagedResult<GetCategoryViewModel>>))]
        public async Task<JsonResult> GetAllPaginatedCategory(PageRequest request)
            => await JsonAsync(_categoryService.GetAllPaginatedCategoriesAsync(request));



        /// <summary>
        /// Get category by id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<GetCategoryViewModel>))]
        public async Task<JsonResult> GetCategoryById([Required] Guid categoryId)
            => await JsonAsync(_categoryService.GetCategoryByIdAsync(categoryId));


        /// <summary>
        /// Add category
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<Guid>))]
        public async Task<JsonResult> AddCategory([Required] AddCategoryViewModel model)
        {
            if (!ModelState.IsValid) return JsonModelStateErrors();
            return await JsonAsync(_categoryService.AddCategoryAsync(model));
        }


        /// <summary>
        /// Update Category
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> UpdateCategory([Required] AddCategoryViewModel model)
        {
            if (!ModelState.IsValid) return JsonModelStateErrors();
            return await JsonAsync(_categoryService.UpdateCategoryAsync(model));
        }



        /// <summary>
        /// Disable categoryId
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> DisableCategory([Required] Guid categoryId)
            => await JsonAsync(_categoryService.DisableCategoryAsync(categoryId));

        /// <summary>
        /// Activate category
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> ActivateCategory([Required] Guid categoryId)
            => await JsonAsync(_categoryService.ActivateCategoryAsync(categoryId));

        /// <summary>
        /// Delete Category
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> DeleteCategory([Required] Guid categoryId)
            => await JsonAsync(_categoryService.DeleteCategoryAsync(categoryId));
    }
}
