using System.ComponentModel.DataAnnotations;
using GR.Core;
using GR.Crm.Products.Abstractions.Models;

namespace GR.Crm.Products.Abstractions.ViewModels.ProductsViewModels
{
    public class GetProductViewModel : ProductTemplate
    {
        public virtual string CategoryName { get; set; }
    }
}
