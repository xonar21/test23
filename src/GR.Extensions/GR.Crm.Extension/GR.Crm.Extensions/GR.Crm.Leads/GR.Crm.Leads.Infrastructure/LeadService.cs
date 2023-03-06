using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using GR.CloudStorage.Abstractions;
using GR.CloudStorage.Abstractions.Enums;
using GR.CloudStorage.Abstractions.Models;
using GR.CloudStorage.Implementation;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Core.Helpers.Responses;
using GR.Crm.Abstractions;
using GR.Crm.Emails.Abstractions;
using GR.Crm.Leads.Abstractions;
using GR.Crm.Leads.Abstractions.Models;
using GR.Crm.Leads.Abstractions.ViewModels;
using GR.Crm.Leads.Infrastructure.Extensions;
using GR.Crm.Organizations.Abstractions;
using GR.Crm.Organizations.Abstractions.ViewModels.ContactsViewModels;
using GR.Crm.PipeLines.Abstractions;
using GR.Crm.Teams.Abstractions;
using GR.Crm.Teams.Abstractions.Helpers;
using GR.Crm.Teams.Abstractions.ViewModels;
using GR.Identity.Abstractions;
using GR.Notifications.Abstractions;
using GR.Notifications.Abstractions.Models.Notifications;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace GR.Crm.Leads.Infrastructure
{
    public class CrmLeadService : ILeadService<Lead>
    {
        #region Injectable

        /// <summary>
        /// Inject context
        /// </summary>
        private readonly ILeadContext<Lead> _context;

        /// <summary>
        /// Inject pipeLine service
        /// </summary>
        private readonly ICrmPipeLineService _crmPipeLineService;


        /// <summary>
        /// Inject team service
        /// </summary>
        private readonly ICrmTeamService _teamService;

        /// <summary>
        /// Inject mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Inject notification
        /// </summary>
        private readonly INotify<GearRole> _notify;

        private readonly IUserTokenDataService _userTokenDataService;

        /// <summary>
        /// Inject user manager
        /// </summary>
        private readonly IUserManager<GearUser> _userManager;

        private readonly IOptionsMonitor<CloudServiceSettings> _optionsMonitor;

        private readonly ILogger<OneDriveService> _logger;

        private readonly IEmailContext _emailContext;

        private readonly ICrmOrganizationContext _organizationContext;

        private readonly ICrmService _crmService;
        /// <summary>
        /// Inject notification service
        /// </summary>
        private readonly ILeadNotificationService _leadNotificationService;


        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="workFlowExecutorService"></param>
        /// <param name="crmPipeLineService"></param>
        /// <param name="teamService"></param>
        /// <param name="mapper"></param>
        /// <param name="notify"></param>
        public CrmLeadService(ILeadContext<Lead> context,
            ICrmPipeLineService crmPipeLineService,
            ICrmTeamService teamService, IMapper mapper, INotify<GearRole> notify,
            IConfiguration configuration,
            ICrmService crmService,
            IUserTokenDataService userTokenDataService,
            IUserManager<GearUser> userManager,
            IOptionsMonitor<CloudServiceSettings> optionsMonitor,
            ILogger<OneDriveService> logger,
            IEmailContext emailContext,
            ICrmOrganizationContext organizationContext,
            ILeadNotificationService leadNotificationService)
        {
            _context = context;
            _crmPipeLineService = crmPipeLineService;
            _teamService = teamService;
            _mapper = mapper;
            _notify = notify;
            _userTokenDataService = userTokenDataService;
            _userManager = userManager;
            _optionsMonitor = optionsMonitor;
            _logger = logger;
            _emailContext = emailContext;
            _organizationContext = organizationContext;
            _crmService = crmService;
            _leadNotificationService = leadNotificationService;
            GlobalCurrency = configuration.GetSection("GeneralConfigurations").GetSection("CurrencyCode").Value;
            ConversionRate = crmService.ConvertCurrencyToDefaultCurrencyAsync("EUR");
        }
        private string GlobalCurrency { get; set; }

        private Task<decimal> ConversionRate { get; set; }
        #region  Lead

        /// <summary>
        /// Add new lead
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<Guid>> AddLeadAsync([Required] AddLeadViewModel model)
        {
            if (model == null) return new InvalidParametersResultModel<Guid>();
            var lead = _mapper.Map<Lead>(model);

            var leadBd = await _context.Leads.FirstOrDefaultAsync(x =>
                x.PipeLineId == model.PipeLineId && x.Name.Trim().ToLower().Equals(model.Name.Trim().ToLower()));

            if (leadBd != null)
                return new ResultModel<Guid>
                {
                    IsSuccess = false,
                    Errors = new List<IErrorModel> { new ErrorModel { Message = "Lead [" + leadBd.Name + "] exist" } }
                };

            await _context.Leads.AddAsync(lead);
            var addLeadResult = await _context.PushAsync();
            if (model.Created != DateTime.UtcNow)
            {
                lead.Created = model.Created;
                _context.Leads.Update(lead);

                var res = await _context.PushAsync();
                if (!res.IsSuccess)
                {
                    addLeadResult.Errors.Concat(res.Errors);
                    return new ResultModel<Guid> { IsSuccess = false, Errors = addLeadResult.Errors };
                }
            }

            foreach (var contact in model.ContactsIds)
            {
                await AddContactToLead(new LeadContactViewModel

                { ContactId = contact, LeadId = lead.Id });
            }


            return addLeadResult.Map<Guid>(lead.Id);
        }

        /// <summary>
        /// get contacts by organizationId
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<PagedResult<PaginateContactViewModel>>> GetAllContactsByLeadIdAsync([Required] PageRequest request, [Required] Guid leadId)
        {
            if (leadId == null)
                return new InvalidParametersResultModel<PagedResult<PaginateContactViewModel>>();

            var contactsList = await _context.LeadsContacts.Where(x => x.LeadId == leadId).Select(x => x.ContactId).ToListAsync();



            var contacts = await _context.Contacts
                .Include(i => i.JobPosition)
                .Include(i => i.Organization)
                .Where(x => (!x.IsDeleted || request.IncludeDeleted) && contactsList.Contains(x.Id))
                .Select(s => new PaginateContactViewModel
                {
                    Id = s.Id,
                    Created = s.Created,
                    JobPosition = s.JobPosition.Name,
                    OrganizationId = s.Organization.Id,
                    Organization = s.Organization.Name,
                    LastName = s.LastName,
                    FirstName = s.FirstName,
                    IsDeleted = s.IsDeleted,
                }).GetPagedAsync(request);

            foreach (var contact in contacts.Result)
            {
                contact.Email = _emailContext.Emails.Where(x => x.ContactId == contact.Id).Select(x => x.Label.ToString() + ": " + x.Email).ToList(); ;
                contact.DialCode = _organizationContext.PhoneLists.Where(x => x.ContactId == contact.Id).Select(x => x.DialCode).ToList();
                contact.Phone = _organizationContext.PhoneLists.Where(x => x.ContactId == contact.Id).Select(x => x.DialCode + "-" + x.Phone).ToList();
            }

            return new SuccessResultModel<PagedResult<PaginateContactViewModel>>(contacts.Map(contacts).Result);
        }
        /// <summary>
        /// get contacts by leadId
        /// </summary>
        /// <param name="leadId"></param>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<GetContactViewModel>>> GetAllContactsByLeadIdAsync(Guid leadId, bool includeDeleted = false)
        {
            var listContacts = await _context.LeadsContacts.Where(x => x.LeadId == leadId).Select(x=> x.ContactId).ToListAsync();
         

            var contacts = await _organizationContext.Contacts
                .Include(i => i.ContactWebProfiles)
                .ThenInclude(i => i.WebProfile)
                .Include(i => i.Organization)
                .Include(i => i.JobPosition)
                .Include(i => i.PhoneList)
                .Where(x => listContacts.Contains(x.Id) && (!x.IsDeleted || includeDeleted))
                .ToListAsync();


            return new SuccessResultModel<IEnumerable<GetContactViewModel>>(contacts.Adapt<IEnumerable<GetContactViewModel>>());
        }

        /// <summary>
        /// Update lead
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> UpdateLeadAsync([Required] UpdateLeadViewModel model, IUrlHelper Url)
        {
            if (model == null) return new InvalidParametersResultModel();

            var lead = await _context.Leads
                .Include(i => i.Team)
                .ThenInclude(i => i.TeamMembers)
                .FirstOrDefaultAsync(x => x.Id.Equals(model.Id));

            if (lead == null) return new NotFoundResultModel();


            var lastStageId = lead.StageId;
            var lastStateId = lead.LeadStateId;
            var lastValue = lead.Value;

            lead.Name = model.Name;
            lead.Created = model.Created;
            lead.Value = model.Value;
            lead.CurrencyCode = model.CurrencyCode;
            if (model.OrganizationId != null) lead.OrganizationId = model.OrganizationId.Value;
            lead.DeadLine = model.DeadLine;
            lead.ClarificationDeadline = model.ClarificationDeadline;
            if(lead.StageId != model.StageId)
            {
                lead.StageId = model.StageId;
                lead.StageChangeDate = DateTime.UtcNow;
            }
            lead.LeadStateId = model.LeadStateId;
            lead.SourceId = model.SourceId;
            lead.Description = model.Description;


            _context.Leads.Update(lead);

            var result = await _context.PushAsync();

            //update contacts list
            var contactsList = await _context.LeadsContacts.Where(x => x.LeadId == model.Id).ToListAsync();
            _context.LeadsContacts.RemoveRange(contactsList);

            await _context.PushAsync();

            foreach (var contact in model.Contacts)
            {
                await AddContactToLead(new LeadContactViewModel
                { ContactId = contact, LeadId = lead.Id });
            }

            //get lead owner
            var leadOwner = await _userManager.UserManager.FindByIdAsync(lead.Team?.TeamMembers?.FirstOrDefault(x => x.TeamRoleId.Equals(TeamResources.Owner))?.UserId.ToString());
            var leadOwnerUsername = leadOwner?.UserName;

            bool isProduction = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production";

            if (result.IsSuccess && lastStageId != lead.StageId)
            {
                var newStage = await _context.Stages.FindByIdAsync(lead.StageId);
                var lastStage = await _context.Stages.FindByIdAsync(lastStageId);

                if (newStage.Name.Equals("Negotiation") && lastStage.Name.Equals("Quotation") && isProduction)
                    await _leadNotificationService.NotifyOnStageChange(lead, "Quotation","Negotiation", Url);

                //notify owner if stage was changed by someone other than them
                if (leadOwner != null && leadOwnerUsername != lead.ModifiedBy)
                {
                    await _notify.SendNotificationAsync(new[] { leadOwner.Id.ToGuid() }, new Notification
                    {

                        Subject = "Info",
                        Content = $"{lead?.Name} stage was changed from {lastStage?.Name} to {newStage?.Name} by {lead.ModifiedBy}",
                        SendEmail = false,
                        Url = "/leads/details?id=" + lead?.Id
                    });

                }            
            }
           
            if (result.IsSuccess && lastValue != lead.Value && !lead.ModifiedBy.Equals("vitalie.tcaci") && isProduction)
                await _leadNotificationService.NotifyOnBudgetChange(lead, lastValue, Url);
            return result;
        }

        /// <summary>
        /// Add contact to lead
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> AddContactToLead(LeadContactViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel();

            var existentContact = await _context.LeadsContacts
                .FirstOrDefaultAsync(x => x.ContactId == model.ContactId && x.LeadId == model.LeadId);

            if (existentContact != null)
                return new ResultModel
                {
                    IsSuccess = false,
                    Errors = new List<IErrorModel> { new ErrorModel { Message = $"{ existentContact.Contact.FirstName} {existentContact.Contact.LastName} is already associated with the opportunity" } }
                };

            var newContact = new LeadContact
            {
                ContactId = model.ContactId,
                LeadId = model.LeadId,
            };

            await _context.LeadsContacts.AddAsync(newContact);

            var result = await _context.PushAsync();
            return new ResultModel { IsSuccess = result.IsSuccess, Errors = result.Errors };
        }


        /// <summary>
        /// Delete contact from opportunity
        /// </summary>
        /// <param name="contactId"></param>
        /// <param name="leadId"></param>
        /// <returns></returns>
        public async Task<ResultModel> DeleteContactFromLead(Guid? contactId, Guid? leadId)
        {
            if (contactId == null)
                return new InvalidParametersResultModel();

            var contact = await _context.LeadsContacts.FirstOrDefaultAsync(x => x.ContactId == contactId && x.LeadId == leadId);

            if (contact == null)
                return new NotFoundResultModel();

            _context.LeadsContacts.Remove(contact);
            return await _context.PushAsync();
        }


        public virtual async Task<ResultModel> UpdateLeadContactAsync(LeadContactViewModel model)
        {
            if (model == null) return new InvalidParametersResultModel();

            var lead = await _context.Leads
                .Include(i => i.Team)
                .ThenInclude(i => i.TeamMembers)
                .FirstOrDefaultAsync(x => x.Id.Equals(model.LeadId));

            if (lead == null) return new NotFoundResultModel();

            lead.ContactId = model.ContactId;

            return await _context.PushAsync();
        }

        /// <summary>
        /// Get initial lead state
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ResultModel<LeadState>> GetInitialLeadStateAsync()
        {
            var leadState = await _context.States
                .AsNoTracking()
                .OrderBy(x => x.Order)
                .FirstOrDefaultAsync();
            if (leadState == null) return new NotFoundResultModel<LeadState>();
            return new SuccessResultModel<LeadState>(leadState);
        }


        /// <summary>
        /// Get lead by Id
        /// </summary>
        /// <param name="leadId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<GetLeadsViewModel>> GetLeadByIdAsync(Guid? leadId)
        {
            if (leadId == null)
                return new InvalidParametersResultModel<GetLeadsViewModel>();

            var lead = await _context
                .BuildLeadsQuery()
                .FirstOrDefaultAsync(x => x.Id == leadId);

            if (lead == null)
                return new NotFoundResultModel<GetLeadsViewModel>();


            var leadToReturn = _mapper.Map<GetLeadsViewModel>(lead);

            var teamRequest = await _teamService.GetTeamMembersByTeamIdAsync(lead.TeamId);
            if (teamRequest.IsSuccess)
            {
                leadToReturn.LeadMembers = teamRequest.Result.Where(x => x.TeamRoleId.Equals(TeamResources.Member));
                leadToReturn.OwnerId = teamRequest.Result.FirstOrDefault(x => x.TeamRoleId.Equals(TeamResources.Owner))?.UserId;
            }


            return new SuccessResultModel<GetLeadsViewModel>(leadToReturn);

        }

        /// <summary>
        /// Get paginated leads
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual async Task<PagedResult<GetLeadsViewModel>> GetPaginatedLeadsAsync(PageRequest request)
        {
            if (request == null)
            {
                var response = new PagedResult<GetLeadsViewModel>();
                response.Errors.Add(new ErrorModel(string.Empty, "Invalid parameters"));
                return response;
            }

            var query = _context
                .BuildLeadsQuery()
                .Where(x => !x.IsDeleted || request.IncludeDeleted)
                .OrderByWithDirection(x => x.GetPropertyValue(request.Attribute), request.Descending);

            var pagedResult = await query.GetPagedAsync(request.Page, request.PageSize);
            var mappedCollection = _mapper.Map<IEnumerable<GetLeadsViewModel>>(pagedResult.Result).ToList();

            foreach (var lead in mappedCollection)
            {
                var teamRequest = await _teamService.GetTeamMembersByTeamIdAsync(lead.TeamId);
                if (teamRequest.IsSuccess)
                {
                    lead.LeadMembers = teamRequest.Result;
                }

                if (!GlobalCurrency.IsNullOrEmpty())
                {
                    lead.Value = Math.Round(lead.Value * ConversionRate.Result, 2);
                    lead.CurrencyCode = GlobalCurrency;
                }
            }

            return pagedResult.Map(mappedCollection);
        }

        /// <summary>
        /// Get all leads
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<Lead>>> GetAllLeadsAsync(bool includeDeleted = false)
        {
            var data = await _context
                .BuildLeadsQuery()
                .Where(x => !x.IsDeleted || includeDeleted)
                .ToListAsync();

            return new SuccessResultModel<IEnumerable<Lead>>(data);
        }

        /// <summary>
        /// Get leads by pipeLine
        /// </summary>
        /// <param name="pipeLineId"></param>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<Lead>>> GetLeadsByPipeLineIdAsync(Guid? pipeLineId, bool includeDeleted = false)
        {
            if (pipeLineId == null) return new InvalidParametersResultModel<IEnumerable<Lead>>();
            var data = await _context
                .BuildLeadsQuery()
                .Where(x => x.PipeLineId.Equals(pipeLineId) && (!x.IsDeleted || includeDeleted))
                .ToListAsync();

            return new SuccessResultModel<IEnumerable<Lead>>(data);
        }

        /// <summary>
        /// Get leads by organization id
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<Lead>>> GetLeadsByOrganizationIdAsync(Guid? organizationId, bool includeDeleted = false)
        {
            if (organizationId == null) return new InvalidParametersResultModel<IEnumerable<Lead>>();
            var data = await _context
                .BuildLeadsQuery()
                .Where(x => x.OrganizationId.Equals(organizationId) && (!x.IsDeleted || includeDeleted))
                .ToListAsync();

            return new SuccessResultModel<IEnumerable<Lead>>(data);
        }

        /// <summary>
        /// Get paginated result of leads by organization id
        /// </summary>
        /// <param name="request"></param>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public virtual async Task<PagedResult<GetLeadsViewModel>> GetPaginatedLeadsByOrganizationIdAsync(PageRequest request, Guid? organizationId)
        {
            if (request == null)
            {
                var response = new PagedResult<GetLeadsViewModel>();
                response.Errors.Add(new ErrorModel(string.Empty, "Invalid parameters"));
                return response;
            }

            var query = _context
                .BuildLeadsQuery()
                .Where(x => x.OrganizationId.Equals(organizationId) && (!x.IsDeleted || request.IncludeDeleted));

            if (request.PageRequestFilters.Select(s => s.Propriety).Contains("Owner"))
            {
                var listOwnerId = request.PageRequestFilters
                    .Where(x => string.Equals(x.Propriety.Trim(), "Owner".Trim(), StringComparison.CurrentCultureIgnoreCase)).Select(s => s.Value.ToStringIgnoreNull()).ToList();

                var listLeadId = new List<Guid>();

                foreach (var ownerId in listOwnerId)
                {
                    var lead = await query.Where(x => x.Team.TeamMembers.FirstOrDefault(i => i.UserId.ToString() == ownerId) != null).ToListAsync();
                    listLeadId.AddRange(lead.Select(s => s.Id));
                }

                query = query.Where(x => listLeadId.Contains(x.Id));

                request.PageRequestFilters = request.PageRequestFilters.Where(s => s.Propriety != "Owner");
            }

            var pagedResult = await query.GetPagedAsync(request);

            var mappedCollection = _mapper.Map<IEnumerable<GetLeadsViewModel>>(pagedResult.Result).ToList();

            foreach (var lead in mappedCollection)
            {
                var teamRequest = await _teamService.GetTeamMembersByTeamIdAsync(lead.TeamId);
                if (teamRequest.IsSuccess)
                {
                    lead.LeadMembers = teamRequest.Result;
                }
            }



            return pagedResult.Map(mappedCollection);
        }

        /// <summary>
        /// Get leads by pipeline with pagination
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pipeLineId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<PagedResult<GetLeadsViewModel>>> GetPaginatedLeadsByPipeLineIdAsync(PageRequest request, Guid? pipeLineId)
        {
            if (request == null)
            {
                return new InvalidParametersResultModel<PagedResult<GetLeadsViewModel>>();
            }

            var query = _context
                .BuildLeadsQuery()
                .Where(x => x.PipeLineId.Equals(pipeLineId) && (!x.IsDeleted || request.IncludeDeleted))
                .Select(s => new GetLeadsViewModel
                {
                    Organization = s.Organization,
                    Name = s.Name,
                    Id = s.Id,
                    Value = s.Value,
                    LeadStateId = s.LeadStateId,
                    Created = s.Created,
                    OrganizationId = s.OrganizationId,
                    PipeLineId = s.PipeLineId,
                    Stage = s.Stage,
                    IsDeleted = s.IsDeleted,
                    LeadState = s.LeadState,
                    StartDate = s.StartDate,
                    Team = s.Team,
                    Author = s.Author,
                    Changed = s.Changed,
                    StageDeadLine = s.StageDeadLine,
                    CurrencyCode = s.CurrencyCode,
                    DeadLine = s.DeadLine,
                    PipeLine = s.PipeLine,
                    TeamId = s.TeamId,
                    OrganizationName = s.Organization.Name,
                    StageName = s.Stage.Name,
                    Currency = s.Currency,
                    ModifiedBy = s.ModifiedBy,
                    Version = s.Version,
                    StageId = s.StageId,
                    TenantId = s.TeamId
                });
            //filter by owner
            if (request.PageRequestFilters.Select(s => s.Propriety).Contains("Owner"))
            {
                var listOwnerId = request.PageRequestFilters
                    .Where(x => string.Equals(x.Propriety.Trim(), "Owner".Trim(), StringComparison.CurrentCultureIgnoreCase)).Select(s => s.Value.ToStringIgnoreNull()).ToList();

                var listLeadId = new List<Guid>();

                foreach (var ownerId in listOwnerId)
                {
                    var lead = await query.Where(x => x.Team.TeamMembers.Where(i => i.TeamRoleId == TeamResources.Owner).FirstOrDefault(i => i.UserId.ToString() == ownerId) != null).ToListAsync();
                    listLeadId.AddRange(lead.Select(s => s.Id));
                }

                query = query.Where(x => listLeadId.Contains(x.Id));

                request.PageRequestFilters = request.PageRequestFilters.Where(s => s.Propriety != "Owner");
            }

            /*//filter by organization
            if (request.PageRequestFilters.Select(s => s.Propriety).Contains("Organization"))
            {
                var listOrganizationId = request.PageRequestFilters
                    .Where(x => string.Equals(x.Propriety.Trim(), "Organization".Trim(), StringComparison.CurrentCultureIgnoreCase)).Select(s => s.Value.ToStringIgnoreNull()).ToList();

                var listLeadId = new List<Guid>();

                foreach (var organizationId in listOrganizationId)
                {
                    var lead = await query.Where(x => x.OrganizationId.ToString() == organizationId).ToListAsync();
                    listLeadId.AddRange(lead.Select(s => s.Id));
                }

                query = query.Where(x => listLeadId.Contains(x.Id));

                request.PageRequestFilters = request.PageRequestFilters.Where(s => s.Propriety != "Organization");
            }*/

            //filter by start date
            if (request.PageRequestFilters.Select(s => s.Propriety).Contains("StartDate"))
            {
                var listStartDates = request.PageRequestFilters
                    .Where(x => string.Equals(x.Propriety.Trim(), "StartDate".Trim(), StringComparison.CurrentCultureIgnoreCase)).Select(s => s.Value.ToStringIgnoreNull()).ToList();

                var listLeadId = new List<Guid>();

                foreach (var startDate in listStartDates)
                {
                    var lead = await query.Where(x => x.StartDate >= startDate.StringToDateTime()).ToListAsync();
                    listLeadId.AddRange(lead.Select(s => s.Id));
                }

                query = query.Where(x => listLeadId.Contains(x.Id));

                request.PageRequestFilters = request.PageRequestFilters.Where(s => s.Propriety != "StartDate");
            }

            //filter by end date
            if (request.PageRequestFilters.Select(s => s.Propriety).Contains("EndDate"))
            {
                var listEndDates = request.PageRequestFilters
                    .Where(x => string.Equals(x.Propriety.Trim(), "EndDate".Trim(), StringComparison.CurrentCultureIgnoreCase)).Select(s => s.Value.ToStringIgnoreNull()).ToList();

                var listLeadId = new List<Guid>();

                foreach (var endDate in listEndDates)
                {
                    var lead = await query.Where(x => x.DeadLine <= endDate.StringToDateTime()).ToListAsync();
                    listLeadId.AddRange(lead.Select(s => s.Id));
                }

                query = query.Where(x => listLeadId.Contains(x.Id));

                request.PageRequestFilters = request.PageRequestFilters.Where(s => s.Propriety != "EndDate");
            }

            var pagedResult = await query.GetPagedAsync(request);

            var mappedCollection = pagedResult.Result;

            foreach (var lead in mappedCollection)
            {
                var teamRequest = await _teamService.GetTeamMembersByTeamIdAsync(lead.TeamId);
                if (teamRequest.IsSuccess)
                {
                    lead.LeadMembers = teamRequest.Result;
                }
                if (!GlobalCurrency.IsNullOrEmpty())
                {
                    lead.Value = Math.Round(lead.Value * ConversionRate.Result, 2);
                    lead.CurrencyCode = GlobalCurrency;
                }
            }


            return new SuccessResultModel<PagedResult<GetLeadsViewModel>>(pagedResult.Map(mappedCollection));
        }

        /// <summary>
        /// get grid leads by pipelineId
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="pipeLineId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<GetGridLeadsViewModel>>> GetGridLeadsByPipeLineIdAsync(IEnumerable<PageRequestFilter> filters, Guid? pipeLineId)
        {
            if (pipeLineId == null)
            {
                return new InvalidParametersResultModel<IEnumerable<GetGridLeadsViewModel>>();
            }

            var query = _context
                .BuildLeadsQuery()
                .Where(x => x.PipeLineId.Equals(pipeLineId) && (!x.IsDeleted))
                .Select(s => new GetLeadsViewModel
                {
                    Organization = s.Organization,
                    Name = s.Name,
                    Id = s.Id,
                    Value = s.Value,
                    GlobalCurrencyValue = s.Value,
                    LeadStateId = s.LeadStateId,
                    Created = s.Created,
                    OrganizationId = s.OrganizationId,
                    PipeLineId = s.PipeLineId,
                    Stage = s.Stage,
                    IsDeleted = s.IsDeleted,
                    LeadState = s.LeadState,
                    StartDate = s.StartDate,
                    StageChangeDate = s.StageChangeDate,
                    Team = s.Team,
                    Author = s.Author,
                    Changed = s.Changed,
                    StageDeadLine = s.StageDeadLine,
                    CurrencyCode = s.CurrencyCode,
                    GlobalCurrencyCode = s.CurrencyCode,
                    DeadLine = s.DeadLine,
                    PipeLine = s.PipeLine,
                    TeamId = s.TeamId,
                    OrganizationName = s.Organization.Name,
                    StageName = s.Stage.Name,
                    Currency = s.Currency,
                    ModifiedBy = s.ModifiedBy,
                    Version = s.Version,
                    StageId = s.StageId,
                    TenantId = s.TeamId
                });

            //filter by owner
            if (filters.Select(s => s.Propriety).Contains("OwnerId"))
            {
                var listOwnerId = filters
                    .Where(x => string.Equals(x.Propriety.Trim(), "OwnerId".Trim(), StringComparison.CurrentCultureIgnoreCase)).Select(s => s.Value.ToStringIgnoreNull()).ToList();

                var listLeadId = new List<Guid>();

                foreach (var ownerId in listOwnerId)
                {
                    var lead = await query.Where(x => x.Team.TeamMembers.Where(i => i.TeamRoleId == TeamResources.Owner).FirstOrDefault(i => i.UserId.ToString() == ownerId) != null).ToListAsync();
                    listLeadId.AddRange(lead.Select(s => s.Id));
                }

                query = query.Where(x => listLeadId.Contains(x.Id));

                filters = filters.Where(s => s.Propriety != "OwnerId");
            }

            //filter by organization
            if (filters.Select(s => s.Propriety).Contains("OrganizationId"))
            {
                var listOrganizationId = filters
                    .Where(x => string.Equals(x.Propriety.Trim(), "OrganizationId".Trim(), StringComparison.CurrentCultureIgnoreCase)).Select(s => s.Value.ToStringIgnoreNull()).ToList();

                var listLeadId = new List<Guid>();

                foreach (var organizationId in listOrganizationId)
                {
                    var lead = await query.Where(x => x.OrganizationId.ToString() == organizationId).ToListAsync();
                    listLeadId.AddRange(lead.Select(s => s.Id));
                }

                query = query.Where(x => listLeadId.Contains(x.Id));

                filters = filters.Where(s => s.Propriety != "OrganizationId");
            }

            //filter by start date
            if (filters.Select(s => s.Propriety).Contains("StartDate"))
            {
                var listStartDates = filters
                    .Where(x => string.Equals(x.Propriety.Trim(), "StartDate".Trim(), StringComparison.CurrentCultureIgnoreCase)).Select(s => s.Value.ToStringIgnoreNull()).ToList();

                var listLeadId = new List<Guid>();

                foreach (var startDate in listStartDates)
                {
                    var lead = await query.Where(x => x.StartDate >= DateTime.Parse(startDate)).ToListAsync();
                    listLeadId.AddRange(lead.Select(s => s.Id));
                }

                query = query.Where(x => listLeadId.Contains(x.Id));

                filters = filters.Where(s => s.Propriety != "StartDate");
            }

            //filter by end date
            if (filters.Select(s => s.Propriety).Contains("EndDate"))
            {
                var listEndDates = filters
                    .Where(x => string.Equals(x.Propriety.Trim(), "EndDate".Trim(), StringComparison.CurrentCultureIgnoreCase)).Select(s => s.Value.ToStringIgnoreNull()).ToList();

                var listLeadId = new List<Guid>();

                foreach (var endDate in listEndDates)
                {
                    var lead = await query.Where(x => x.DeadLine <= DateTime.Parse(endDate)).ToListAsync();
                    listLeadId.AddRange(lead.Select(s => s.Id));
                }

                query = query.Where(x => listLeadId.Contains(x.Id));

                filters = filters.Where(s => s.Propriety != "EndDate");
            }

            var leads = await query.ToListAsync();
            foreach (var lead in leads)
            {
                if (!GlobalCurrency.IsNullOrEmpty() && GlobalCurrency != lead.CurrencyCode)
                {
                    lead.GlobalCurrencyValue = Math.Round(lead.Value * ConversionRate.Result, 2);
                    lead.GlobalCurrencyCode = GlobalCurrency;
                }
                else if (lead.CurrencyCode != "EUR")
                {
                    var rate = await _crmService.ConvertCurrencyToEURAsync(lead.CurrencyCode);
                    lead.GlobalCurrencyValue = Math.Round(lead.Value * rate, 2);
                    lead.GlobalCurrencyCode = "EUR";
                }

                var teamRequest = await _teamService.GetTeamMembersByTeamIdAsync(lead.TeamId);
                if (teamRequest.IsSuccess)
                {
                    var owner = teamRequest.Result.FirstOrDefault(x => x.TeamRoleId.Equals(TeamResources.Owner));
                    lead.OwnerId = owner?.Id;
                    lead.Owner = owner?.FirstName + ' ' + owner?.LastName;
                }
            }

            var leadsByStage = leads.GroupBy(x => x.Stage.Order + "-" + x.StageName);
            leadsByStage = leadsByStage.OrderBy(x => Convert.ToInt32(x.Key.Split('-')[0]));
            var result = new List<GetGridLeadsViewModel>();

            foreach(var group in leadsByStage)
            {
                var stageLeads = group.OrderBy(x => x.DeadLine).ToList();
                stageLeads.ForEach(x => x.DaysInStage = Convert.ToInt32((DateTime.UtcNow - x.StageChangeDate).TotalDays));
                result.Add(new GetGridLeadsViewModel
                {
                    StageName = group.Key.Split('-')[1],
                    LeadsInStage = stageLeads,
                    StageId = _context.Stages.FirstOrDefault(x => x.Name.Equals(group.Key.Split('-')[1])).Id
                });
            }

            return new SuccessResultModel<IEnumerable<GetGridLeadsViewModel>>(result);
        }

        /// <summary>
        /// Get leads by stage id
        /// </summary>
        /// <param name="stageId"></param>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<GetLeadsViewModel>>> GetLeadsByStageIdAsync(Guid? stageId, bool includeDeleted = false)
        {
            if (stageId == null)
                return new InvalidParametersResultModel<IEnumerable<GetLeadsViewModel>>();

            var leads = await _context.Leads
                .Where(x => x.StageId == stageId && (!x.IsDeleted || includeDeleted))
                .ToListAsync();

            return new SuccessResultModel<IEnumerable<GetLeadsViewModel>>(leads.Adapt<IEnumerable<GetLeadsViewModel>>());
        }

        /// <summary>
        /// Find lead by id
        /// </summary>
        /// <param name="leadId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<Lead>> FindLeadByIdAsync(Guid? leadId)
        {
            if (leadId == null) return new InvalidParametersResultModel<Lead>();
            var lead = await _context
                .BuildLeadsQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == leadId);
            if (lead == null) return new NotFoundResultModel<Lead>();
            return new SuccessResultModel<Lead>(lead);
        }

        /// <summary>
        /// Move lead to another stage
        /// </summary>
        /// <param name="leadId"></param>
        /// <param name="stageId"></param>
        /// <param name="stateId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> MoveLeadToStageAsync(Guid? leadId, Guid? stageId, Guid? stateId, IUrlHelper Url)
        {
            if (stageId == null) return new InvalidParametersResultModel();
            var lead = await _context.Leads.Include(i => i.LeadState)
                .Include(i => i.Team)
                .ThenInclude(i => i.TeamMembers)
                .FirstOrDefaultAsync(x => x.Id == leadId);
            if (lead == null) return new NotFoundResultModel();
            if (lead.StageId == stageId) return new SuccessResultModel<object>().ToBase();

            var lastStageId = lead.StageId;

            var stageRequest = await _crmPipeLineService.FindStageByIdAsync(stageId);
            if (!stageRequest.IsSuccess) return stageRequest.ToBase();
            var stage = stageRequest.Result;

            if (stage.Term != null)
                lead.StageDeadLine = DateTime.Now.AddDays(stage.Term.Value);
            lead.StageId = stage.Id;
            lead.StageChangeDate = DateTime.UtcNow;
            lead.LeadStateId = stateId;

            _context.Leads.Update(lead);
            var result = await _context.PushAsync();

            //get lead owner
            var leadOwner = await _userManager.UserManager.FindByIdAsync(lead.Team.TeamMembers.FirstOrDefault(x => x.TeamRoleId.Equals(TeamResources.Owner))?.UserId.ToString());
            var leadOwnerUsername = leadOwner?.UserName;

            var newStage = await _context.Stages.FindByIdAsync(lead.StageId);
            var lastStage = await _context.Stages.FindByIdAsync(lastStageId);
            bool isProduction = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production";

            //notify owner if stage was changed by someone other than them
            if (result.IsSuccess && leadOwner != null && !leadOwnerUsername.Equals(lead.ModifiedBy))
            {
                await _notify.SendNotificationAsync(new[] { leadOwner.Id.ToGuid() }, new Notification
                {

                    Subject = "Info",
                    Content = $"{lead?.Name} stage was changed from {lastStage?.Name} to {newStage?.Name} by {lead.ModifiedBy}",
                    SendEmail = false,
                    Url = "/leads/details?id=" + lead?.Id
                });
            }

            if (newStage.Name.Equals("Negotiation") && lastStage.Name.Equals("Quotation") && isProduction)
                await _leadNotificationService.NotifyOnStageChange(lead, "Quotation", "Negotiation", Url);

            if (!result.IsSuccess) return result;

            return result;
        }

        /// <summary>
        /// Change lead state
        /// </summary>
        /// <param name="leadId"></param>
        /// <param name="stateId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> ChangeLeadStateAsync(Guid? leadId, Guid? stateId)
        {
            if (leadId == null || stateId == null) return new InvalidParametersResultModel();
            var lead = await _context.Leads
                .Include(i => i.LeadState)
                .Include(i => i.Team)
                .ThenInclude(i => i.TeamMembers)
                .FirstOrDefaultAsync(x => x.Id == leadId);

            if (lead == null) return new NotFoundResultModel();
            var initialStateName = lead.LeadState?.Name;
            if (lead.LeadStateId == stateId) return new SuccessResultModel<object>().ToBase();

            lead.LeadStateId = stateId.Value;

            var stageQuery = await _context.LeadStateStage
                .Where(x => x.StateId.Equals(stateId))
                .Select(x => x.StageId)
                .ToListAsync();

            var stage = await _context.Stages.OrderBy(x => x.Order).FirstOrDefaultAsync(x => x.PipeLineId == lead.PipeLineId && stageQuery.Contains(x.Id));
            var lastStage = await _context.Stages.FindByIdAsync(lead.StageId);

            if (lead.StageId != stage.Id)
            {
                lead.StageId = stage.Id;
                lead.StageChangeDate = DateTime.UtcNow;
            }

            _context.Leads.Update(lead);

            var result = await _context.PushAsync();

            if (!result.IsSuccess) return result;

            //get lead owner
            var leadOwner = await _userManager.UserManager.FindByIdAsync(lead.Team.TeamMembers.FirstOrDefault(x => x.TeamRoleId.Equals(TeamResources.Owner))?.UserId.ToString());

            if(lastStage.Id != lead.StageId) 
            {
                var newStage = await _context.Stages.FindByIdAsync(lead.StageId);
                await _notify.SendNotificationAsync(new[] { leadOwner.Id.ToGuid() }, new Notification
                {

                    Subject = "Info",
                    Content = $"{lead?.Name} stage was changed from {lastStage?.Name} to {newStage?.Name} by {lead.ModifiedBy}",
                    SendEmail = false,
                    Url = "/leads/details?id=" + lead?.Id
                });
            }
           
            var newState = await _context.States.FindByIdAsync(stateId);
            return result;
        }


        /// <summary>
        /// Disable lead
        /// </summary>
        /// <param name="leadId"></param>
        /// <returns></returns>
        public async Task<ResultModel> DisableLeadAsync(Guid? leadId)
            => await _context.DisableRecordAsync<Lead>(leadId);

        /// <summary>
        /// Activate lead
        /// </summary>
        /// <param name="leadId"></param>
        /// <returns></returns>
        public async Task<ResultModel> ActivateLeadAsync(Guid? leadId)
            => await _context.ActivateRecordAsync<Lead>(leadId);


        /// <summary>
        /// Delete lead
        /// </summary>
        /// <param name="leadId"></param>
        /// <returns></returns>
        public async Task<ResultModel> DeleteLeadAsync(Guid? leadId)
            => await _context.RemovePermanentRecordAsync<Lead>(leadId);

        /// <summary>
        /// UploadFile
        /// </summary>
        /// <param name="projectNumber"></param>
        /// <param name="activityNumber"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<ResultModel> UploadFile(Guid LeadId, IFormFile file)
        {
            var fileSize = 60 * 1024 * 1024;
            if (file == null)
                return new InvalidParametersResultModel();
            if (file.Length > fileSize)
                return new ResultModel { IsSuccess = false, Errors = new List<IErrorModel> { new ErrorModel { Message = "Maximum file size was exceeded!" } } };
            var user = await _userManager.GetCurrentUserAsync();

            await RefreshUserToken(user.Result.Id.ToGuid(), ExternalProviders.OneDrive,
                _optionsMonitor.CurrentValue.ClientId,
                _optionsMonitor.CurrentValue.ReturnUrl,
                _optionsMonitor.CurrentValue.ClientSecret);

            var userAccessToken = await _userTokenDataService.GetUserAccessToken(user.Result.Id.ToGuid(), ExternalProviders.OneDrive);

            if (userAccessToken.IsNullOrEmpty())
                return new ResultModel { IsSuccess = false, Errors = new List<IErrorModel> { new ErrorModel { Message = "Could not get user refresh token!" } } };

            var lead = await _context.Leads.FirstOrDefaultAsync(i => i.Id == LeadId);

            if (lead == null)
                return new NotFoundResultModel();

            var uploadFileResult = await new OneDriveService(_optionsMonitor, _logger)
                .UploadFile(new CloudStorageApiRequestModel
                {
                    AccessToken = userAccessToken,
                    Path = "drive/special/approot:/Crm/" + "Leads/" + $"L{lead.Number.ToString()}" + "/Files",
                    ElementName = file.FileName,
                    File = file
                });

            if (uploadFileResult.IsSuccessStatusCode)
                return new ResultModel { IsSuccess = true };

            return new ResultModel { IsSuccess = false };
        }


        /// <summary>
        /// Delete file
        /// </summary>
        /// <param name="id"></param>
        /// <param name="paths"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task<ResultModel> DeleteFile(string id, string path, string fileName)
        {
            var user = await _userManager.GetCurrentUserAsync();
            await RefreshUserToken(user.Result.Id.ToGuid(), ExternalProviders.OneDrive,
                _optionsMonitor.CurrentValue.ClientId,
                _optionsMonitor.CurrentValue.ReturnUrl,
                _optionsMonitor.CurrentValue.ClientSecret);

            var userAccessToken = await _userTokenDataService.GetUserAccessToken(user.Result.Id.ToGuid(), ExternalProviders.OneDrive);

            if (userAccessToken.IsNullOrEmpty())
                return new ResultModel { IsSuccess = false, Errors = new List<IErrorModel> { new ErrorModel { Message = "Could not get user refresh token!" } } };

            try
            {
                var hasOperationCompletedWithSuccess = false;


                var element = new CloudStorageApiRequestModel
                {
                    Id = id,
                    AccessToken = userAccessToken,
                    ElementName = fileName,
                    Path = "drive/special/approot:/" + path
                };
                hasOperationCompletedWithSuccess |= (await new OneDriveService(_optionsMonitor, _logger).DeleteElement(element)).IsSuccessStatusCode;

                if (hasOperationCompletedWithSuccess)
                    return new ResultModel { IsSuccess = true };
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Catch this: " + e.Message);
            }

            return new ResultModel { IsSuccess = false };
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="leadId"></param>
        /// <param name="changeRequestId"></param>
        /// <returns></returns>
        public async Task<ResultModel<List<FileViewModel>>> GetFiles(Guid? leadId)
        {
            if (leadId == null)
                return new InvalidParametersResultModel<List<FileViewModel>>();
            var lead = await _context.Leads.FirstOrDefaultAsync(x => x.Id == leadId);

            if (lead == null)
                return new NotFoundResultModel<List<FileViewModel>>();
            var user = await _userManager.GetCurrentUserAsync();
            await RefreshUserToken(user.Result.Id.ToGuid(), ExternalProviders.OneDrive,
                _optionsMonitor.CurrentValue.ClientId,
                _optionsMonitor.CurrentValue.ReturnUrl,
                _optionsMonitor.CurrentValue.ClientSecret);
            var userAccessToken = await _userTokenDataService.GetUserAccessToken(user.Result.Id.ToGuid(), ExternalProviders.OneDrive);

            try
            {
                var leadFiles = await new OneDriveService(_optionsMonitor, _logger).GetChildren(

                    new CloudStorageApiRequestModel
                    {
                        AccessToken = userAccessToken,
                        Path = "drive/special/approot:/Crm/" + "Leads/" + $"L{lead.Number.ToString()}" + "/Files"
                    });

                return new ResultModel<List<FileViewModel>> { IsSuccess = true, Result = _mapper.Map<List<FileViewModel>>(leadFiles) };
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Catch this: " + e.Message);
            }
        }

        #endregion

        #region LeadState

        /// <summary>
        /// Add lead state
        /// </summary>
        /// <param name="name"></param>
        /// <param name="styleClass"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public async Task<ResultModel<Guid>> AddLeadStateAsync([Required] string name, string styleClass, string description)
        {
            if (name.IsNullOrEmpty()) return new InvalidParametersResultModel<Guid>("Name must be not null");
            var order = await _context.States.CountAsync();
            var leadState = new LeadState
            {
                Name = name,
                Order = order + 1,
                StateStyleClass = styleClass,
                Description = description
            };

            await _context.States.AddAsync(leadState);
            var dbResult = await _context.PushAsync();
            return dbResult.IsSuccess ? new SuccessResultModel<Guid>(leadState.Id)
                : dbResult.Map(Guid.Empty);
        }

        /// <summary>
        /// Order lead states
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<ResultModel> OrderLeadStateAsync(IEnumerable<OrderLeadStatesViewModel> data)
        {
            foreach (var stateOrder in data)
            {
                var state = await _context.States.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id.Equals(stateOrder.StateId));
                if (state == null) continue;
                state.Order = stateOrder.Order;
                _context.States.Update(state);
            }

            return await _context.PushAsync();
        }

        /// <summary>
        /// Get all lead states
        /// </summary>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        public async Task<ResultModel<IEnumerable<LeadState>>> GetAllLeadStatesAsync(bool includeDeleted = false)
        {
            var data = await _context.States
                .Where(x => !x.IsDeleted || includeDeleted)
                .OrderBy(x => x.Order)
                .ToListAsync();

            return new SuccessResultModel<IEnumerable<LeadState>>(data);
        }

        /// <summary>
        /// Get all lead states by stage
        /// </summary>
        /// <param name="includeDeleted"></param>
        /// <param name="stageId"></param>
        /// <returns></returns>
        public async Task<ResultModel<IEnumerable<LeadState>>> GetAllLeadStatesByStageAsync(Guid stageId, bool includeDeleted = false)
        {
            var data = await _context.States
                .Include(x => x.Stages)
                .Where(x => !x.IsDeleted || includeDeleted)
                .Where(x => x.Stages.Select(y => y.StageId).Contains(stageId))
                .OrderBy(x => x.Order)
                .ToListAsync();

            return new SuccessResultModel<IEnumerable<LeadState>>(data);
        }

        /// <summary>
        /// Get lead state by id
        /// </summary>
        /// <param name="stateId"></param>
        /// <returns></returns>
        public async Task<ResultModel<LeadState>> GetLeadStateByIdAsync(Guid? stateId)
        {
            if (stateId == null)
                return new InvalidParametersResultModel<LeadState>();

            var data = await _context.States
                .FirstOrDefaultAsync(x => x.Id.Equals(stateId));

            if (data == null)
                return new NotFoundResultModel<LeadState>();

            return new SuccessResultModel<LeadState>(data);
        }

        /// <summary>
        /// Rename lead state
        /// </summary>
        /// <param name="leadStateId"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="styleClass"></param>
        /// <returns></returns>
        public async Task<ResultModel> UpdateLeadStateAsync(Guid? leadStateId, string name, string description, string styleClass)
        {
            if (leadStateId == null || name.IsNullOrEmpty())
                return new InvalidParametersResultModel();

            var leadState = await _context.States.FindByIdAsync(leadStateId);
            if (leadState == null) return new NotFoundResultModel();

            if (leadState.IsSystem)
                return new ResultModel { IsSuccess = false, Errors = new List<IErrorModel> { new ErrorModel { Message = "Is system state" } } };

            leadState.Name = name;
            leadState.Description = description;
            leadState.StateStyleClass = styleClass;
            _context.States.Update(leadState);
            return await _context.PushAsync();
        }

        /// <summary>
        /// Disable lead state
        /// </summary>
        /// <param name="leadStateId"></param>
        /// <returns></returns>
        public async Task<ResultModel> DisableLeadStateAsync(Guid? leadStateId)
        {
            var leadState = await _context.States.FindByIdAsync(leadStateId);
            if (leadState == null) return new NotFoundResultModel();

            if (leadState.IsSystem)
                return new ResultModel { IsSuccess = false, Errors = new List<IErrorModel> { new ErrorModel { Message = "Is system state" } } };

            leadState.IsDeleted = true;
            _context.States.Update(leadState);
            return await _context.PushAsync();
        }


        /// <summary>
        /// Activate lead state
        /// </summary>
        /// <param name="leadStateId"></param>
        /// <returns></returns>
        public async Task<ResultModel> ActivateLeadStateAsync(Guid? leadStateId)
            => await _context.DisableRecordAsync<LeadState>(leadStateId);

        /// <summary>
        /// Remove lead state
        /// </summary>
        /// <param name="leadStateId"></param>
        /// <returns></returns>
        public async Task<ResultModel> RemoveLeadStateAsync(Guid? leadStateId)
        {
            var leadState = await _context.States.FindByIdAsync(leadStateId);
            if (leadState == null) return new NotFoundResultModel();

            if (leadState.IsSystem)
                return new ResultModel { IsSuccess = false, Errors = new List<IErrorModel> { new ErrorModel { Message = "Is system state" } } };

            _context.States.Remove(leadState);
            return await _context.PushAsync();
        }



        /// <summary>
        /// Seed lead state 
        /// </summary>
        /// <returns></returns>
        public virtual async Task SeedSystemLeadState()
        {
            var path = Path.Combine(AppContext.BaseDirectory, "Configuration/SystemLeadState.json");
            var items = JsonParser.ReadArrayDataFromJsonFile<ICollection<LeadState>>(path);

            if (items == null)
                return;

            if (items.Any())
            {
                await _context.States.AddRangeAsync(items);
                await _context.PushAsync();
            }
        }

        #endregion

        #region Teams

        /// <summary>
        /// Set team for lead
        /// </summary>
        /// <param name="lead"></param>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> SetTeamForLeadAsync(Lead lead, Guid? teamId)
        {
            if (lead == null || teamId == null) return new InvalidParametersResultModel();
            lead.TeamId = teamId;
            _context.Leads.Update(lead);
            return await _context.PushAsync();
        }

        /// <summary>
        /// Get leads count by organization
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public async Task<ResultModel<int>> GetLeadsCountByOrganizationAsync(Guid? organizationId)
        {
            if (organizationId == null) return new InvalidParametersResultModel<int>();
            var count = await _context.Leads.Where(x => x.IsDeleted == false)
                .CountAsync(x => x.OrganizationId.Equals(organizationId));

            return new SuccessResultModel<int>(count);
        }

        /// <summary>
        /// Set owner for lead
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="leadId"></param>
        /// <param name="listMembersId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<Guid>> SetLeadOwnerAsync(Guid? userId, Guid? leadId, IEnumerable<Guid> listMembersId, IUrlHelper Url)
        {
            if (userId == null || leadId == null) return new InvalidParametersResultModel<Guid>();
            var leadRequest = await FindLeadByIdAsync(leadId);

            if (!leadRequest.IsSuccess)
                return new ResultModel<Guid> { IsSuccess = leadRequest.IsSuccess, Errors = leadRequest.Errors };


            var listUsersIdAssignedToLead = listMembersId.ToList();
            listUsersIdAssignedToLead.Add(userId.Value);

            var lead = leadRequest.Result;
            if (lead.HasTeam())
            {
                var currentOwner = lead.Team.TeamMembers
                    .FirstOrDefault(x => x.TeamRoleId.Equals(TeamResources.Owner));

                if (currentOwner != null)
                {
                    await _teamService.DeleteMemberToTeamAsync(currentOwner.Id);
                }

                var members = lead.Team.TeamMembers.Where(x => x.TeamRoleId.Equals(TeamResources.Member)).ToList();

                if (members.Any())
                    foreach (var member in members)
                    {
                        await _teamService.DeleteMemberToTeamAsync(member.Id);
                    }

                var addResult = await _teamService.AddNewMemberToTeamAsync(new TeamMemberViewModel
                {
                    TeamRoleId = TeamResources.Owner,
                    TeamId = lead.TeamId.GetValueOrDefault(),
                    UserId = userId.Value
                });

                if (listMembersId == null || !listMembersId.Any()) return addResult;

                foreach (var memberId in listMembersId)
                {
                    await _teamService.AddNewMemberToTeamAsync(new TeamMemberViewModel
                    {
                        TeamRoleId = TeamResources.Member,
                        TeamId = lead.TeamId.GetValueOrDefault(),
                        UserId = memberId
                    });
                }

                /*if (addResult.IsSuccess)
                {
                    var notification = new Notification
                    {
                        Content = $"Update lead {lead?.Name} ",
                        Subject = "Info",
                        NotificationTypeId = NotificationType.Info
                    };

                    await _notify.SendNotificationAsync(listUsersIdAssignedToLead, notification);
                    await _notify.SendNotificationToSystemAdminsAsync(notification);
                }*/

                return addResult;
            }

            var addTeamRequest = await _teamService.AddTeamAsync(new AddTeamViewModel
            {
                Name = $"{lead.Name} Team"
            });

            if (!addTeamRequest.IsSuccess) return addTeamRequest.Map<Guid>();
            var teamId = (Guid)addTeamRequest.Result;
            var mapTeamToLead = await SetTeamForLeadAsync(lead, teamId);
            if (!mapTeamToLead.IsSuccess)
            {
                //remove team
                return mapTeamToLead.Map<Guid>();
            }

            var addMemberResult = await _teamService.AddNewMemberToTeamAsync(new TeamMemberViewModel
            {
                TeamRoleId = TeamResources.Owner,
                TeamId = teamId,
                UserId = userId.Value
            });


            if (listMembersId == null || !listMembersId.Any()) return !addMemberResult.IsSuccess ? addMemberResult.Map<Guid>()
                : addMemberResult.Map(addMemberResult.Result);

            foreach (var memberId in listMembersId)
            {
                await _teamService.AddNewMemberToTeamAsync(new TeamMemberViewModel
                {
                    TeamRoleId = TeamResources.Member,
                    TeamId = lead.TeamId.GetValueOrDefault(),
                    UserId = memberId
                });
            }


            if (addMemberResult.IsSuccess)
            {
                await _notify.SendNotificationAsync(listUsersIdAssignedToLead, new Notification
                {

                    Subject = "Info",
                    Content = $"New opportunity \"{lead?.Name}\" has been created for you by {lead?.Author}",
                    SendEmail = false,
                    Url = "/leads/details?id=" + lead?.Id
                });

                await _leadNotificationService.NotifyOnLeadAdd(listUsersIdAssignedToLead, lead, Url);
                //await _notify.SendNotificationToSystemAdminsAsync(notification);
            }



            return !addMemberResult.IsSuccess ? addMemberResult.Map<Guid>()
                : addMemberResult.Map(addMemberResult.Result);
        }

        #endregion


        public async Task<ResultModel<Guid>> AddNoGoStateAsync(AddNoGoStateViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel<Guid>();

            var noGoState = _mapper.Map<NoGoState>(model);

            _context.NoGoStates.Add(noGoState);

            var result = await _context.PushAsync();

            return result.Map(noGoState.Id);
        }

        public async Task<ResultModel> UpdateNoGoStateAsync(UpdateNoGoStateViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel();

            var noGoState = await _context.NoGoStates.FirstOrDefaultAsync(x => x.Id == model.State.Id);

            if (noGoState == null)
                return new NotFoundResultModel();

            noGoState.Name = model.State.Name;

            _context.NoGoStates.Update(noGoState);

            var result = await _context.PushAsync();

            /*if (model.SelectedLeads.Count > 0)
            {
                var res = await RemoveLeadsFromNoGoStateAsync(model.SelectedLeads);
                if (!res.IsSuccess)
                {
                    result.IsSuccess = false;
                    result.Errors.Concat(res.Errors);
                }
            }*/

            return result;
        }

        public async Task<ResultModel<NoGoStateViewModel>> GetNoGoStateByIdAsync(Guid? Id)
        {
            if (Id == null)
                return new InvalidParametersResultModel<NoGoStateViewModel>();

            var noGoState = await _context.NoGoStates.Include(i => i.Leads).FirstOrDefaultAsync(x => x.Id == Id);

            if (noGoState == null)
                return new NotFoundResultModel<NoGoStateViewModel>();

            return new SuccessResultModel<NoGoStateViewModel> { Result = _mapper.Map<NoGoStateViewModel>(noGoState) };
        }

        public async Task<ResultModel> ActivateNoGoStateAsync(Guid? Id) =>
            await _context.ActivateRecordAsync<NoGoState>(Id);
        public async Task<ResultModel> DisableNoGoStateAsync(Guid? Id) =>
            await _context.DisableRecordAsync<NoGoState>(Id);

        public async Task<ResultModel> DeleteNoGoStateAsync(Guid? Id) =>
            await _context.RemovePermanentRecordAsync<NoGoState>(Id);

        public async Task<ResultModel<IEnumerable<NoGoStateViewModel>>> GetAllNoGoStatesAsync()
        {
            var noGoStates = _context.NoGoStates.Where(x => !x.IsDeleted);

            return new SuccessResultModel<IEnumerable<NoGoStateViewModel>> { Result = _mapper.Map<IEnumerable<NoGoStateViewModel>>(noGoStates) };
        }


        public async Task<ResultModel<PagedResult<NoGoStateViewModel>>> GetAllPaginatedNoGoStatesAsync(PageRequest request)
        {
            var query = _context.NoGoStates
                .Where(x => !x.IsDeleted || request.IncludeDeleted);

            var pagedResult = await query.GetPagedAsync(request);
            var map = pagedResult.Map(_mapper.Map<IEnumerable<NoGoStateViewModel>>(pagedResult.Result));

            return new SuccessResultModel<PagedResult<NoGoStateViewModel>>(map);
        }


        /*public async Task<ResultModel> AddLeadToNoGoStateAsync(LeadToNoGoStateViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel();

            var lead = await _context.Leads.FirstOrDefaultAsync(x => x.Id == model.LeadId.Value);

            if (lead == null)
                return new NotFoundResultModel();

            var noGoState = await _context.NoGoStates.FirstOrDefaultAsync(x => x.Id == model.NoGoStateId.Value);

            if (noGoState == null)
                return new NotFoundResultModel();

            lead.NoGoStateId = noGoState.Id;
            _context.Leads.Update(lead);
            var result = await _context.PushAsync();

            return result;
        }*/

        /*public async Task<ResultModel<IEnumerable<GetLeadsViewModel>>> GetLeadsByNoGoStateIdAsync(Guid? Id)
        {
            if (Id == null)
                return new InvalidParametersResultModel<IEnumerable<GetLeadsViewModel>>();

            var leads = _context.Leads.Where(x => x.NoGoStateId == Id && !x.IsDeleted);


            return new SuccessResultModel<IEnumerable<GetLeadsViewModel>> { Result = _mapper.Map<IEnumerable<GetLeadsViewModel>>(leads) };
        }*/


        /*public async Task<ResultModel> RemoveLeadsFromNoGoStateAsync(List<Guid> LeadsIds)
        {
            if (LeadsIds.Count > 0)
            {
                var leads = _context.Leads.Where(x => LeadsIds.Contains(x.Id));

                foreach (var lead in leads)
                {
                    lead.NoGoStateId = null;
                }

                _context.Leads.UpdateRange(leads);

                var result = await _context.PushAsync();

                return result;
            }

            return new ResultModel { IsSuccess = true };
        }*/

        public async Task<ResultModel> AddProductOrServicesAsync(List<AddProductOrServiceViewModel> model)
        {
            var prodServ = _mapper.Map<List<ProductOrServiceList>>(model);
            _context.ProductOrServiceLists.AddRange(prodServ);
            var result = await _context.PushAsync();
            return result;
        }

        public async Task<ResultModel> DeleteProductOrServicesAsync(Guid Id)
        {
            var productOrService = await _context.ProductOrServiceLists.FirstOrDefaultAsync(x => x.Id == Id);
            if (productOrService == null)
                return new NotFoundResultModel();
            _context.ProductOrServiceLists.Remove(productOrService);
            return await _context.PushAsync();
        }

        public async Task<ResultModel> UpdateProductOrServicesAsync(List<UpdateProductOrServiceViewModel> model)
        {

            foreach (var prodOrServ in model)
            {
                var productOrService = await _context.ProductOrServiceLists.FirstOrDefaultAsync(x => x.Id == prodOrServ.Id);
                if (productOrService != null)
                {

                    productOrService.ProductOrServiceId = prodOrServ.ProductOrServiceId;

                    productOrService.ProductTypeId = prodOrServ.ProductTypeId;

                    productOrService.TechnologyTypeId = prodOrServ.TechnologyTypeId;

                    productOrService.ServiceTypeId = prodOrServ.ServiceTypeId;

                    productOrService.DevelopmentVariationId = prodOrServ.DevelopmentVariationId;

                    productOrService.ConsultancyVariationId = prodOrServ.ConsultancyVariationId;

                    productOrService.QAVariationId = prodOrServ.QAVariationId;

                    productOrService.DesignVariationId = prodOrServ.DesignVariationId;

                    productOrService.DevelopementFrameworkId = prodOrServ.DevelopementFrameworkId;

                    productOrService.PMFrameworkId = prodOrServ.PMFrameworkId;
                    _context.ProductOrServiceLists.Update(productOrService);
                }
            }
            return await _context.PushAsync();
        }

        #region Helper
        private async Task RefreshUserToken(Guid userId, ExternalProviders provider, string clientId,
                string redirectUri, string clientSecret)
        {
            var dict = new Dictionary<string, string>
            {
                {"client_id", clientId},
                {"redirect_uri", redirectUri},
                {"client_secret", clientSecret},
                {"refresh_token", await _userTokenDataService.GetUserRefreshToken(userId,provider)},
                {"grant_type", "refresh_token"}
            };

            var client = new HttpClient();
            dict.TryGetValue("refresh_token", out var refreshToken);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var postAction = await client.PostAsync("https://login.microsoftonline.com/common/oauth2/v2.0/token",
                new FormUrlEncodedContent(dict));
            var result = JsonConvert.DeserializeObject<CloudLoginModel>(await postAction.Content.ReadAsStringAsync());
            await _userTokenDataService.SetUpUserToken(result.AccessToken, refreshToken, userId, provider);
        }

 
        #endregion


    }


}


