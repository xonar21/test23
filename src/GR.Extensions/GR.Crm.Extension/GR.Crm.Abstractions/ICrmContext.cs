using GR.Core.Abstractions;
using GR.Crm.Abstractions.Models;
using GR.Crm.Abstractions.Models.ProductConfiguration.Services;
using Microsoft.EntityFrameworkCore;

namespace GR.Crm.Abstractions
{
    public interface ICrmContext : IDbContext
    {
        /// <summary>
        /// Job position
        /// </summary>
        DbSet<JobPosition> JobPositions { get; set; }

        /// <summary>
        /// Source
        /// </summary>
        DbSet<Source> Sources { get; set; }

        /// <summary>
        /// Solution Types
        /// </summary>
        DbSet<SolutionType> SolutionTypes { get; set; }

        /// <summary>
        /// Technology Types
        /// </summary>
        DbSet<TechnologyType> TechnologyTypes { get; set; }

        /// <summary>
        /// Service Types
        /// </summary>
        DbSet<ServiceType> ServiceTypes { get; set; }

        /// <summary>
        ///  Product Types
        /// </summary>
        DbSet<ProductType> ProductTypes { get; set; }

        /// <summary>
        /// Currencies
        /// </summary>
        DbSet<Currency> Currencies { get; set; } 
        
        /// <summary>
        /// Campaign Types
        /// </summary>
        DbSet<CampaignType> CampaignTypes { get; set; } 

        ///<summary>
        /// BinaryFyle
        /// </summary>
        DbSet<BinaryFile> BinaryFiles { get; set; }

        /// <summary>
        /// Product/Service
        /// </summary>
        DbSet<ProductOrService> ProductOrServices { get; set; }

        DbSet<DevelopementFramework> DevelopementFrameworks { get; set; }

        DbSet<PMFramework> PMFrameworks { get; set; }

        DbSet<ConsultancyVariation> ConsultancyVariations { get; set; }

        DbSet<DesignVariation> DesignVariations { get; set; }

        DbSet<DevelopmentVariation> DevelopmentVariations { get; set; }

        DbSet<QAVariation> QAVariations { get; set; }

    }
}