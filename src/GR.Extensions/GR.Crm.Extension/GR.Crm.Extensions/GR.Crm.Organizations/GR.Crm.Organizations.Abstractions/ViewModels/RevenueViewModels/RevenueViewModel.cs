using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GR.Crm.Organizations.Abstractions.ViewModels.RevenueViewModels
{
    public class RevenueViewModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public virtual Guid Id { get; set; }

        /// <summary>
        /// Organization Id
        /// </summary>
        [Required]
        public virtual Guid OrganizationId { get; set; }

        /// <summary>
        /// Year
        /// </summary>
        public virtual DateTime Year { get; set; }

        /// <summary>
        /// Currency reference
        /// </summary>
        public virtual string CurrencyCode { get; set; }

        /// <summary>
        /// Amount
        /// </summary>
        public decimal Amount { get; set; }
    }
}
