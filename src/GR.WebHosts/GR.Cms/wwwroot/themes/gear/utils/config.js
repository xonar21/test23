const apiEndpoints = {
	GetMenus: '/Menu/GetMenus',
	Agreement: {
		GetAllAgreements: '/api/Agreement/GetAllAgreements',
		GetAllTablePaginatedAgreements: '/api/Agreement/GetAllTablePaginatedAgreements',
		GetAllPaginatedAgreements: '/api/Agreement/GetAllPaginatedAgreements',
		GetAllAgreementsByLeadId: '/api/Agreement/GetAllAgreementsByLeadId',
		GetAllAgreementsByOrganizationIdId: '/api/Agreement/GetAllAgreementsByOrganizationIdId',
		GetAgreementById: '/api/Agreement/GetAgreementById',
		AddAgreement: '/api/Agreement/AddAgreement',
		UpdateAgreement: '/api/Agreement/UpdateAgreement',
		DeleteAgreement: '/api/Agreement/DeleteAgreement',
		DisableAgreement: '/api/Agreement/DisableAgreement',
		ActivateAgreement: '/api/Agreement/ActivateAgreement',
		GenerateFileContractForAgreement: '/api/Agreement/GenerateFileContractForAgreement'
	},
	BusinessUnit: {
		CreateBusinessUnit: '/api/BusinesUnit/CreateBusinessUnit',
		Edit: '/api/BusinesUnit/Edit',
		GetBusinessUnitById: '/api/BusinesUnit/GetBusinessUnitById',
		DeleteBusinessUnit: '/api/BusinesUnit/DeleteBusinessUnit',
		GetBusinessUnitList: '/api/BusinesUnit/GetBusinessUnitList',
		AssignLeader: '/api/BusinesUnit/AssignLeader',
		Rename: '/api/BusinesUnit/Rename',
		ActivateBusinessUnit: '/api/BusinesUnit/ActivateBusinessUnit',
		AddDepartments: '/api/BusinesUnit/AddDepartments',
		RemoveDepartment: '/api/BusinesUnit/RemoveDepartment',
		GetAllDepartments: '/api/BusinessUnit/GetDepartments'
	},
	GeneralConfiguration: {
		GetGeneralConfigurations: '/api/GeneralConfiguration/GetGeneralConfigurations',
		UpdateGeneralconfigurations: '/api/GeneralConfiguration/UpdateGeneralConfigurations'
	},
	CrmReport: {
		LeadReport: '/api/CrmReport/LeadReport',
		DownloadLeadXLS: '/api/CrmReport/DownloadLeadReportXLSX',
		GetXLXByID: '/api/CrmReport/GetXLSXFileById',
		DownloadLeadCSV: '/api/CrmReport/DownloadLeadReportCSV',
		PaymentsReport: '/api/CrmReport/PaymentsReport',
		AgreementsReport: '/api/CrmReport/AgreementsReport',
		TaskReport: '/api/CrmReport/TaskReport'
	},
	Comments: {
		AddCommentAsync: '/api/Comment/AddCommentAsync',
		UpdateComment: '/api/Comment/UpdateComment',
		GetCommentByIdAsync: '/api/Comment/GetCommentByIdAsync',
		DeleteCommentAsync: '/api/Comment/DeleteCommentAsync',
		GetAllCommentByLeadId: '/api/Comment/GetAllCommentByLeadId',
		GetAllCommentsByOrganizationId: '/api/Comment/GetAllCommentsByOrganizationId'
	},
	CrmVocabularies: {
		GetAllPaginatedJobPositions: '/api/CrmVocabularies/GetAllPaginatedJobPositions',
		GetAllJobPositions: '/api/CrmVocabularies/GetAllJobPositions',
		GetJobPositionById: '/api/CrmVocabularies/GetJobPositionById',
		AddNewJobPosition: '/api/CrmVocabularies/AddNewJobPosition',
		UpdateJobPosition: '/api/CrmVocabularies/UpdateJobPosition',
		DeleteJobPositionById: '/api/CrmVocabularies/DeleteJobPositionById',
		ActivateJobPositionById: '/api/CrmVocabularies/ActivateJobPositionById',
		DisableJobPositionById: '/api/CrmVocabularies/DisableJobPositionById',
		GetAllPaginatedSources: '/api/CrmVocabularies/GetAllPaginatedSources',
		GetAllSourcesAsync: '/api/CrmVocabularies/GetAllSourcesAsync',
		GetSourceById: '/api/CrmVocabularies/GetSourceById',
		AddSource: '/api/CrmVocabularies/AddSource',
		UpdateSource: '/api/CrmVocabularies/UpdateSource',
		DeleteSource: '/api/CrmVocabularies/DeleteSource',
		ActivateSource: '/api/CrmVocabularies/ActivateSource',
		DisableSource: '/api/CrmVocabularies/DisableSource',
		GetAllPaginatedSolutionType: '/api/CrmVocabularies/GetAllPaginatedSolutionType',
		GetAllSolutionType: '/api/CrmVocabularies/GetAllSolutionType',
		GetSolutionTypeById: '/api/CrmVocabularies/GetSolutionTypeById',
		AddSolutionType: '/api/CrmVocabularies/AddSolutionType',
		UpdateSolutionType: '/api/CrmVocabularies/UpdateSolutionType',
		DeleteSolutionType: '/api/CrmVocabularies/DeleteSolutionType',
		ActivateSolutionType: '/api/CrmVocabularies/ActivateSolutionType',
		DisableSolutionType: '/api/CrmVocabularies/DisableSolutionType',
		GetAllPaginatedTechnologyType: '/api/CrmVocabularies/GetAllPaginatedTechnologyType',
		GetAllTechnologyType: '/api/CrmVocabularies/GetAllTechnologyType',
		GetTechnologyTypeById: '/api/CrmVocabularies/GetTechnologyTypeById',
		AddTechnologyType: '/api/CrmVocabularies/AddTechnologyType',
		UpdateTechnologyType: '/api/CrmVocabularies/UpdateTechnologyType',
		DeleteTechnologyType: '/api/CrmVocabularies/DeleteTechnologyType',
		ActivateTechnologyType: '/api/CrmVocabularies/ActivateTechnologyType',
		DisableTechnologyType: '/api/CrmVocabularies/DisableTechnologyType',
		GetAllPaginatedProductType: '/api/CrmVocabularies/GetAllPaginatedProductType',
		GetAllProductType: '/api/CrmVocabularies/GetAllProductType',
		GetProductTypeById: '/api/CrmVocabularies/GetProductTypeById',
		AddProductType: '/api/CrmVocabularies/AddProductType',
		UpdateProductType: '/api/CrmVocabularies/UpdateProductType',
		DeleteProductType: '/api/CrmVocabularies/DeleteProductType',
		ActivateProductType: '/api/CrmVocabularies/ActivateProductType',
		DisableProductType: '/api/CrmVocabularies/DisableProductType',
		GetAllPaginatedServiceType: '/api/CrmVocabularies/GetAllPaginatedServiceType',
		GetAllServiceType: '/api/CrmVocabularies/GetAllServiceType',
		GetServiceTypeById: '/api/CrmVocabularies/GetServiceTypeByIdA',
		AddServiceType: '/api/CrmVocabularies/AddServiceType',
		UpdateServiceType: '/api/CrmVocabularies/UpdateServiceType',
		DeleteServiceType: '/api/CrmVocabularies/DeleteServiceType',
		ActivateServiceType: '/api/CrmVocabularies/ActivateServiceType',
		DisableServiceType: '/api/CrmVocabularies/DisableServiceType',
		GetAllPaginatedCampaignType: '/api/CrmVocabularies/GetAllPaginatedCampaignType',
		GetAllCampaignType: '/api/CrmVocabularies/GetAllCampaignType',
		GetCampaignTypeById: '/api/CrmVocabularies/GetCampaignTypeById',
		AddCampaignType: '/api/CrmVocabularies/AddCampaignType',
		UpdateCampaignType: '/api/CrmVocabularies/UpdateCampaignType',
		DeleteCampaignType: '/api/CrmVocabularies/DeleteCampaignType',
		ActivateCampaignType: '/api/CrmVocabularies/ActivateCampaignType',
		DisableCampaignType: '/api/CrmVocabularies/DisableCampaignType',
		GetAllDevelopementServices: '/api/CrmVocabularies/GetAllDevelopmentVariations',
		GetAllConsultancyServices: '/api/CrmVocabularies/GetAllConsultancyVariations',
		GetAllQAServices: '/api/CrmVocabularies/GetAllQaVariations',
		GetAllDesigneServices: '/api/CrmVocabularies/GetAllDesigneVariations',
		GetAllPMFrameworks: '/api/CrmVocabularies/GetAllPMFrameworks',
		GetAllDevelopementFrameworks: '/api/CrmVocabularies/GetAllDevelopementFrameworks',
		GetAllProductsAndServices: '/api/CrmVocabularies/GetAllProductsAndServices',
		AddDevelopmentVariation: '/api/CrmVocabularies/AddDevelopmentVariation',
		DeleteDevelopmentVariation: '/api/CrmVocabularies/DeleteDevelopmentVariation',
		UpdateDevelopmentVariation: '/api/CrmVocabularies/UpdateDevelopmentVariation',
		GetDevelopmentVariationById: '/api/CrmVocabularies/GetDevelopmentVariationById',
		GetPaginatedDevelopmentVariation: '/api/CrmVocabularies/GetPaginatedDevelopmentVariation',
		AddDesigneVariation: '/api/CrmVocabularies/AddDesigneVariation',
		DeleteDesigneVariation: '/api/CrmVocabularies/DeleteDesigneVariation',
		UpdateDesigneVariation: '/api/CrmVocabularies/UpdateDesigneVariation',
		GetDesigneVariationById: '/api/CrmVocabularies/GetDesigneVariationById',
		GetPaginatedDesigneVariations: '/api/CrmVocabularies/GetPaginatedDesigneVariations',
		AddQaVariation: '/api/CrmVocabularies/AddQaVariation',
		DeleteQaVariation: '/api/CrmVocabularies/DeleteQaVariation',
		UpdateQaVariation: '/api/CrmVocabularies/UpdateQaVariation',
		GetQaVariationById: '/api/CrmVocabularies/GetQaVariationById',
		GetPaginatedQaVariations: '/api/CrmVocabularies/GetPaginatedQaVariations',
		AddConsultancyVariation: '/api/CrmVocabularies/AddConsultancyVariation',
		DeleteConsultancyVariation: '/api/CrmVocabularies/DeleteConsultancyVariation',
		UpdateconsultancyVariation: '/api/CrmVocabularies/UpdateconsultancyVariation',
		GetConsultancyVariationById: '/api/CrmVocabularies/GetConsultancyVariationById',
		GetPaginatedConsultancyVariations: '/api/CrmVocabularies/GetPaginatedConsultancyVariations',
		AddDevelopmentFramework: '/api/CrmVocabularies/AddDevelopmentFramework',
		DeleteDevelopmentFramework: '/api/CrmVocabularies/DeleteDevelopmentFramework',
		UpdateDevelopmentFramework: '/api/CrmVocabularies/UpdateDevelopmentFramework',
		GetDevelopmentFramework: '/api/CrmVocabularies/GetDevelopmentFramework',
		GetPaginatedDevelopmentFramework: '/api/CrmVocabularies/GetPaginatedDevelopmentFramework',

		AddPmFramework: '/api/CrmVocabularies/AddPmFramework',
		DeletePmFramework: '/api/CrmVocabularies/DeletePmFramework',
		UpdatePmFramework: '/api/CrmVocabularies/UpdatePmFramework',
		GetPmFrameworkById: '/api/CrmVocabularies/GetPmFrameworkById',
		GetPaginatedPmFrameworks: '/api/CrmVocabularies/GetPaginatedPmFrameworks'
	},
	Employee: {
		GetAllEmployees: '/api/Employee/GetAllEmployees',
		GetAllPaginatedEmployees: '/api/Employee/GetAllPaginatedEmployees',
		GetEmployeeById: '/api/Employee/GetEmployeeById',
		DisableEmployeeById: '/api/Employee/DisableEmployeeById',
		ActivateEmployeeById: '/api/Employee/ActivateEmployeeById',
		DeleteEmployeeById: '/api/Employee/DeleteEmployeeById',
		AddNewEmployee: '/api/Employee/AddNewEmployee',
		UpdateEmployee: '/api/Employee/UpdateEmployee'
	},
	Organization: {
		GetPaginatedOrganization: '/api/Organizations/GetPaginatedOrganization',
		GetOrganizationById: '/api/Organizations/GetOrganizationById',
		GetAllOrganization: '/api/Organizations/GetAllOrganization',
		GetAllClientOrganization: '/api/Organizations/GetAllClientOrganization',
		GetAllProspectOrganization: '/api/Organizations/GetAllProspectOrganization',
		GetAllLeadOrganization: '/api/Organizations/GetAllLeadOrganization',
		MigrationProspectOrganizationToClientById: '/api/Organizations/MigrationProspectOrganizationToClientById',
		MigrationProspectToLeadById: '/api/Organizations/MigrationProspectToLeadById',
		DeleteOrganizationPermanentlyById: '/api/Organizations/DeleteOrganizationPermanentlyById',
		DeactivateOrganizationById: '/api/Organizations/DeactivateOrganizationById',
		ActivateOrganizationById: '/api/Organizations/ActivateOrganizationById',
		AddNewOrganization: '/api/Organizations/AddNewOrganization',
		UpdateOrganization: '/api/Organizations/UpdateOrganization',
		MergeOrganizations: '/api/Organizations/MergeOrganizations',
		GetAllOrganizationsByType: '/api/Organizations/GetAllOrganizationsByType',
		ImportOrganizations: '/api/Organizations/ImportOrganizations',
		UpdateOrganizationStage: '/api/Organizations/UpdateOrganizationStage',
		UpdateOrganizationState: '/api/Organizations/UpdateOrganizationState'
	},
	OrganizationAddress: {
		GetAllAddresses: '/api/OrganizationAddresses/GetAllAddresses',
		GetAddressById: '/api/OrganizationAddresses/GetAddressById',
		GetAddressesByOrganizationId: '/api/OrganizationAddresses/GetAddressesByOrganizationId',
		AddOrganizationAddress: '/api/OrganizationAddresses/AddOrganizationAddress',
		UpdateOrganizationAddress: '/api/OrganizationAddresses/UpdateOrganizationAddress',
		DisableAddressById: '/api/OrganizationAddresses/DisableAddressById',
		ActivateAddressById: '/api/OrganizationAddresses/ActivateAddressById',
		DeleteOrganizationAddress: '/api/OrganizationAddresses/DeleteOrganizationAddress',
		GetCityById: '/api/OrganizationAddresses/GetCityById',
		GetAllPaginatedCities: '/api/OrganizationAddresses/GetAllPaginatedCities',
		GetAllCities: '/api/OrganizationAddresses/GetAllCities',
		GetAllCitiesByRegionId: '/api/OrganizationAddresses/GetAllCitiesByRegionId',
		GetAllCitiesByCountryId: '/api/OrganizationAddresses/GetAllCitiesByCountryId',
		AddCity: '/api/OrganizationAddresses/AddCity',
		UpdateCity: '/api/OrganizationAddresses/UpdateCity',
		DeleteCityByI: '/api/OrganizationAddresses/DeleteCityByI',
		DisableCity: '/api/OrganizationAddresses/DisableCity',
		ActivateCity: '/api/OrganizationAddresses/ActivateCity',
		GetRegionById: '/api/OrganizationAddresses/GetRegionById',
		GetAllPaginatedRegions: '/api/OrganizationAddresses/GetAllPaginatedRegions',
		GetAllRegions: '/api/OrganizationAddresses/GetAllRegions',
		GetAllRegionsByCountryId: '/api/OrganizationAddresses/GetAllRegionsByCountryId',
		AddRegion: '/api/OrganizationAddresses/AddRegion',
		UpdateRegion: '/api/OrganizationAddresses/UpdateRegion',
		DeleteRegionById: '/api/OrganizationAddresses/DeleteRegionById',
		DisableRegion: '/api/OrganizationAddresses/DisableRegion',
		ActivateRegion: '/api/OrganizationAddresses/ActivateRegion',
		GetAllCountries: '/api/OrganizationAddresses/GetAllCountries',
		ActivateRegion: '/api/OrganizationAddresses/ActivateRegion'

	},
	OrganizationRevenue: {
		GetRevenueById: '/api/OrganizationRevenue/GetRevenueById',
		GetOrganizationRevenues: '/api/OrganizationRevenue/GetAllRevenuesByOrganization',
		AddNewOrganizationRevenue: '/api/OrganizationRevenue/AddNewOrganizationRevenue',
		DeleteOrganizationRevenue: '/api/OrganizationRevenue/DeleteOrganizationRevenue',
		UpdateOrganizationRevenue: '/api/OrganizationRevenue/UpdateOrganizationRevenue'
	},
	OrganizationHelper: {
		GetSelectorsForOrganization: '/api/OrganizationHelper/GetSelectorsForOrganization',
		GetAllOrganizationStages: '/api/OrganizationHelper/GetAllOrganizationStages',
		GetAllOrganizationStates: '/api/OrganizationHelper/GetAllOrganizationStates',
		GetOrganizationStatesByStage: '/api/OrganizationHelper/GetOrganizationStatesByStage'
	},
	CrmCommon: {
		GetAllCurrencies: '/api/CrmCommon/GetAllCurrencies',
		GetAllOrganizationForTable: '/api/CrmCommon/GetAllOrganizationPaginated',
		GetPaginatedAudit: '/api/CrmCommon/GetPaginatedAudit',
		NotifyUsersAboutLeadsWithNoActiveTasks: '/api/CrmCommon/NotifyUsersAboutLeadsWithNoActiveTasks',
		NotifyUsersAboutDeadlines: '/api/CrmCommon/NotifyUsersAboutDeadlines'
	},
	Address: {
		GetAllCountries: '/api/Address/GetAllCountries',
		GetCitiesByCountryId: '/api/Address/GetCitiesByCountryId',
		GetUserAddresses: '/api/Address/GetUserAddresses',
		AddNewAddress: '/api/Address/AddNewAddress',
		DeleteAddress: '/api/Address/DeleteAddress'
	},
	Contract: {
		AddContractTemplate: '/api/Contracts/AddContractTemplate',
		UpdateContractTemplate: '/api/Contracts/UpdateContractTemplate',
		DisableContractTemplate: '/api/Contracts/DisableContractTemplate',
		ActivateContractTemplate: '/api/Contracts/ActivateContractTemplate',
		DeleteContractTemplate: '/api/Contracts/DeleteContractTemplate',
		GetContractTemplateSections: '/api/Contracts/GetContractTemplateSections',
		FindContractTemplateById: '/api/Contracts/FindContractTemplateById',
		GetAllContractTemplate: '/api/Contracts/GetAllContractTemplate',
		GetAllPaginatedContractTemplate: '/api/Contracts/GetAllPaginatedContractTemplate',
		FindSectionById: '/api/Contracts/FindSectionById',
		AddSectionToContractTemplate: '/api/Contracts/AddSectionToContractTemplate',
		DeleteContractSection: '/api/Contracts/DeleteContractSection',
		UpdateContractSection: '/api/Contracts/UpdateContractSection',
		OrderSection: '/api/Contracts/OrderSection'
	},
	Contact: {
		GetAllContactsPaginated: '/api/Contact/GetAllContactsPaginated',
		GetAllContacts: '/api/Contact/GetAllContacts',
		GetContactById: '/api/Contact/GetContactById',
		GetContactByOrganizationId: '/api/Contact/GetContactByOrganizationId',
		AddNewContact: '/api/Contact/AddNewContact',
		UpdateContact: '/api/Contact/UpdateContact',
		DeleteContactById: '/api/Contact/DeleteContactById',
		DeactivateContactById: '/api/Contact/DeactivateContactById',
		ActivateContactById: '/api/Contact/ActivateContactById',
		GetWebProfileById: '/api/Contact/GetWebProfileById',
		GetAllWebProfile: '/api/Contact/GetAllWebProfile',
		AddWebProfile: '/api/Contact/AddWebProfile',
		UpdateWebProfile: '/api/Contact/UpdateWebProfile',
		DeleteWebProfilePermanently: '/api/Contact/DeleteWebProfilePermanently',
		GetContactWebProfileById: '/api/Contact/GetContactWebProfileById',
		GetContactWebProfileByContactId: '/api/Contact/GetContactWebProfileByContactId',
		AddContactWebProfile: '/api/Contact/AddContactWebProfile',
		UpdateContactWebProfile: '/api/Contact/UpdateContactWebProfile',
		DeleteContactWebProfilePermanently: '/api/Contact/DeleteContactWebProfilePermanently',
		DisableContactWebProfileById: '/api/Contact/DisableContactWebProfileById',
		ActivateContactWebProfileById: '/api/Contact/ActivateContactWebProfileById',
		MergeContacts: '/api/Contact/MergeContacts',
		ImportContacts: '/api/Contact/ImportContacts'
	},
	CrmDashboard: {
		GetLeadDashboardIndices: '/api/CrmDashboard/GetLeadDashboardIndices',
		GetTaskDashboardIndices: '/api/CrmDashboard/GetTaskDashboardIndices',
		GetOrganizationDashboardIndices: '/api/CrmDashboard/GetOrganizationDashboardIndices',
		GetLeadsAnalysisReport: '/api/CrmDashboard/GetLeadsAnalysisReport',
		GetLeadsByStatusAndValue: '/api/CrmDashboard/GetLeadsByStatusAndValue'
	},
	Notifications: {
		GetNotificationsByUserId: '/api/Notifications/GetNotificationsByUserId',
		GetUserNotificationsWithPagination: '/api/Notifications/GetUserNotificationsWithPagination',
		GetNotificationById: '/api/Notifications/GetNotificationById',
		MarkAsRead: '/api/Notifications/MarkAsRead',
		PermanentlyDeleteNotification: '/api/Notifications/PermanentlyDeleteNotification',
		ClearAllByUserId: '/api/Notifications/ClearAllByUserId',
		MarkAllUserNotificationsAsRead: '/api/Notifications/MarkAllUserNotificationsAsRead'
	},
	PipeLines: {
		GetAll: "/api/PipeLine/GetAllPipeLines",
		GetPipeLineById: '/api/PipeLine/GetPipeLineById',
		GetAllPaginatedPipeLines: '/api/PipeLine/GetAllPaginatedPipeLines',
		GetPipeLineStages: '/api/PipeLine/GetPipeLineStages',
		AddPipeLine: '/api/PipeLine/AddPipeLine',
		UpdatePipeLine: '/api/PipeLine/UpdatePipeLine',
		FindStageById: '/api/PipeLine/FindStageById',
		UpdateStage: '/api/PipeLine/UpdateStage',
		OrderStages: '/api/PipeLine/OrderStages',
		AddStageToPipeLine: '/api/PipeLine/AddStageToPipeLine',
		DisableStage: '/api/PipeLine/DisableStage',
		ActivateStage: '/api/PipeLine/ActivateStage',
		RemoveStagePermanently: '/api/PipeLine/RemoveStagePermanently',
		DisablePipeLine: '/api/PipeLine/DisablePipeLine',
		ActivatePipeLine: '/api/PipeLine/ActivatePipeLine',
		RemovePipeLinePermanently: '/api/PipeLine/RemovePipeLinePermanently'
	},
	Industry: {
		GetAllPaginatedIndustries: '/api/Industry/GetAllPaginatedIndustries',
		GetAllIndustries: '/api/Industry/GetAllIndustries',
		GetIndustryById: '/api/Industry/GetIndustryById',
		DisableIndustryById: '/api/Industry/DisableIndustryById',
		ActivateIndustryById: '/api/Industry/ActivateIndustryById',
		DeleteIndustryById: '/api/Industry/DeleteIndustryById',
		AddNewIndustry: '/api/Industry/AddNewIndustry',
		UpdateIndustry: '/api/Industry/UpdateIndustry'
	},
	JobPosition: {
		GetAllPaginatedJobPositions: '/api/CrmVocabularies/GetAllPaginatedJobPositions',
		GetAllJobPositions: '/api/CrmVocabularies/GetAllJobPositions',
		GetJobPositionById: '/api/CrmVocabularies/GetJobPositionById',
		AddNewJobPosition: '/api/CrmVocabularies/AddNewJobPosition',
		UpdateJobPosition: '/api/CrmVocabularies/UpdateJobPosition',
		DeleteJobPositionById: '/api/CrmVocabularies/DeleteJobPositionById',
		ActivateJobPositionById: '/api/CrmVocabularies/ActivateJobPositionById',
		DisableJobPositionById: '/api/CrmVocabularies/DisableJobPositionById'
	},
	Leads: {
		GetLeadById: '/api/Leads/GetLeadById',
		GetLeadStateById: '/api/Leads/GetLeadStateById',
		GetAllLeads: '/api/Leads/GetAllLeads',
		GetGridLeadsByPipelineId: '/api/Leads/GetGridLeadsByPipelineId',
		GetLeadsByPipeLineId: '/api/Leads/GetLeadsByPipeLineId',
		GetLeadsByOrganizationId: '/api/Leads/GetLeadsByOrganizationId',
		GetPaginatedLeadsByPipeLineId: '/api/Leads/GetPaginatedLeadsByPipeLineId',
		GetPaginatedLeads: '/api/Leads/GetPaginatedLeads',
		GetPaginatedLeadsByOrganizationId: '/api/Leads/GetPaginatedLeadsByOrganizationId',
		AddLead: '/api/Leads/AddLead',
		UpdateLead: '/api/Leads/UpdateLead',
		AddLeadContact: '/api/Leads/AddLeadContact',
		DeleteLeadContact: '/api/Leads/DeleteLeadContact',
		ActivateLead: '/api/Leads/ActivateLead',
		DisableLead: '/api/Leads/DisableLead',
		DeleteLead: '/api/Leads/DeleteLead',
		MoveLeadToStage: '/api/Leads/MoveLeadToStage',
		AddLeadState: '/api/Leads/AddLeadState',
		OrderLeadStates: '/api/Leads/OrderLeadStates',
		GetAllLeadStates: '/api/Leads/GetAllLeadStates',
		ActivateLeadState: '/api/Leads/ActivateLeadState',
		DisableLeadState: '/api/Leads/DisableLeadState',
		RemoveLeadState: '/api/Leads/RemoveLeadState',
		RenameLeadState: '/api/Leads/RenameLeadState',
		ChangeLeadState: '/api/Leads/ChangeLeadState',
		SetLeadMembers: '/api/Leads/SetLeadMembers',
		MergeLeads: '/api/Leads/MergeLeads',
		ImportLeads: '/api/Leads/ImportLeads',
		AddNoGoState: '/api/Leads/AddNoGoState',
		UpdateNoGoState: '/api/Leads/UpdateNoGoState',
		GetNoGoStateById: '/api/Leads/GetNoGoStateById',
		ActivateNoGoState: '/api/Leads/ActivateNoGoState',
		DisableNoGoState: '/api/Leads/DisableNoGoState',
		DeleteNoGoState: '/api/Leads/DeleteNoGoState',
		GetAllNoGoStates: '/api/Leads/GetAllNoGoStates',
		GetAllPaginatedNoGoStates: '/api/Leads/GetAllPaginatedNoGoStates',
		AddLeadToNoGoState: '/api/Leads/AddLeadToNoGoState',
		RemoveLeadsFromNoGoState: '/api/Leads/RemoveLeadsFromNoGoState',
		GetLeadsByNoGoState: '/api/Leads/GetLeadsByNoGoState',
		GetAllLeadStatesByStage: '/api/Leads/GetAllLeadStatesByStage',
		GetAllContactsByLeadId: '/api/Leads/GetAllContactsByLeadId',
		GetFiles: '/api/Leads/GetFiles',
		UploadFile: '/api/Leads/UploadFile',
		AddProductOrServices: '/api/Leads/AddProductOrServices',
		UpdateProductOrServices: '/api/Leads/UpdateProductOrServices',
		DeleteProductOrServices: '/api/Leads/DeleteProductOrServices'
	},
	Location: {
		AddNewCountry: '/api/Location/AddNewCountry',
		DeleteCountry: '/api/Location/DeleteCountry',
		GetAllCountries: '/api/Location/GetAllCountries',
		GetAllPaginatedCountries: '/api/Location/GetAllPaginatedCountries',
		GetCountryById: '/api/Location/GetCountryById',
		UpdateCountry: '/api/Location/UpdateCountry',
		GetCitiesByCountry: '/api/Location/GetCitiesByCountry',
		AddCityToCountry: '/api/Location/AddCityToCountry',
		RemoveCity: '/api/Location/RemoveCity',
		GetCityById: '/api/Location/GetCityById',
		UpdateCity: '/api/Location/UpdateCity'
	},
	Localization: {
		GetAvailablesLanguages: '/Localization/GetAvailablesLanguages',
		ChangeLanguage: '/Localization/ChangeLanguage',
		GetCurrentLanguage: '/Localization/GetCurrentLanguage'
	},
	Payments: {
		GetPaymentById: '/api/Payment/GetPaymentById',
		GetAllPayments: '/api/Payment/GetAllPayments',
		GetAllPaginatedPayments: '/api/Payment/GetAllPaginatedPayments',
		AddPayment: '/api/Payment/AddPayment',
		UpdatePayment: '/api/Payment/UpdatePayment',
		DisablePayment: '/api/Payment/DisablePayment',
		ActivatePayment: '/api/Payment/ActivatePayment',
		DeletePayment: '/api/Payment/DeletePayment',
		ImportXml: '/api/Payment/ImportXml'
	},
	PaymentCode: {
		GetPaymentCodeById: '/api/CrmPaymentCode/GetPaymentCodeById',
		GetPaymentCodeByCode: '/api/CrmPaymentCode/GetPaymentCodeByCode',
		GetAllPaymentCode: '/api/CrmPaymentCode/GetAllPaymentCode',
		GetAllPaginatedPaymentCode: '/api/CrmPaymentCode/GetAllPaginatedPaymentCode',
		AddPaymentCode: '/api/CrmPaymentCode/AddPaymentCode',
		UpdatePaymentCode: '/api/CrmPaymentCode/UpdatePaymentCode',
		DisablePaymentCode: '/api/CrmPaymentCode/DisablePaymentCode',
		ActivatePaymentCode: '/api/CrmPaymentCode/ActivatePaymentCode',
		RemovePaymentCode: '/api/CrmPaymentCode/RemovePaymentCode'
	},
	Product: {
		GetAllProducts: '/api/Product/GetAllProducts',
		GetAllCategories: '/api/Product/GetAllCategories',
		GetAllProductManufactories: '/api/Product/GetAllProductManufactories',
		GetAllPaginatedProduct: '/api/Product/GetAllPaginatedProduct',
		GetProductById: '/api/Product/GetProductById',
		AddProduct: '/api/Product/AddProduct',
		UpdateProduct: '/api/Product/UpdateProduct',
		DisableProduct: '/api/Product/DisableProduct',
		ActivateProduct: '/api/Product/ActivateProduct',
		DeleteProduct: '/api/Product/DeleteProduct',
		GetAllProductsOfTypeProduct: '/api/Product/GetAllProductsOfTypeProduct',
		GetAllProductsOfTypeService: '/api/Product/GetAllProductsOfTypeService',
		AddProductVariation: '/api/Product/AddProductVariation',
		UpdateProductVariation: '/api/Product/UpdateProductVariation',
		DeleteProductVariation: '/api/Product/DeleteProductVariation',
		GetProductVariationByProductTemplateId: '/api/Product/GetProductVariationByProductTemplateId',
		AddProductDeliverable: '/api/Product/AddProductDeliverable',
		UpdateProductDeliverable: '/api/Product/UpdateProductDeliverable',
		DeleteProductDeliverable: '/api/Product/DeleteProductDeliverable',
		GetProductDeliverableByProductVariationId: '/api/Product/GetProductDeliverableByProductVariationId',
		ActivateProductVariation: '/api/Product/ActivateProductVariation',
		DisableProductVariation: '/api/Product/DisableProductVariation',
		GetProductDeliverableById: '/api/Product/GetProductDeliverableById',
		GetProductVariationById: '/api/Product/GetProductVariationById',
		RemoveDeliverableFromVariation: '/api/Product/RemoveDeliverableFromVariation',
		GetAllPaginatedProductDeliverable: '/api/Product/GetAllPaginatedProductDeliverable',
		GetAllProductVariations: '/api/Product/GetAllProductVariations',
		DisableProductDeliverable: '/api/Product/DisableProductDeliverable',
		ActivateProductDeliverable: '/api/Product/ActivateProductDeliverable',
		GetProductDeliverablesWithNoVariation: '/api/Product/GetProductDeliverablesWithNoVariation'
	},
	Phone: {
		GetPhoneById: "/api/Phone/GetPhoneById",
		GetPhonesByContactId: "/api/Phone/GetPhonesByContactId",
		AddNewPhone: "/api/Phone/AddNewPhone",
		AddPhoneRange: "/api/Phone/AddPhoneRange",
		UpdatePhone: "/api/Phone/UpdatePhone",
		UpdateRangePhone: "/api/Phone/UpdateRangePhone",
		DeletePhoneById: "/api/Phone/DeletePhoneById",
		GetAllPhoneLabels: "/api/Phone/GetAllPhoneLabels"
	},
	Email: {
		GetEmailById: '/api/Email/GetEmailById',
		GetEmailsByContactId: '/api/Email/GetEmailsByContactId',
		GetEmailsByOrganizationId: '/api/Email/GetEmailsByOrganizationId',
		GetEmailsByLeadId: '/api/Email/GetEmailsByLeadId',
		GetAllEmailLabel: '/api/Email/GetAllEmailLabel',
		AddNewEmail: '/api/Email/AddNewEmail',
		AddEmailRange: '/api/Email/AddEmailRange',
		UpdateEmailAsync: '/api/Email/UpdateEmailAsync',
		UpdateRangeEmail: '/api/Email/UpdateRangeEmail',
		DeleteEmailById: '/api/Email/DeleteEmailById'
	},
	Category: {
		GetAllCategories: "/api/Category/GetAllCategories",
		GetAllPaginatedCategory: "/api/Category/GetAllPaginatedCategory",
		GetCategoryById: "/api/Category/GetCategoryById",
		AddCategory: "/api/Category/AddCategory",
		UpdateCategory: "/api/Category/UpdateCategory",
		DisableCategory: "/api/Category/DisableCategory",
		ActivateCategory: "/api/Category/ActivateCategory",
		DeleteCategory: "/api/Category/DeleteCategory"
	},
	Manufactory: {
		GetAllManufactories: "/api/Manufactory/GetAllManufactories",
		GetAllPaginatedManufactories: "/api/Manufactory/GetAllPaginatedManufactoy",
		GetManufactoryById: "/api/Manufactory/GetManufactoryById",
		AddManufactory: "/api/Manufactory/AddManufactory",
		UpdateManufactory: "/api/Manufactory/UpdateManufactory",
		DisableManufactory: "/api/Manufactory/DisableManufactory",
		ActivateManufactory: "/api/Manufactory/ActivateManufactory",
		DeleteManufactory: "/api/Manufactory/DeleteManufactory"
	},
	Roles: {
		RefreshCachedPermissionsForEachRole: '/api/Roles/RefreshCachedPermissionsForEachRole',
		GetUsersInRoleForCurrentCompany: '/api/Roles/GetUsersInRoleForCurrentCompany',
		ChangeUserRoles: '/api/Roles/ChangeUserRoles',
		GetUserRoles: '/api/Roles/GetUserRoles',
		GetUserRolesWithAllRoles: '/api/Roles/GetUserRolesWithAllRoles',
		GetRolesById: '/api/Roles/GetRolesById',
		GetAllRolesAsync: '/api/Roles/GetAllRolesAsync',
		GetAllPaginatedRoles: '/api/Roles/GetAllPaginatedRoles',
		CurrentUserIsAdministrator: '/api/Roles/CurrentUserIsAdministrator',
		AddRole: '/api/Roles/AddRole',
		GetPermissionsByClientId: '/api/Roles/GetPermissionsByClientId',
		GetClients: '/api/Roles/GetClients',
		EditRole: '/api/Roles/EditRole',
		DeactivateRole: '/api/Roles/DeactivateRole',
		ActivateRole: '/api/Roles/ActivateRole',
		DeleteRole: '/api/Roles/DeleteRole'
	},
	TaskManager: {
		GetTaskPriorityList: '/api/TaskManager/GetTaskPriorityList',
		GetTaskStatusList: '/api/TaskManager/GetTaskStatusList',
		GetUsersList: '/api/TaskManager/GetUsersList',
		GetTask: '/api/TaskManager/GetTask',
		GetUserTasks: '/api/TaskManager/GetUserTasks',
		GetAssignedTasks: '/api/TaskManager/GetAssignedTasks',
		GetAllUsersTasks: '/api/TaskManager/GetAllUsersTasks',
		GetAllTasks: '/api/TaskManager/GetAllTasks',
		GetTaskByLeadId: '/api/TaskManager/GetTaskByLeadId',
		GetTaskItems: '/api/TaskManager/GetTaskItems',
		CreateTask: '/api/TaskManager/CreateTask',
		UpdateTask: '/api/TaskManager/UpdateTask',
		DeleteTask: '/api/TaskManager/DeleteTask',
		DeleteTaskPermanent: '/api/TaskManager/DeleteTaskPermanent',
		RestoreTask: '/api/TaskManager/RestoreTask',
		CreateTaskItem: '/api/TaskManager/CreateTaskItem',
		UpdateTaskItem: '/api/TaskManager/UpdateTaskItem',
		DeleteTaskItem: '/api/TaskManager/DeleteTaskItem'
	},
	TaskType: {
		GetAllPaginatedTaskType: '/api/TaskType/GetAllPaginatedTaskType',
		GetAllTaskType: '/api/TaskType/GetAllTaskType',
		GetTaskTypeById: '/api/TaskType/GetTaskTypeById',
		AddTaskTypeAsync: '/api/TaskType/AddTaskTypeAsync',
		UpdateTaskType: '/api/TaskType/UpdateTaskType',
		DeleteTaskType: '/api/TaskType/DeleteTaskType',
		ActivateTaskType: '/api/TaskType/ActivateTaskType',
		DisableTaskType: '/api/TaskType/DisableTaskType',
	},
	Team: {
		GetTeamById: '/api/Team/GetTeamById',
		GetPaginatedTeam: '/api/Team/GetPaginatedTeam',
		GetAllTeams: '/api/Team/GetAllTeams',
		GetTeamsByUserId: '/api/Team/GetTeamsByUserId',
		DeleteTeamById: '/api/Team/DeleteTeamById',
		DisableTeam: '/api/Team/DisableTeam',
		ActivateTeam: 'api/Team/ActivateTeam',
		AddTeam: '/api/Team/AddTeam',
		UpdateTeam: '/api/Team/UpdateTeam',
		GetTeamRoleById: '/api/Team/GetTeamRoleById',
		GetAllTeamRoles: '/api/Team/GetAllTeamRoles',
		GetTeamUserRole: '/api/Team/GetTeamUserRole',
		DeleteTeamRole: '/api/Team/DeleteTeamRole',
		AddTeamRole: '/api/Team/AddTeamRole',
		UpdateTeamRole: '/api/Team/UpdateTeamRole',
		GetTeamMemberById: '/api/Team/GetTeamMemberById',
		GetAllTeamMember: '/api/Team/GetAllTeamMember',
		GetTeamMembersByTeamId: '/api/Team/GetTeamMembersByTeamId',
		AddNewMemberToTeam: '/api/Team/AddNewMemberToTeam',
		DeleteMemberToTeam: '/api/Team/DeleteMemberToTeam'
	},
	Users: {
		GetUserById: '/api/Users/GetUserById',
		GetPaginatedUser: '/api/Users/GetPaginatedUser',
		GetUsers: '/api/Users/GetUsers',
		DeactivateUser: '/api/Users/DeactivateUser',
		ActivateUser: '/api/Users/ActivateUser',
		GetCurrentUserInfo: '/api/Users/GetCurrentUserInfo',
		AddUser: '/api/Users/AddUser',
		UpdateUser: '/api/Users/UpdateUser',
		DeleteUser: '/api/Users/DeleteUser',
		InviteNewUserAsync: '/api/Users/InviteNewUserAsync',
		ChangePassword: '/api/Users/ChangePassword',
		UpdateAccountInformation: '/api/Users/UpdateAccountInformation',
		GetUserRolesByUserId: '/api/Users/GetUserRolesByUserId',
		UpdateUserRolesByUserId: '/api/Users/UpdateUserRolesByUserId',
		GetUsersFromLdap: '/api/Users/GetUsersFromLdap'
	},
	WorkCategory: {
		GetAllPaginatedWorkCategory: '/api/WorkCategory/GetAllPaginatedWorkCategory',
		GetAllWorkCategories: '/api/WorkCategory/GetAllWorkCategories',
		GetWorkCategoryById: '/api/WorkCategory/GetWorkCategoryById',
		AddWorkCategory: '/api/WorkCategory/AddWorkCategory',
		UpdateWorkCategory: '/api/WorkCategory/UpdateWorkCategory',
		DeleteWorkCategoryById: '/api/WorkCategory/DeleteWorkCategoryById',
		DisableWorkCategoryByI: '/api/WorkCategory/DisableWorkCategoryByI',
		ActivateWorkCategoryById: '/api/WorkCategory/ActivateWorkCategoryById'
	},
	Campaign: {
		GetAllCampaignsPaginated: '/api/Campaign/GetAllCampaignsPaginated',
		GetAllCampaigns: '/api/Campaign/GetAllCampaigns',
		AddNewCampaign: '/api/Campaign/AddNewCampaign',
		DisableCampaignById: '/api/Campaign/DisableCampaignById',
		EnableCampaignById: '/api/Campaign/EnableCampaignById',
		DeleteCampaignById: '/api/Campaign/DeleteCampaignById',
		GetCampaignById: '/api/Campaign/GetCampaignById',
		UpdateCampaign: '/api/Campaign/UpdateCampaignById',
	},
	MarketingList: {
		GetMarketingListsPaginated: '/api/MarketingList/GetPaginatedMarketingList',
		GetAllMarketingLists: '/api/MarketingList/GetAllMarketingLists',
		AddNewMarketingList: '/api/MarketingList/AddMarketingList',
		AddNewMemberToList: '/api/MarketingList/AddNewMemberOrganizationToList',
		UpdateMarketingList: '/api/MarketingList/UpdateMarketingListById',
		DisableMarketingList: '/api/MarketingList/DisableMarketingListById',
		EnableMarketingList: '/api/MarketingList/EnableMarketingListById',
		DeleteMarketingList: '/api/MarketingList/DeleteMarketingListById ',
		GetMarketingListById: '/api/MarketingList/GetMarketingListById',

	}
}
