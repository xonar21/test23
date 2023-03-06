using GR.Core;
using GR.Crm.Abstractions.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace GR.Crm.Organizations.Abstractions.Models
{
    public class Revenue : BaseModel
    {
        /// <summary>
        /// Organization id
        /// </summary>
        [Required]
        public virtual Guid OrganizationId { get; set; }

        /// <summary>
        /// Organization
        /// </summary>
        public virtual Organization Organization { get; set; }

        /// <summary>
        /// Year
        /// </summary>
        [Required]
        public virtual DateTime Year { get; set; }

        /// <summary>
        /// Currency reference
        /// </summary>
        public virtual Currency Currency { get; set; }
        [Required]
        public virtual string CurrencyCode { get; set; }

        /// <summary>
        /// Amount
        /// </summary>
        public decimal Amount { get; set; }
    }
}
