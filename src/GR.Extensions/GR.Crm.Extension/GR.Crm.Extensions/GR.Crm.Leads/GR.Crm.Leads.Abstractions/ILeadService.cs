using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using GR.CloudStorage.Abstractions.Models;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Crm.Leads.Abstractions.Models;
using GR.Crm.Leads.Abstractions.ViewModels;
using GR.Crm.Organizations.Abstractions.ViewModels.ContactsViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GR.Crm.Leads.Abstractions
{
    public interface ILeadService<TLead> where TLead : Lead
    {
        #region Leads

        /// <summary>
        /// Get all leads
        /// </summary>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<TLead>>> GetAllLeadsAsync(bool includeDeleted);

        /// <summary>
        /// Get lead by Id
        /// </summary>
        /// <param name="leadId"></param>
        /// <returns></returns>
        Task<ResultModel<GetLeadsViewModel>> GetLeadByIdAsync(Guid? leadId);

        /// <summary>
        /// Get leads by pipeLine
        /// </summary>
        /// <param name="pipeLineId"></param>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<TLead>>> GetLeadsByPipeLineIdAsync(Guid? pipeLineId, bool includeDeleted);

        /// <summary>
        /// Get leads by pipeline with pagination
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pipeLineId"></param>
        /// <returns></returns>
        Task<ResultModel<PagedResult<GetLeadsViewModel>>> GetPaginatedLeadsByPipeLineIdAsync(PageRequest request, Guid? pipeLineId);

        /// <summary>
        /// get contacts by organizationId
        /// </summary>
        /// <param name="leadId"></param>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        Task<ResultModel<PagedResult<PaginateContactViewModel>>> GetAllContactsByLeadIdAsync([Required] PageRequest request, [Required] Guid leadId);

        /// <summary>
        /// Get leads by organization
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<Lead>>> GetLeadsByOrganizationIdAsync(Guid? organizationId, bool includeDeleted);

        /// <summary>
        /// Get paginated result of leads by organization id
        /// </summary>
        /// <param name="request"></param>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        Task<PagedResult<GetLeadsViewModel>> GetPaginatedLeadsByOrganizationIdAsync(PageRequest request, Guid? organizationId);

        /// <summary>
        /// Find lead by id
        /// </summary>
        /// <param name="leadId"></param>
        /// <returns></returns>
        Task<ResultModel<TLead>> FindLeadByIdAsync(Guid? leadId);

        /// <summary>
        /// Get leads by stage id
        /// </summary>
        /// <param name="stageId"></param>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<GetLeadsViewModel>>> GetLeadsByStageIdAsync(Guid? stageId, bool includeDeleted);

        /// <summary>
        /// Move lead to another stage
        /// </summary>
        /// <param name="leadId"></param>
        /// <param name="stageId"></param>
        /// <param name="stateId"></param>
        /// <returns></returns>
        Task<ResultModel> MoveLeadToStageAsync(Guid? leadId, Guid? stageId, Guid? stateId, IUrlHelper Url);

        /// <summary>
        /// Set owner for team
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="leadId"></param>
        /// <param name="listMembersId"></param>
        /// <returns></returns>
        Task<ResultModel<Guid>> SetLeadOwnerAsync(Guid? userId, Guid? leadId, IEnumerable<Guid> listMembersId, IUrlHelper Url);

        /// <summary>
        /// Set team for lead
        /// </summary>
        /// <param name="lead"></param>
        /// <param name="teamId"></param>
        /// <returns></returns>
        Task<ResultModel> SetTeamForLeadAsync(Lead lead, Guid? teamId);

        /// <summary>
        /// Get leads count by organization
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        Task<ResultModel<int>> GetLeadsCountByOrganizationAsync(Guid? organizationId);

        /// <summary>
        /// Add lead
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<Guid>> AddLeadAsync([Required] AddLeadViewModel model);

        /// <summary>
        /// Add contact to lead
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel> AddContactToLead(LeadContactViewModel model);

        /// <summary>
        /// Delete contact from lead
        /// </summary>
        /// <param name="contactId"></param>
        /// <param name="leadId"></param>
        /// <returns></returns>
        Task<ResultModel> DeleteContactFromLead(Guid? contactId, Guid? leadId);

        /// <summary>
        /// Get initial lead state
        /// </summary>
        /// <returns></returns>
        Task<ResultModel<LeadState>> GetInitialLeadStateAsync();

        /// <summary>
        /// Get leads paginated
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagedResult<GetLeadsViewModel>> GetPaginatedLeadsAsync(PageRequest request);

        /// <summary>
        /// Update lead
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel> UpdateLeadAsync([Required] UpdateLeadViewModel model, IUrlHelper Url);

        /// <summary>
        /// Disable lead
        /// </summary>
        /// <param name="leadId"></param>
        /// <returns></returns>
        Task<ResultModel> DisableLeadAsync(Guid? leadId);

        /// <summary>
        /// Activate lead
        /// </summary>
        /// <param name="leadId"></param>
        /// <returns></returns>
        Task<ResultModel> ActivateLeadAsync(Guid? leadId);


        /// <summary>
        /// Delete lead
        /// </summary>
        /// <param name="leadId"></param>
        /// <returns></returns>
        Task<ResultModel> DeleteLeadAsync(Guid? leadId);

        /// <summary>
        /// Get grid leads by pipelineId
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="pipeLineId"></param>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<GetGridLeadsViewModel>>> GetGridLeadsByPipeLineIdAsync(IEnumerable<PageRequestFilter> filters, Guid? pipeLineId);


        Task<ResultModel> UpdateLeadContactAsync(LeadContactViewModel model);
        #endregion

        #region Lead states

        /// <summary>
        /// Add lead state
        /// </summary>
        /// <param name="name"></param>
        /// <param name="styleClass"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        Task<ResultModel<Guid>> AddLeadStateAsync([Required] string name, string styleClass, string description);

        /// <summary>
        /// Order lead states
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<ResultModel> OrderLeadStateAsync(IEnumerable<OrderLeadStatesViewModel> data);

        /// <summary>
        /// Get all lead states
        /// </summary>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<LeadState>>> GetAllLeadStatesAsync(bool includeDeleted);

        /// <summary>
        /// Get all lead states
        /// </summary>
        /// <param name="includeDeleted"></param>
        /// <param name="stageId"></param>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<LeadState>>> GetAllLeadStatesByStageAsync(Guid stageId, bool includeDeleted);

        /// <summary>
        /// Get lead state by id
        /// </summary>
        /// <param name="stateId"></param>
        /// <returns></returns>
        Task<ResultModel<LeadState>> GetLeadStateByIdAsync(Guid? stateId);

        /// <summary>
        /// Disable lead state
        /// </summary>
        /// <param name="leadStateId"></param>
        /// <returns></returns>
        Task<ResultModel> DisableLeadStateAsync(Guid? leadStateId);

        /// <summary>
        /// Activate lead state
        /// </summary>
        /// <param name="leadStateId"></param>
        /// <returns></returns>
        Task<ResultModel> ActivateLeadStateAsync(Guid? leadStateId);

        /// <summary>
        /// Remove lead state
        /// </summary>
        /// <param name="leadStateId"></param>
        /// <returns></returns>
        Task<ResultModel> RemoveLeadStateAsync(Guid? leadStateId);

        /// <summary>
        /// Update lead state
        /// </summary>
        /// <param name="leadStateId"></param>
        /// <param name="name"></param>
        /// <param name="styleClass"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        Task<ResultModel> UpdateLeadStateAsync(Guid? leadStateId, string name, string description, string styleClass);

        /// <summary>
        /// Change lead state
        /// </summary>
        /// <param name="leadId"></param>
        /// <param name="stateId"></param>
        /// <returns></returns>
        Task<ResultModel> ChangeLeadStateAsync(Guid? leadId, Guid? stageId);

        /// <summary>
        /// Seed lead state 
        /// </summary>
        /// <returns></returns>
        Task SeedSystemLeadState();

        /// <summary>
        /// UploadFile
        /// </summary>
        /// <param name="LeadId"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        Task<ResultModel> UploadFile(Guid LeadId, IFormFile file);

        Task<ResultModel> DeleteFile(string id, string path, string fileName);

        Task<ResultModel<List<FileViewModel>>> GetFiles(Guid? leadId);

        #endregion

        #region NoGoState
        /// <summary>
        /// Add NoGoState
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<Guid>> AddNoGoStateAsync(AddNoGoStateViewModel model);


        /// <summary>
        /// Update NoGoState
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel> UpdateNoGoStateAsync(UpdateNoGoStateViewModel model);


        /// <summary>
        /// Get NoGoState by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ResultModel<NoGoStateViewModel>> GetNoGoStateByIdAsync(Guid? Id);

        /// <summary>
        /// Activate NoGoState by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ResultModel> ActivateNoGoStateAsync(Guid? Id);


        /// <summary>
        /// Disable NoGoState by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ResultModel> DisableNoGoStateAsync(Guid? Id);

        /// <summary>
        /// Delete NoGoState by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ResultModel> DeleteNoGoStateAsync(Guid? Id);


        /// <summary>
        /// Get all NoGoStates
        /// </summary>
        Task<ResultModel<IEnumerable<NoGoStateViewModel>>> GetAllNoGoStatesAsync();


        /// <summary>
        /// Get all pagged NoGoStates
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ResultModel<PagedResult<NoGoStateViewModel>>> GetAllPaginatedNoGoStatesAsync(PageRequest request);


        //Task<ResultModel> AddLeadToNoGoStateAsync(LeadToNoGoStateViewModel model);


        //Task <ResultModel<IEnumerable<GetLeadsViewModel>>> GetLeadsByNoGoStateIdAsync(Guid? Id);

        //Task<ResultModel> RemoveLeadsFromNoGoStateAsync(List<Guid> LeadsIds);
        #endregion

        Task<ResultModel> AddProductOrServicesAsync(List<AddProductOrServiceViewModel> model);

        Task<ResultModel> UpdateProductOrServicesAsync(List<UpdateProductOrServiceViewModel> model);

        Task<ResultModel> DeleteProductOrServicesAsync(Guid Id);
    }
}