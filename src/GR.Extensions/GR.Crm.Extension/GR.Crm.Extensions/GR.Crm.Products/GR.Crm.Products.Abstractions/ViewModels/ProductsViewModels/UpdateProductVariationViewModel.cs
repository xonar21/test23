using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Crm.Products.Abstractions.ViewModels.ProductsViewModels
{
    public class UpdateProductVariationViewModel
    {
        public virtual ProductVariationViewModel ProducVariation { get; set; }

        public virtual List<Guid> DeliverablesIds { get; set; }

        public virtual List<Guid> SelectedDeliverablesIds { get; set; }
    }
}
