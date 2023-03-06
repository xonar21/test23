using GR.Crm.Marketing.Abstractions.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GR.Crm.Marketing.Abstractions.Extensions
{
    public class ValidationCampaignDateTimeAttributeExtensions : ValidationAttribute
    {

        public ValidationCampaignDateTimeAttributeExtensions(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        private new string ErrorMessage { get; set; }


        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var date = (DateTime)value;
            var campaign = (CampaignViewModel)validationContext.ObjectInstance;

            return date.Date >= campaign.StartDate.Date ? ValidationResult.Success : new ValidationResult(ErrorMessage);
        }

    }
}
