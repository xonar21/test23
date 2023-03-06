using GR.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GR.Crm.Products.Abstractions.Models
{
    public class ProductVariation : BaseModel
    {
        [Required]
        public virtual string TechnologyStack { get; set; }

        public virtual string ProductType { get; set; }

        [Required]
        public virtual Guid ProductTemplateId { get; set; }

        public virtual ProductTemplate ProductTemplate { get; set; }

        public virtual IEnumerable<ProductDeliverables> ProductDeliverables { get; set; }
    }
}
