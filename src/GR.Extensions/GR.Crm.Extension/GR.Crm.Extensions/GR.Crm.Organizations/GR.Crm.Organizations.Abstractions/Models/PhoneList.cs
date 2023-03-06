using GR.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GR.Crm.Organizations.Abstractions.Models
{
    public class PhoneList : BaseModel
    {
        /// <summary>
        /// Phone
        /// </summary>
        [Required]
        [MinLength(8)]
        public string Phone { get; set; }

        [Required]
        public string DialCode { get; set; }

        [Required]
        public string CountryCode { get; set; }

        public virtual Label? Label { get; set; }

        /// <summary>
        /// Contact Id
        /// </summary>
        [Required]
        public virtual Guid ContactId { get; set; }


        /// <summary>
        /// Contact
        /// </summary>
        public Contact Contact { get; set; }
    }
}
