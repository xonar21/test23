using GR.Core.Helpers;
using GR.Crm.Organizations.Abstractions.Models;
using GR.Crm.Organizations.Abstractions.ViewModels.OrganizationStagesViewModels;
using GR.Crm.Organizations.Abstractions.ViewModels.OrganizationStatesViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GR.Crm.Organizations.Abstractions
{
    public interface ICrmOrganizationHelperService
    {

        /// <summary>
        /// Add organization stage 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<Guid>> AddStageAsync(AddOrganizationStageViewModel model);

        /// <summary>
        /// Get all organization stages
        /// </summary>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<OrganizationStage>>> GetAllStagesAsync();

        /// <summary>
        /// Find stage by id
        /// </summary>
        /// <param name="stageId"></param>
        /// <returns></returns>
        Task<ResultModel<OrganizationStage>> GetStageByIdAsync(Guid? stageId);

        /// <summary>
        /// Add organization state 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<Guid>> AddStateAsync(AddOrganizationStateViewModel model);

        /// <summary>
        /// Get all organization states
        /// </summary>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<OrganizationState>>> GetAllStatesAsync();

        /// <summary>
        /// Get all organization states by stage
        /// </summary>
        /// <param name="stageId"></param>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<OrganizationState>>> GetAllStatesByStageAsync(Guid stageId);
    }
}
