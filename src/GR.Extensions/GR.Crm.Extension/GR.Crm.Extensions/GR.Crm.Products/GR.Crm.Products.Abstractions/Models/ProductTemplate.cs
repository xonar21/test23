using GR.Core;
using GR.Crm.Abstractions.Models;
using GR.Crm.Products.Abstractions.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GR.Crm.Products.Abstractions.Models
{
    public class ProductTemplate : BaseModel
    {

        /// <summary>
        /// Title
        /// </summary>
        [Required]
        public virtual string Name { get; set; }

        /// <summary>
        /// ProductCode
        /// </summary>
        [Required]
        public virtual string ProductCode { get; set; }

        /// <summary>
        /// Product Type (Product or Service)
        /// </summary>
        public virtual TypeOfProduct Type { get; set; } = TypeOfProduct.Product;

        /// <summary>
        /// Product Category
        /// </summary>

        public virtual Category Category { get; set; }

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
        public virtual ProductManufactories ProductManufactories { get; set; }

        public virtual Guid ProductManufactoriesId { get; set; }

        /// <summary>
        /// Selling Price
        /// </summary>
        public virtual decimal SellingPrice { get; set; }

        /// <summary>
        /// Currency reference
        /// </summary>
        public virtual Currency Currency { get; set; }

        public virtual string CurrencyCode { get; set; }

        public virtual IEnumerable<ProductVariation> ProductVariations { get; set; }

    }
}
