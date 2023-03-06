using GR.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GR.Crm.Emails.Abstractions.Models
{
    public class EmailList : BaseModel
    {
        [Required]
        [MaxLength(50)]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        public Label? Label { get; set; }

        public Guid? ContactId { get; set; }

        public Guid? OrganizationId { get; set; }
    }
}
