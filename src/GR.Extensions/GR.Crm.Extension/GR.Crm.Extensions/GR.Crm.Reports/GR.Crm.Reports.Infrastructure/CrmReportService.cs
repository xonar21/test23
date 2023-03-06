using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspose.Cells;
using AutoMapper;
using ClosedXML.Excel;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Core.Helpers.Responses;
using GR.Crm.Abstractions;
using GR.Crm.Leads.Abstractions;
using GR.Crm.Leads.Abstractions.Models;
using GR.Crm.Leads.Abstractions.ViewModels;
using GR.Crm.Payments.Abstractions;
using GR.Crm.Reports.Abstraction;
using GR.Crm.Reports.Abstraction.ViewModels.AgreementViewModels;
using GR.Crm.Reports.Abstraction.ViewModels.LeadReportViewModels;
using GR.Crm.Reports.Abstraction.ViewModels.PaymentReportViewModel;
using GR.Crm.Teams.Abstractions.Helpers;
using GR.Identity.Abstractions;
using GR.TaskManager.Abstractions;
using GR.TaskManager.Abstractions.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace GR.Crm.Reports.Infrastructure
{
    public class CrmReportService : ICrmReportService
    {

        #region Inject

        ///// <summary>
        ///// Inject context
        ///// </summary>
        //private readonly ICrmReportContext _context;

        /// <summary>
        /// Inject user manager
        /// </summary>
        private readonly IUserManager<GearUser> _userManager;

        /// <summary>
        /// Inject task context
        /// </summary>
        private readonly ITaskManagerContext _taskManagerContext;

        /// <summary>
        /// Inject Lead context
        /// </summary>
        private readonly ILeadContext<Lead> _leadContext;

        /// <summary>
        /// Inject Payment context
        /// </summary>
        private readonly IPaymentContext _paymentContext;


        /// <summary>
        /// InjectMapper
        /// </summary>
        private readonly IMapper _mapper;

        #endregion

        public CrmReportService(
            IUserManager<GearUser> userManager,
            ITaskManagerContext taskManagerContext,
            ILeadContext<Lead> leadContext, IPaymentContext paymentContext,
            IMapper mapper,
            ICrmService crmService,
            IConfiguration configuration)
        {
            //_context = context;
            _userManager = userManager;
            _taskManagerContext = taskManagerContext;
            _leadContext = leadContext;
            _paymentContext = paymentContext;
            _mapper = mapper;
            GlobalCurrency = configuration.GetSection("GeneralConfigurations").GetSection("CurrencyCode").Value;
            ConversionRate = crmService.ConvertCurrencyToDefaultCurrencyAsync("EUR");
        }

        private string GlobalCurrency { get; set; }

        private Task<decimal> ConversionRate { get; set; }

        #region Lead Report

        /// <summary>
        /// Lead report
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<ReportLeadViewModel>>> LeadReportAsync(IEnumerable<PageRequestFilter> filters, IEnumerable<string> listGroupProperties)
        {
            var pageRequestFilters = filters.ToList();

            var query = _leadContext.Leads
                .Include(x => x.Organization)
                .Include(x => x.Team)
                .ThenInclude(x => x.TeamMembers)
                .ThenInclude(x => x.TeamRole)
                .Include(x => x.PipeLine)
                .SetPeriodByProperty(pageRequestFilters, "Created");

            #region FilterByOwner


            var listOwnerId = pageRequestFilters
                .Where(x => string.Equals(x.Propriety.Trim(), "Owner".Trim(), StringComparison.CurrentCultureIgnoreCase))
                .Select(s => s.Value)
                .ToList();

            if (listOwnerId.Any())
            {
                var listTeamId = await query.SelectMany(s => s.Team.TeamMembers)
                    .Where(x => x.TeamRoleId == TeamResources.Owner).Select(s => s.TeamId).ToListAsync();

                if (listTeamId.Any())
                    query = query.Where(x => x.TeamId.HasValue && listTeamId.Contains(x.TeamId.Value));
            }

            #endregion
            

            pageRequestFilters = ExcludeCustomFilterFromListFilters(pageRequestFilters).ToList();

            if (pageRequestFilters.Any())
                query = query.FilterSourceByFilters(pageRequestFilters);
            var mappedQuery = query.Select(s => new ReportLeadMappedViewModel
            {
                Created = s.Created,
                Id = s.Id,
                Stage = s.Stage.Name,
                State = s.LeadState.Name,
                Value = !GlobalCurrency.IsNullOrEmpty() ?  Math.Round(s.Value * ConversionRate.Result, 2) : s.Value,
                Pipeline = s.PipeLine.Name,
                OwnerId = s.Team.TeamMembers.FirstOrDefault(x => x.TeamRoleId == TeamResources.Owner) == null ? null
                    : s.Team.TeamMembers.FirstOrDefault(x => x.TeamRoleId == TeamResources.Owner).UserId.ToString()
            }); ;

            var listToReturn = mappedQuery.GroupBy(g => new
            {
                State = listGroupProperties.Contains("State") ? g.State : null,
                Stage = listGroupProperties.Contains("Stage") ? g.Stage : null,
                Pipeline = listGroupProperties.Contains("PipeLine") ? g.Pipeline : null,
                OwnerId = !string.IsNullOrEmpty(g.OwnerId) && listGroupProperties.Contains("Owner") ? _userManager.UserManager.FindByIdAsync(g.OwnerId).Result.UserName : null,
                /*Data = listGroupProperties.Contains("Data") || !listGroupProperties.Any() ? g.Created.ToString("dd/MM/yyyy") : null,*/
            })
                /*.OrderByDescending(x => x.Key.Data.IsNullOrEmpty() ? (DateTime?)null : DateTime.ParseExact(x.Key.Data, "dd/MM/yyyy", null))*/
                .Select(s => new
                    ReportLeadViewModel
                {
                    GroupKeys = s.Key,
                    Count = s.Count(),
                    Id = s.Select(x => x.Id),
                    SumNumberOfUnits = s.Where(x => x.UnitsNumber > 0).Sum(sum => sum.UnitsNumber),
                    SumValue = s.Where(x => x.Value > 0).Sum(sum => sum.Value),
                    Leads = s.ToList()
                });

            //var newListToReturn = listToReturn.ToList().Add(returnedLeads);
            //newListToReturn.Concat(returnedLeads);


             return new SuccessResultModel<IEnumerable<ReportLeadViewModel>>(listToReturn);
        }

        /// <summary>
        /// Download Lead Report Excel
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public async Task<byte[]> DownloadLeadReportExcel(List<DownloadLeadReportViewModel> report)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("LedsReport");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Data";
                worksheet.Cell(currentRow, 2).Value = "Lead Name";
                worksheet.Cell(currentRow, 3).Value = "Total Sum";
                worksheet.Cell(currentRow, 4).Value = "Commision";
                worksheet.Cell(currentRow, 5).Value = "Commision Amount";
                foreach (var section in report)
                {
                    
                    var leads = await GetLeadsByIdList(section.Id);
                    foreach(var lead in leads)
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = lead.Created.Date.ToString();
                        worksheet.Cell(currentRow, 2).Value = lead.Name;
                        worksheet.Cell(currentRow, 3).Value = lead.Value;
                        worksheet.Cell(currentRow, 4).Value = section.AverageCommission;
                        worksheet.Cell(currentRow, 5).Value = (section.AverageCommission * lead.Value).ToString() + lead.CurrencyCode;
                    }
                    
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }

            }
        }

        /// <summary>
        /// Download Lead Report Csv
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public async Task<SuccessResultModel<MemoryStream>> DownloadLeadReportCsv(List<DownloadLeadReportViewModel> report)
        {
            var builder = new StringBuilder();
            builder.AppendLine("Data,Lead Name,Total Sum,Commision,Commision Amount");
            foreach (var section in report)
            {

                var leads = await GetLeadsByIdList(section.Id);
                foreach (var lead in leads)
                {
                    builder.AppendLine($"{lead.Created}," +
                        $"{lead.Name},{lead.Value}," +
                        $"{section.AverageCommission}," +
                        $"{(section.AverageCommission * lead.Value).ToString() + lead.CurrencyCode}");
                }

            }

            return new SuccessResultModel<MemoryStream>(new MemoryStream(Encoding.UTF8.GetBytes(builder.ToString())));
        }

        #endregion


        #region Payments report

        /// <summary>
        /// Payments report 
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<ReportPaymentViewModel>>> PaymentsReportAsync(IEnumerable<PageRequestFilter> filters, IEnumerable<string> listGroupProperties)
        {
            var pageRequestFilters = filters.ToList();

            var selectStartDate = pageRequestFilters.FirstOrDefault(x => string.Equals(x.Propriety.Trim(), "StartDate", StringComparison.CurrentCultureIgnoreCase))?.Value;
            var selectEndDate = pageRequestFilters.FirstOrDefault(x => string.Equals(x.Propriety.Trim(), "EndDate", StringComparison.CurrentCultureIgnoreCase))?.Value;

            if (!DateTime.TryParse(selectStartDate, out var startDate))
                startDate = DateTime.Now.AddDays(-30);

            if (!DateTime.TryParse(selectEndDate, out var endDate))
                endDate = DateTime.Now;

            var query = _paymentContext.PaymentMappers
                .Include(i => i.Payment)
                .Include(i => i.Organization)
                .Where(x => x.Payment.DateTransaction >= startDate && x.Payment.DateTransaction <= endDate);


            var listToReturn = query.GroupBy(g => new
            {
                Data = listGroupProperties.Contains("Data") || !listGroupProperties.Any() ? g.Payment.DateTransaction.ToString("dd.MM.yyyy") : null,

            })
                .Select(s => new
                    ReportPaymentViewModel
                {
                    GroupKeys = s.Key,
                    Count = s.Count(),
                    SumQuantity = s.Where(x => x.Payment.Quantity > 0).Sum(sum => sum.Payment.Quantity),
                    SumValue = s.Where(x => x.Payment.TotalPrice > 0).Sum(sum => sum.Payment.TotalPrice)
                });


            return new SuccessResultModel<IEnumerable<ReportPaymentViewModel>>(listToReturn);
        }

        #endregion


        #region Agreements report

        /// <summary>
        /// Agreement report 
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<ReportAgreementViewModel>>> AgreementsReportAsync(IEnumerable<PageRequestFilter> filters, IEnumerable<string> listGroupProperties)
        {

            var pageRequestFilters = filters.ToList();


            var query = _leadContext.Agreements
                .Include(i => i.Organization)
                .Include(i => i.Lead)
                .SetPeriodByProperty(pageRequestFilters, "Created");


            var listUsers = _userManager.UserManager.Users;

            #region FilterByAgent

            var listAgentId = pageRequestFilters
                .Where(x => string.Equals(x.Propriety.Trim(), "Agent".Trim(), StringComparison.CurrentCultureIgnoreCase))
                .Select(s => s.Value)
                .ToList();

            if (listAgentId.Any())
            {
                var listFiltersUserName = listUsers.Where(x => listAgentId.Contains(x.Id.ToString())).Select(s => s.UserName);

                if (listFiltersUserName.Any())
                    query = query.Where(x => listFiltersUserName.Contains(x.Author));
            }

            #endregion

            var listToReturn = query.GroupBy(g => new
            {
                Agent = listGroupProperties.Contains("Agent") ? g.Author : null,
                Data = listGroupProperties.Contains("Data") || !listGroupProperties.Any() ? g.Created.ToString("dd.MM.yyyy") : null,

            })
                .Select(s => new
                    ReportAgreementViewModel
                {
                    GroupKeys = s.Key,
                    Count = s.Count(),
                    SumValue = s.Where(x => x.Values > 0).Sum(sum => sum.Values),
                });


            return new SuccessResultModel<IEnumerable<ReportAgreementViewModel>>(listToReturn);
        }

        #endregion


        #region Task report



        public virtual async Task<ResultModel<IEnumerable<ReportLeadViewModel>>> TaskReportAsync(IEnumerable<PageRequestFilter> filters, IEnumerable<string> listGroupProperties)
        {
            var pageRequestFilters = filters.ToList();

            var query = _taskManagerContext.Tasks
                .Include(i=> i.AssignedUsers)
                .SetPeriodByProperty(pageRequestFilters, "Created");


            var queryLead = await _leadContext.Leads
                .Include(i=> i.Organization)
                .ToListAsync();

            var queryAgreement = await _leadContext.Agreements.ToListAsync();


            var listUsers = _userManager.UserManager.Users;

            #region FilterByAgent

            var listAgentId = pageRequestFilters
                .Where(x => string.Equals(x.Propriety.Trim(), "Agent".Trim(), StringComparison.CurrentCultureIgnoreCase))
                .Select(s => s.Value)
                .ToList();

            if (listAgentId.Any())
            {
                var listFiltersUserName = listUsers.Where(x => listAgentId.Contains(x.Id.ToString())).Select(s => s.UserName).ToList();
                

                if (listFiltersUserName.Any())
                    query = query.Where(x =>
                        listFiltersUserName.Contains(x.Author) ||
                        x.AssignedUsers.Select(s => s.UserId).Any(i => listAgentId.Contains(i.ToString())));
            }

            #endregion


            #region FilterByPriority

            var listPriorities = pageRequestFilters
                .Where(x => string.Equals(x.Propriety.Trim(), "Priority".Trim(), StringComparison.CurrentCultureIgnoreCase))
                .Select(s => s.Value)
                .ToList();



            if (listPriorities.Any())
                query = query.Where(x =>
                    listPriorities.Contains(x.TaskPriority.ToString()) ||
                    listPriorities.Contains(((int) x.TaskPriority).ToString()));


            #endregion


            #region FilterByStatus

            var listStatus = pageRequestFilters
                .Where(x => string.Equals(x.Propriety.Trim(), "Status".Trim(), StringComparison.CurrentCultureIgnoreCase))
                .Select(s => s.Value)
                .ToList();



            if (listPriorities.Any())
                query = query.Where(x =>
                    listPriorities.Contains(x.Status.ToString()) ||
                    listPriorities.Contains(((int)x.Status).ToString()));


            #endregion


            var queryGetTask = query
                .Select(s => new GetTaskViewModel
                {
                    Id = s.Id,
                    Created = s.Created,
                    TaskNumber = s.TaskNumber,
                    StartDate = s.StartDate,
                    EndDate = s.EndDate,
                    Description = s.Description,
                    Name = s.Name,
                    Status = s.Status,
                    TaskPriority = s.TaskPriority,
                    IsDeleted = s.IsDeleted,
                    UserId = s.UserId,
                    Author = s.Author,
                    LeadId = s.LeadId,
                    Lead = s.LeadId.HasValue ?  queryLead.FirstOrDefault(x=> x.Id == s.LeadId.Value) : null,
                    LeadName = s.LeadId.HasValue ? queryLead.FirstOrDefault(x => x.Id == s.LeadId.Value).Name : null,
                    AgreementId = s.AgreementId,
                    AgreementName = s.AgreementId.HasValue ? queryAgreement.FirstOrDefault(x => x.Id == s.AgreementId.Value).Name : null,
                });


            var listToReturn = queryGetTask.GroupBy(g => new
            {
                Lead = listGroupProperties.Contains("Lead") && g.LeadName != null ? g.LeadName : null,
                Status = listGroupProperties.Contains("Status") ? g.Status.ToString() : null,
                Priority = listGroupProperties.Contains("Priority") ? g.TaskPriority.ToString() : null,
                // OwnerId = !string.IsNullOrEmpty(g.OwnerId) && listGroupProperties.Contains("Owner") ? g.OwnerId : null,
                Agreement = listGroupProperties.Contains("Agreement") && g.AgreementName != null ? g.AgreementName : null,
                Data = listGroupProperties.Contains("Data") || !listGroupProperties.Any() ? g.Created.ToString("dd.MM.yyyy") : null,
            })
                .OrderByDescending(x => x.Key.Data.IsNullOrEmpty() ? (DateTime?)null : DateTime.ParseExact(x.Key.Data, "dd.MM.yyyy", null))
                .Select(s => new
                    ReportLeadViewModel
                {
                    GroupKeys = s.Key,
                    Count = s.Count()
                });

            return new SuccessResultModel<IEnumerable<ReportLeadViewModel>>(listToReturn);
        }




        #endregion



        #region Helpers


        private static IEnumerable<PageRequestFilter> ExcludeCustomFilterFromListFilters(
             IEnumerable<PageRequestFilter> filters)
        {
            return filters.Where(x =>
                        !string.Equals(x.Propriety.Trim(), "Owner".Trim(), StringComparison.CurrentCultureIgnoreCase)
                     && !string.Equals(x.Propriety.Trim(), "Agent".Trim(), StringComparison.CurrentCultureIgnoreCase)
                     && !string.Equals(x.Propriety.Trim(), "StartDate", StringComparison.CurrentCultureIgnoreCase)
                     && !string.Equals(x.Propriety.Trim(), "EndDate", StringComparison.CurrentCultureIgnoreCase));
        }


    private async Task<IEnumerable<Lead>> GetLeadsByIdList(List<Guid> Ids)
        {
            var leads = new List<Lead>();

            foreach (var id in Ids)
            {
                var leadFromDb = await _leadContext.Leads.FirstOrDefaultAsync(x => x.Id == id);

                if (leadFromDb != null)
                {
                    leads.Add(leadFromDb);
                }
            }

            return leads;
        }
        #endregion
    }
}
