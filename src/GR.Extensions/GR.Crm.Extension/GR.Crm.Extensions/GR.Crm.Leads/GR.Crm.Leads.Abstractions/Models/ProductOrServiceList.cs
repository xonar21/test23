using GR.Audit.Abstractions.Attributes;
using GR.Audit.Abstractions.Enums;
using GR.Core;
using GR.Crm.Abstractions.Models;
using GR.Crm.Abstractions.Models.ProductConfiguration.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GR.Crm.Leads.Abstractions.Models
{
    [TrackEntity(Option = TrackEntityOption.SelectedFields)]
    public class ProductOrServiceList : BaseModel
    {
        [TrackField(Option = TrackFieldOption.Allow)]
        public Guid LeadId { get; set; }

        public virtual Lead Lead { get; set; }

        [TrackField(Option = TrackFieldOption.Allow)]
        public Guid ProductOrServiceId { get; set; }

        public virtual ProductOrService ProductOrService { get; set;}

        [TrackField(Option = TrackFieldOption.Allow)]
        public Guid? ProductTypeId { get; set; }

        public virtual ProductType ProductType { get; set; }

        [TrackField(Option = TrackFieldOption.Allow)]
        public Guid? TechnologyTypeId { get; set; }

        public virtual TechnologyType TechnologyType { get; set; }

        [TrackField(Option = TrackFieldOption.Allow)]
        public Guid? ServiceTypeId { get; set; }

        public virtual ServiceType ServiceType { get; set; }

        [TrackField(Option = TrackFieldOption.Allow)]
        public Guid? DevelopmentVariationId { get; set; }

        public virtual DevelopmentVariation DevelopmentVariation { get; set; }

        [TrackField(Option = TrackFieldOption.Allow)]
        public Guid? ConsultancyVariationId { get; set; }

        public virtual ConsultancyVariation ConsultancyVariation { get; set; }

        [TrackField(Option = TrackFieldOption.Allow)]
        public Guid? QAVariationId { get; set; }

        public virtual QAVariation QAVariation { get; set; }

        [TrackField(Option = TrackFieldOption.Allow)]
        public Guid? DesignVariationId { get; set; }

        public virtual DesignVariation DesignVariation { get; set; }

        [TrackField(Option = TrackFieldOption.Allow)]
        public Guid? DevelopementFrameworkId { get; set; }

        public virtual DevelopementFramework DevelopementFramework { get; set; }

        [TrackField(Option = TrackFieldOption.Allow)]
        public Guid? PMFrameworkId { get; set; }

        public virtual PMFramework PMFramework { get; set; }
    }
}
