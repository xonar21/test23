using System;
using Gr.Crm.Products.Abstractions.Models;
using GR.Core;
using GR.Crm.Leads.Abstractions.Models;
using GR.Crm.Organizations.Abstractions.Models;
using GR.Crm.Products.Abstractions.Models;

namespace GR.Crm.Payments.Abstractions.Models
{
    public class PaymentMapped : BaseModel
    {
        /// <summary>
        /// Payment reference
        /// </summary>
        public virtual Guid? PaymentId { get; set; }

        public virtual Payment Payment { get; set; }

        /// <summary>
        /// Organization reference
        /// </summary>
        public virtual Guid? OrganizationId { get; set; }

        public virtual Organization Organization { get; set; }


        /// <summary>
        /// Product Reference
        /// </summary>
        public virtual Guid? ProductId { get; set; }

        public virtual ProductTemplate Product { get; set; }

        /// <summary>
        /// Payment code reference
        /// </summary>
        public virtual PaymentCode PaymentCode { get; set; }
        public virtual Guid PaymentCodeId { get; set; }
    }
}
