using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Crm.Organizations.Abstractions.Enums;
using GR.Crm.Organizations.Abstractions.ViewModels.OrganizationsViewModels;
using Microsoft.AspNetCore.Http;

namespace GR.Crm.Organizations.Abstractions
{
    public interface ICrmOrganizationService
    {
        #region Organizations
        /// <summary>
        /// Get all paginated  organization
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ResultModel<PagedResult<GetOrganizationViewModel>>> GetPaginatedOrganizationAsync(PageRequest request);

        /// <summary>
        /// find organization by id
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        Task<ResultModel<GetOrganizationViewModel>> FindOrganizationByIdAsync(Guid? organizationId);

        /// <summary>
        /// Find organization by fiscal code
        /// </summary>
        /// <param name="fiscalCode"></param>
        /// <returns></returns>
       Task<ResultModel<GetOrganizationViewModel>> GetOrganizationByFiscalCodeAsync(string fiscalCode);

        /// <summary>
        /// Get all active organizations
        /// </summary>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<GetOrganizationViewModel>>> GetAllActiveOrganizationsAsync(bool includeDeleted);

        /// <summary>
        /// Delete organization
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        Task<ResultModel> DeleteOrganizationAsync(Guid? organizationId);

        /// <summary>
        /// Deactivate organization
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        Task<ResultModel> DeactivateOrganizationAsync(Guid? organizationId);

        /// <summary>
        /// Activate organization
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        Task<ResultModel> ActivateOrganizationAsync(Guid? organizationId);

        /// <summary>
        /// Add new organization
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<Guid>> AddNewOrganizationAsync(OrganizationViewModel model);

        /// <summary>
        /// Update organization
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<Guid>> UpdateOrganizationAsync(OrganizationViewModel model);

        /// <summary>
        /// Import organization 
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        Task<ResultModel> ImportOrganizationAsync(IFormFile formFile);


        /// <summary>
        /// Move organization to another stage
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="stageId"></param>
        /// <param name="stateId"></param>
        /// <returns></returns>
        Task<ResultModel> UpdateOrganizationStageAsync(Guid? organizationId, Guid? stageId, Guid? stateId);


        /// <summary>
        /// Change organization state
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="stateId"></param>
        /// <returns></returns>
        Task<ResultModel> UpdateOrganizationStateAsync(Guid? organizationId, Guid? stateId);

        #endregion

    }
}
