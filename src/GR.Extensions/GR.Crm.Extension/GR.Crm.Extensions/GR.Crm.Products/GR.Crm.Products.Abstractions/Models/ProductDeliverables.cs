using GR.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GR.Crm.Products.Abstractions.Models
{
    public class ProductDeliverables: BaseModel
    {
        [Required]
        public virtual string  Name { get; set; }

        public virtual Guid? ProductVariationId { get; set; }

        public virtual ProductVariation ProductVariation { get; set; }
    }
}
