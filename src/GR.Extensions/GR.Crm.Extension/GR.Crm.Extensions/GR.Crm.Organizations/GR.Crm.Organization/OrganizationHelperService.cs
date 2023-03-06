using AutoMapper;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.Core.Helpers.Responses;
using GR.Crm.Organizations.Abstractions;
using GR.Crm.Organizations.Abstractions.Models;
using GR.Crm.Organizations.Abstractions.ViewModels.OrganizationStagesViewModels;
using GR.Crm.Organizations.Abstractions.ViewModels.OrganizationStatesViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Crm.Organizations
{
    public class OrganizationHelperService : ICrmOrganizationHelperService
    {
        #region Injectable

        /// <summary>
        /// Inject organization context
        /// </summary>
        private readonly ICrmOrganizationContext _context;

        /// <summary>
        /// Inject mapper
        /// </summary>
        private readonly IMapper _mapper;

        #endregion

        #region Ctor
        public OrganizationHelperService(ICrmOrganizationContext context,
            IMapper mapper
            )
        {
            _context = context;
            _mapper = mapper;
        }
        #endregion

        #region Stages
        /// <summary>
        /// Add new organization stage
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<Guid>> AddStageAsync(AddOrganizationStageViewModel model)
        {
            if (model == null) return InvalidParametersResultModel<Guid>.Instance;


            var stageBd = await _context.OrganizationStages.FirstOrDefaultAsync(x =>
               string.Equals(x.Name.Trim(), model.Name.Trim(), StringComparison.CurrentCultureIgnoreCase));

            if (stageBd != null)
                return new ResultModel<Guid> { IsSuccess = false, Errors = new List<IErrorModel> { new ErrorModel { Message = "Stage [" + model.Name + "] exists" } } };

            var stage = new OrganizationStage
            {
                Name = model.Name,
                DisplayOrder = model.DisplayOrder
              
            };
            await _context.OrganizationStages.AddAsync(stage);
            var result = await _context.PushAsync();

            return new ResultModel<Guid> { IsSuccess = result.IsSuccess, Errors = result.Errors, Result = stage.Id };
        }

        /// <summary>
        /// Get all organization stages
        /// </summary>
        /// <returns></returns>
        public async Task<ResultModel<IEnumerable<OrganizationStage>>> GetAllStagesAsync()
        {
            var query = await _context.OrganizationStages
                .OrderBy(x => x.DisplayOrder)
                .ToListAsync();

            return new SuccessResultModel<IEnumerable<OrganizationStage>>(query);
        }

        /// <summary>
        /// Find stage by id
        /// </summary>
        /// <param name="stageId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<OrganizationStage>> GetStageByIdAsync(Guid? stageId)
        {
            if (stageId == null) return new NotFoundResultModel<OrganizationStage>();
            var stage = await _context.OrganizationStages.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(stageId));
            if (stage == null) return new NotFoundResultModel<OrganizationStage>();
            return new SuccessResultModel<OrganizationStage>(stage);
        }

        #endregion

        #region States

        /// <summary>
        /// Add new organization state
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<Guid>> AddStateAsync(AddOrganizationStateViewModel model)
        {
            if (model == null) return InvalidParametersResultModel<Guid>.Instance;


            var stateBd = await _context.OrganizationStages.FirstOrDefaultAsync(x =>
               string.Equals(x.Name.Trim(), model.Name.Trim(), StringComparison.CurrentCultureIgnoreCase));

            if (stateBd != null)
                return new ResultModel<Guid> { IsSuccess = false, Errors = new List<IErrorModel> { new ErrorModel { Message = "Stage [" + model.Name + "] exists" } } };

            var state = new OrganizationState
            {
                Name = model.Name,
                StateStyleClass = model.StateStyleClass

            };

   
            await _context.OrganizationStates.AddAsync(state);
            var result = await _context.PushAsync();

            var stateStage = new OrganizationStateStage
            {
                StateId = state.Id,
                StageId = model.StageId
            };

            return new ResultModel<Guid> { IsSuccess = result.IsSuccess, Errors = result.Errors, Result = state.Id };
        }

        /// <summary>
        /// Get all organization states
        /// </summary>
        /// <returns></returns>
        public async Task<ResultModel<IEnumerable<OrganizationState>>> GetAllStatesAsync()
        {
            var query = await _context.OrganizationStates
                .ToListAsync();

            return new SuccessResultModel<IEnumerable<OrganizationState>>(query);
        }

        /// <summary>
        /// Get all organization states
        /// </summary>
        ///  <param name="stageId"></param>
        /// <returns></returns>
        public async Task<ResultModel<IEnumerable<OrganizationState>>> GetAllStatesByStageAsync(Guid stageId)
        {
            var query = await _context.OrganizationStates
                .Include(x => x.Stages)
                .Where(x => x.Stages.Select(y => y.StageId).Contains(stageId))
                .ToListAsync();

            return new SuccessResultModel<IEnumerable<OrganizationState>>(query);
        }

        #endregion

    }
}
