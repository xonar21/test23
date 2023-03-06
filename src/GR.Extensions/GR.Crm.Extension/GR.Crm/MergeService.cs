using DocumentFormat.OpenXml.Spreadsheet;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.Core.Helpers.Responses;
using GR.Crm.Abstractions;
using GR.Crm.Abstractions.ViewModels.MergeViewModels;
using GR.Crm.Leads.Abstractions;
using GR.Crm.Leads.Abstractions.Models;
using GR.Crm.Leads.Abstractions.ViewModels;
using GR.Crm.Organizations.Abstractions;
using GR.Crm.Organizations.Abstractions.Models;
using GR.Crm.Organizations.Abstractions.ViewModels.ContactsViewModels;
using GR.Crm.Organizations.Abstractions.ViewModels.OrganizationsViewModels;
using GR.Crm.Teams.Abstractions;
using GR.Crm.Teams.Abstractions.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Remotion.Linq.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GR.Crm
{
    public class MergeService : ICrmMergeService
    {

        #region Injectable
        /// <summary>
        /// Inject ILeadServices
        /// </summary>
        private readonly ILeadService<Lead> _leadService;


        /// <summary>
        /// Inject organization context
        /// </summary>
        private readonly ICrmOrganizationContext _organizationContext;


        /// <summary>
        /// Inject organization context
        /// </summary>
        private readonly ILeadContext<Lead> _leadContext;


        /// <summary>
        /// Inject crm organization
        /// </summary>
        private readonly ICrmOrganizationService _organizationService;


        /// <summary>
        /// Inject team context
        /// </summary>
        private readonly ICrmTeamContext _teamContext;


        /// <summary>
        /// Inject crm contact
        /// </summary>
        private readonly ICrmContactService _contactService;


        #endregion

        public MergeService(ILeadService<Lead> leadService, 
                            ICrmOrganizationContext organizationContext,
                            ILeadContext<Lead> leadContext,
                            ICrmOrganizationService organizationService,
                            ICrmContactService contactService,
                            ICrmTeamContext teamContext)
        {
            _leadService = leadService;
            _organizationContext = organizationContext;
            _leadContext = leadContext;
            _organizationService = organizationService;
            _contactService = contactService;
            _teamContext = teamContext;
        }


        /// <summary>
        /// Merge Contacts
        /// </summary>
        /// <param name="mergeContactsViewModel"></param>
        /// <returns></returns>
        public async Task<ResultModel> MergeContactsAsync(MergeContactsViewModel mergeContactsViewModel)
        {
            var contact = await _organizationContext.Contacts
                                .FirstOrDefaultAsync(x => x.Id == mergeContactsViewModel.SourceContact);
            if (contact == null)
                return new NotFoundResultModel();

            var toResult = new List<ResultModel>();
            var updatedContact = new ContactViewModel
            {
                Id = contact.Id,
                OrganizationId = mergeContactsViewModel.SourceOrganization,
                Email = mergeContactsViewModel.SourceEmail,
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                Description = contact.Description,
                JobPositionId = mergeContactsViewModel.SourceJobPostion != null ? mergeContactsViewModel.SourceJobPostion : contact.JobPositionId 
            };
            await _contactService.UpdateContactAsync(updatedContact);

            foreach (var remainConact in mergeContactsViewModel.RemainingContacts)
            {
                toResult.Add(await _contactService.DeactivateContactAsync(remainConact));
            }

            var resultFail = new ResultModel { IsSuccess = false };
            foreach (var result in toResult)
            {
                if (result.IsSuccess == false)
                {
                    resultFail.Errors.Add((IErrorModel)result.Errors);
                }
            }

            if (resultFail.Errors.Count > 0) return resultFail;

            return new ResultModel { IsSuccess = true };
        
        }

        /// <summary>
        /// Merge Leads
        /// </summary>
        /// <param name="mergeLeadsViewModel"></param>
        /// <returns></returns>
        public async Task<ResultModel> MergeLeadsAsync(MergeLeadsViewModel mergeLeadsViewModel, IUrlHelper Url)
        {
            var lead = await _leadContext.Leads.FirstOrDefaultAsync(i => i.Id == Guid.Parse(mergeLeadsViewModel.SourceLeadId));
            if (lead == null)
                return new NotFoundResultModel();
            var toResult = new List<ResultModel>();

            if (mergeLeadsViewModel.TeamId != null)
            {
                var teamId = Guid.Parse(mergeLeadsViewModel.TeamId);
                lead.TeamId = teamId;
                _leadContext.Leads.Update(lead);
                toResult.Add(await _leadContext.PushAsync());

                if(mergeLeadsViewModel.LeadMembersIds.Count > 0)
                {
                    var members = new List<TeamMember>();
                    foreach(var member in mergeLeadsViewModel.LeadMembersIds)
                    {
                        var teamMember = await _teamContext.TeamMembers.FirstOrDefaultAsync(x => x.Id == Guid.Parse(member));
                        if(teamMember != null)
                        {
                            teamMember.TeamId = teamId;
                            members.Add(teamMember);
                        }
                    }
                    _teamContext.TeamMembers.UpdateRange(members);
                    toResult.Add(await _teamContext.PushAsync());
                }
                if(mergeLeadsViewModel.OwnerId != null)
                {
                    var teamMember = await _teamContext.TeamMembers.FirstOrDefaultAsync(x => x.Id == Guid.Parse(mergeLeadsViewModel.OwnerId));
                    teamMember.TeamId = teamId;
                    _teamContext.TeamMembers.Update(teamMember);
                    toResult.Add(await _teamContext.PushAsync());
                }
            }
            
            var updateLead = new UpdateLeadViewModel
            {
                Id = lead.Id,
                Created = lead.Created,
                Name = lead.Name,
                OrganizationId = mergeLeadsViewModel.OrganizationId != null ? Guid.Parse(mergeLeadsViewModel.OrganizationId) : lead.OrganizationId,
                Value = lead.Value,
                CurrencyCode = lead.CurrencyCode,
                DeadLine = lead.DeadLine,
                LeadStateId = mergeLeadsViewModel.LeadStateId != null ? Guid.Parse(mergeLeadsViewModel.LeadStateId) : lead.LeadStateId,
                StageId = lead.StageId,
                ClarificationDeadline = lead.ClarificationDeadline,
                //ContactId = mergeLeadsViewModel.ContactId != null ? Guid.Parse(mergeLeadsViewModel.ContactId) : lead.ContactId,
                SourceId = mergeLeadsViewModel.SourceId != null ? Guid.Parse(mergeLeadsViewModel.SourceId) : lead.SourceId,
                Description = lead.Description
            };

            toResult.Add(await _leadService.UpdateLeadAsync(updateLead, Url));

            foreach (var remainingLead in mergeLeadsViewModel.RemainingLeadsIds)
            {
                toResult.Add(await _leadService.DisableLeadAsync(Guid.Parse(remainingLead)));
            }

            var resultFail = new ResultModel { IsSuccess = false };
            foreach (var result in toResult)
            {
                if (result.IsSuccess == false)
                {
                    resultFail.Errors.Add((IErrorModel)result.Errors);
                }
            }

            if (resultFail.Errors.Count > 0) return resultFail;

            return new ResultModel { IsSuccess = true };
        }


        /// <summary>
        /// Merge Organizations
        /// </summary>
        /// <param name="mergeOrganizationsViewModel"></param>
        /// <returns></returns>
        public async Task<ResultModel> MergeOrganizationsAsync(MergeOrganizationsViewModel mergeOrganizationsViewModel)
        {
            var organization = await _organizationContext.Organizations.FirstOrDefaultAsync(x => x.Id == mergeOrganizationsViewModel.TargetOrganization);
            var toResult = new List<ResultModel>();
            if (organization == null)
                return new NotFoundResultModel();

            if (mergeOrganizationsViewModel.SourceContacts != null)
            {
                var contactToBeAdded = new List<Contact>();
                foreach (var contactId in mergeOrganizationsViewModel.SourceContacts)
                {
                    var contact = await _organizationContext.Contacts.FirstOrDefaultAsync(x => x.Id == contactId);
                    if (contact != null)
                    {
                        contact.OrganizationId = mergeOrganizationsViewModel.TargetOrganization;
                        contactToBeAdded.Add(contact);
                    }
                }
                if (contactToBeAdded.Count > 0)
                {
                    _organizationContext.Contacts.UpdateRange(contactToBeAdded);
                    toResult.Add(await _organizationContext.PushAsync());
                }
            }

            if (mergeOrganizationsViewModel.SourceAddrress != null)
            {
                var addressesToBeAdded = new List<OrganizationAddress>();
                foreach (var addressId in mergeOrganizationsViewModel.SourceAddrress)
                {
                    var address = await _organizationContext.OrganizationAddresses.FirstOrDefaultAsync(x => x.Id == addressId);
                    if (address != null)
                    {
                        address.OrganizationId = mergeOrganizationsViewModel.TargetOrganization;
                        addressesToBeAdded.Add(address);
                    }
                }
                if (addressesToBeAdded.Count > 0)
                {
                    _organizationContext.OrganizationAddresses.UpdateRange(addressesToBeAdded);
                    toResult.Add(await _organizationContext.PushAsync());
                }

            }

            if (mergeOrganizationsViewModel.SourceLeads != null)
            {
                var leadsToBeAdded = new List<Lead>();
                foreach (var leadId in mergeOrganizationsViewModel.SourceLeads)
                {
                    var lead = await _leadContext.Leads.FirstOrDefaultAsync(x => x.Id == leadId);
                    if (lead != null)
                    {
                        lead.OrganizationId = mergeOrganizationsViewModel.TargetOrganization;
                        leadsToBeAdded.Add(lead);
                    }
                }
                if (leadsToBeAdded.Count > 0)
                {
                    _leadContext.Leads.UpdateRange(leadsToBeAdded);
                    toResult.Add(await _leadContext.PushAsync());
                }
            }

            switch (mergeOrganizationsViewModel.TargetAction)
            {
                case "delete":
                    foreach (var org in mergeOrganizationsViewModel.RemainingOrgs)
                    {
                        toResult.Add(await _organizationService.DeleteOrganizationAsync(org));
                    }
                    break;
                case "deactivate":
                    foreach (var org in mergeOrganizationsViewModel.RemainingOrgs)
                    {
                        toResult.Add(await _organizationService.DeactivateOrganizationAsync(org));
                    }
                    break;
                default:
                    return new NotFoundResultModel();
            }
            var resultFail = new ResultModel { IsSuccess = false };
            foreach (var result in toResult)
            {
                if (result.IsSuccess == false)
                {
                    resultFail.Errors.Add((IErrorModel)result.Errors);
                }
            }

            if (resultFail.Errors.Count > 0) return resultFail;

            return new ResultModel { IsSuccess = true };
        }
    }
}
