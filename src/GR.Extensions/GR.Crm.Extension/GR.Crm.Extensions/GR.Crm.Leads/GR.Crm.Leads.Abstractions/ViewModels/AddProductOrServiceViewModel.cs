using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GR.Crm.Leads.Abstractions.ViewModels
{
    public class AddProductOrServiceViewModel
    {
        public Guid LeadId { get; set; }

        public Guid ProductOrServiceId { get; set; }

        public Guid? ProductTypeId { get; set; }

        public Guid? TechnologyTypeId { get; set; }

        public Guid? ServiceTypeId { get; set; }

        public Guid? DevelopmentVariationId { get; set; }

        public Guid? ConsultancyVariationId { get; set; }

        public Guid? QAVariationId { get; set; }

        public Guid? DesignVariationId { get; set; }

        public Guid? DevelopementFrameworkId { get; set; }

        public Guid? PMFrameworkId { get; set; }
    }
}
