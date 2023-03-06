using AutoMapper;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.Core.Helpers.Responses;
using GR.Crm.Emails.Abstractions;
using GR.Crm.Emails.Abstractions.Models;
using GR.Crm.Emails.Abstractions.ViewModels;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GR.Crm.Emails.Infrastructure
{
    public class EmailService : IEmailService
    {
        #region Injectable
        private readonly IEmailContext _emailContext;

        private readonly IMapper _mapper;
        #endregion

        public EmailService(IMapper mapper,
            IEmailContext emailContext)
        {
            _mapper = mapper;
            _emailContext = emailContext;
        }

        public async Task<ResultModel> AddEmailRangeAsync(List<AddEmailViewModel> model)
        {
            if (model == null)
                return new InvalidParametersResultModel();

            foreach (var email in model)
            {
                var newEmail = new EmailList
                {
                    Email = email.Email,
                    ContactId = email.ContactId,
                    OrganizationId = email.OrganizationId,
                    Label = email.Label
                };

                await _emailContext.Emails.AddAsync(newEmail);
            }

            var result = await _emailContext.PushAsync();

            return new ResultModel { IsSuccess = result.IsSuccess, Errors = result.Errors };
        }

        public async Task<ResultModel<Guid>> AddNewEmailAsync(AddEmailViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel<Guid>();

            var newEmail = new EmailList
            {
                Email = model.Email,
                ContactId = model.ContactId,
                OrganizationId = model.OrganizationId,
                Label = model.Label
            };

            await _emailContext.Emails.AddAsync(newEmail);
            var result = await _emailContext.PushAsync();

            return new ResultModel<Guid> { IsSuccess = result.IsSuccess, Errors = result.Errors, Result = newEmail.Id };
        }

        public async Task<ResultModel> DeleteEmailAsync(Guid? emailId)
        {
            if (emailId == null)
                return new InvalidParametersResultModel();

            var email = await _emailContext.Emails.FirstOrDefaultAsync(x => x.Id == emailId);

            if (email == null)
                return new NotFoundResultModel();

            _emailContext.Emails.Remove(email);
            return await _emailContext.PushAsync();
        }

        public async Task<ResultModel<List<string>>> GetAllEmailLabelsAsync()
        {
            var labels = GetEnumList<Label>();
            return new SuccessResultModel<List<string>>(labels);
        }

        public async Task<ResultModel<IEnumerable<EmailViewModel>>> GetAllEmailsAsync(bool includeDeleted)
        {
            var listEmails = await _emailContext.Emails
                .Where(x => !x.IsDeleted || includeDeleted)
                .ToListAsync();

            return new SuccessResultModel<IEnumerable<EmailViewModel>>(listEmails.Adapt<IEnumerable<EmailViewModel>>());
        }

        public async Task<ResultModel<EmailViewModel>> GetEmailByIdAsync(Guid? emailId)
        {
            if (emailId == null)
                return new InvalidParametersResultModel<EmailViewModel>();

            var email = await _emailContext.Emails
                .FirstOrDefaultAsync(x => x.Id == emailId);

            if (email == null)
                return new InvalidParametersResultModel<EmailViewModel>();

            return new SuccessResultModel<EmailViewModel>(_mapper.Map<EmailViewModel>(email));
        }

        public async Task<ResultModel<IEnumerable<EmailViewModel>>> GetEmailsByContactIdAsync(Guid? contactId, bool includeDeleted = false)
        {
            if (contactId == null)
                return new InvalidParametersResultModel<IEnumerable<EmailViewModel>>();

            var emailList = await _emailContext.Emails
                .Where(x => x.ContactId == contactId)
                .ToListAsync();

            return new SuccessResultModel<IEnumerable<EmailViewModel>>(emailList.Adapt<IEnumerable<EmailViewModel>>());

        }

        public async Task<ResultModel<IEnumerable<EmailViewModel>>> GetEmailsByOrganizationIdAsync(Guid? organizationId, bool includeDeleted = false)
        {
            if (organizationId == null)
                return new InvalidParametersResultModel<IEnumerable<EmailViewModel>>();

            var emailList = await _emailContext.Emails
                .Where(x => x.OrganizationId == organizationId)
                .ToListAsync();

            return new SuccessResultModel<IEnumerable<EmailViewModel>>(emailList.Adapt<IEnumerable<EmailViewModel>>());
        }

        public async Task<ResultModel> UpdateEmailAsync(EmailViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel();

            var email = await _emailContext.Emails.FirstOrDefaultAsync(x => x.Id == model.Id);

            if (email == null)
                return new InvalidParametersResultModel();

            email.Email = model.Email;
            email.Label = model.Label;

            _emailContext.Emails.Update(email);

            return await _emailContext.PushAsync();
        }

        public async Task<ResultModel> UpdateRangeEmailAsync(List<EmailViewModel> model)
        {
            if (model == null)
                return new InvalidParametersResultModel();

            var emailListToUpdate = new List<EmailList>();

            foreach(var email in model)
            {
                var emailToUpdate = _emailContext.Emails.FirstOrDefault(x => x.Id == email.Id);

                if (emailToUpdate == null)
                    return new InvalidParametersResultModel();

                emailToUpdate.Email = email.Email;
                emailToUpdate.Label = email.Label;

                emailListToUpdate.Add(emailToUpdate);
            }
            _emailContext.Emails.UpdateRange(emailListToUpdate);

            return await _emailContext.PushAsync();
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
