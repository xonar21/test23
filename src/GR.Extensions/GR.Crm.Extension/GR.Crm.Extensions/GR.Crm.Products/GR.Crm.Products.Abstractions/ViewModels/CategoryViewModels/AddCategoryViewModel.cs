using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GR.Crm.Products.Abstractions.ViewModels.CategoryViewModels
{
    public class AddCategoryViewModel
    {
        public virtual Guid? Id { get; set; }

        /// <summary>
        /// Category name
        /// </summary>
        [Required]
        public virtual string Name { get; set; }

        /// <summary>
        /// Display name
        /// </summary>
        [Required]
        public virtual string DisplayName { get; set; }

        /// <summary>
        /// Category Description
        /// </summary>
        [Required]
        public virtual string Description { get; set; }

        /// <summary>
        /// Category Display Order
        /// </summary>
        [Required]
        public virtual int DisplayOrder { get; set; }

        /// <summary>
        /// Parent Category Id
        /// </summary>
        public virtual Guid? ParentCategoryId { get; set; }

        /// <summary>
        /// Category Is Published
        /// </summary>
        [Required]
        public virtual bool IsPublished { get; set; }
    }
}
