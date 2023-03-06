using Gr.Crm.Products.Abstractions;
using GR.Core.BaseControllers;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Crm.Products.Abstractions;
using GR.Crm.Products.Abstractions.ViewModels.ManufactoryViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace GR.Crm.Products.Razor.Controllers
{
    public class ManufactoryController: BaseGearController
    {
        #region Injectable

        /// <summary>
        /// Inject category service
        /// </summary>
        /// <returns></returns>
        private readonly IManufactoryService _manufactoryService;
        #endregion

        public IActionResult Index()
        {
            return View();
        }

        public ManufactoryController(IManufactoryService manufactoryService)
        {
            _manufactoryService = manufactoryService;
        }

        /// <summary>
        /// Get all categories
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<IEnumerable<GetManufactoryViewModel>>))]
        public async Task<JsonResult> GetAllManufactories(bool includeDeleted = false)
            => await JsonAsync(_manufactoryService.GetAllManufactoriesAsync(includeDeleted));


        /// <summary>
        /// Get all paginated category
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<PagedResult<GetManufactoryViewModel>>))]
        public async Task<JsonResult> GetAllPaginatedManufactoy(PageRequest request)
            => await JsonAsync(_manufactoryService.GetAllPaginatedManufactoriesAsync(request));



        /// <summary>
        /// Get category by id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<GetManufactoryViewModel>))]
        public async Task<JsonResult> GetManufactoryById([Required] Guid manufactoryId)
            => await JsonAsync(_manufactoryService.GetManufactoryByIdAsync(manufactoryId));


        /// <summary>
        /// Add category
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<Guid>))]
        public async Task<JsonResult> AddManufactory([Required] AddManufactoryViewModel model)
        {
            if (!ModelState.IsValid) return JsonModelStateErrors();
            return await JsonAsync(_manufactoryService.AddManufactoryAsync(model));
        }


        /// <summary>
        /// Update Category
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> UpdateManufactory([Required] AddManufactoryViewModel model)
        {
            if (!ModelState.IsValid) return JsonModelStateErrors();
            return await JsonAsync(_manufactoryService.UpdateManufactoryAsync(model));
        }



        /// <summary>
        /// Disable categoryId
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> DisableManufactory([Required] Guid manufactoryId)
            => await JsonAsync(_manufactoryService.DisableManufactoryAsync(manufactoryId));

        /// <summary>
        /// Activate category
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> ActivateManufactory([Required] Guid manufactoryId)
            => await JsonAsync(_manufactoryService.ActivateManufactoryAsync(manufactoryId));

        /// <summary>
        /// Delete Category
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> DeleteManufactory([Required] Guid manufactoryId)
            => await JsonAsync(_manufactoryService.DeleteManufactoryAsync(manufactoryId));
    }
}
