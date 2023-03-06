using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GR.Crm.Products.Abstractions.ViewModels.ProductsViewModels
{
    public class AddProductVariationViewModel
    {
        [Required]
        public virtual string TechnologyStack { get; set; }

        public virtual string ProductType { get; set; }

        [Required]
        public virtual Guid ProductTemplateId { get; set; }


        public virtual List<Guid> SelectedDeliverablesIds { get; set; }
    }
}
