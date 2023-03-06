using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Core.Helpers.Responses;
using GR.Crm.Emails.Abstractions;
using GR.Crm.Leads.Abstractions;
using GR.Crm.Leads.Abstractions.Models;
using GR.Crm.Organizations.Abstractions;
using GR.Crm.Organizations.Abstractions.Enums;
using GR.Crm.Organizations.Abstractions.Models;
using GR.Crm.Organizations.Abstractions.ViewModels.OrganizationsViewModels;
using GR.Identity.Abstractions;
using GR.Notifications.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;


namespace GR.Crm.Organizations
{
    public class OrganizationService : ICrmOrganizationService
    {

        #region Injectable

        /// <summary>
        /// Inject organization context
        /// </summary>
        private readonly ICrmOrganizationContext _organizationContext;

        /// <summary>
        /// Inject mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Inject notification
        /// </summary>
        private readonly INotify<GearRole> _notify;

        /// <summary>
        /// Inject address service
        /// </summary>
        private readonly IOrganizationAddressService _addressService;

        /// <summary>
        /// Inject organization helper service
        /// </summary>
        private readonly ICrmOrganizationHelperService _organizationHelperService;

        /// <summary>
        /// Inject crm lead service
        /// </summary>
        private readonly ILeadService<Lead> _leadService;

        private readonly IEmailContext _emailContext;

        #endregion


        private const string notificationTitle = "Organization Notification";
        private const string verifiedAfiliat = "Virified afiliat #{0} if exist in sistem.";

        public OrganizationService(ICrmOrganizationContext organizationContext,
            IMapper mapper,
            INotify<GearRole> notify,
            IOrganizationAddressService addressService,
            ICrmOrganizationHelperService organizationHelperService,
             ILeadService<Lead> leadService,
            IEmailContext emailContext)
        {
            _organizationContext = organizationContext;
            _mapper = mapper;
            _notify = notify;
            _addressService = addressService;
            _organizationHelperService = organizationHelperService;
            _leadService = leadService;
            _emailContext = emailContext;
        }

        /// <summary>
        /// Get all active organizations
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ResultModel<PagedResult<GetOrganizationViewModel>>> GetPaginatedOrganizationAsync(PageRequest request)
        {
            if (request == null)
            {
                return new InvalidParametersResultModel<PagedResult<GetOrganizationViewModel>>();
            }

            var query = _organizationContext.Organizations
                .Include(x => x.Stage)
                .Include(x => x.State)
                .Include(x => x.Contacts)
                .ThenInclude(x => x.ContactWebProfiles)
                .ThenInclude(x => x.WebProfile)
                .Include(i => i.Industry)
                .Include(i => i.Addresses)
                .ThenInclude(i => i.City)
                .ThenInclude(i => i.Region)
                .ThenInclude(i => i.Country)
                .Include(i => i.Employee)
                .Where(x => !x.IsDeleted || request.IncludeDeleted);

            var newListOfFilters = new List<PageRequestFilter>();
            var listOfCountryIds = new List<string>();

            var newRequest = new PageRequest()
            {
                Page = request.Page,
                PageSize = request.PageSize,
                Descending = request.Descending,
                Attribute = request.Attribute,
                IncludeDeleted = request.IncludeDeleted,
                GSearch = request.GSearch,
                RegexExpression = request.RegexExpression

            };

            foreach (var i in request.PageRequestFilters)
            {
                if ( i.Propriety == "countryId")
                {
                    listOfCountryIds.Add(i.Value);
                }
                else
                {
                    newListOfFilters.Add(i);
                }
            }
            if (listOfCountryIds.Any())
            { 
                var tempQuery =
                     from org in _organizationContext.Organizations
                     join addr in _organizationContext.OrganizationAddresses on org.Id equals addr.OrganizationId
                     join city in _organizationContext.Cities on addr.CityId equals city.Id
                     join region in _organizationContext.Regions on city.RegionId equals region.Id
                     join country in _organizationContext.Countries on region.CountryId equals country.Id
                     where(listOfCountryIds.Contains(country.Id.ToString()))
                     select org;

                query = tempQuery;
            }
      

            newRequest.PageRequestFilters = newListOfFilters;

            var pagedResult = await query.GetPagedAsync(newRequest);

            var map = pagedResult.Map(_mapper.Map<IEnumerable<GetOrganizationViewModel>>(pagedResult.Result));

            foreach(var org in map.Result)
            {
                org.EmailList = await _emailContext.Emails.Where(x => x.OrganizationId == org.Id).ToListAsync();
            }
            return new SuccessResultModel<PagedResult<GetOrganizationViewModel>>(map);
        }

        /// <summary>
        /// Find organization by id
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<GetOrganizationViewModel>> FindOrganizationByIdAsync(Guid? organizationId)
        {
            if (organizationId == null)
                return new InvalidParametersResultModel<GetOrganizationViewModel>();

            var organizationBd = await _organizationContext.Organizations
                .Include(x => x.Stage)
                .Include(x => x.State)
                .Include(x => x.Contacts)
                .ThenInclude(x => x.ContactWebProfiles)
                .ThenInclude(x => x.WebProfile)
                .Include(x => x.Contacts)
                .ThenInclude(x => x.PhoneList)
                .Include(i => i.Industry)
                .Include(i => i.Addresses)
                .ThenInclude(i => i.City)
                .ThenInclude(i => i.CrmCountry)
                .Include(i => i.Addresses)
                .ThenInclude(i => i.City)
                .ThenInclude(i => i.Region)
                .ThenInclude(i => i.Country)
                .Include(i => i.Employee)
                .Include(i => i.EmailList)
                .FirstOrDefaultAsync(x => x.Id == organizationId);

            if (organizationBd == null)
                return new NotFoundResultModel<GetOrganizationViewModel>();

            var organization = _mapper.Map<GetOrganizationViewModel>(organizationBd);
            return new SuccessResultModel<GetOrganizationViewModel>(organization);

        }


        /// <summary>
        /// Find organization by fiscal code
        /// </summary>
        /// <param name="fiscalCode"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<GetOrganizationViewModel>> GetOrganizationByFiscalCodeAsync(string fiscalCode)
        {
            if (string.IsNullOrEmpty(fiscalCode))
                return new InvalidParametersResultModel<GetOrganizationViewModel>();

            var organizationBd = await _organizationContext.Organizations
                .Include(x => x.Contacts)
                .ThenInclude(x => x.ContactWebProfiles)
                .ThenInclude(x => x.WebProfile)
                .Include(i => i.Industry)
                .Include(i => i.Addresses)
                .ThenInclude(i => i.City)
                .ThenInclude(i => i.Region)
                .Include(i => i.Employee)
                .Include(i => i.EmailList)
                .FirstOrDefaultAsync(x => string.Equals(x.FiscalCode.Trim(), fiscalCode.Trim(), StringComparison.CurrentCultureIgnoreCase));

            if (organizationBd == null)
                return new NotFoundResultModel<GetOrganizationViewModel>();

            return new SuccessResultModel<GetOrganizationViewModel>(_mapper.Map<GetOrganizationViewModel>(organizationBd));

        }


        /// <summary>
        /// Get all active organizations
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<GetOrganizationViewModel>>> GetAllActiveOrganizationsAsync(bool includeDeleted = false)
        {

            var query =  await _organizationContext.Organizations
                .Include(x => x.EmailList)
                .Include(x => x.Stage)
                .Include(x => x.State)
                .Include(x => x.Contacts)
                .ThenInclude(x => x.PhoneList)
                .Include(x => x.Contacts)
                .ThenInclude(x => x.ContactWebProfiles)
                .ThenInclude(x => x.WebProfile)
                .Include(i => i.Industry)
                .Include(i => i.Addresses)
                .ThenInclude(i => i.City)
                .ThenInclude(i => i.Region)
                .Include(i => i.Employee)
                .Where(x => !x.IsDeleted || includeDeleted)
                .OrderBy(n => n.Name)
                .ToListAsync();

            var listOrganizations = query.Select(async s => new GetOrganizationViewModel
            {
                Contacts = s.Contacts,
                Id = s.Id,
                Author = s.Author,
                TenantId = s.TenantId,
                Created = s.Created,
                Name = s.Name,
                Stage = s.Stage,
                StageId = s.StageId,
                StageChangeDate = s.StageChangeDate,
                DaysInStage = Convert.ToInt32((DateTime.UtcNow - s.StageChangeDate).TotalDays),
                State = s.State,
                StateId = s.StateId,
                IsDeleted = s.IsDeleted,
                EmailList = s.EmailList,
                Phone = !s.Phone.IsNullOrEmpty() ? s.Phone : s.Contacts.Any() && !s.Contacts.OrderBy(x => x.Created).FirstOrDefault(x => x.IsDeleted.Equals(false)).IsNull() ? s.Contacts.OrderBy(x => x.Created).FirstOrDefault(x => x.IsDeleted.Equals(false))?.PhoneList.FirstOrDefault()?.Phone : "No phone",
                LeadCount = (await _leadService.GetLeadsCountByOrganizationAsync(s.Id)).Result
            }).Select(s => s.Result).ToList();

            return new SuccessResultModel<IEnumerable<GetOrganizationViewModel>>(listOrganizations);
        }

        /// <summary>
        /// Delete organization async
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DeleteOrganizationAsync(Guid? organizationId)
        {
            if (organizationId == null)
                return new InvalidParametersResultModel();

            var emails = await _emailContext.Emails.Where(x => x.OrganizationId == organizationId).ToListAsync();
            if (emails.Count() > 0)
            {
                foreach (var email in emails)
                    _emailContext.Emails.Remove(email);
                await _emailContext.PushAsync();
            }


            var organization = await _organizationContext.Organizations
                .Include(i => i.EmailList)
                .FirstOrDefaultAsync(x => x.Id == organizationId);


            if (organization == null)
                return new NotFoundResultModel();

            _organizationContext.Organizations.Remove(organization);
            return await _organizationContext.PushAsync();
        }

        /// <summary>
        /// Deactivate organization
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DeactivateOrganizationAsync(Guid? organizationId) =>
            await _organizationContext.DisableRecordAsync<Organization>(organizationId);

        /// <summary>
        /// Activate organization
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> ActivateOrganizationAsync(Guid? organizationId) =>
            await _organizationContext.ActivateRecordAsync<Organization>(organizationId);

        /// <summary>
        /// add new organization async
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<Guid>> AddNewOrganizationAsync(OrganizationViewModel model)
        {
            if (model == null)
                return new NotFoundResultModel<Guid>();
                var newOrganization = _mapper.Map<Organization>(model);
                await _organizationContext.Organizations.AddAsync(newOrganization);
                var result = await _organizationContext.PushAsync();

                return result.Map(newOrganization.Id);
        }

        /// <summary>
        /// update exist client async
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<Guid>> UpdateOrganizationAsync(OrganizationViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel<Guid>();


            var organization = await _organizationContext.Organizations
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            if (organization == null)
                return new NotFoundResultModel<Guid>();

            organization.Name = model.Name;
            organization.Brand = model.Brand;
            if (organization.StageId != model.StageId)
            {
                organization.StageId = model.StageId;
                organization.StageChangeDate = DateTime.UtcNow;
            }
            organization.StateId = model.StateId;
            organization.Phone = model.Phone;
            organization.DialCode = model.DialCode;
            organization.WebSite = model.WebSite;
            organization.FiscalCode = model.FiscalCode;
            organization.IBANCode = model.IBANCode;
            organization.ResponsibleForPhoneNumber = model.ResponsibleForPhoneNumber;
            organization.Bank = model.Bank;
            organization.EmployeeId = model.EmployeeId;
            organization.IndustryId = model.IndustryId;
            organization.Description = model.Description;
            organization.CodSwift = model.CodSwift;
            organization.VitCode = model.VitCode;
            organization.DateOfFounding = model.DateOfFounding;

            _organizationContext.Organizations.Update(organization);
            var result = await _organizationContext.PushAsync();

            return new ResultModel<Guid> { IsSuccess = result.IsSuccess, Errors = result.Errors, Result = organization.Id };
        }
       
        /// <summary>
        /// Import organization 
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> ImportOrganizationAsync(IFormFile formFile)
        {

            var listExcel = new List<ExcelReadOrganizationViewModel>();

            using (var stream = new MemoryStream())
            {
                await formFile.CopyToAsync(stream);

                IWorkbook workbook;

                if (formFile.FileName.IndexOf(".xlsx", StringComparison.Ordinal) > 0)
                    workbook = WorkbookFactory.Create(stream);
                else if (formFile.FileName.IndexOf(".xls", StringComparison.Ordinal) > 0)
                    workbook = new HSSFWorkbook(stream);
                else
                    return new ResultModel
                    {
                        IsSuccess = false,
                        Errors = new List<IErrorModel> { new ErrorModel { Message = "File is not in excel format" } }
                    };

                var sheet = workbook.GetSheetAt(0);


                for (var item = sheet.FirstRowNum + 1; item <= sheet.LastRowNum; item++)
                {
                    var row = sheet.GetRow(item);
                    if (row == null)
                        continue;

                    if (string.IsNullOrEmpty(row.GetCell(3).CellStringValue())
                        || string.IsNullOrEmpty(row.GetCell(5).CellStringValue())
                        || string.IsNullOrEmpty(row.GetCell(7).CellStringValue()))
                        continue;

                    listExcel.Add(new ExcelReadOrganizationViewModel
                    {
                        FiscalCode = row.GetCell(3).CellStringValue(),
                        IBAN = row.GetCell(4).CellStringValue(),
                        Name = row.GetCell(5).CellStringValue(),
                        Address = row.GetCell(6).CellStringValue(),
                        Bank = row.GetCell(11).CellStringValue(),
                    });
                }
            }


            if (!listExcel.Any())
            {
                return new NotFoundResultModel();
            }

            await AddOrganizationFromExcelAsync(listExcel);

            return new ResultModel { IsSuccess = true };
        }


        /// <summary>
        /// Import organization from excel data 
        /// </summary>
        /// <param name="listOrganization"></param>
        /// <returns></returns>
        private async Task<ResultModel> AddOrganizationFromExcelAsync(IEnumerable<ExcelReadOrganizationViewModel> listOrganization)
        {

            if (listOrganization == null || !listOrganization.Any())
                return new InvalidParametersResultModel();


            var toResult = new ResultModel { IsSuccess = true };

            foreach (var organization in listOrganization)
            {
                var workCategory = new List<Guid>();

                var result = await AddNewOrganizationAsync(new OrganizationViewModel
                {
                    Bank = organization.Bank,
                    Name = organization.Name,
                    FiscalCode = organization.FiscalCode,
                    IBANCode = organization.IBAN,
                    Phone = organization.Phone,
                    DialCode = organization.DialCode,
                    StageId = _organizationContext.OrganizationStages.OrderBy(x => x.DisplayOrder).First().Id
                });

            }
            return toResult;
        }

        /// <summary>
        /// Move lead to another stage
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="stageId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> UpdateOrganizationStageAsync(Guid? organizationId, Guid? stageId, Guid? stateId)
        {
            if (stageId == null) return new InvalidParametersResultModel();
            var organization = await _organizationContext.Organizations
                .FirstOrDefaultAsync(x => x.Id == organizationId);
            
            if (organizationId == null) return new NotFoundResultModel();
            if (organization.StageId == stageId) return new SuccessResultModel<object>().ToBase();

            var stageRequest = await _organizationHelperService.GetStageByIdAsync(stageId);
            if (!stageRequest.IsSuccess) return stageRequest.ToBase();
            var stage = stageRequest.Result;

            organization.StageId = stage.Id;
            organization.StateId = stateId;
            organization.StageChangeDate = DateTime.UtcNow;

            _organizationContext.Organizations.Update(organization);

            var result = await _organizationContext.PushAsync();

            if (!result.IsSuccess) return result;

            return result;
        }

        /// <summary>
        /// Change organization state
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="stateId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> UpdateOrganizationStateAsync(Guid? organizationId, Guid? stateId)
        {
            if (organizationId == null || stateId == null) return new InvalidParametersResultModel();

            var organization = await _organizationContext.Organizations
                .Include(i => i.State)
                .FirstOrDefaultAsync(x => x.Id == organizationId);

            if (organization == null) return new NotFoundResultModel();

            if (organization.StateId == stateId) return new SuccessResultModel<object>().ToBase();

            organization.StateId = stateId.Value;

            var stateQuery = await _organizationContext.OrganizationStates
                .Include(x => x.Stages)
                .ThenInclude(x => x.Stage)
                .FirstOrDefaultAsync(x => x.Id.Equals(stateId));

            var lastStage = organization.StageId;
            organization.StageId = stateQuery.Stages.OrderBy(x=>x.Stage.DisplayOrder).FirstOrDefault(x => x.StateId == stateId.Value).StageId;

            if (lastStage != organization.StageId)
                organization.StageChangeDate = DateTime.UtcNow;

            _organizationContext.Organizations.Update(organization);

            var result = await _organizationContext.PushAsync();

            if (!result.IsSuccess) return result;
           
            return result;
        }

        #region Helper


        private bool ContainsString(string initial, string contains)
        {

            initial = initial.ToLower();
            contains = contains.ToLower();

            var replaceList = new Dictionary<string, string>() {
                {"ă", "a"},
                {"î", "i"},
                {"ș", "s"},
                {"ț", "t"},
                {"â", "a"}
            };


            foreach (var replace in replaceList)
            {
                initial = initial.Replace(replace.Key, replace.Value);
                contains = contains.Replace(replace.Key, replace.Value);
            }

            return initial.Contains(contains);
        }

        #endregion
    }
}
