using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Crm.Abstractions.Models;
using GR.Crm.Abstractions.ViewModels.AuditViewModels;

namespace GR.Crm.Abstractions
{
    public interface ICrmService
    {
        /// <summary>
        /// Get all currencies
        /// </summary>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<Currency>>> GetAllCurrenciesAsync();


        ///<summary>
        ///Update general configurations
        /// </summary>
        Task<ResultModel> UpdateGeneralConfigurations(GeneralConfigurations model);


        ///<summary>
        ///Get general configurations
        /// </summary>
        Task<ResultModel<GeneralConfigurations>> GetGeneralConfigurations();

        Task<ResultModel<List<Valute>>> GetOfficialCourseRateFromBNMAsync(string date);

        Task<decimal> ConvertCurrencyToDefaultCurrencyAsync(string currentCurrency);

        Task<decimal> ConvertCurrencyToEURAsync(string currentCurrency);

        Task<ResultModel<PagedResult<GetPaginatedAuditViewModel>>> GetPaginatedAuditAsync(PageRequest request, Guid RecordId);
    }
}