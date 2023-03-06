using GR.Crm.Organizations.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GR.Crm.Organizations.Abstractions.ViewModels.PhoneListViewModels
{
    public class AddPhoneViewModel
    {
        [Required]
        [MinLength(8)]
        public string Phone { get; set; }

        [Required]
        public string DialCode { get; set; }

        [Required]
        public string CountryCode { get; set; }

        public virtual Label Label { get; set; } = Label.Main;

        /// <summary>
        /// ContactId
        /// </summary>
        [Required]
        public virtual Guid ContactId { get; set; }
    }
}
