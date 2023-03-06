using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;
using AutoMapper;
using GR.Audit.Abstractions;
using GR.Core.Abstractions;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Core.Helpers.Responses;
using GR.Crm.Abstractions;
using GR.Crm.Abstractions.Models;
using GR.Crm.Abstractions.ViewModels.AuditViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace GR.Crm
{
    public class CrmService : ICrmService
    {
        #region Injectable

        /// <summary>
        /// Inject context
        /// </summary>
        private readonly ICrmContext _context;

        /// <summary>
        /// Inject mapper
        /// </summary>
        /// <param name="context"></param>
        private readonly IMapper _mapper;

        private readonly IConfiguration _configuration;

        private readonly IWritableOptions<GeneralConfigurations> _writableOptions;

        private readonly ITrackerDbContext _trackerDbContext;

        #endregion

        public CrmService(ICrmContext context,
            IMapper mapper,
            IConfiguration configuration,
            IWritableOptions<GeneralConfigurations> writableOptions,
            ITrackerDbContext trackerDbContext)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
            _writableOptions = writableOptions;
            _trackerDbContext = trackerDbContext;
        }

        

        /// <summary>
        /// Get all currencies
        /// </summary>
        /// <returns></returns>
        public async Task<ResultModel<IEnumerable<Currency>>> GetAllCurrenciesAsync()
        {
            var data = await _context.Currencies
                .AsNoTracking()
                .ToListAsync();
            return new SuccessResultModel<IEnumerable<Currency>>(data);
        }
        


        /// <summary>
        /// GetGeneral Configurations
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResultModel<GeneralConfigurations>> GetGeneralConfigurations()
        {
            var general = new GeneralConfigurations 
                            { CurrencyCode = _configuration.GetSection("GeneralConfigurations").GetSection("CurrencyCode").Value };

            if (general.CurrencyCode == "")
            {
                return new ResultModel<GeneralConfigurations>
                {
                    IsSuccess = true,
                    Errors = new List<IErrorModel> { new ErrorModel { Message = "There is no general configuration at the momemnt!" } },
                    Result = null
                };
            }

            return new ResultModel<GeneralConfigurations> {IsSuccess = true, Result = general};
        }



        /// <summary>
        /// UpdateGeneralConfigurations
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResultModel> UpdateGeneralConfigurations(GeneralConfigurations model)
        {
            if (model == null)
                return new InvalidParametersResultModel();

            _writableOptions.Update(opt =>
            {
                opt.CurrencyCode = model.CurrencyCode;
            });

            return new ResultModel { IsSuccess = true};
        }


        ///<summary>
        /// GetOfficialCourseRateFromBNM 
        /// </summary>
        public async Task<ResultModel<List<Valute>>> GetOfficialCourseRateFromBNMAsync(string date)//date format dd.mm.yyyy
        {
            string url = _configuration.GetSection("BNMCurrencyUrl").GetSection("Url").Value + date;
            using (HttpClient client = new HttpClient())
            {
                var stringResponse = await client.GetStringAsync(url);
                StringReader XMLStringContent = new StringReader(GetXMLContent(stringResponse));
                var serializer = new XmlSerializer(typeof(List<Valute>), new XmlRootAttribute("ValCurs"));
                List<Valute> valutes = (List<Valute>)serializer.Deserialize(XMLStringContent);
                return new ResultModel<List<Valute>> { IsSuccess = true, Result = valutes };
            }
        }
        
        public async Task<decimal> ConvertCurrencyToDefaultCurrencyAsync(string currentCurrency)
        {
            var currencyList = await GetOfficialCourseRateFromBNMAsync(DateTime.UtcNow.ToString("dd.MM.yyyy"));
            string defaultCurrency = _configuration.GetSection("GeneralConfigurations").GetSection("CurrencyCode").Value;

            var defaultValute = currencyList.Result.FirstOrDefault(x => x.CharCode == defaultCurrency);
            var currentValute = currencyList.Result.FirstOrDefault(x => x.CharCode == currentCurrency);

            if (defaultValute == null)
                defaultValute = new Valute { Value = 1 };

            if (currentValute == null)
                currentValute = new Valute { Value = 1 };

            return currentValute.Value / defaultValute.Value;
            
        }

        public async Task<decimal> ConvertCurrencyToEURAsync(string currentCurrency)
        {
            var currencyList = await GetOfficialCourseRateFromBNMAsync(DateTime.UtcNow.ToString("dd.MM.yyyy"));
            string defaultCurrency = "EUR";

            var defaultValute = currencyList.Result.FirstOrDefault(x => x.CharCode == defaultCurrency);
            var currentValute = currencyList.Result.FirstOrDefault(x => x.CharCode == currentCurrency);

            if (defaultValute == null)
                defaultValute = new Valute { Value = 1 };

            if (currentValute == null)
                currentValute = new Valute { Value = 1 };

            return currentValute.Value / defaultValute.Value;

        }

        public async Task<ResultModel<PagedResult<GetPaginatedAuditViewModel>>> GetPaginatedAuditAsync(PageRequest request, Guid RecordId)
        {
            if (request == null)
            {
                return new InvalidParametersResultModel<PagedResult<GetPaginatedAuditViewModel>>();
            }

            var query = await _trackerDbContext.TrackAudits
                .Include(x => x.AuditDetailses)
                .OrderBy(x => x.Created)
                .Where(x => (!x.IsDeleted || request.IncludeDeleted) && x.RecordId == RecordId)
                .GetPagedAsync(request);

            var map = query.Map(_mapper.Map<IEnumerable<GetPaginatedAuditViewModel>>(query.Result.Reverse()));

            return new SuccessResultModel<PagedResult<GetPaginatedAuditViewModel>>(map);
        }

        #region Helpers
        public string GetXMLContent(string httpClientStringResponse)
        {
            if (!httpClientStringResponse.IsNullOrEmpty())
            {

                var start = httpClientStringResponse.IndexOf("<ValCurs");
                var end = httpClientStringResponse.Length - 1;
                if(start != -1) 
                {
                    //subtract </ValCurs>
                    var XMLContent = httpClientStringResponse.Substring(start, end - start);
                    return XMLContent;
                }
                return "";
            }

            return "";
        }
        #endregion
    }
}
