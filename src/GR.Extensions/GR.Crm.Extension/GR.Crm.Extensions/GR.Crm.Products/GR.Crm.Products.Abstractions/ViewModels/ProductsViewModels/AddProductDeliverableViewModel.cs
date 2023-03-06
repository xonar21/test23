using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GR.Crm.Products.Abstractions.ViewModels.ProductsViewModels
{
    public class AddProductDeliverableViewModel
    {
        [Required]
        public virtual string Name { get; set; }

        public virtual Guid? ProductVariationId { get; set; }
    }
}
