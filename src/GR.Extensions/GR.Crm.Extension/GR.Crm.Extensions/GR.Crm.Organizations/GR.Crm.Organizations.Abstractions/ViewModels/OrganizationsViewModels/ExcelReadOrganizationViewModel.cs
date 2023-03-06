using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Crm.Organizations.Abstractions.ViewModels.OrganizationsViewModels
{
    public class ExcelReadOrganizationViewModel
    {
        /// <summary>
        /// Organization Name
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Fiscal code
        /// </summary>
        public virtual string FiscalCode { get; set; }

        /// <summary>
        /// IBAN Code
        /// </summary>
        public virtual string IBAN { get; set; }

        /// <summary>
        /// Bank
        /// </summary>
        public virtual string Bank { get; set; }

        /// <summary>
        /// Address
        /// </summary>
        public virtual string Address { get; set; }

        public string Phone { get; set; }

        public string DialCode { get; set; }

    }
}
