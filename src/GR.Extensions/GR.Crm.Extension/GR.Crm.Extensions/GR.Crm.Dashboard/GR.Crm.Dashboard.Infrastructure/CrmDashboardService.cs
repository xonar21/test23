using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.Core.Helpers.Filters.Enums;
using GR.Core.Helpers.Pagination;
using GR.Core.Helpers.Responses;
using GR.Crm.Abstractions;
using GR.Crm.Abstractions.Models;
using GR.Crm.Dashboard.Abstractions;
using GR.Crm.Dashboard.Abstractions.ViewModel;
using GR.Crm.Leads.Abstractions;
using GR.Crm.Leads.Abstractions.Models;
using GR.Crm.Organizations.Abstractions;
using GR.Crm.PipeLines.Abstractions;
using GR.Crm.Teams.Abstractions.Helpers;
using GR.Identity.Abstractions;
using GR.TaskManager.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace GR.Crm.Dashboard.Infrastructure
{
    public class CrmDashboardService : ICrmDashboardService
    {

        #region Injected

        /// <summary>
        /// inject lead context
        /// </summary>
        private readonly ILeadContext<Lead> _leadContext;

        /// <summary>
        /// Inject PipeLine
        /// </summary>
        private readonly IPipeLineContext _pipeLineContext;

        /// <summary>
        /// Inject Crm 
        /// </summary>
        private readonly ICrmContext _crmContext;

        private readonly ICrmOrganizationContext _organizationContext;

        private readonly IIdentityContext _identityContext;

        private readonly ICrmService _crmService;

        private readonly ITaskManagerContext _taskManagerContext;

        private readonly IUserManager<GearUser> _userService;


        #endregion


        public CrmDashboardService(ILeadContext<Lead> leadContext,
            IPipeLineContext pipeLineContext,
            ICrmContext crmContext,
            ICrmOrganizationContext organizationContext,
            IIdentityContext identityContext,
            IConfiguration configuration,
            ICrmService crmService,
            ITaskManagerContext taskManagerContext,
            IUserManager<GearUser> userService)
        {
            _leadContext = leadContext;
            _pipeLineContext = pipeLineContext;
            _crmContext = crmContext;
            _organizationContext = organizationContext;
            _identityContext = identityContext;
            _crmService = crmService;
            _taskManagerContext = taskManagerContext;
            _userService = userService;
            GlobalCurrency = configuration.GetSection("GeneralConfigurations").GetSection("CurrencyCode").Value;
            ConversionRate = crmService.ConvertCurrencyToDefaultCurrencyAsync("EUR");
        }
        private string GlobalCurrency { get; set; }

        private Task<decimal> ConversionRate { get; set; }

        /// <summary>
        /// Get list lead indices
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<LeadDashboardViewModel>>> GetLeadDashboardIndicesAsync(IEnumerable<PageRequestFilter> filters)
        {

            var leadQuery = _leadContext.Leads
            .Include(i => i.LeadState)
            .Include(i => i.PipeLine)
            .Include(i => i.Stage)
            .Include(i => i.Contact)
            .Include(i => i.ProductOrServiceList)
            .Include(i => i.Team)
            .ThenInclude(x => x.TeamMembers)
            .ThenInclude(x => x.TeamRole)
            .NonDeleted()
            .ToCollection();

            var pipeLineQuery = _pipeLineContext.PipeLines.NonDeleted();
            var stagesQuery = _pipeLineContext.Stages.NonDeleted().Select(x => x.Name).Distinct().ToList();
            var productTypesQuery = _crmContext.ProductTypes.NonDeleted();
            var sourcesQuery = _crmContext.Sources.NonDeleted();
            var technologyTypesQuery = _crmContext.TechnologyTypes.NonDeleted();
            var currencyCodes = _crmContext.Currencies.Select(x => x.Code);

            var selectPeriod = "";

            //when no filters were provided
            if (filters.FirstOrDefault(x => x.Propriety == "Period").IsNull())
            {
                var date = leadQuery.OrderBy(x => x.Created).FirstOrDefault().Created;
                selectPeriod = date.ToString("dd.MM.yyyy") + "," + DateTime.Now.ToString("dd.MM.yyyy");
            }
            else
            {
                selectPeriod = filters.FirstOrDefault(x => x.Propriety == "Period").Value.ToString();
            }

            var startDate = selectPeriod.Split(',').First().StringToDateTime() ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var endDate = selectPeriod.Split(',').Last().StringToDateTime() ?? DateTime.Now;

            var days = (endDate.Date - startDate.Date).Days;
            var lastPeriodStartDate = startDate.AddDays(-days);

            var listIndices = new List<LeadDashboardViewModel>();

            if (!GlobalCurrency.IsNullOrEmpty())
            {
                foreach (var lead in leadQuery.Where(x => x.CurrencyCode != GlobalCurrency))
                {
                    var rate = await _crmService.ConvertCurrencyToDefaultCurrencyAsync(lead.CurrencyCode);
                    lead.Value = Math.Round(lead.Value * rate, 2);
                    lead.CurrencyCode = GlobalCurrency;
                };
            }
            else
            {
                foreach (var lead in leadQuery.Where(x => x.CurrencyCode != "EUR"))
                {
                    var rate = await _crmService.ConvertCurrencyToEURAsync(lead.CurrencyCode);
                    lead.Value = Math.Round(lead.Value * rate, 2);
                    lead.CurrencyCode = "EUR";
                };
                GlobalCurrency = "EUR";
            }
            var selectedPipelines = filters.Where(x => x.Propriety == "PipeLine");
            if (selectedPipelines.Count() == 1)
            {
                var pipeLine = selectedPipelines.FirstOrDefault().Value;
                pipeLineQuery = pipeLineQuery.Where(x => x.Id.ToString() == pipeLine);
            }

            var currentLeadsQuery = leadQuery.Where(x => x.Created >= startDate && x.Created <= endDate);
            var lastLeadsQuery = leadQuery.Where(x => x.Created < startDate && x.Created >= lastPeriodStartDate);

            var totalLeadSum = currentLeadsQuery.Sum(x => x.Value);

            foreach (var pipeLine in pipeLineQuery)
            {
                var leads = leadQuery.Where(x => x.PipeLineId == pipeLine.Id);
                var currentLeads = currentLeadsQuery.Where(x => x.PipeLineId == pipeLine.Id);

                var lastLeads = lastLeadsQuery.Where(x => x.PipeLineId == pipeLine.Id);

                var leadInfo = new LeadDashboardViewModel
                {
                    PipeLine = pipeLine.Name,
                    LeadCount = currentLeads.Count()
                };

                var currentLeadsWithState = currentLeads.Where(x => !x.LeadState.IsNull());
                var lastLeadsWithState = lastLeads.Where(x => !x.LeadState.IsNull());

                //lead New
                leadInfo.NewLead = currentLeadsWithState.Count(x => string.Equals(x.LeadState.Name, "New", StringComparison.CurrentCultureIgnoreCase));
                var lastNewLead = lastLeadsWithState.Count(x => string.Equals(x.LeadState.Name, "New", StringComparison.CurrentCultureIgnoreCase));
                leadInfo.NewLeadProgress = CalculationPercentageIncrease(leadInfo.NewLead, lastNewLead);
                leadInfo.NewLeadEvolution = currentLeadsWithState.Where(x => string.Equals(x.LeadState.Name, "New", StringComparison.CurrentCultureIgnoreCase))
                    .GroupBy(g => new { Data = g.Created })
                                .Select(s => new DashboardEvolutionValues()
                                { GroupKey = s.Key, Value = s.Count() });

                //lead Won
                leadInfo.WonLead = currentLeadsWithState.Count(x => string.Equals(x.LeadState.Name, "Won", StringComparison.CurrentCultureIgnoreCase));
                var lastWonLead = lastLeadsWithState.Count(x => string.Equals(x.LeadState.Name, "Won", StringComparison.CurrentCultureIgnoreCase));
                leadInfo.WonLeadProgress = CalculationPercentageIncrease(leadInfo.WonLead, lastWonLead);
                leadInfo.WonLeadEvolution = currentLeadsWithState.Where(x => string.Equals(x.LeadState.Name, "Won", StringComparison.CurrentCultureIgnoreCase))
                    .GroupBy(g => new { Data = g.Created })
                    .Select(s => new DashboardEvolutionValues()
                    { GroupKey = s.Key, Value = s.Count() });


                //lead lost
                leadInfo.LostLead = currentLeadsWithState.Count(x => string.Equals(x.LeadState.Name, "Lost", StringComparison.CurrentCultureIgnoreCase));
                var lastLostLead = lastLeadsWithState.Count(x => string.Equals(x.LeadState.Name, "Lost", StringComparison.CurrentCultureIgnoreCase));
                leadInfo.LostLeadProgress = CalculationPercentageIncrease(leadInfo.LostLead, lastLostLead);
                leadInfo.LostLeadEvolution = currentLeadsWithState.Where(x => string.Equals(x.LeadState.Name, "Lost", StringComparison.CurrentCultureIgnoreCase))
                    .GroupBy(g => new { Data = g.Created })
                    .Select(s => new DashboardEvolutionValues()
                    { GroupKey = s.Key, Value = s.Count() });

                //lead sum
                leadInfo.LeadSum = currentLeads.Sum(s => s.Value);
                var lastLeadSum = lastLeads.Sum(s => s.Value);
                leadInfo.LeadSumProgress = CalculationPercentageIncrease(leadInfo.LostLead, lastLostLead);

                //won lead sum
                leadInfo.WonLeadSum = currentLeadsWithState.Where(x => x.LeadState.Name == "Won").Sum(s => s.Value);
                var lastWonLeadsSum = lastLeadsWithState.Where(x => x.LeadState.Name == "Won").Sum(s => s.Value);
                leadInfo.WonLeadSumProgress = CalculationPercentageIncrease(leadInfo.WonLeadSum, lastWonLeadsSum);

                //leads with user
                var leadsWithUsers = currentLeads.Where(x => x.HasTeam()).ToList();
                //leadsWithUsers.Concat(lastLeads.Where(x => x.HasTeam());

                foreach (var lead in leadsWithUsers)
                {
                    var owner = lead.Team.TeamMembers
                    .FirstOrDefault(x => x.TeamRoleId.Equals(TeamResources.Owner));
                    if (owner != null)
                    {
                        var user = await _userService.UserManager.FindByIdAsync(owner.UserId.ToString());
                        if (user != null)
                        {
                            leadInfo.LeadsByUsers.Add(new LeadsByUsersViewModel
                            {
                                Id = user.Id.ToString(),
                                Name = user.UserName,
                                NrLeads = 1,
                                TotalSum = lead.Value,
                                CurrencyCode = GlobalCurrency,
                                Pipeline = pipeLine.Name
                            });
                        }
                    }
                }

                listIndices.Add(leadInfo);

                foreach (var stage in stagesQuery)
                {
                    var leadInfoByStage = GetLeadsByStage(currentLeadsQuery, lastLeadsQuery, stage, pipeLine.Name);
                    listIndices.Add(leadInfoByStage.Result);
                }

            }


            if (filters.Select(s => s.Propriety).Contains("PipeLine"))
            {
                var listFilterPipeLines = filters.Where(x => x.Propriety == "PipeLine").Select(s => s.Value.ToStringIgnoreNull());
                foreach (var lead in leadQuery.Reverse())
                {
                    if (!listFilterPipeLines.Contains(lead.PipeLineId.ToString()))
                    {
                        leadQuery.Remove(lead);
                    }
                }
            }

            var productPercentageSum = 0.0;
            foreach (var productType in productTypesQuery)
            {
                var leadInfo = GetLeadsByProduct(currentLeadsQuery, productType);
                productPercentageSum += leadInfo.Result.LeadCountPercentage;

                listIndices.Add(leadInfo.Result);
            }
            if (productPercentageSum < 100)
            {
                var leadInfo = new LeadDashboardViewModel
                {
                    ProductType = "No product",
                    LeadCountPercentage = Math.Round(100 - productPercentageSum, 2),
                };

                listIndices.Add(leadInfo);
            }

            var sourcePercentageSum = 0.0;
            foreach (var source in sourcesQuery)
            {
                var leadInfo = GetLeadsBySource(currentLeadsQuery, source);
                sourcePercentageSum += leadInfo.Result.LeadCountPercentage;

                listIndices.Add(leadInfo.Result);
            }
            if (sourcePercentageSum < 100)
            {
                var leadInfo = new LeadDashboardViewModel
                {
                    Source = "No source",
                    LeadCountPercentage = Math.Round(100 - sourcePercentageSum, 2),
                };

                listIndices.Add(leadInfo);
            }

            var leadsWithTechnologyType = currentLeadsQuery.Where(x => x.ProductOrServiceList.Where(y => !y.TechnologyTypeId.IsNull()).Any());

            var technologyTypeCountPercentage = 0.0;

            foreach (var technologyType in technologyTypesQuery)
            {

                var leadInfo = GetLeadsByTechnologyType(leadsWithTechnologyType, technologyType, currentLeadsQuery.Count());
                technologyTypeCountPercentage += leadInfo.Result.LeadCountPercentage;
                listIndices.Add(leadInfo.Result);
            }

            if (technologyTypeCountPercentage < 100)
            {
                var leadInfo = new LeadDashboardViewModel
                {
                    TechnologyType = "N/A",
                    LeadCountPercentage = Math.Round(100 - technologyTypeCountPercentage, 2),
                };

                listIndices.Add(leadInfo);
            }

            var year = new DateTime(startDate.Year, 1, 1);
            var currentYearLeadInfo = GetLeadInfoByYear(leadQuery, year);
            listIndices.Add(currentYearLeadInfo.Result);

            var currentMonth = new DateTime(year.Year, year.Month, 1);
            while (currentMonth < endDate)
            {
                var leadInfo = GetLeadInfoByMonth(leadQuery, currentMonth);
                listIndices.Add(leadInfo.Result);
                currentMonth = currentMonth.AddMonths(1);
            }

            return new SuccessResultModel<IEnumerable<LeadDashboardViewModel>>(listIndices);
        }

        /// <summary>
        /// Get list task indices
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<TaskDashboardViewModel>>> GetTaskDashboardIndicesAsync(IEnumerable<PageRequestFilter> filters)
        {

            var taskQuery = await _taskManagerContext.Tasks
            .Include(i => i.TaskType)
            .NonDeleted()
            .ToListAsync();

            var selectPeriod = "";

            //when no filters were provided
            if (filters.FirstOrDefault(x => x.Propriety == "Period").IsNull())
            {
                var date = taskQuery.OrderBy(x => x.Created).FirstOrDefault().Created;
                selectPeriod = date.ToString("dd.MM.yyyy") + "," + DateTime.Now.ToString("dd.MM.yyyy");
            }
            else
            {
                selectPeriod = filters.FirstOrDefault(x => x.Propriety == "Period").Value.ToString();
            }
            var startDate = selectPeriod.Split(',').First().StringToDateTime() ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var endDate = selectPeriod.Split(',').Last().StringToDateTime() ?? DateTime.Now;

            var days = (endDate.Date - startDate.Date).Days;
            var lastPeriodStartDate = startDate.AddDays(-days);

            var taskTypesQuery = await _taskManagerContext.TaskTypes.NonDeleted().ToListAsync();

            var currentTasksQuery = taskQuery.Where(x => x.Created >= startDate && x.Created <= endDate);

            var listIndices = new List<TaskDashboardViewModel>();

            var taskPercentageSum = 0.0;

            foreach (var taskType in taskTypesQuery)
            {
                var percentage = 0.0;
                var tasks = currentTasksQuery.Where(x => x.TaskTypeId == taskType.Id);

                if (currentTasksQuery.Count() > 0) percentage = Math.Round((double)tasks.Count() / currentTasksQuery.Count() * 100, 2);
                var taskInfo = new TaskDashboardViewModel
                {
                    TaskType = taskType.Name,
                    CountPercentage = percentage
                };
                taskPercentageSum += percentage;
                listIndices.Add(taskInfo);
            }

            if (taskPercentageSum < 100 && currentTasksQuery.Count() > 0)
            {
                var taskInfo = new TaskDashboardViewModel
                {
                    TaskType = "Other",
                    CountPercentage = Math.Round(100 - taskPercentageSum, 2),
                };

                listIndices.Add(taskInfo);
            }

            return new SuccessResultModel<IEnumerable<TaskDashboardViewModel>>(listIndices);
        }

        /// <summary>
        /// Get list organization indices
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<OrganizationDashboardViewModel>>> GetOrganizationDashboardIndicesAsync(IEnumerable<PageRequestFilter> filters)
        {

            var organizationsQuery = _organizationContext.Organizations
            .Include(i => i.Stage)
            .Include(i => i.State)
            .NonDeleted()
            .ToCollection();

            var selectPeriod = "";

            //when no filters were provided
            if (filters.FirstOrDefault(x => x.Propriety == "Period").IsNull())
            {
                var date = organizationsQuery.OrderBy(x => x.Created).FirstOrDefault().Created;
                selectPeriod = date.ToString("dd.MM.yyyy") + "," + DateTime.Now.ToString("dd.MM.yyyy");
            }
            else
            {
                selectPeriod = filters.FirstOrDefault(x => x.Propriety == "Period").Value.ToString();
            }

            var startDate = selectPeriod.Split(',').First().StringToDateTime() ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var endDate = selectPeriod.Split(',').Last().StringToDateTime() ?? DateTime.Now;

            var days = (endDate.Date - startDate.Date).Days;
            var lastPeriodStartDate = startDate.AddDays(-days);

            var statesQuery = _organizationContext.OrganizationStates.NonDeleted().Distinct();

            var listIndices = new List<OrganizationDashboardViewModel>();

            var statePercentageSum = 0.0;

            var currentOrganizationsQuery = organizationsQuery.Where(x => x.Created >= startDate && x.Created <= endDate);

            foreach (var state in statesQuery)
            {
                var organizations = currentOrganizationsQuery.Where(x => x.StateId == state.Id);

                var percentage = 0.0;
                //determine what percentage of total organizations have the current state 
                if (organizations.Count() > 0) percentage = Math.Round((double)organizations.Count() / organizationsQuery.Count() * 100, 2);


                var organizationsInfo = new OrganizationDashboardViewModel
                {
                    State = state.Name,
                    OrganizationsCount = organizations.Count(),
                    OrganizationsCountPercentage = percentage
                };

                statePercentageSum += percentage;

                listIndices.Add(organizationsInfo);
            }

            //determine what percentage of total organizations do not have a state
            if (statePercentageSum < 100)
            {
                var organizationInfo = new OrganizationDashboardViewModel
                {
                    State = "No state",
                    OrganizationsCountPercentage = Math.Round(100 - statePercentageSum, 2),
                };

                listIndices.Add(organizationInfo);
            }

            return new SuccessResultModel<IEnumerable<OrganizationDashboardViewModel>>(listIndices);
        }


        public async Task<ResultModel<IEnumerable<GetLeadAnalysisReportByYearViewModel>>> GetLeadsAnalysisReport(IEnumerable<PageRequestFilter> filters)
        {
            var leadQuery = _leadContext.Leads
            .Include(i => i.Stage)
            .Include(i => i.Team)
            .ThenInclude(x => x.TeamMembers)
            .ThenInclude(x => x.TeamRole)
            .Where(x => x.Stage.Name == "New")
            .NonDeleted()
            .ToCollection();

            var pipeLineQuery = _pipeLineContext.PipeLines.NonDeleted();

            var selectPeriod = "";

            //when no filters were provided
            if (filters.FirstOrDefault(x => x.Propriety == "Period").IsNull())
            {
                var date = leadQuery.OrderBy(x => x.Created).FirstOrDefault().Created;
                selectPeriod = date.ToString("dd.MM.yyyy") + "," + DateTime.Now.ToString("dd.MM.yyyy");
            }
            else
            {
                selectPeriod = filters.FirstOrDefault(x => x.Propriety == "Period").Value.ToString();
            }

            var startDate = selectPeriod.Split(',').First().StringToDateTime() ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var endDate = selectPeriod.Split(',').Last().StringToDateTime() ?? DateTime.Now;

            var days = (endDate.Date - startDate.Date).Days;
            var lastPeriodStartDate = startDate.AddDays(-days);

            var listIndices = new List<GetLeadsAnalysisReportViewModel>();

            if (!GlobalCurrency.IsNullOrEmpty())
            {
                foreach (var lead in leadQuery)
                {
                    lead.Value = Math.Round(lead.Value * ConversionRate.Result, 2);
                    lead.CurrencyCode = GlobalCurrency;
                };
            }
            //filter by created
            leadQuery = leadQuery.Where(x => (x.Created >= startDate && x.Created <= endDate)
                                                    || (x.Created < startDate && x.Created >= lastPeriodStartDate)).ToCollection();

            var leadsWithOwner = leadQuery.Where(x => !x.Team.IsNull())
                                        .Where(x => !(x.Team.TeamMembers.FirstOrDefault(l => l.TeamRoleId.Equals(TeamResources.Owner))).IsNull())
                                        .ToCollection();
            var leadsWithOutOwner = leadQuery.Where(x => !leadsWithOwner.Contains(x)).ToCollection();

            var leadsWithOwnerGroup = leadsWithOwner.OrderBy(x => x.Created)
                                                    .GroupBy(s => s.Created.ToString("yyyy")
                                                    + '-' + s.Created.ToString("MMMM") + '-'
                                                    + _identityContext.Users.FirstOrDefault(u =>
                                                                                u.Id == s.Team.TeamMembers.FirstOrDefault(l
                                                                                                                => l.TeamRoleId.Equals(TeamResources.Owner))
                                                                                                                .UserId.ToString()).UserName);
            var leadsWithOutOwnerGroup = leadsWithOutOwner.OrderBy(s => s.Created)
                                                    .GroupBy(s => s.Created.ToString("yyyy")
                                                    + '-' + s.Created.ToString("MMMM"));

            //.OrderByDescending(s => s.Created).GroupBy(s => s.Created.ToString("yyyy") + s.Created.ToString("MMMM"));
            foreach (var leadGroup in leadsWithOwnerGroup)
            {
                var keySplit = leadGroup.Key.Split('-');
                listIndices.Add(new GetLeadsAnalysisReportViewModel
                {
                    Cases = leadGroup.Count(),
                    ExpectedRevenue = leadGroup.Sum(x => x.Value),
                    CurrencyCode = !GlobalCurrency.IsNullOrEmpty() ? GlobalCurrency : "",
                    Year = keySplit[0],
                    Month = keySplit[1],
                    OwnerName = keySplit[2]
                }); ;
            }

            foreach (var leadGroup in leadsWithOutOwnerGroup)
            {
                var keySplit = leadGroup.Key.Split('-');
                listIndices.Add(new GetLeadsAnalysisReportViewModel
                {
                    Cases = leadGroup.Count(),
                    ExpectedRevenue = leadGroup.Sum(x => x.Value),
                    CurrencyCode = !GlobalCurrency.IsNullOrEmpty() ? GlobalCurrency : "",
                    Year = keySplit[0],
                    Month = keySplit[1],
                    OwnerName = "-N/A"
                }); ;
            }

            var groupByUserAndYear = listIndices.GroupBy(x => x.OwnerName + '-' + x.Year);
            var result = new List<GetLeadAnalysisReportByYearViewModel>();
            foreach (var group in groupByUserAndYear)
            {
                var keySplict = group.Key.Split('-');
                result.Add(new GetLeadAnalysisReportByYearViewModel
                {
                    Year = keySplict[1],
                    OwnerName = keySplict[0],
                    Values = group.ToList()
                });
            }

            return new SuccessResultModel<IEnumerable<GetLeadAnalysisReportByYearViewModel>>(result);
        }


        public async Task<ResultModel<IEnumerable<LeadByStatusAndValueResultViewModel>>> GetLeadsByStatusAndValueAsync(IEnumerable<PageRequestFilter> filters)
        {
            var leadQuery = _leadContext.Leads
            .Include(x => x.LeadState)
            .Where(x => !x.IsDeleted);

            var selectPeriod = "";

            //when no filters were provided
            if (filters.FirstOrDefault(x => x.Propriety == "Period").IsNull())
            {
                var date = leadQuery.OrderBy(x => x.Created).FirstOrDefault().Created;
                selectPeriod = date.ToString("dd.MM.yyyy") + "," + DateTime.Now.ToString("dd.MM.yyyy");
            }
            else
            {
                selectPeriod = filters.FirstOrDefault(x => x.Propriety == "Period").Value.ToString();
            }

            var startDate = selectPeriod.Split(',').First().StringToDateTime() ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var endDate = selectPeriod.Split(',').Last().StringToDateTime() ?? DateTime.Now;

            var days = (endDate.Date - startDate.Date).Days;
            var lastPeriodStartDate = startDate.AddDays(-days);

            leadQuery = leadQuery.Where(x => x.Created >= startDate && x.Created <= endDate);
            foreach (var lead in leadQuery.Where(x => x.CurrencyCode != GlobalCurrency))
            {
                var rate = await _crmService.ConvertCurrencyToDefaultCurrencyAsync(lead.CurrencyCode);
                lead.Value = Math.Round(lead.Value * rate, 2);
            };
            var leadsByStatus = leadQuery.Select(s => new LeadByStatusAndValueViewModel
                {
                    StateName = s.LeadState.Name,
                    LeadName = s.Name,
                    Value =s.Value
                });
            var leadGroupsByStatus = leadsByStatus.GroupBy(x => x.StateName);
            var result = new List<LeadByStatusAndValueResultViewModel>();
            foreach(var leadGroup in leadGroupsByStatus)
            {
                result.Add(new LeadByStatusAndValueResultViewModel
                {
                    StateName = leadGroup.Key,
                    TotalValue = leadGroup.Sum(x => x.Value),
                    Leads = leadGroup.ToList()
                });
            }

            return new SuccessResultModel<IEnumerable<LeadByStatusAndValueResultViewModel>>(result);
        }

        #region Helper

        private static decimal CalculationPercentageIncrease(decimal newValue, decimal oldValue)
        {
            if (newValue == 0 && oldValue == 0)
                return 0;

            if (newValue > 0 && oldValue == 0)
                return 100;

            if (newValue == 0 && oldValue > 0)
                return -100;

            return newValue > oldValue ?
                Math.Round(Math.Abs((oldValue - newValue) / newValue * 100), 2)
                : Math.Round((newValue - oldValue) / oldValue * 100, 2);
        }

        private ResultModel<LeadDashboardViewModel> GetLeadsByStage(IEnumerable<Lead> currentLeadsQuery, IEnumerable<Lead> lastLeadsQuery, string stage, string pipeLine)
        {
            //var leads = leadQuery.Where(x => x.Stage.Name == stage);
            var currentLeads = currentLeadsQuery.Where(x => x.Stage.Name == stage && x.PipeLine.Name == pipeLine);

            var lastLeads = lastLeadsQuery.Where(x => x.Stage.Name == stage);

            var leadInfo = new LeadDashboardViewModel
            {
                Stage = stage,
                LeadCount = currentLeads.Count(),
                PipeLine = pipeLine,
                LeadCountPercentage = currentLeadsQuery.Count() > 0 ? Math.Round(((double)currentLeads.Count() / currentLeadsQuery.Count() * 100), 2) : 0,
                LeadSum = currentLeads.Sum(s => s.Value)
            };

            return new SuccessResultModel<LeadDashboardViewModel>(leadInfo);
        }

        private ResultModel<LeadDashboardViewModel> GetLeadsByProduct(IEnumerable<Lead> currentLeadsQuery, ProductType productType)
        {
            var leads = currentLeadsQuery.Where(x => x.ProductOrServiceList.Where(y => y.ProductTypeId == productType.Id).Any()).Count();
            // var allLeads = leadQuery.Where(x => x.ProductTypeId.HasValue).Count();
            var percentage = 0.0;
            if (currentLeadsQuery.Count() > 0) percentage = Math.Round((double)leads / currentLeadsQuery.Count() * 100, 2);

            var leadInfo = new LeadDashboardViewModel
            {
                ProductType = productType.Name,
                LeadCountPercentage = percentage
            };
            return new SuccessResultModel<LeadDashboardViewModel>(leadInfo);
        }

        private ResultModel<LeadDashboardViewModel> GetLeadsBySource(IEnumerable<Lead> currentLeadsQuery, Source source)
        {
            var leads = currentLeadsQuery.Where(x => x.SourceId == source.Id);

            var percentage = 0.0;
            if (currentLeadsQuery.Count() > 0) percentage = Math.Round((double)leads.Count() / currentLeadsQuery.Count() * 100, 2);

            var leadInfo = new LeadDashboardViewModel
            {
                Source = source.Name,
                LeadCountPercentage = percentage,
                LeadSum = leads.Sum(s => s.Value)
            };

            return new SuccessResultModel<LeadDashboardViewModel>(leadInfo);
        }

        private ResultModel<LeadDashboardViewModel> GetLeadsByTechnologyType(IEnumerable<Lead> currentLeadsQuery, TechnologyType technologyType, int totalLeadsCount)
        {

            var leads = currentLeadsQuery.Where(x => x.ProductOrServiceList.Where(y => y.TechnologyTypeId == technologyType.Id).Any());

            var leadInfo = new LeadDashboardViewModel
            {
                TechnologyType = technologyType.Name,
                LeadCount = leads.Count(),
                LeadCountPercentage = Math.Round((double)leads.Count() / totalLeadsCount * 100, 2),
                LeadSum = leads.Sum(s => s.Value),
                WonLeadSum = leads.Where(x => !x.LeadState.IsNull() && x.LeadState.Name == "Won").Sum(x => x.Value),
                LostLeadSum = leads.Where(x => !x.LeadState.IsNull() && x.LeadState.Name == "Lost").Sum(x => x.Value)

            };

            return new SuccessResultModel<LeadDashboardViewModel>(leadInfo);
        }

        private ResultModel<LeadDashboardViewModel> GetLeadInfoByMonth(IEnumerable<Lead> leadQuery, DateTime month)
        {
            var leads = leadQuery.Where(x => x.Created >= month && x.Created <= month.AddMonths(1));
            var wonLeads = leads.Where(x => !x.LeadState.IsNull() && x.LeadState.Name == "Won");
            var lostLeads = leads.Where(x => !x.LeadState.IsNull() && x.LeadState.Name == "Lost");

            var wonLeadsSum = wonLeads.Sum(s => s.Value);
            var lostLeadsSum = lostLeads.Sum(s => s.Value);

            var leadInfo = new LeadDashboardViewModel
            {
                Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month.Month),
                LeadSum = leads.Sum(s => s.Value)
            };

            leadInfo.LostAndWonValues.Add(lostLeadsSum);
            leadInfo.LostAndWonValues.Add(wonLeadsSum);

            return new SuccessResultModel<LeadDashboardViewModel>(leadInfo);
        }

        private ResultModel<LeadDashboardViewModel> GetLeadInfoByYear(IEnumerable<Lead> leadQuery, DateTime year)
        {
            var leads = leadQuery.Where(x => x.Created >= year && x.Created <= DateTime.Now);
            var wonLeads = leads.Where(x => !x.LeadState.IsNull() && x.LeadState.Name == "Won");
            var lostLeads = leads.Where(x => !x.LeadState.IsNull() && x.LeadState.Name == "Lost");

            var lostLeadPercentage = Math.Round((double)lostLeads.Count() / leads.Count() * 100, 2);
            var wonLeadPercentage = Math.Round((double)wonLeads.Count() / leads.Count() * 100, 2);

            var leadInfo = new LeadDashboardViewModel
            {
                LeadCountPercentage = 100.0 - wonLeadPercentage - lostLeadPercentage,
                WonLeadCountPercentage = wonLeadPercentage,
                LostLeadCountPercentage = lostLeadPercentage

            };

            return new SuccessResultModel<LeadDashboardViewModel>(leadInfo);
        }
        #endregion
    }
}
