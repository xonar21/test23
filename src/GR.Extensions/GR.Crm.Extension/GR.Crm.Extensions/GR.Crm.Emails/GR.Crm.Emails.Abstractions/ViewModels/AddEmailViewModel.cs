using GR.Crm.Emails.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GR.Crm.Emails.Abstractions.ViewModels
{
    public class AddEmailViewModel
    {
        [Required]
        [MaxLength(50)]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        public Label Label { get; set; } = Label.General;

        public Guid? ContactId { get; set; }

        public Guid? OrganizationId { get; set; }

        public Guid? LeadId { get; set; }
    }
}
