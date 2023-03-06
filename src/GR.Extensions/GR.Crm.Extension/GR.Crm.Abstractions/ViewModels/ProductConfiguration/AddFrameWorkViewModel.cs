using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GR.Crm.Abstractions.ViewModels.ProductConfiguration
{
    public class AddFrameWorkViewModel
    {
        [Required]
        public string Name { get; set; }
    }
}
