using GR.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GR.Crm.Products.Abstractions.Models
{
    public class ProductManufactories : BaseModel
    {
        /// <summary>
        /// Name
        /// </summary>
        [Required]
        public virtual string Name { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [Required]
        public virtual string Description { get; set; }
    }
}
