using GR.Crm.Organizations.Abstractions.ViewModels.OrganizationsViewModels;
using System;
using System.ComponentModel.DataAnnotations;

namespace GR.Crm.Organizations.Abstractions.Extensions
{
    class ValidationOrganizationDateTimeAttributeExtensions : ValidationAttribute
    {

        public ValidationOrganizationDateTimeAttributeExtensions(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        private new string ErrorMessage { get; set; }


        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var date = (DateTime)value;
            var organization = (OrganizationViewModel)validationContext.ObjectInstance;

            return date.Date >= organization.DateOfFounding.Value.Date ? ValidationResult.Success : new ValidationResult(ErrorMessage);
        }
    }
}
