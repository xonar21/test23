using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GR.Audit.Contexts;
using GR.Core;
using GR.Core.Helpers;
using GR.Crm.Contracts.Abstractions;
using GR.Crm.Contracts.Abstractions.Models;
using GR.Crm.Abstractions.Models;
using GR.Crm.Abstractions.ViewModels.CurrencyViewModels;
using GR.Crm.Leads.Abstractions;
using GR.Crm.Leads.Abstractions.Models;
using GR.Crm.Organizations.Abstractions.Models;
using GR.Crm.Payments.Abstractions;
using GR.Crm.Payments.Abstractions.Models;
using GR.Crm.PipeLines.Abstractions.Models;
using GR.Crm.Teams.Abstractions;
using Microsoft.EntityFrameworkCore;
using GR.Crm.Teams.Abstractions.Models;
using Mapster;
using GR.Crm.Marketing.Abstractions.Models;
using GR.Crm.Marketing.Abstractions;
using GR.Crm.Products.Abstractions.Models;
using Gr.Crm.Products.Abstractions.Models;
using Gr.Crm.Comments.Abstractions;
using Gr.Crm.Comments.Abstractions.Models;
using GR.Crm.Emails.Abstractions;
using GR.Crm.Emails.Abstractions.Models;
using GR.Crm.Abstractions.Models.ProductConfiguration.Services;
using GR.Audit.Abstractions;
using GR.Crm.BussinesUnits.Abstractions;
using GR.Crm.BussinesUnits.Abstractions.Models;

namespace GR.Crm.Data
{
    public class CrmDbContext : TrackerDbContext,
        ITrackerDbContext,
        ILeadContext<Lead>,
        IContractsContext,
        ICrmTeamContext, 
        IPaymentContext,
        ICrmCampaignContext, 
        ICrmMarketingListContext,
        ICommentContext,
        IEmailContext,
        IBusinessUnitContext
    {
        /// <summary>
        /// Schema
        /// Do not remove this, is used on audit 
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        public const string Schema = "Crm";

        /// <inheritdoc />
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options"></param>
        public CrmDbContext(DbContextOptions<CrmDbContext> options) : base(options)
        {
        }

        #region Entities

        /// <summary>
        /// Organization
        /// </summary>
        public virtual DbSet<Organization> Organizations { get; set; }

        /// <summary>
        /// Organization stages
        /// </summary>
        public virtual DbSet<OrganizationStage> OrganizationStages { get; set; }

        /// <summary>
        /// Organization states
        /// </summary>
        public virtual DbSet<OrganizationState> OrganizationStates { get; set; }

        /// <summary>
        /// Organization states stages
        /// </summary>
        public virtual DbSet<OrganizationStateStage> OrganizationStatesStages { get; set; }

        /// <summary>
        /// Organization
        /// </summary>
        public virtual DbSet<OrganizationAddress> OrganizationAddresses { get; set; }

        /// <summary>
        /// Cities 
        /// </summary>
        public virtual DbSet<City> Cities { get; set; }

        /// <summary>
        /// Regions
        /// </summary>
        public virtual DbSet<Region> Regions { get; set; }

        /// <summary>
        /// Countries
        /// </summary>
        public virtual DbSet<CrmCountry> Countries { get; set; }


        /// <summary>
        /// Industries
        /// </summary>
        public virtual DbSet<Industry> Industries { get; set; }

        /// <summary>
        /// Employees
        /// </summary>
        public virtual DbSet<Employee> Employees { get; set; }

        /// <summary>
        /// Contacts
        /// </summary>
        public virtual DbSet<Contact> Contacts { get; set; }

        /// <summary>
        /// Contact web profile
        /// </summary>
        public virtual DbSet<ContactWebProfile> ContactWebProfiles { get; set; }

        /// <summary>
        /// web profile
        /// </summary>
        public virtual DbSet<WebProfile> WebProfiles { get; set; }

        /// <summary>
        /// Organization revenues
        /// </summary>
        public virtual DbSet<Revenue> Revenues { get; set; }

        /// <summary>
        /// PipeLines
        /// </summary>
        public virtual DbSet<PipeLine> PipeLines { get; set; }

        /// <summary>
        /// Stages
        /// </summary>
        public virtual DbSet<Stage> Stages { get; set; }


        /// <summary>
        /// NoGoStates
        /// </summary>
        public virtual DbSet<NoGoState> NoGoStates { get; set; }


        /// <summary>
        /// Contract templates
        /// </summary>
        public virtual DbSet<ContractTemplate> ContractTemplates { get; set; }

        /// <summary>
        /// Contract sections
        /// </summary>
        public virtual DbSet<ContractSection> ContractSections { get; set; }

        /// <summary>
        /// Job positions
        /// </summary>
        public virtual DbSet<JobPosition> JobPositions { get; set; }

        /// <summary>
        /// Currencies
        /// </summary>
        public virtual DbSet<Currency> Currencies { get; set; }

        /// <summary>
        /// Leads
        /// </summary>
        public virtual DbSet<Lead> Leads { get; set; }

        public virtual DbSet<LeadContact> LeadsContacts { get; set; }

        public virtual DbSet<ProductOrServiceList> ProductOrServiceLists { get; set; }

        /// <summary>
        /// Lead files
        /// </summary>
        public virtual DbSet<LeadFile> LeadFiles { get; set; }

        /// <summary>
        /// Lead states
        /// </summary>
        public virtual DbSet<LeadState> States { get; set; }

        /// <summary>
        /// Lead state stage
        /// </summary>
        public virtual DbSet<LeadStateStage> LeadStateStage { get; set; }

        /// <summary>
        /// Agreements
        /// </summary>
        public virtual DbSet<Agreement> Agreements { get; set; }

        /// <summary>
        /// Products
        /// </summary>
        public virtual DbSet<ProductTemplate> Products { get; set; }

        /// <summary>
        /// Product variation
        /// </summary>
        public virtual DbSet<ProductVariation> ProductVariations { get; set; }

        public virtual DbSet<ProductDeliverables> ProductDeliverables { get; set; }

        /// <summary>
        /// Category
        /// </summary>
        public virtual DbSet<Category> Categories { get; set; }

        /// <summary>
        /// Product Category
        /// </summary>
        public virtual DbSet<ProductCategory> ProductCategories { get; set; }

        /// <summary>
        /// Product Manufactories
        /// </summary>
        public virtual DbSet<ProductManufactories> ProductManufactories { get; set; }

        /// <summary>
        /// Team
        /// </summary>
        public virtual DbSet<Team> Teams { get; set; }

        /// <summary>
        /// Team members
        /// </summary>
        public virtual DbSet<TeamMember> TeamMembers { get; set; }

        /// <summary>
        /// Team role
        /// </summary>
        public virtual DbSet<TeamRole> TeamRoles { get; set; }


        /// <summary>
        /// Payment
        /// </summary>
        public virtual DbSet<Payment> Payments { get; set; }

        /// <summary>
        /// Payment Mappers
        /// </summary>
        public virtual DbSet<PaymentMapped> PaymentMappers { get; set; }

        /// <summary>
        /// Payment Codes
        /// </summary>
        public virtual DbSet<PaymentCode> PaymentCodes { get; set; }

        /// <summary>
        /// Source
        /// </summary>
        public virtual DbSet<Source> Sources { get; set; }

        /// <summary>
        /// Solution Types
        /// </summary>
        public virtual DbSet<SolutionType> SolutionTypes { get; set; }

        /// <summary>
        /// Technology Types
        /// </summary>
        public virtual DbSet<TechnologyType> TechnologyTypes { get; set; }

        /// <summary>
        /// Service Types
        /// </summary>
        public virtual DbSet<ServiceType> ServiceTypes { get; set; }

        /// <summary>
        ///  Product Types
        /// </summary>
        public virtual DbSet<ProductType> ProductTypes { get; set; }

        /// <summary>
        /// Product/Service
        /// </summary>
        public virtual DbSet<ProductOrService> ProductOrServices { get; set; }

        /// <summary>
        /// Campaign Types
        /// </summary>
        public virtual DbSet<CampaignType> CampaignTypes { get; set; }

        /// <summary>
        /// Campaign
        /// </summary>
        public virtual DbSet<Campaign> Campaigns { get; set; }

        /// <summary>
        /// Campaign
        /// </summary>
        public virtual DbSet<MarketingList> MarketingLists { get; set; }

        /// <summary>
        /// Marketing Lists and Organizations Joining Table
        /// </summary>
        public virtual DbSet<MarketingListOrganization> MarketingListOrganizations { get; set; }

        /// <summary>
        /// Campaigns and Marketing Lists Joining Table
        /// </summary>
        public virtual DbSet<CampaignMarketingList> CampaignsMarketingLists { get; set; }

        ///<summary>
        /// PhoneList
        /// </summary>
        public virtual DbSet<PhoneList> PhoneLists { get; set; }

        /// <summary>
        /// Binary Files
        /// </summary>
        public DbSet<BinaryFile> BinaryFiles { get; set; }

        /// <summary>
        /// Comments
        /// </summary>
        public DbSet<Comment> Comments { get; set; }


        /// <summary>
        /// CommentAssigned Users
        /// </summary>
        public DbSet<CommentAssignedUsers> CommentAssignedUsers { get; set; }

        /// <summary>
        /// Emails
        /// </summary>
        public DbSet<EmailList> Emails { get; set; }

        public DbSet<DevelopementFramework> DevelopementFrameworks { get; set; }

        public DbSet<PMFramework> PMFrameworks { get; set; }

        public DbSet<ConsultancyVariation> ConsultancyVariations { get; set; }

        public DbSet<DesignVariation> DesignVariations { get; set; }

        public DbSet<DevelopmentVariation> DevelopmentVariations { get; set; }


        public DbSet<QAVariation> QAVariations { get; set; }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        
        public DbSet<BusinessUnit> BusinessUnits { get; set; }
        
        public DbSet<Department> Departments { get; set; }
        
        public DbSet<DepartmentTeam> DepartmentTeams { get; set; }
        
        public DbSet<Grading> Gradings { get; set; }
        
        public DbSet<JobDepartmentTeam> JobDepartmentTeams { get; set; }
        
        public DbSet<JobPositionGrading> JobPositionGradings { get; set; }
        
        public DbSet<UserDepartmentTeam> UserDepartmentTeams { get; set; }
        
        public DbSet<BusinessJobPosition> BusinessJobPositions { get; set; }
        #endregion

        /// <summary>
        /// This method is invoked on system installation
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public override Task InvokeSeedAsync(IServiceProvider services)
        {
            GearApplication.BackgroundTaskQueue.PushBackgroundWorkItemInQueue(async x =>
            {
                var dataService = IoC.Resolve<ICrmTeamService>();
                if (dataService == null) throw new Exception("ICrmTeamService is not registered");
                await dataService.SeedTeamRole();
            });

            GearApplication.BackgroundTaskQueue.PushBackgroundWorkItemInQueue(async x =>
            {
                var dataService = IoC.Resolve<ILeadService<Lead>>();
                if (dataService == null) throw new Exception("ILeadService is not registered");
                await dataService.SeedSystemLeadState();
            });

            return Task.CompletedTask;
        }

        /// <summary>
        /// Configuration
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema(Schema);
            #region ProductOrServiceList
            builder.Entity<ProductOrServiceList>().HasOne(x => x.PMFramework)
                .WithOne()
                .HasForeignKey<ProductOrServiceList>(x => x.PMFrameworkId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<ProductOrServiceList>().HasOne(x => x.ProductOrService)
                .WithOne()
                .HasForeignKey<ProductOrServiceList>(x => x.ProductOrServiceId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ProductOrServiceList>().HasOne(x => x.ProductType)
                .WithOne()
                .HasForeignKey<ProductOrServiceList>(x => x.ProductTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ProductOrServiceList>().HasOne(x => x.TechnologyType)
                .WithOne()
                .HasForeignKey<ProductOrServiceList>(x => x.TechnologyTypeId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<ProductOrServiceList>().HasOne(x => x.ServiceType)
                .WithOne()
                .HasForeignKey<ProductOrServiceList>(x => x.ServiceTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ProductOrServiceList>().HasOne(x => x.DevelopmentVariation)
                .WithOne()
                .HasForeignKey<ProductOrServiceList>(x => x.DevelopmentVariationId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<ProductOrServiceList>().HasOne(x => x.ConsultancyVariation)
                .WithOne()
                .HasForeignKey<ProductOrServiceList>(x => x.ConsultancyVariationId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<ProductOrServiceList>().HasOne(x => x.QAVariation)
                .WithOne()
                .HasForeignKey<ProductOrServiceList>(x => x.QAVariationId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<ProductOrServiceList>().HasOne(x => x.DesignVariation)
                .WithOne()
                .HasForeignKey<ProductOrServiceList>(x => x.DesignVariationId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<ProductOrServiceList>().HasOne(x => x.DevelopementFramework)
                .WithOne()
                .HasForeignKey<ProductOrServiceList>(x => x.DevelopementFrameworkId)
                .OnDelete(DeleteBehavior.SetNull);

            #endregion

            builder.Entity<Lead>().Property(l => l.Number)
                .HasDefaultValueSql("nextval('\"Crm\".\"OrderNumbers\"')");

            builder.Entity<Currency>().HasKey(x => x.Code);

            /*builder.Entity<Lead>()
                .Property(p => p.StageChangeDate)
                .HasDefaultValue(DateTime.UtcNow);*/

            /*builder.Entity<Organization>()
             .Property(p => p.StageChangeDate)
            .HasDefaultValue(DateTime.UtcNow);*/

            builder.Entity<Organization>()
                .Property(p => p.DialCode)
                .HasDefaultValue("+373");
    
            builder.Entity<OrganizationStage>().HasIndex(x => x.DisplayOrder).IsUnique();
            builder.Entity<OrganizationStateStage>().HasKey(x => new { x.StateId, x.StageId });
            builder.Entity<OrganizationStateStage>().HasOne(x => x.State).WithMany(y => y.Stages).HasForeignKey(x => x.StateId);

            builder.Entity<Organization>().HasMany<Lead>().WithOne(x => x.Organization);

            builder.Entity<EmailList>().HasOne<Organization>().WithMany(x => x.EmailList).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<EmailList>().HasOne<Contact>().WithMany(x => x.EmailList).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<OrganizationStage>().HasMany<Organization>().WithOne(x => x.Stage).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<CampaignType>().HasMany<Campaign>().WithOne(x => x.CampaignType);
            var currenciesFilePath = Path.Combine(AppContext.BaseDirectory, "Configuration/currencies.json");
            var currencies = JsonParser.ReadObjectDataFromJsonFile<Dictionary<string, CurrencyViewModel>>(currenciesFilePath)
                .Select(x => x.Value.Adapt<Currency>()).ToList();
            builder.Entity<Currency>().HasData(currencies);

            builder.Entity<MarketingListOrganization>().HasKey(x => new { x.MarketingListId, x.OrganizationId });
            builder.Entity<CampaignMarketingList>().HasKey(x => new { x.CampaignId, x.MarketingListId });

            builder.Entity<LeadContact>().HasKey(k => new { k.ContactId, k.LeadId });

            builder.Entity<LeadContact>()
                .HasOne(x => x.Lead)
                .WithMany(y => y.Contacts)
                .HasForeignKey(k => k.LeadId);

            builder.Entity<LeadStateStage>().HasKey(x => new { x.StateId, x.StageId });
            builder.Entity<LeadStateStage>().HasOne(x => x.State).WithMany(y => y.Stages).HasForeignKey(x => x.StateId);
            builder.Entity<Comment>()
                .HasOne(x => x.ParrentComent)
                .WithMany(x => x.CommentReply)
                .Metadata.DeleteBehavior = DeleteBehavior.Cascade;

            builder.Entity<Lead>().HasMany<Comment>().WithOne(x => x.Lead).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Organization>().HasMany<Comment>().WithOne(x => x.Organization).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<CommentAssignedUsers>().HasKey(x => new { x.CommentId, x.UserId });

            #region BusinessUnit
            builder.Entity<BusinessUnit>()
                .Property(e => e.Description)
                .HasMaxLength(900);

            builder.Entity<BusinessUnit>()
                .HasOne(x => x.BusinessUnitLead)
                .WithMany(x => x.BusinessUnits)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<BusinessUnit>()
                .HasMany(x => x.Departments)
                .WithOne(x => x.BusinessUnit)
                .OnDelete(DeleteBehavior.SetNull);
            #endregion

            #region BusinessUnitJobPostion
            builder.Entity<BusinessJobPosition>()
                .Property(x => x.Description)
                .HasMaxLength(1000);

            builder.Entity<BusinessJobPosition>()
                .HasMany(x => x.JobDepartmentTeams)
                .WithOne(x => x.JobPosition)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region Department
            builder.Entity<Department>()
                .Property(e => e.Description)
                .HasMaxLength(900);

            builder.Entity<Department>()
                .HasOne(x => x.DepartmentLead)
                .WithMany(x => x.Department)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Department>()
                .HasMany(x => x.DepartmentTeams)
                .WithOne(x => x.Department)
                .OnDelete(DeleteBehavior.SetNull);

            #endregion

            #region DepartmentTeam
            builder.Entity<DepartmentTeam>()
                .Property(e => e.Description)
                .HasMaxLength(900);

            builder.Entity<DepartmentTeam>().HasMany(x => x.JobDepartmentTeams)
                .WithOne(x => x.DepartmentTeam)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<DepartmentTeam>().HasMany(x => x.UserDepartmentTeams)
                .WithOne(x => x.DepartmentTeam)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<DepartmentTeam>().HasOne(x => x.DepartmentTeamLead)
                .WithMany(x => x.DepartmentTeam)
                .OnDelete(DeleteBehavior.SetNull);
            #endregion

            #region JobDepartmentTeam
            builder.Entity<JobDepartmentTeam>()
                .Property(x => x.JobPositionId)
                .IsRequired();

            builder.Entity<JobDepartmentTeam>().Property(x => x.DepartmentTeamId)
                .IsRequired();

            builder.Entity<JobDepartmentTeam>().HasKey(x => new { x.JobPositionId, x.DepartmentTeamId });

            builder.Entity<JobDepartmentTeam>().HasOne(x => x.DepartmentTeam)
                .WithMany(x => x.JobDepartmentTeams)
                .HasForeignKey(x => x.DepartmentTeamId);

            builder.Entity<JobDepartmentTeam>().HasOne(x => x.JobPosition)
                .WithMany(x => x.JobDepartmentTeams)
                .HasForeignKey(x => x.JobPositionId);

            #endregion

            #region UserDepartmentTeam
            builder.Entity<UserDepartmentTeam>().Property(x => x.UserId)
                .IsRequired();

            builder.Entity<UserDepartmentTeam>().Property(x => x.DeparmentTeamId)
                .IsRequired();

            builder.Entity<UserDepartmentTeam>().HasKey(x => new { x.DeparmentTeamId, x.UserId });

            builder.Entity<UserDepartmentTeam>().HasOne(x => x.DepartmentTeam)
                .WithMany(x => x.UserDepartmentTeams)
                .HasForeignKey(x => x.DeparmentTeamId);

            builder.Entity<UserDepartmentTeam>().HasOne(x => x.User)
                .WithMany(x => x.UserDepartmentTeams)
                .HasForeignKey(x => x.UserId);
            #endregion

            #region JobPositionGrading
            builder.Entity<JobPositionGrading>()
                .HasKey(x => new { x.JobPositionId, x.GradingId });

            builder.Entity<JobPositionGrading>().Property(x => x.GradingId)
                .IsRequired();

            builder.Entity<JobPositionGrading>().Property(x => x.JobPositionId)
                .IsRequired();

            builder.Entity<JobPositionGrading>().HasOne(x => x.JobPosition)
                .WithMany(x => x.JobPositionGradings)
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey(x => x.JobPositionId);

            builder.Entity<JobPositionGrading>().HasOne(x => x.Grading)
                .WithMany(x => x.JobPositionGradings)
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey(x => x.GradingId);
            #endregion

            #region ApplicationUser
            builder.Entity<ApplicationUser>()
                .HasMany(x => x.DepartmentTeam)
                .WithOne(x => x.DepartmentTeamLead)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<ApplicationUser>().HasMany(x => x.Department)
                .WithOne(x => x.DepartmentLead)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<ApplicationUser>().HasMany(x => x.UserDepartmentTeams)
                .WithOne(x => x.User)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion
        }
    }
}
