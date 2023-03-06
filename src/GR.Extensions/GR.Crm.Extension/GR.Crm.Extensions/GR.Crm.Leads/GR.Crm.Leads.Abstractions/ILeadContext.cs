using GR.Crm.Leads.Abstractions.Models;
using GR.Crm.Organizations.Abstractions;
using GR.Crm.PipeLines.Abstractions;
using GR.Crm.Products.Abstractions.Models;
using Microsoft.EntityFrameworkCore;

namespace GR.Crm.Leads.Abstractions
{
    public interface ILeadContext<TLead> : ICrmOrganizationContext, IPipeLineContext
        where TLead : Lead
    {
        /// <summary>
        /// Leads
        /// </summary>
        DbSet<TLead> Leads { get; set; }

        /// <summary>
        /// States
        /// </summary>
        DbSet<LeadState> States { get; set; }

        /// <summary>
        /// Agreements
        /// </summary>
        DbSet<Agreement> Agreements { get; set; }

        /// <summary>
        /// ProductTemplate
        /// </summary>
        DbSet<ProductTemplate> Products { get; set; }

        /// <summary>
        /// ProductVariations
        /// </summary>
        DbSet<ProductVariation> ProductVariations { get; set; }

        DbSet<ProductDeliverables> ProductDeliverables { get; set; }

        /// <summary>
        /// Categories
        /// </summary>
        DbSet<Category> Categories { get; set; }

        /// <summary>
        /// Agreements
        /// </summary>
        DbSet<ProductManufactories> ProductManufactories { get; set; }

        /// <summary>
        /// Lead files
        /// </summary>
        DbSet<LeadFile> LeadFiles { get; set; }


        /// <summary>
        /// NoGoStates
        /// </summary>
        DbSet<NoGoState> NoGoStates { get; set; }

        DbSet<LeadStateStage> LeadStateStage { get; set; }

        DbSet<LeadContact> LeadsContacts { get; set; }

        DbSet<ProductOrServiceList> ProductOrServiceLists { get; set; }
    }
}