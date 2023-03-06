using GR.Crm.Abstractions.Models;
using GR.Crm.Products.Abstractions.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GR.Crm.Products.Abstractions.ViewModels.ProductsViewModels
{
    public class AddProductViewModel
    {
        public virtual Guid? Id { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        [Required]
        public virtual string Name { get; set; }

        /// <summary>
        /// ProductCode
        /// </summary>
        public virtual string ProductCode { get; set; }

        /// <summary>
        /// Product Type (Product or Service)
        /// </summary>
        [Required]
        public virtual TypeOfProduct Type { get; set; } = TypeOfProduct.Product;

        /// <summary>
        /// Category
        /// </summary>
        public virtual Guid CategoryId { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// EAN
        /// </summary>
        [Required]
        public virtual string EAN { get; set; }

        /// <summary>
        /// Warranty
        /// </summary>
        public virtual string Warranty { get; set; }

        /// <summary>
        /// CanBeSold
        /// </summary>
        public virtual bool CanBeSold { get; set; }

        /// <summary>
        /// Product Manufactory
        /// </summary>
        public virtual Guid ProductManufactoriesId { get; set; }

        /// <summary>
        /// Selling Price
        /// </summary>
        [Required]
        public virtual decimal SellingPrice { get; set; }

        /// <summary>
        /// Currency reference
        /// </summary>
        public virtual string CurrencyCode { get; set; }
    }
}
