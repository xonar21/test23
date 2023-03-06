#region Usings

using GR.Audit;
using GR.Audit.Abstractions.Extensions;
using GR.Backup.Abstractions.BackgroundServices;
using GR.Backup.Abstractions.Extensions;
using GR.Backup.PostGresSql;
using GR.Core.Extensions;
using GR.Dashboard;
using GR.Dashboard.Abstractions;
using GR.Dashboard.Abstractions.Extensions;
using GR.Dashboard.Abstractions.Models.WidgetTypes;
using GR.Dashboard.Data;
using GR.Dashboard.Razor.Extensions;
using GR.Dashboard.Renders;
using GR.DynamicEntityStorage.Extensions;
using GR.Email;
using GR.Email.Abstractions.Extensions;
using GR.Email.Razor.Extensions;
using GR.Entities;
using GR.Entities.Abstractions.Extensions;
using GR.Entities.Data;
using GR.Entities.EntityBuilder.Postgres;
using GR.Entities.EntityBuilder.Postgres.Controls.Query;
using GR.Entities.Extensions;
using GR.Entities.Razor.Extensions;
using GR.Entities.Security;
using GR.Entities.Security.Abstractions.Extensions;
using GR.Entities.Security.Data;
using GR.Entities.Security.Razor.Extensions;
using GR.Forms.Abstractions.Extensions;
using GR.Forms.Data;
using GR.Forms.Razor.Extensions;
using GR.Identity.Abstractions;
using GR.Identity.Abstractions.Extensions;
using GR.Identity.Abstractions.Models.MultiTenants;
using GR.Identity.Data;
using GR.Identity.IdentityServer4.Extensions;
using GR.Identity.LdapAuth;
using GR.Identity.LdapAuth.Abstractions.Extensions;
using GR.Identity.LdapAuth.Abstractions.Models;
using GR.Identity.Permissions;
using GR.Identity.Permissions.Abstractions.Extensions;
using GR.Identity.Services;
using GR.Install;
using GR.Install.Abstractions.Extensions;
using GR.Localization;
using GR.Localization.Abstractions;
using GR.Localization.Abstractions.Extensions;
using GR.Localization.Abstractions.Models;
using GR.MultiTenant.Abstractions.Extensions;
using GR.MultiTenant.Razor.Extensions;
using GR.PageRender;
using GR.PageRender.Abstractions.Extensions;
using GR.PageRender.Data;
using GR.PageRender.Razor.Extensions;
using GR.Report.Abstractions.Extensions;
using GR.Report.Dynamic;
using GR.Report.Dynamic.Data;
using GR.Report.Dynamic.Razor.Extensions;
using GR.TaskManager.Abstractions.Extensions;
using GR.TaskManager.Data;
using GR.TaskManager.Razor.Extensions;
using GR.TaskManager.Services;
using GR.WebApplication.Extensions;
using GR.WebApplication.Helpers;
using GR.WorkFlows;
using GR.WorkFlows.Abstractions.Extensions;
using GR.WorkFlows.Abstractions.Models;
using GR.WorkFlows.Data;
using GR.WorkFlows.Razor.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GR.CloudStorage.Abstractions.Extensions;
using GR.CloudStorage.Implementation;
using GR.Crm;
using GR.Forms;
using GR.Identity.Data.Groups;
using GR.Identity.Razor.Extensions;
using GR.Localization.Razor.Extensions;
using GR.UI.Menu;
using GR.UI.Menu.Abstractions.Extensions;
using GR.UI.Menu.Data;
using GR.Crm.Data;
using GR.Crm.Abstractions.Extensions;
using GR.Crm.Contracts.Infrastructure;
using GR.Crm.PipeLines;
using GR.Crm.PipeLines.Abstractions.Extensions;
using GR.Crm.Razor.Extensions;
using GR.Crm.Contracts.Abstractions.Extensions;
using GR.Crm.Dashboard.Abstractions.Extensions;
using GR.Crm.Dashboard.Infrastructure;
using GR.Crm.Leads.Abstractions.Extensions;
using GR.Crm.Leads.Abstractions.Models;
using GR.Crm.Leads.Infrastructure;
using GR.Crm.Teams.Implementation;
using GR.Crm.Teams.Abstractions.Extensions;
using GR.Crm.Organizations;
using GR.Crm.Organizations.Abstractions.Extensions;
using GR.Crm.Payments.Abstractions;
using GR.Crm.Payments.Abstractions.Extensions;
using GR.Crm.Payments.Implementation;
using Gr.Crm.Comments.Abstractions.Extensions;
using Microsoft.AspNetCore.Http;
using OrganizationService = GR.MultiTenant.Services.OrganizationService;
using GR.Crm.Reports.Abstraction.Extensions;
using GR.Crm.Reports.Abstraction;
using GR.Crm.Reports.Infrastructure;
using Microsoft.AspNetCore.Identity;
using GR.Crm.Organizations.Abstractions.ViewModels.OrganizationsViewModels;
using NPOI.SS.Formula.Functions;
using GR.Crm.Marketing.Abstractions.Extensions;
using GR.Crm.Marketing.Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;
using IdentityModel.Client;
using GR.Crm.Products.Infrastructure;
using Gr.Crm.Comments.Infrastructure;
using GR.Crm.Emails.Infrastructure;
using GR.Crm.Emails.Abstractions.Extensions;
using GR.Audit.Contexts;
using GR.Notifications;
using GR.Notifications.Data;
using GR.Notifications.Hub.Hubs;
using GR.Notifications.Seeders;
using GR.Notifications.Abstractions.Extensions;
using GR.Notifications.Razor.Extensions;
using GR.Crm.BussinesUnits.Abstractions;
using GR.Crm.BussinesUnits.Abstractions.Extensions;
using GR.Crm.BussinesUnits.Infrastructure;

#endregion Usings

namespace GR.Cms
{
	public class Startup : GearCoreStartup
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="configuration"></param>
		/// <param name="env"></param>
		public Startup(IConfiguration configuration, IHostingEnvironment env) : base(configuration, env) { }

		/// <summary>
		/// Configure cms app
		/// </summary>
		/// <param name="app"></param>
		public override void Configure(IApplicationBuilder app)
		{
			app.UseGearWebApp(config =>
			{
				config.AppName = "CRM APP";
				config.HostingEnvironment = HostingEnvironment;
				config.Configuration = Configuration;
				app.UseCookiePolicy();
				app.UseAuthentication();
				app.UsePathBase("/crm");

				config.CustomMapRules = new Dictionary<string, Action<HttpContext>>
				{
					{
						"/", context => context.MapTo("/dashboard")
					}
				};
			});
			app.UseNotificationsHub<GearNotificationHub>();
		}

		/// <summary>
		/// This method gets called by the runtime. Use this method to add services to the container.
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public override IServiceProvider ConfigureServices(IServiceCollection services) =>
			services.RegisterGearWebApp(Configuration, HostingEnvironment, config =>
		{
			config.Configuration = Configuration;
			config.HostingEnvironment = HostingEnvironment;
			config.CacheConfiguration.UseInMemoryCache = true;

			//Register mappings from modules
			services.AddAutoMapper(this.GetAutoMapperProfilesFromAllAssemblies().ToArray());

			services.ConfigureNonBreakingSameSiteCookies();

			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
					.AddCookie(options =>
					{
						// add an instance of the patched manager to the options:
						options.CookieManager = new ChunkingCookieManagerPatched();
					});
			

			services.ConfigureApplicationCookie(option =>
			{
				option.Cookie.Name = ".AspNet.CRM";
				option.Cookie.HttpOnly = true;
				option.Cookie.SameSite = SameSiteMode.None;
				option.Cookie.SecurePolicy = CookieSecurePolicy.Always;
			});

			services.AddMemoryCache();


			//------------------------------Identity Module-------------------------------------
			config.GearServices.AddIdentityModule<ApplicationDbContext>()
				.AddIdentityUserManager<IdentityUserManager, GearUser>()
				.AddIdentityModuleStorage<ApplicationDbContext>(Configuration, MigrationsAssembly)
				.RegisterGroupRepository<GroupRepository<ApplicationDbContext>, ApplicationDbContext, GearUser>()
				.AddAppProvider<AppProvider>()
				.AddUserAddressService<UserAddressService>()
				.AddIdentityModuleEvents()
				.RegisterLocationService<LocationService>()
				.AddIdentityRazorModule();

			config.GearServices.AddAuthentication(Configuration)
				.AddPermissionService<PermissionService<ApplicationDbContext>>()
				.AddIdentityModuleProfileServices()
				.AddIdentityServer(Configuration, MigrationsAssembly);

			services.Configure<IdentityOptions>(options =>
			{
				// Default Password settings.
				options.Password.RequireDigit = true;
				options.Password.RequireLowercase = true;
				options.Password.RequireUppercase = true;
				options.Password.RequiredLength = 6;
				options.Password.RequiredUniqueChars = 1;
			});

			//---------------------------------------Entity Module-------------------------------------
			config.GearServices.AddEntityModule<EntitiesDbContext, EntityService>()
				.AddEntityModuleQueryBuilders<NpgTableQueryBuilder, NpgEntityQueryBuilder, NpgTablesService>()
				.AddEntityModuleStorage<EntitiesDbContext>(options =>
				{
					options.GetDefaultOptions(Configuration);
					options.EnableSensitiveDataLogging();
				})
				.AddEntityModuleEvents()
				.RegisterEntityBuilderJob()
				.AddEntityRazorUIModule();

			//------------------------------Entity Security Module-------------------------------------
			config.GearServices.AddEntityRoleAccessModule<EntityRoleAccessService<EntitySecurityDbContext, ApplicationDbContext>>()
				.AddEntityModuleSecurityStorage<EntitySecurityDbContext>(options =>
				{
					options.GetDefaultOptions(Configuration);
					options.EnableSensitiveDataLogging();
				})
				.AddEntitySecurityRazorUIModule();

			//----------------------------------------Audit Module-------------------------------------
			config.GearServices.AddAuditModule<AuditManager>();

			//---------------------------Dynamic repository Module-------------------------------------
			config.GearServices.AddDynamicDataProviderModule<EntitiesDbContext>();

			//------------------------------------Dashboard Module-------------------------------------
			config.GearServices.AddDashboardModule<DashboardService, WidgetGroupRepository, WidgetService>()
				.AddDashboardModuleStorage<DashBoardDbContext>(options =>
				{
					options.GetDefaultOptions(Configuration);
					options.EnableSensitiveDataLogging();
				})
				.RegisterDashboardEvents()
				.AddDashboardRazorUIModule()
				.AddDashboardRenderServices(new Dictionary<Type, Type>
				{
					{typeof(IWidgetRenderer<ReportWidget>), typeof(ReportWidgetRender)},
					{typeof(IWidgetRenderer<CustomWidget>), typeof(CustomWidgetRender)},
				})
				.RegisterProgramAssembly(typeof(Program));

			/*//-------------------------------Notification Module-------------------------------------
			config.GearServices.AddNotificationModule<Notify<ApplicationDbContext, GearRole, GearUser>, GearRole>()
				.AddNotificationSubscriptionModule<NotificationSubscriptionRepository>()
				.AddNotificationModuleEvents()
				.AddNotificationSubscriptionModuleStorage<NotificationDbContext>(options =>
				{
					options.GetDefaultOptions(Configuration);
					options.EnableSensitiveDataLogging();
				})
				.AddNotificationRazorUIModule();*/



			////-------------------------------EF core Notification Module-------------------------------------
			config.GearServices.AddNotificationModule<Notify<ApplicationDbContext, GearRole, GearUser>, GearRole>()
				.AddNotificationSeeder<EfCoreNotificationSeederService>()
				.AddNotificationModuleStorage<NotificationDbContext>(options =>
				{
					options.GetDefaultOptions(Configuration);
					options.EnableSensitiveDataLogging();
				})
				.RegisterNotificationsHubModule<CommunicationHub>()
				.AddNotificationRazorUiModule();
			//---------------------------------Localization Module-------------------------------------
			config.GearServices
				.AddLocalizationModule<LocalizationService, YandexTranslationProvider, JsonStringLocalizer>(
					new TranslationModuleOptions
					{
						Configuration = Configuration,
						LocalizationProvider = LocalizationProvider.Yandex
					})
				.AddLocalizationRazorModule();

			//--------------------------------------Menu UI Module-------------------------------------
			config.GearServices.AddMenuModule<MenuService>()
				.AddMenuModuleStorage<MenuDbContext>(options =>
				{
					options.GetDefaultOptions(Configuration);
					options.EnableSensitiveDataLogging();
				});


			//------------------------------Database backup Module-------------------------------------
			config.GearServices.RegisterDatabaseBackupRunnerModule<BackupTimeService<PostGreSqlBackupSettings>,
					PostGreSqlBackupSettings, PostGreBackupService>(Configuration);

			//------------------------------------Task Module-------------------------------------
			config.GearServices.AddTaskModule<TaskManager.Services.TaskManager, TaskManagerNotificationService>()
				.AddTaskModuleStorage<TaskManagerDbContext>(options =>
				{
					options.GetDefaultOptions(Configuration);
					options.EnableSensitiveDataLogging();
				})
				.AddTaskManagerRazorUIModule()
				.AddTaskTypeModule<TaskTypeService, TaskManagerDbContext>();

			//-----------------------------------------Form Module-------------------------------------
			config.GearServices.AddFormModule<FormDbContext>()
				.AddFormModuleStorage<FormDbContext>(options =>
				{
					options.GetDefaultOptions(Configuration);
					options.EnableSensitiveDataLogging();
				})
				.RegisterFormService<FormService<FormDbContext>>()
				.AddFormStaticFilesModule();

			//-----------------------------------------Page Module-------------------------------------
			config.GearServices.AddPageModule()
				.AddPageModuleStorage<DynamicPagesDbContext>(options =>
				{
					options.GetDefaultOptions(Configuration);
					options.EnableSensitiveDataLogging();
				})
				.AddPageRenderUIModule<PageRender.PageRender>()
				.RegisterViewModelService<ViewModelService>()
				.AddPageAclService<PageAclService>();

			//---------------------------------------Report Module-------------------------------------
			config.GearServices.AddDynamicReportModule<DynamicReportsService<DynamicReportDbContext>>()
				.AddDynamicReportModuleStorage<DynamicReportDbContext>(options =>
				{
					options.GetDefaultOptions(Configuration);
					options.EnableSensitiveDataLogging();
				})
				.AddReportUIModule();

			//----------------------------------------Installer Module-------------------------------------
			config.GearServices.AddInstallerModule<GearWebInstallerService>();

			//----------------------------------------Email Module-------------------------------------
			config.GearServices.AddEmailModule<EmailSender>()
				.AddEmailRazorUIModule()
				.BindEmailSettings(Configuration);

			//----------------------------------------Ldap Module-------------------------------------
			config.GearServices
				.AddIdentityLdapModule<LdapUser, LdapService<LdapUser>, LdapUserManager<LdapUser>>(
					Configuration)
				.AddLdapAuthentication<LdapAuthorizeService>(options =>
				{
					options.AutoImportOnLogin = true;
				});

			//---------------------------------Multi Tenant Module-------------------------------------
			config.GearServices.AddTenantModule<OrganizationService, Tenant>()
				.AddMultiTenantRazorUIModule();

			//-------------------------------------Workflow module-------------------------------------
			config.GearServices.AddWorkFlowModule<WorkFlow, WorkFlowCreatorService, WorkFlowExecutorService>()
				.AddWorkflowModuleStorage<WorkFlowsDbContext>(options =>
				{
					options.GetDefaultOptions(Configuration);
					options.EnableSensitiveDataLogging();
				})
				.AddWorkflowRazorModule();


			//----------------------------------------CRM module-------------------------------------
			config.GearServices.AddCrmModule<CrmService>()
				.AddCrmModuleStorage<CrmDbContext>(options =>
				{
					options.GetDefaultOptions(Configuration);
					options.EnableSensitiveDataLogging();
				})
				.AddCrmAuditModule<CrmService, CrmDbContext>()
				.AddCrmJobPositionModule<VocabulariesService, CrmDbContext>()
				.AddCrmLeadModule<CrmLeadService, LeadNotificationService, CrmDbContext, Lead>()
				.AddCrmLeadProductModule<ProductService, CrmDbContext>()
				.AddProductCategoryModule<CategoryService, CrmDbContext>()
				.AddProductManufactoryModule<ManufactoryService, CrmDbContext>()
				.AddCrmLeadFileModule<LeadFileService, CrmDbContext>()
				.AddCrmLeadAgreementModule<AgreementService, CrmDbContext>()
				.AddCrmMergeModule<MergeService, CrmDbContext>()
				.AddCrmImportExportModule<ImportExportService, CrmDbContext>()
				.AddCustomTags()
				.AddCrmMarketingListModule<MarketingListService, CrmDbContext>()
				.BindGeneralConfigurationSettings(Configuration)
				.AddCrmPipeLineModule<PipeLineService, CrmDbContext>()
				.AddCrmContractsModule<CrmContractsService, CrmDbContext>()
				.AddCrmTeamModule<TeamService, CrmDbContext>()
				.AddCrmCommentsModule<CommentsService, CrmDbContext>()
				.AddCrmEmailModule<EmailService, CrmDbContext>()
				.AddCrmOrganizationModule<GR.Crm.Organizations.OrganizationService, CrmDbContext>()
				.AddCrmIndustryModule<IndustryService, CrmDbContext>()
				.AddCrmEmployeesModule<EmployeeService, CrmDbContext>()
				.AddCrmAddressModule<OrganizationAddressService, CrmDbContext>()
				.AddCrmRevenueModule<OrganizationRevenueService, CrmDbContext>()
				.AddCrmOrganizationStagesModule<OrganizationHelperService, CrmDbContext>()
				.AddCrmPhoneListModule<PhoneListService, CrmDbContext>()
				.AddCrmContactModule<ContactService, CrmDbContext>()
				.AddCrmPaymentModule<PaymentService, PaymentCodeService, CrmDbContext>()
				.AddCrmDashboardModule<CrmDashboardService>()
				.AddCrmReportModule<CrmReportService>()
				.AddCrmRazorUIModule()
				.AddCrmCampaignModule<CampaignService, CrmDbContext>()
				.AddCrmBusinessUnitModule<BusinessUnitService, CrmDbContext>()
				.RegisterAuditFor<CrmDbContext>("Crm Module")
				.AddCrmNotificationModule<CrmNotificationService, CrmDbContext>();
				/*.AddCrmProductModule<ProductService>()
				.AddCrmCategoryModule<CategoryService>()
				.AddCrmManufactoryModule<ManufactoryService>()*/


			//---------------------------------------------------- Cloud storage -------------------------------
			config.GearServices.OneDriveSettings(Configuration)
				.AddStorageBaseServiceModule<OneDriveService>()
				.AddUserTokenDataServiceModule<BaseCloudUserDataService>()
				.AddCloudUserAuthorizationServiceModule<OneDriveUserAuthorizationService>()
				.AddCloudStorage<ApplicationDbContext>();
				

		});

		

		
	}
}