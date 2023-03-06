using DocumentFormat.OpenXml.Wordprocessing;
using Gr.Crm.Products.Abstractions.Models;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.Crm.Abstractions;
using GR.Crm.Abstractions.Helpers;
using GR.Crm.Abstractions.Models;
using GR.Crm.Abstractions.Models.ProductConfiguration.Services;
using GR.Crm.Leads.Abstractions;
using GR.Crm.Leads.Abstractions.Models;
using GR.Crm.Leads.Abstractions.ViewModels;
using GR.Crm.Migrations;
using GR.Crm.Organizations.Abstractions;
using GR.Crm.Organizations.Abstractions.Models;
using GR.Crm.Organizations.Abstractions.ViewModels.ContactsViewModels;
using GR.Crm.Organizations.Abstractions.ViewModels.OrganizationsViewModels;
using GR.Crm.PipeLines.Abstractions.Models;
using GR.Crm.Products.Abstractions.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using NPOI.HSSF.UserModel;
using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using Polly.Caching;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace GR.Crm
{
    public class ImportExportService : ICrmImportExportService
    {
        #region Injectable

        /// <summary>
        /// Inject context
        /// </summary>
        private readonly ICrmOrganizationContext _context;


        /// <summary>
        /// Inject lead service
        /// </summary>
        private readonly ILeadService<Lead> _leadService;


        /// <summary>
        /// Inject leadContext
        /// </summary>
        private readonly ILeadContext<Lead> _leadContext;


        /// <summary>
        /// Inject contact service
        /// </summary>
        private readonly ICrmContactService _crmContactService;


        /// <summary>
        /// Inject organization service
        /// </summary>
        private readonly ICrmOrganizationService _crmOrganizationService;

        #endregion

        private string Action { get; set; }

        public ImportExportService(ICrmOrganizationContext context,
                                    ICrmOrganizationService crmOrganizationService,
                                    ICrmContactService crmContactService,
                                    ILeadContext<Lead> leadContext,
                                    ILeadService<Lead> leadService)
        {
            _context = context;
            _crmOrganizationService = crmOrganizationService;
            _crmContactService = crmContactService;
            _leadContext = leadContext;
            _leadService = leadService;
        }

        /// <summary>
        /// Import Organization
        /// </summary>
        /// <param name="propsToImport, parameters, row"></param>
        /// <returns></returns>
        public async Task<ResultModel> ImportOrganizationFromFileAsync(Dictionary<string, int> propsToImport,
                                                            string[] parameters,
                                                            string[] row)
        {
            var toResult = new List<ResultModel>();
            Industry industry = null;
            if (propsToImport.ContainsKey("Industry"))
            {
                industry = await _context.Industries.FirstOrDefaultAsync(x => x.Name == row[propsToImport["Industry"]]);
            }
                
            var organization = new Organization();
            var machingOrgs = new List<Organization>();
            if (parameters.Contains("Name"))
            {
                organization = await _context.Organizations.FirstOrDefaultAsync(x => x.Name == row[propsToImport["Name"]]);
                if (organization != null) machingOrgs.Add(organization);
            }
            if (parameters.Contains("IBANCode"))
            {
                organization = await _context.Organizations.FirstOrDefaultAsync(x => x.IBANCode == row[propsToImport["IBANCode"]]);
                if (organization != null) machingOrgs.Add(organization);
            }
            if (parameters.Contains("Bank"))
            {
                organization = await _context.Organizations.FirstOrDefaultAsync(x => x.Bank == row[propsToImport["Bank"]]);
                if (organization != null) machingOrgs.Add(organization);
            }
            if (parameters.Contains("Phone"))
            {
                organization = await _context.Organizations.FirstOrDefaultAsync(x => x.Phone == row[propsToImport["Phone"]]);
                if (organization != null) machingOrgs.Add(organization);
            }
            if (parameters.Contains("WebSite"))
            {
                organization = await _context.Organizations.FirstOrDefaultAsync(x => x.WebSite == row[propsToImport["WebSite"]]);
                if (organization != null) machingOrgs.Add(organization);
            }
            if (parameters.Contains("FiscalCode"))
            {
                organization = await _context.Organizations.FirstOrDefaultAsync(x => x.FiscalCode == row[propsToImport["FiscalCode"]]);
                if (organization != null) machingOrgs.Add(organization);
            }
            if (parameters.Contains("CodSwift"))
            {
                organization = await _context.Organizations.FirstOrDefaultAsync(x => x.CodSwift == row[propsToImport["CodSwift"]]);
                if (organization != null) machingOrgs.Add(organization);
            }
            if (parameters.Contains("VitCode"))
            {
                organization = await _context.Organizations.FirstOrDefaultAsync(x => x.VitCode == row[propsToImport["VitCode"]]);
                if (organization != null) machingOrgs.Add(organization);
            }
            if (parameters.Contains("Description"))
            {
                organization = await _context.Organizations.FirstOrDefaultAsync(x => x.Description == row[propsToImport["Description"]]);
                if (organization != null) machingOrgs.Add(organization);
            }
            if (parameters.Contains("Industry") && industry != null)
            {
                organization = await _context.Organizations.FirstOrDefaultAsync(x => x.IndustryId == industry.Id);
                if (organization != null) machingOrgs.Add(organization);
            }
            //remove duplicatest
            machingOrgs = machingOrgs.Distinct().ToList();
            var newOrg = new OrganizationViewModel
            {
                Name = propsToImport.ContainsKey("Name") ? row[propsToImport["Name"]] : "",
                IBANCode = propsToImport.ContainsKey("IBANCode") ? row[propsToImport["IBANCode"]] : "",
                Brand = "",
                Bank = propsToImport.ContainsKey("Bank") ? row[propsToImport["Bank"]] : "",
                Email = propsToImport.ContainsKey("Email") ? row[propsToImport["Email"]] : "",
                Phone = propsToImport.ContainsKey("Phone") && int.TryParse(row[propsToImport["Phone"]], out int aux) ? row[propsToImport["Phone"]] : "",
                WebSite = propsToImport.ContainsKey("Website") ? row[propsToImport["Website"]] : "",
                FiscalCode = propsToImport.ContainsKey("FiscalCode") ? row[propsToImport["FiscalCode"]] : "",
                CodSwift = propsToImport.ContainsKey("CodSwift") ? row[propsToImport["CodSwift"]] : "",
                VitCode = propsToImport.ContainsKey("VitCode") ? row[propsToImport["VitCode"]] : "",
                Description = propsToImport.ContainsKey("Description") ? row[propsToImport["Description"]] : "",
                IndustryId = propsToImport.ContainsKey("Industry") ? (industry != null ? industry.Id : (Guid?)null) : (Guid?)null,
            };
            var currentAction = Action.Split(',');
            foreach(var act in currentAction)
            {
                switch (act)
                {
                    case "Delete":
                        {
                            if (machingOrgs.Count > 0)
                            {
                                _context.Organizations.RemoveRange(machingOrgs);
                                toResult.Add(await _context.PushAsync());
                            }
                            var result = await _crmOrganizationService.AddNewOrganizationAsync(newOrg);
                            toResult.Add(new ResultModel
                            {
                                IsSuccess = result.IsSuccess,
                                Errors = result.Errors
                            });
                        }
                        break;
                    case "Update":
                        {
                            toResult.Add(UpdateOrganization(machingOrgs, newOrg).Result);
                        }
                        break;
                    case "No maching":
                        {
                            if (machingOrgs.Count == 0)
                            {
                                var result = await _crmOrganizationService.AddNewOrganizationAsync(newOrg);
                                toResult.Add(new ResultModel
                                {
                                    IsSuccess = result.IsSuccess,
                                    Errors = result.Errors
                                });
                            }
                        }
                        break;
                    case "More than one":
                        {
                            if (machingOrgs.Count > 1)
                            {
                                var result = await _crmOrganizationService.AddNewOrganizationAsync(newOrg);
                                toResult.Add(new ResultModel
                                {
                                    IsSuccess = result.IsSuccess,
                                    Errors = result.Errors
                                });
                            }
                        }
                        break;
                }
            }

            return ReturnResult(toResult);
        }


        /// <summary>
        /// Import Contact
        /// </summary>
        /// <param name="propsToImport, parameters, row"></param>
        /// <returns></returns>
        public async Task<ResultModel> ImportContactFromFileAsync(Dictionary<string, int> propsToImport,
                                                             string[] parameters,
                                                             string[] row)
        {
            var toResult = new List<ResultModel>();

            JobPosition jobPosition = null;
            if (propsToImport.ContainsKey("JobPosition")) jobPosition = await _context.JobPositions.FirstOrDefaultAsync(x => x.Name == row[propsToImport["JobPosition"]]);
            Organization organization = null;
            if (propsToImport.ContainsKey("Organization")) organization = await _context.Organizations.FirstOrDefaultAsync(x => x.Name.Contains(row[propsToImport["Organization"]]));
            var machingContacts = new List<Contact>();

            var contact = new Contact();
            if (parameters.Contains("FirstName"))
            {
                contact = await _context.Contacts.FirstOrDefaultAsync(x => x.FirstName == row[propsToImport["FirstName"]]);
                if (contact != null) machingContacts.Add(contact);
            }
            if (parameters.Contains("LastName"))
            {
                contact = await _context.Contacts.FirstOrDefaultAsync(x => x.LastName == row[propsToImport["LastName"]]);
                if (contact != null) machingContacts.Add(contact);
            }
            if (parameters.Contains("Description"))
            {
                contact = await _context.Contacts.FirstOrDefaultAsync(x => x.Description == row[propsToImport["Description"]]);
                if (contact != null) machingContacts.Add(contact);
            }
            if (parameters.Contains("Organization"))
            {
                contact = await _context.Contacts.FirstOrDefaultAsync(x => x.OrganizationId == organization.Id);
                if (contact != null) machingContacts.Add(contact);
            }
            //remove duplicates
            machingContacts = machingContacts.Distinct().ToList();
            var currentAction = Action.Split(',');

            var newContact = new ContactViewModel
            {
                FirstName = propsToImport.ContainsKey("FirstName") ? row[propsToImport["FirstName"]] : "",
                LastName = propsToImport.ContainsKey("LastName") ? row[propsToImport["LastName"]] : "",
                Description = propsToImport.ContainsKey("Description") ? row[propsToImport["Description"]] : "",
                JobPositionId = jobPosition != null ? jobPosition.Id : (Guid?)null,
                Email = propsToImport.ContainsKey("Email") ? row[propsToImport["Email"]] : ""
            };
            if (organization != null)
            {
                newContact.OrganizationId = organization.Id;
                foreach (var act in currentAction)
                {
                    switch (act)
                    {
                        case "Delete":
                            {
                                if (machingContacts.Count > 0)
                                {
                                    _context.Contacts.RemoveRange(machingContacts);
                                    toResult.Add(await _context.PushAsync());
                                }
                                var result = await _crmContactService.AddNewContactAsync(newContact);
                                toResult.Add(new ResultModel
                                {
                                    IsSuccess = result.IsSuccess,
                                    Errors = result.Errors
                                });
                            }
                            break;
                        case "Update":
                            {
                                toResult.Add(UpdateContact(machingContacts, newContact).Result);
                            }
                            break;
                        case "No maching":
                            {
                                if (machingContacts.Count == 0)
                                {
                                    var result = await _crmContactService.AddNewContactAsync(newContact);
                                    toResult.Add(new ResultModel
                                    {
                                        IsSuccess = result.IsSuccess,
                                        Errors = result.Errors
                                    });
                                }
                            }
                            break;
                        case "More than one":
                            {
                                if (machingContacts.Count > 1)
                                {
                                    var result = await _crmContactService.AddNewContactAsync(newContact);
                                    toResult.Add(new ResultModel
                                    {
                                        IsSuccess = result.IsSuccess,
                                        Errors = result.Errors
                                    });
                                }
                            }
                            break;
                    }
                }
            }
            else
            {
                if (currentAction.Count() == 1 && currentAction[0] == "Update")
                {
                    //fill orgId with a dummy Id
                    newContact.OrganizationId = Guid.NewGuid();
                    toResult.Add(UpdateContact(machingContacts, newContact).Result);

                }
            }

            return ReturnResult(toResult);
        }




        /// <summary>
        /// Import Lead
        /// </summary>
        /// <param name="propsToImport, parameters, row"></param>
        /// <returns></returns>
        public async Task<ResultModel> ImportLeadFromFileAsync(Dictionary<string, int> propsToImport,
                                                             string[] parameters,
                                                             string[] row, IUrlHelper Url)
        {
            var lead = new Lead(); 
            var toResult = new List<ResultModel>();
            var machingLeads = new List<Lead>();
            if (parameters.Contains("Name"))
            {
                lead = await _leadContext.Leads.FirstOrDefaultAsync(x => x.Name == row[propsToImport["Name"]]);
                if (lead != null) machingLeads.Add(lead);
            }
            if (parameters.Contains("Value"))
            {
                lead = await _leadContext.Leads.FirstOrDefaultAsync(x => x.Value.ToString() == row[propsToImport["Value"]]);
                if (lead != null) machingLeads.Add(lead);
            }
            if (parameters.Contains("CurrencyCode"))
            {
                lead = await _leadContext.Leads.FirstOrDefaultAsync(x => x.CurrencyCode == row[propsToImport["CurrencyCode"]]);
                if (lead != null) machingLeads.Add(lead);
            }
            if (parameters.Contains("DeadLine"))
            {
                lead = await _leadContext.Leads.FirstOrDefaultAsync(x => x.DeadLine.ToString() == row[propsToImport["DeadLine"]]);
                if (lead != null) machingLeads.Add(lead);
            }
            if (parameters.Contains("ClarificationDeadLine"))
            {
                lead = await _leadContext.Leads.FirstOrDefaultAsync(x => x.ClarificationDeadline.ToString() == row[propsToImport["ClarificationDeadLine"]]);
                if (lead != null) machingLeads.Add(lead);
            }
            if (parameters.Contains("Description"))
            {
                lead = await _leadContext.Leads.FirstOrDefaultAsync(x => x.Description == row[propsToImport["Description"]]);
                if (lead != null) machingLeads.Add(lead);
            }
            var organization = new Organization();
            if (propsToImport.ContainsKey("Organization"))
            {
                organization = await _context.Organizations.FirstOrDefaultAsync(x => x.Name == row[propsToImport["Organization"]]);
                if (parameters.Contains("Organization") && organization != null)
                {
                    lead = await _leadContext.Leads.FirstOrDefaultAsync(x => x.OrganizationId == organization.Id);
                    if (lead != null) machingLeads.Add(lead);
                }
            }
            var pipeLine = new PipeLine();
            if (propsToImport.ContainsKey("PipeLine"))
            {
                pipeLine = await _leadContext.PipeLines.FirstOrDefaultAsync(x => x.Name == row[propsToImport["PipeLine"]]);
                if (parameters.Contains("PipeLine") && pipeLine != null)
                {
                    lead = await _leadContext.Leads.FirstOrDefaultAsync(x => x.PipeLineId == pipeLine.Id);
                    if (lead != null) machingLeads.Add(lead);
                }
            }
            var stage = new Stage();
            if (propsToImport.ContainsKey("Stage"))
            {
                stage = await _leadContext.Stages.FirstOrDefaultAsync(x => x.Name == row[propsToImport["Stage"]]);
                if (parameters.Contains("Stage") && stage != null)
                {
                    lead = await _leadContext.Leads.FirstOrDefaultAsync(x => x.StageId == stage.Id);
                    if (lead != null) machingLeads.Add(lead);
                }
            }
            var leadState = new LeadState();
            if (propsToImport.ContainsKey("LeadState"))
            {
                leadState = await _leadContext.States.FirstOrDefaultAsync(x => x.Name == row[propsToImport["LeadState"]]);
                if (parameters.Contains("LeadState") && leadState != null)
                {
                    lead = await _leadContext.Leads.FirstOrDefaultAsync(x => x.LeadStateId == leadState.Id);
                    if (lead != null) machingLeads.Add(lead);
                }
            }
            var contact = new Contact();
            if (propsToImport.ContainsKey("Contact"))
            {
                contact = await _leadContext.Contacts.FirstOrDefaultAsync(x => x.FirstName + x.LastName == row[propsToImport["Contact"]]);
                if (parameters.Contains("Contact") && contact != null)
                {
                    lead = await _leadContext.Leads.FirstOrDefaultAsync(x => x.ContactId == contact.Id);
                    if (lead != null) machingLeads.Add(lead);
                }
            }
            var solutionType = new SolutionType();
            var source = new Source();
            if (propsToImport.ContainsKey("Source"))
            {
                source = await _leadContext.Sources.FirstOrDefaultAsync(x => x.Name == row[propsToImport["Source"]]);
                if (parameters.Contains("Source") && source != null)
                {
                    lead = await _leadContext.Leads.FirstOrDefaultAsync(x => x.SourceId == source.Id);
                    if (lead != null) machingLeads.Add(lead);
                }
            }
            var technologyType = new TechnologyType();

            //remove duplicates
            machingLeads = machingLeads.Distinct().ToList();
            var newLead = new AddLeadViewModel
            {
                Value = propsToImport.ContainsKey("Value") ?
                        decimal.Parse(row[propsToImport["Value"]]): (decimal)0,
                Name = propsToImport.ContainsKey("Name") ?
                        row[propsToImport["Name"]] : "--",
                CurrencyCode = propsToImport.ContainsKey("CurrencyCode") ?
                        row[propsToImport["CurrencyCode"]] : "",
                DeadLine = propsToImport.ContainsKey("DeadLine") ?
                        row[propsToImport["DeadLine"]].StringToDateTime() : "".StringToDateTime(),
                ClarificationDeadline = propsToImport.ContainsKey("ClarificationDeadLine") ?
                        row[propsToImport["ClarificationDeadLine"]].StringToDateTime() : "".StringToDateTime(),
                SourceId = propsToImport.ContainsKey("Source") && source != null ?
                        source.Id : (Guid?)null,
                Description = propsToImport.ContainsKey("Description") ? row[propsToImport["Description"]] : ""
            };
            var currentAction = Action.Split(',');
            if (organization != null && pipeLine != null 
                && stage != null && leadState != null)
            {
                newLead.OrganizationId = propsToImport.ContainsKey("Organization") && organization != null ?
                        organization.Id : (Guid?)null;
                newLead.PipeLineId = propsToImport.ContainsKey("PipeLine") && pipeLine != null ?
                        pipeLine.Id : (Guid?)null;
                newLead.StageId = propsToImport.ContainsKey("Stage") && stage != null ?
                        stage.Id : (Guid?)null;
                newLead.LeadStateId = propsToImport.ContainsKey("LeadState") && leadState != null ?
                        leadState.Id : (Guid?)null;
                foreach (var act in currentAction)
                {
                    switch (act)
                    {
                        case "Delete":
                            {
                                if (machingLeads.Count > 0)
                                {
                                    _leadContext.Leads.RemoveRange(machingLeads);
                                    toResult.Add(await _context.PushAsync());
                                }
                                var result = await _leadService.AddLeadAsync(newLead);
                                toResult.Add(new ResultModel
                                {
                                    IsSuccess = result.IsSuccess,
                                    Errors = result.Errors
                                });
                            }
                            break;
                        case "Update":
                            {
                                toResult.Add(UpdateLead(machingLeads, newLead, Url).Result);
                            }
                            break;
                        case "No maching":
                            {
                                if (machingLeads.Count == 0)
                                {
                                    var result = await _leadService.AddLeadAsync(newLead);
                                    toResult.Add(new ResultModel
                                    {
                                        IsSuccess = result.IsSuccess,
                                        Errors = result.Errors
                                    });
                                }
                            }
                            break;
                        case "More than one":
                            {
                                if (machingLeads.Count > 1)
                                {
                                    var result = await _leadService.AddLeadAsync(newLead);
                                    toResult.Add(new ResultModel
                                    {
                                        IsSuccess = result.IsSuccess,
                                        Errors = result.Errors
                                    });
                                }
                            }
                            break;
                    }
                }
            }
            else
            {
                if(currentAction.Count() == 1 && currentAction[0] == "Update")
                {
                    //fill required fields with dummy data;
                    newLead.OrganizationId = Guid.NewGuid();
                    newLead.PipeLineId = Guid.NewGuid();
                    newLead.StageId = Guid.NewGuid();
                    newLead.LeadStateId = Guid.NewGuid();
                    toResult.Add(UpdateLead(machingLeads, newLead, Url).Result);
                }
            }
            return ReturnResult(toResult);
        }


        /// <summary>
        /// ImportAsync
        /// </summary>
        /// <param name="objToImport"></param>
        /// <returns></returns>
        public async Task<ResultModel> ImportAsync(IFormCollection objToImport, IUrlHelper Url)
        {
            var toResult = new List<ResultModel>();
            Action = objToImport["Action"];

            Dictionary<string, int> propsToImport = new Dictionary<string, int>();

            string property = objToImport["Properties"];
            string[] properties = property.Split(',');

            string param = objToImport["Parameters"];
            string[] parameters = param.Split(',');
            if (objToImport.Files[0].FileName.EndsWith("xls") || objToImport.Files[0].FileName.EndsWith("xlsx"))
            {
                using (var stream = new MemoryStream())
                {
                    await objToImport.Files[0].CopyToAsync(stream);

                    IWorkbook workbook;

                    if (objToImport.Files[0].FileName.IndexOf(".xlsx", StringComparison.Ordinal) > 0)
                        workbook = WorkbookFactory.Create(stream);
                    else if (objToImport.Files[0].FileName.IndexOf(".xls", StringComparison.Ordinal) > 0)
                        workbook = new HSSFWorkbook(stream);
                    else
                        return new ResultModel
                        {
                            IsSuccess = false,
                            Errors = new List<IErrorModel> { new ErrorModel { Message = "File is not in excel format" } }
                        };
                    var sheet = workbook.GetSheetAt(0);

                    var currentRow = sheet.GetRow(0);

                    for (int index = 0; index < currentRow.Cells.Count; index++)
                    {
                        if (properties.Contains(currentRow.Cells[index].CellStringValue()))
                            propsToImport.Add(currentRow.Cells[index].CellStringValue(), index);
                    }

                    switch (objToImport["Type"])
                    {
                        case "Contact":
                            {
                                for (var item = sheet.FirstRowNum + 1; item <= sheet.LastRowNum; item++)
                                {
                                    toResult.Add(await ImportContactFromFileAsync(propsToImport, parameters, GetRowValue(sheet.GetRow(item))));
                                }
                                break;
                            }
                        case "Organization":
                            {
                                for (var item = sheet.FirstRowNum + 1; item <= sheet.LastRowNum; item++)
                                {
                                    toResult.Add(await ImportOrganizationFromFileAsync(propsToImport, parameters, GetRowValue(sheet.GetRow(item))));
                                }
                                break;
                            }
                        case "Lead":
                            {
                                for (var item = sheet.FirstRowNum + 1; item <= sheet.LastRowNum; item++)
                                {
                                    toResult.Add(await ImportLeadFromFileAsync(propsToImport, parameters, GetRowValue(sheet.GetRow(item)), Url));
                                }
                                break;
                            }
                    }
                }
            }

            if (objToImport.Files[0].FileName.EndsWith("csv"))
            {
                using (var sreader = new StreamReader(objToImport.Files[0].OpenReadStream()))
                {
                    string[] headers = sreader.ReadLine().Split(objToImport["Delimiter"]);

                    for (int index = 0; index < headers.Count(); index++)
                    {
                        if (properties.Contains(headers[index]))
                            propsToImport.Add(headers[index], index);
                    }

                    switch (objToImport["Type"])
                    {
                        case "Contact":
                            {
                                while (!sreader.EndOfStream)
                                {
                                    string[] row = sreader.ReadLine().Split(objToImport["Delimiter"]);
                                    toResult.Add(await ImportContactFromFileAsync(propsToImport, parameters, row));
                                }
                                break;
                            }
                        case "Organization":
                            {
                                while (!sreader.EndOfStream)
                                {
                                    string[] row = sreader.ReadLine().Split(objToImport["Delimiter"]);
                                    toResult.Add(await ImportOrganizationFromFileAsync(propsToImport, parameters, row));
                                }
                                break;
                            }
                        case "Lead":
                            {
                                while (!sreader.EndOfStream)
                                {
                                    string[] row = sreader.ReadLine().Split(objToImport["Delimiter"]);
                                    toResult.Add(await ImportLeadFromFileAsync(propsToImport, parameters, row, Url));
                                }
                                break;
                            }
                    }
                }
            }
            throw new NotImplementedException();
        }

        #region Helpers
        /// <summary>
        /// Read row
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public string[] GetRowValue(IRow row)
        {
            string[] rowEl = new string[row.Count()];
            for (int index = 0; index < row.Count(); index++)
            {
                rowEl[index] = row.Cells[index].CellStringValue();
            }
            return rowEl;
        }


        /// <summary>
        /// Return Result
        /// </summary>
        /// <param name="toResult"></param>
        /// <returns></returns>
        public ResultModel ReturnResult(List<ResultModel> toResult)
        {
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
        /// Update Lead
        /// </summary>
        /// <param name="machingLeads, newLead"></param>
        /// <returns></returns>
        public async Task<ResultModel> UpdateLead(List<Lead> machingLeads, AddLeadViewModel newLead, IUrlHelper Url)
        {
            var toResult = new List<ResultModel>();
            foreach (var machingLead in machingLeads)
            {
                var updateLead = new UpdateLeadViewModel
                {
                    Id = machingLead.Id,
                    Created = machingLead.Created,
                    Name = machingLead.Name,
                    OrganizationId = machingLead.OrganizationId,
                    Value = machingLead.Value != 0 ?
                        machingLead.Value : newLead.Value,
                    CurrencyCode = machingLead.CurrencyCode != "" ?
                        machingLead.CurrencyCode : newLead.CurrencyCode,
                    DeadLine = machingLead.DeadLine != "".StringToDateTime() ?
                        machingLead.DeadLine : newLead.DeadLine,
                    LeadStateId = machingLead.LeadStateId,
                    StageId = machingLead.StageId,
                    ClarificationDeadline = machingLead.ClarificationDeadline != "".StringToDateTime() ?
                        machingLead.ClarificationDeadline : newLead.ClarificationDeadline,
                    SourceId = machingLead.SourceId != (Guid?)null ?
                        machingLead.SourceId : newLead.SourceId,
                    Description = machingLead.Description != "" ?
                        machingLead.Description : newLead.Description
                };

                toResult.Add(await _leadService.UpdateLeadAsync(updateLead, Url));
            }
            return ReturnResult(toResult);
        }


        /// <summary>
        /// Update Contact
        /// </summary>
        /// <param name="machingContacts, newContact"></param>
        /// <returns></returns>
        public async Task<ResultModel> UpdateContact(List<Contact> machingContacts, ContactViewModel newContact)
        {
            var toResult = new List<ResultModel>();

            foreach (var cont in machingContacts)
            {
                var updateContact = new ContactViewModel
                {
                    Id = cont.Id,
                    FirstName = cont.FirstName != null && cont.FirstName != "" ? cont.FirstName : newContact.FirstName,
                    LastName = cont.LastName != null && cont.LastName != "" ? cont.LastName : newContact.LastName,
                    Description = cont.Description != null && cont.Description != "" ? cont.Description : newContact.Description,
                    JobPositionId = cont.JobPositionId != null ? cont.JobPositionId : newContact.JobPositionId,
                    OrganizationId = cont.OrganizationId != null ? cont.OrganizationId : newContact.OrganizationId
                };

                var result = new ResultModel<Guid>();

                result = await _crmContactService.UpdateContactAsync(updateContact);
                toResult.Add(new ResultModel
                {
                    IsSuccess = result.IsSuccess,
                    Errors = result.Errors
                });
            }

            return ReturnResult(toResult);
        }



        /// <summary>
        /// Update Orgaization
        /// </summary>
        /// <param name="machingOrg, newOrg"></param>
        /// <returns></returns>
        public async Task<ResultModel> UpdateOrganization(List<Organization> machingOrgs, OrganizationViewModel newOrg)
        {
            var toResult = new List<ResultModel>();
            foreach (var org in machingOrgs)
            {
                var updateOrg = new OrganizationViewModel
                {
                    Id = org.Id,
                    Name = org.Name != null && org.Name != "" ? org.Name : newOrg.Name,
                    IBANCode = org.IBANCode != null && org.IBANCode != "" ? org.IBANCode : newOrg.IBANCode,
                    Bank = org.Bank != null && org.Bank != "" ? org.Bank : newOrg.Bank,
                    Brand = org.Brand != null && org.Brand != "" ? org.Brand : newOrg.Brand,
                    Phone = org.Phone != null && org.Phone != "" ? org.Phone : newOrg.Phone,
                    WebSite = org.WebSite != null && org.WebSite != "" ? org.WebSite : newOrg.WebSite,
                    FiscalCode = org.FiscalCode != null && org.FiscalCode != "" ? org.FiscalCode : newOrg.FiscalCode,
                    CodSwift = org.CodSwift != null && org.CodSwift != "" ? org.CodSwift : newOrg.CodSwift,
                    VitCode = org.VitCode != null && org.VitCode != "" ? org.VitCode : newOrg.VitCode,
                    Description = org.Description != null && org.Description != "" ? org.Description : newOrg.Description,
                    IndustryId = org.IndustryId != null ? org.IndustryId : newOrg.IndustryId,
                };
                var result = new ResultModel<Guid>();

                result = await _crmOrganizationService.UpdateOrganizationAsync(updateOrg);
                toResult.Add(new ResultModel
                {
                    IsSuccess = result.IsSuccess,
                    Errors = result.Errors
                });
            }

            return ReturnResult(toResult);
        }
        #endregion
    }
}
