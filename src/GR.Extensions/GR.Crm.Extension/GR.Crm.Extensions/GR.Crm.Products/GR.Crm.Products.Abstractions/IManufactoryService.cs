using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Crm.Products.Abstractions.ViewModels.ManufactoryViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gr.Crm.Products.Abstractions
{
    public interface IManufactoryService
    {
        /// <summary>
        /// Add manufactory 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<Guid>> AddManufactoryAsync(AddManufactoryViewModel model);

        /// <summary>
        /// Update manufactory
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel> UpdateManufactoryAsync(AddManufactoryViewModel model);

        /// <summary>
        /// Get manufactory by id
        /// </summary>
        /// <param name="manufactoryId"></param>
        /// <returns></returns>
        Task<ResultModel<GetManufactoryViewModel>> GetManufactoryByIdAsync(Guid? manufactoryId);

        /// <summary>
        /// Get all manufactory
        /// </summary>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<GetManufactoryViewModel>>> GetAllManufactoriesAsync(bool includeDeleted);

        /// <summary>
        /// Get all paginated manufactories
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ResultModel<PagedResult<GetManufactoryViewModel>>> GetAllPaginatedManufactoriesAsync(PageRequest request);

        /// <summary>
        /// Disable manufactory
        /// </summary>
        /// <param name="manufactoryId"></param>
        /// <returns></returns>
        Task<ResultModel> DisableManufactoryAsync(Guid? manufactoryId);

        /// <summary>
        /// Activate manufactory
        /// </summary>
        /// <param name="manufactoryId"></param>
        /// <returns></returns>
        Task<ResultModel> ActivateManufactoryAsync(Guid? manufactoryId);

        /// <summary>
        /// Delete manufactory
        /// </summary>
        /// <param name="manufactoryId"></param>
        /// <returns></returns>
        Task<ResultModel> DeleteManufactoryAsync(Guid? manufactoryId);
    }
}
