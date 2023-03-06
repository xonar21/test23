using GR.Crm.Products.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Gr.Crm.Products.Abstractions.Models
{
    public class ProductCategory
    {

        public virtual Guid Id { get; set; }

        /// <summary>
        /// Reference to product
        /// </summary>
        public virtual ProductTemplate Product { get; set; }
        [Required]
        public virtual Guid ProductId { get; set; }

        /// <summary>
        /// Reference to category
        /// </summary>
        public virtual Category Category { get; set; }
        [Required]
        public virtual Guid CategoryId { get; set; }
    }
}
