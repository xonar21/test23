using GR.Core.Helpers;
using GR.Crm.Organizations.Abstractions.Models;
using GR.Crm.Organizations.Abstractions.ViewModels.PhoneListViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GR.Crm.Organizations.Abstractions
{
    public interface ICrmPhoneListService
    {
        /// <summary>
        /// Get all Phone
        /// </summary>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<GetPhoneViewModel>>> GetAllPhonesAsync(bool includeDeleted);

        /// <summary>
        /// Get phone by id
        /// </summary>
        /// <param name="phoneId"></param>
        /// <returns></returns>
        Task<ResultModel<GetPhoneViewModel>> GetPhoneByIdAsync(Guid? phoneId);

        /// <summary>
        /// Get phones by contactId
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<GetPhoneViewModel>>> GetPhonesByContactIdAsync(Guid? contactId, bool includeDeleted = false);

        /// <summary>
        /// Add new phone
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<Guid>> AddNewPhoneAsync(AddPhoneViewModel model);

        /// <summary>
        /// Update phone
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<Guid>> UpdatePhoneAsync(PhoneViewModel model);

        /// <summary>
        /// Delete permanently phone async
        /// </summary>
        /// <param name="phoneId"></param>
        /// <returns></returns>
        Task<ResultModel> DeletePhoneAsync(Guid? phoneId);

        /// <summary>
        /// Add phone range
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel> AddPhoneRangeAsync(List<AddPhoneViewModel> model);

        /// <summary>
        /// Update range Phone contact
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel> UpdateRangePhoneAsync(List<PhoneViewModel> model);

        /// <summary>
        /// Get all phone labels
        /// </summary>
        /// <returns></returns>
        Task<ResultModel<List<string>>> GetAllPhoneLabelsAsync();
    }
}
