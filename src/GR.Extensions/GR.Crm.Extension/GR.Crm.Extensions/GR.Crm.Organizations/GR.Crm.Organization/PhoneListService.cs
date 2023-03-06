using AutoMapper;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.Core.Helpers.Responses;
using GR.Crm.Organizations.Abstractions;
using GR.Crm.Organizations.Abstractions.Models;
using GR.Crm.Organizations.Abstractions.ViewModels.PhoneListViewModels;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace GR.Crm.Organizations
{
    public class PhoneListService : ICrmPhoneListService
    {

        #region Injectable
        /// <summary>
        /// Inject context
        /// </summary>
        private readonly ICrmOrganizationContext _organizationContext;

        private readonly IMapper _mapper;
        #endregion

        public PhoneListService(ICrmOrganizationContext organizationContext,
            IMapper mapper)
        {
            _organizationContext = organizationContext;
            _mapper = mapper;
        }


        /// <summary>
        /// Add new phone
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<Guid>> AddNewPhoneAsync(AddPhoneViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel<Guid>();
            var newPhone = new PhoneList
            {
                DialCode = model.DialCode[0] == '+' ? model.DialCode : '+' + model.DialCode,
                CountryCode = model.CountryCode,
                Phone = model.Phone,
                ContactId = model.ContactId,
                Label = model.Label
            };

            await _organizationContext.PhoneLists.AddAsync(newPhone);
            var result = await _organizationContext.PushAsync();

            return new ResultModel<Guid> { IsSuccess = result.IsSuccess, Errors = result.Errors, Result = newPhone.Id };
        }

        /// <summary>
        /// Add phone range
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> AddPhoneRangeAsync(List<AddPhoneViewModel> model)
        {
            if (model == null)
                return new InvalidParametersResultModel();
            foreach(var phone in model)
            {
                var newPhone = new PhoneList
                {
                    DialCode = phone.DialCode[0] == '+' ? phone.DialCode : '+' + phone.DialCode, 
                    CountryCode = phone.CountryCode,
                    Phone = phone.Phone,
                    ContactId = phone.ContactId,
                    Label =phone.Label 
                };
                await _organizationContext.PhoneLists.AddAsync(newPhone); 
            }
            
            var result = await _organizationContext.PushAsync();

            return new ResultModel { IsSuccess = result.IsSuccess, Errors = result.Errors};
        }

        /// <summary>
        /// Delete permanently phone async
        /// </summary>
        /// <param name="phoneId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DeletePhoneAsync(Guid? phoneId)
        {
            if (phoneId == null)
                return new InvalidParametersResultModel();

            var phone = await _organizationContext.PhoneLists.FirstOrDefaultAsync(x => x.Id == phoneId);

            if (phone == null)
                return new NotFoundResultModel();

            _organizationContext.PhoneLists.Remove(phone);
            return await _organizationContext.PushAsync();
        }

        public async Task<ResultModel<List<string>>> GetAllPhoneLabelsAsync()
        {
            var labels = GetEnumList<Label>();
            return new SuccessResultModel<List<string>>(labels);
        }

        /// <summary>
        /// Get all phones
        /// </summary>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<GetPhoneViewModel>>> GetAllPhonesAsync(bool includeDeleted)
        {
            var listPhones = await _organizationContext.PhoneLists
                .Where(x => !x.IsDeleted || includeDeleted)
                .ToListAsync();
            return new SuccessResultModel<IEnumerable<GetPhoneViewModel>>(listPhones.Adapt<IEnumerable<GetPhoneViewModel>>());
        }

        /// <summary>
        /// Get phone by id
        /// </summary>
        /// <param name="phoneId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<GetPhoneViewModel>> GetPhoneByIdAsync(Guid? phoneId)
        {
            if (phoneId == null)
                return new InvalidParametersResultModel<GetPhoneViewModel>();

            var phone = await _organizationContext.PhoneLists
                .FirstOrDefaultAsync(x => x.Id == phoneId);
            if (phone == null)
                return new NotFoundResultModel<GetPhoneViewModel>();
            return new SuccessResultModel<GetPhoneViewModel>(_mapper.Map<GetPhoneViewModel>(phone));
        }

        /// <summary>
        /// Get phone by contactId
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<GetPhoneViewModel>>> GetPhonesByContactIdAsync(Guid? contactId, bool includeDeleted = false)
        {
            if (contactId == null)
                return new InvalidParametersResultModel<IEnumerable<GetPhoneViewModel>>();

            var listPhones = await _organizationContext.PhoneLists
                .Where(x => x.ContactId == contactId && (!x.IsDeleted || includeDeleted))
                .ToListAsync();

            return new SuccessResultModel<IEnumerable<GetPhoneViewModel>>(listPhones.Adapt<IEnumerable<GetPhoneViewModel>>());
        }

        /// <summary>
        /// Phone contact
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<Guid>> UpdatePhoneAsync(PhoneViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel<Guid>();

            PhoneList phone = _organizationContext.PhoneLists
                .FirstOrDefaultAsync(x => x.Id == model.Id).Result;

            if (phone == null)
                return new NotFoundResultModel<Guid>();
            
            phone.DialCode = phone.DialCode[0] == '+' ? phone.DialCode : '+' + phone.DialCode;
            phone.CountryCode = model.CountryCode;
            phone.Phone = model.Phone;
            phone.ContactId = model.ContactId;
            phone.Label = model.Label;

            _organizationContext.PhoneLists.Update(phone);
            var result = await _organizationContext.PushAsync();

            return new ResultModel<Guid>
            { IsSuccess = result.IsSuccess, Errors = result.Errors, Result = phone.Id };
        }

        /// <summary>
        /// Update range Phone contact
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> UpdateRangePhoneAsync(List<PhoneViewModel> model)
        {
            if (model == null)
                return new InvalidParametersResultModel();

            var phoneList = new List<PhoneList>();

            foreach(var phone in model)
            {
                var phoneFromDb = await _organizationContext.PhoneLists.FirstOrDefaultAsync(x => x.Id == phone.Id);
                if (phoneFromDb == null)
                    return new NotFoundResultModel();

                phoneFromDb.DialCode = phone.DialCode[0] == '+' ? phone.DialCode : '+' + phone.DialCode;
                phoneFromDb.CountryCode = phone.CountryCode;
                phoneFromDb.Phone = phone.Phone;
                phoneFromDb.ContactId = phone.ContactId;
                phoneFromDb.Label = phone.Label;
                phoneList.Add(phoneFromDb);
            }

            _organizationContext.PhoneLists.UpdateRange(phoneList);
            var result = await _organizationContext.PushAsync();

            return new ResultModel { IsSuccess = result.IsSuccess, Errors = result.Errors};
        }

        #region Helper 
        private static List<string> GetEnumList<T>()
        {
            return Enum.GetNames(typeof(T))
                   .Select(s => s).ToList();
        }
        #endregion
    }
}
