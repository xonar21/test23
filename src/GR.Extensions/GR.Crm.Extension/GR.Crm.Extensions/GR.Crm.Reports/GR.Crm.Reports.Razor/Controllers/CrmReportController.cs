using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using GR.Core.BaseControllers;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Core.Extensions;
using GR.Crm.Abstractions.Models;
using GR.Crm.Leads.Abstractions;
using GR.Crm.Leads.Abstractions.Models;
using GR.Crm.Reports.Abstraction;
using GR.Crm.Reports.Abstraction.ViewModels.LeadReportViewModels;
using GR.Crm.Reports.Abstraction.ViewModels.PaymentReportViewModel;
using GR.Identity.Abstractions;
using GR.Identity.Data.Permissions;
using GR.Identity.Permissions.Abstractions.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GR.Crm.Reports.Razor.Controllers
{
    [Authorize]
    public class CrmReportController : BaseGearController
    {

        #region Inject

        /// <summary>
        /// Inject Crm report service
        /// </summary>
        private readonly ICrmReportService _crmReportService;


        /// <summary>
        /// Inject LeadContext
        /// </summary>
        private readonly ILeadContext<Lead> _leadContext;

        #endregion


        public CrmReportController(ICrmReportService crmReportService,
            ILeadContext<Lead> leadContext,
            IConfiguration configuration)
        {
            _crmReportService = crmReportService;
            _leadContext = leadContext;
            GlobalCurrency = configuration.GetSection("GeneralConfigurations").GetSection("CurrencyCode").Value;
        }

        private string GlobalCurrency { get; set; }

        public IActionResult Index(string type)
        {
            ViewBag.Type = type;
            if (type == "leads")
            {
                ViewBag.Currency = GlobalCurrency != "" ? GlobalCurrency : "EUR";
            }
            return View();
        }

        /// <summary>
        /// Get Lead report 
        /// </summary>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<IEnumerable<ReportLeadViewModel>>))]
        public async Task<JsonResult> LeadReport(IEnumerable<PageRequestFilter> filters, IEnumerable<string> listGroupProperties)
            => await JsonAsync(_crmReportService.LeadReportAsync(filters, listGroupProperties));


        /// <summary>
        /// Get payment report
        /// </summary>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<IEnumerable<ReportPaymentViewModel>>))]
        public async Task<JsonResult> PaymentsReport(IEnumerable<PageRequestFilter> filters, IEnumerable<string> listGroupProperties)
            => await JsonAsync(_crmReportService.PaymentsReportAsync(filters, listGroupProperties));



        /// <summary>
        /// Get agreement report
        /// </summary>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<IEnumerable<ReportPaymentViewModel>>))]
        public async Task<JsonResult> AgreementsReport(IEnumerable<PageRequestFilter> filters, IEnumerable<string> listGroupProperties)
            => await JsonAsync(_crmReportService.AgreementsReportAsync(filters, listGroupProperties));


        /// <summary>
        /// Get agreement report
        /// </summary>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<IActionResult>))]
        public async Task<JsonResult> TaskReport(IEnumerable<PageRequestFilter> filters, IEnumerable<string> listGroupProperties)
            => await JsonAsync(_crmReportService.TaskReportAsync(filters, listGroupProperties), SerializerSettings);


        /// <summary>
        /// Get Lead report 
        /// </summary>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<Guid>))]
        public async Task<ResultModel<Guid>> DownloadLeadReportXLSX([FromBody] List<DownloadLeadReportViewModel> model)
        {

            var biteArr = await _crmReportService.DownloadLeadReportExcel(model);
            var binaryFile = new BinaryFile { DataFiles = biteArr };
            _leadContext.BinaryFiles.Add(binaryFile);
            var result = await _leadContext.PushAsync();

            return new ResultModel<Guid> { IsSuccess = true, Result = binaryFile.Id };
        }

        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(FileResult))]
        public async Task<FileResult> GetXLSXFileById([Required] Guid Id)
        {
            var binaryFile = await _leadContext.BinaryFiles.FirstOrDefaultAsync(x => x.Id == Id);
            if(binaryFile != null)
            {
                var file = File(binaryFile.DataFiles,
                             "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                             "LeadReport.xlsx");
                _leadContext.BinaryFiles.Remove(binaryFile);
                await _leadContext.PushAsync();
                return file;

            }
            return null;
        }


        /// <summary>
        /// Get Lead report 
        /// </summary>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<FileResult>))]
        public async Task<ResultModel<FileResult>> DownloadLeadReportCSV([FromBody] List<DownloadLeadReportViewModel> model)
        {
            var result = await _crmReportService.DownloadLeadReportCsv(model);
            return new ResultModel<FileResult>
            {
                IsSuccess = true,
                Result = result.IsSuccess ? File(result.Result.ToArray(), "text/csv", "LeadReport.csv") : null
            };
                
        }
    }
}
    