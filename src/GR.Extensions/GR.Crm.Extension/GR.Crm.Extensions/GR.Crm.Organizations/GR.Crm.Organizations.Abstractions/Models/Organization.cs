using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GR.Core;
using GR.Crm.Emails.Abstractions.Models;
using GR.Audit.Abstractions.Attributes;
using GR.Audit.Abstractions.Enums;

namespace GR.Crm.Organizations.Abstractions.Models
{
    [TrackEntity(Option = TrackEntityOption.SelectedFields)]
    public class Organization : BaseModel
    {
        /// <summary>
        /// Client Name
        /// </summary>
        [Required]
        [MinLength(2)]
        [MaxLength(128)]
        [TrackField(Option = TrackFieldOption.Allow)]
        public virtual string Name { get; set; }

        /// <summary>
        /// Brand
        /// </summary>
        [MinLength(2)]
        [MaxLength(128)]
        [TrackField(Option = TrackFieldOption.Allow)]
        public virtual string Brand { get; set; }

        /// <summary>
        /// Reference to organization stage
        /// </summary>
        [TrackField(Option = TrackFieldOption.Allow)]
        public virtual Guid? StageId { get; set; }
        public OrganizationStage Stage { get; set; }

        public DateTime StageChangeDate { get; set; } =  DateTime.UtcNow;
        /// <summary>
        /// Reference to organization state
        /// </summary>
        [TrackField(Option = TrackFieldOption.Allow)]
        public virtual Guid? StateId { get; set; }
        public OrganizationState State { get; set; }

        /// <summary>
        /// Bank 
        /// </summary>
        [TrackField(Option = TrackFieldOption.Allow)]
        public virtual string Bank { get; set; }


        public virtual List<EmailList> EmailList { get; set; }

        /// <summary>
        /// Phone
        /// </summary>
        [MaxLength(50)]
        [TrackField(Option = TrackFieldOption.Allow)]
        public virtual string Phone { get; set; }

        public virtual string DialCode { get; set; } = "+373";

        /// <summary>
        /// Responsible for phone number
        /// </summary>
        [MinLength(2)]
        [MaxLength(128)]
        public virtual string ResponsibleForPhoneNumber { get; set; }

        /// <summary>
        /// Web site
        /// </summary>
        [MaxLength(50)]
        public virtual string WebSite { get; set; }

        /// <summary>
        /// Fiscal code
        /// </summary>
        [MaxLength(15)]
        [MinLength(6)]
        [TrackField(Option = TrackFieldOption.Allow)]
        public virtual string FiscalCode { get; set; }

        /// <summary>
        /// IBAN code
        /// </summary>
        [MaxLength(128)]
        [TrackField(Option = TrackFieldOption.Allow)]
        public virtual string IBANCode { get; set; }

        /// <summary>
        /// cod swift
        /// </summary>
        public virtual string CodSwift { get; set; }

        /// <summary>
        /// cod TVA
        /// </summary>
        public virtual string VitCode { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [TrackField(Option = TrackFieldOption.Allow)]
        public virtual string Description { get; set; }

        /// <summary>
        /// List contacts
        /// </summary>
        public virtual IEnumerable<Contact> Contacts { get; set; } = new List<Contact>();

        /// <summary>
        /// Industry Id
        /// </summary>
        [TrackField(Option = TrackFieldOption.Allow)]
        public virtual Guid? IndustryId { get; set; }


        /// <summary>
        /// Industry
        /// </summary>
        public Industry Industry { get; set; }

        /// <summary>
        /// Employees 
        /// </summary>
        public virtual Employee Employee { get; set; }

        /// <summary>
        /// Employee id
        /// </summary>
        [TrackField(Option = TrackFieldOption.Allow)]
        public virtual Guid? EmployeeId { get; set; }

        /// <summary>
        /// Addresses 
        /// </summary>
        public virtual IEnumerable<OrganizationAddress> Addresses { get; set; }

        /// <summary>
        /// Date of Founding
        /// </summary>
        public virtual DateTime? DateOfFounding { get; set; }

        /// <summary>
        /// Revenues 
        /// </summary>
        public virtual IEnumerable<Revenue> Revenues { get; set; } = new List<Revenue>();


    }
}
