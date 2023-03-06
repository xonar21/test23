Vue.use(Vuex);

let storageStateSidebar = false;
if (typeof localStorage.sidebarOpen !== "undefined") {
	storageStateSidebar = JSON.parse(localStorage.sidebarOpen);
}

const state = {
	sidebarOpen: storageStateSidebar,
	userSidebarOpen: false,
	userNotifications: {
		count: 0,
		notifications: []
	},
	menuLoaded: false,
	customBreadcrumbs: [],

	// Initialize state allMenus (Alexandru Barac)
	allMenus: [],
	allPipelines: [],
	allPipelinesForSelect: [],
	allOrganizations: [],
	allOrganizationsForSelect: [],
	allUsers: [],
	allLeads: [],
	gridLeads: [],
	allCurrencies: [],
	allContracts: [],
	allContacts: [],
	allProducts: [],
	allAgreements: [],
	allTaskPriorityList: [],
	allTaskStatusList: [],
	allTaskTypes: [],
	allProductTypes: [],
	userInfo: {},
	allCountries: [],
	allEmployees: [],
	allJobPositions: [],
	selectorsForOrganization: [],
	allOrganizationStages: [],
	allOrganizationStagesList: [],
	allSources: [],
	allTechnologyTypes: [],
	allSolutionTypes: [],
	allEmailLabels: [],
	allPhoneLabels: [],
	allProductServices: [],
	allServiceTypes: [],
	allDevelopementServices: [],
	allQaServices: [],
	allConsultancyServices: [],
	allDesigneServices: [],
	allPmFrameWorks: [],
	allDevelopementFrameWorks: [],
	allTechnologyTypes: [],
	allLdapUsers: [],
	pipelineStages: [],
	contactsByOrganizationId: [],
	userNotifications: [],
	allRoles: [],
	requestedUserRoles: []
};

const mutations = {
	toggleUserSidebar(state) {
		state.userSidebarOpen = !state.userSidebarOpen;
	},
	closeUserSidebar(state) {
		state.userSidebarOpen = false;
	},
	toggleSidebar(state) {
		state.sidebarOpen = !state.sidebarOpen;
		localStorage.sidebarOpen = state.sidebarOpen;
	},
	closeSidebar(state) {
		state.sidebarOpen = false;
		localStorage.sidebarOpen = state.sidebarOpen
	},
	openSidebar(state) {
		state.sidebarOpen = true;
		localStorage.sidebarOpen = true;
	},
	changeCountNotifications(state, count) {
		state.userNotifications.count = count;
	},
	chanegMenuLoaded(state) {
		state.menuLoaded = true;
	},
	setCustomBreadcrumbs(state, list) {
		state.customBreadcrumbs = list;
	},

	// Initialize mutation allMenus (Alexandru Barac)
	setMenus(state, menus) {
		state.allMenus = menus;
	},
	setPipelines(state, pipes) {
		state.allPipelines = pipes;
	},
	setPipelinesForSelect(state, pipes) {
		state.allPipelinesForSelect = pipes;
	},
	setOrganizations(state, orgs) {
		state.allOrganizations = orgs;
	},
	setOrganizationsForSelect(state, orgs) {
		state.allOrganizationsForSelect = orgs;
	},
	setUsers(state, users) {
		state.allUsers = users;
	},
	setLeads(state, leads) {
		state.allLeads = leads;
	},
	setGridLeads(state, gridLeads) {
		state.gridLeads = gridLeads;
	},
	setCurrencies(state, currencies) {
		state.allCurrencies = currencies;
	},
	setContracts(state, contracts) {
		state.allContracts = contracts;
	},
	setContacts(state, contacts) {
		state.allContacts = contacts;
	},
	setProducts(state, products) {
		state.allProducts = products;
	},
	setAgreements(state, agreements) {
		state.allAgreements = agreements;
	},
	setTaskPriorityList(state, taskPriorityList) {
		state.allTaskPriorityList = taskPriorityList;
	},
	setTaskStatusList(state, taskStatusList) {
		state.allTaskStatusList = taskStatusList;
	},
	setProductTypes(state, productTypes) {
		state.allProductTypes = productTypes;
	},
	setUserInfo(state, userInfo) {
		state.userInfo = userInfo;
	},
	setCountries(state, countries) {
		state.allCountries = countries;
	},
	setEmployees(state, employees) {
		state.allEmployees = employees;
	},
	setJobPositions(state, jobPositions) {
		state.allJobPositions = jobPositions;
	},
	setSelectorsForOrganization(state, selectorsForOrgs) {
		state.selectorsForOrganization = selectorsForOrgs;
	},
	setOrganizationStages(state, organizationStages) {
		state.allOrganizationStages = organizationStages;
	},
	setOrganizationStagesList(state, organizationStages) {
		state.allOrganizationStagesList = organizationStages;
	},
	setSources(state, sources) {
		state.allSources = sources;
	},
	setTechnologyType(state, technologyTypes) {
		state.allTechnologyTypes = technologyTypes;
	},
	setSolutionTypes(state, solutionTypes) {
		state.allSolutionTypes = solutionTypes;
	},
	setEmailLabels(state, emailLabels) {
		state.allEmailLabels = emailLabels;
	},
	setPhoneLabels(state, phoneLabels) {
		state.allPhoneLabels = phoneLabels;
	},
	setTaskTypes(state, taskTypes) {
		state.allTaskTypes = taskTypes;
	},
	setProductServices(state, productServices) {
		state.allProductServices = productServices;
	},
	setServicesTypes(state, servicesTypes) {
		state.allServiceTypes = servicesTypes;
	},
	setDevelopementServices(state, developementServices) {
		state.allDevelopementServices = developementServices
	},
	setDesigneServices(state, designeServices) {
		state.allDesigneServices = designeServices;
	},
	setQaServices(state, qaServices) {
		state.allQaServices = qaServices;
	},
	setConsultancyServices(state, consultancyServices) {
		state.allConsultancyServices = consultancyServices;
	},
	setPMFrameWorks(state, pmFrameWorks) {
		state.allPmFrameWorks = pmFrameWorks;
	},
	setDevelopementFrameWorks(state, developementFrameWorks) {
		state.allDevelopementFrameWorks = developementFrameWorks;
	},
	setTechnologyTypes(state, technologyTypes) {
		state.allTechnologyTypes = technologyTypes;
	},
	setPipeLineStages(state, pipelineStages) {
		state.pipelineStages = pipelineStages;
	},
	setContactsByOrganizationId(state, contacts) {
		state.contactsByOrganizationId = contacts;
	},
	setUserNotififications(state, notifications) {
		state.userNotifications = notifications;
	},
	setLdapUsers(state, ldapUsers) {
		state.allLdapUsers = ldapUsers;
	},
	setAllRoles(state, roles) {
		state.allRoles = roles;
	},
	setRequestedUserRoles(state, roles) {
		state.requestedUserRoles = roles;
	},
	unsetRoles(state) {
		state.requestedUserRoles = [];
	}
};
const actions = {
	toggleUserSidebarAction(context) {
		context.commit("toggleUserSidebar");
	},
	closeUserSidebarAction(context) {
		context.commit("closeUserSidebar");
	},
	toggleSidebarAction(context) {
		context.commit("toggleSidebar");
	},
	closeSidebarAction(context) {
		context.commit("closeSidebar");
	},
	openSidebarAction(context) {
		context.commit("openSidebar");
	},
	changeCount(context, count) {
		context.commit("changeCountNotifications", count);
	},
	setCustomBreadcrumbsAction(context, list) {
		context.commit("setCustomBreadcrumbs", list);
	},
	chanegMenuLoadedAction(context) {
		context.commit("chanegMenuLoaded");
	},

	// Initialize action getAllMenus (Alexandru Barac)
	getAllMenus(context) {
		Promise.all([
			customAjaxRequest(apiEndpoints.GetMenus, 'GET'),
			customAjaxRequest(apiEndpoints.PipeLines.GetAll, 'GET')])
			.then(results => {
				const pipeLinesMenuId = "4549176b-9065-4a6e-be0c-e25176dfec4f";
				const menus = results[0];
				const pipeLines = results[1].sort((a, b) => {
					const bandA = a['name'].toUpperCase();
					const bandB = b['name'].toUpperCase();
					let comparison = 0;
					if (bandA > bandB) {
						comparison = 1;
					} else if (bandA < bandB) {
						comparison = -1;
					}
					return comparison;
				});
				for (let i = 0; i < menus.length; i++) {
					if (menus[i].id === pipeLinesMenuId) {
						for (let j = 0; j < pipeLines.length; j++) {
							menus[i].children.push({
								order: j,
								name: pipeLines[j].name,
								children: [],
								href: `/PipeLine/PipeLineLeads/${pipeLines[j].name.split(' ').join('_')}`
							});
						}
					}
				}
				context.commit('setMenus', menus);
			}).catch(e => {
				console.warn(e);
			});
	},
	getAllPipelines(context) {
		customAjaxRequest(apiEndpoints.PipeLines.GetAll)
			.then(result => {
				const convertedResult = result.map(e => {
					const newObj = {
						label: e.name,
						value: e.id,
						active: false
					}
					return newObj;
				});
				context.commit('setPipelines', convertedResult);
			});
	},
	getAllPipelinesForSelect(context) {
		customAjaxRequest(apiEndpoints.PipeLines.GetAll)
			.then(result => {
				const convertedResult = result.map(e => {
					const newObj = {
						label: e.name,
						value: e.id,
					}
					return newObj;
				});
				context.commit('setPipelinesForSelect', convertedResult);
			});
	},
	getAllOrganizations(context) {
		customAjaxRequest(apiEndpoints.Organization.GetAllOrganization)
			.then(result => {
				context.commit('setOrganizations', result);
			});
	},
	getAllOrganizationsForSelect(context) {
		customAjaxRequest(apiEndpoints.Organization.GetAllOrganization)
			.then(result => {
				const convertedResult = result.map(e => {
					const newObj = {
						label: e.name,
						value: e.id,
					}
					return newObj;
				});
				context.commit('setOrganizationsForSelect', convertedResult);
			});
	},
	getAllContacts(context) {
		customAjaxRequest(apiEndpoints.Contact.GetAllContacts)
			.then(result => {
				const convertedResult = result.map(e => {
					const newObj = {
						label: `${e.firstName.trim()} ${e.lastName.trim()}`,
						value: e.id
					}
					return newObj;
				});
				context.commit('setContacts', convertedResult);
			});
	},
	getAllUsers(context) {
		customAjaxRequest(apiEndpoints.Users.GetUsers)
			.then(result => {
				const convertedResult = result.map(e => {
					const newObj = {
						label: e.email,
						value: e.id,
					}
					return newObj;
				});
				context.commit('setUsers', convertedResult);
			});
	},
	getAllLdapUsers(context) {
		customAjaxRequest(apiEndpoints.Users.GetUsersFromLdap, "GET")
			.then(result => {
				const convertedResult = result.map(e => {
					const newObj = {
						label: e.fullName,
						value: e.id
					}
					return newObj;
				});
				context.commit('setLdapUsers', convertedResult);
			});
	},
	getAllLeads(context) {
		customAjaxRequest(apiEndpoints.Leads.GetAllLeads)
			.then(result => {
				context.commit('setLeads', result);
			});
	},
	getGridLeadsByPipelineId(context, requestData) {
		customAjaxRequest(apiEndpoints.Leads.GetGridLeadsByPipelineId, "POST", requestData)
			.then(result => {
				context.commit('setGridLeads', result);
			});
	},
	getAllCurrencies(context) {
		customAjaxRequest(apiEndpoints.CrmCommon.GetAllCurrencies)
			.then(result => {
				const convertedResult = result.map(e => {
					const newObj = {
						label: e.name,
						value: e.code,
					}
					return newObj;
				});
				context.commit('setCurrencies', convertedResult);
			});
	},
	getAllContractTemlate(context) {
		customAjaxRequest(apiEndpoints.Contract.GetAllContractTemplate)
			.then(result => {
				const convertedResult = result.map(e => {
					const newObj = {
						label: e.name,
						value: e.id,
					}
					return newObj;
				});
				context.commit('setContracts', convertedResult);
			});
	},
	getAllProducts(context) {
		customAjaxRequest(apiEndpoints.Product.GetAllProducts)
			.then(result => {
				const convertedResult = result.map(e => {
					const newObj = {
						label: e.name,
						value: e.id,
					}
					return newObj;
				});
				context.commit('setProducts', convertedResult);
			});
	},
	getAllAgreements(context) {
		customAjaxRequest(apiEndpoints.Agreement.GetAllAgreements)
			.then(result => {
				const convertedResult = result.map(e => {
					const newObj = {
						label: e.name,
						value: e.id,
					}
					return newObj;
				});
				context.commit('setAgreements', convertedResult);
			});
	},
	getAllTaskPriorityList(context) {
		customAjaxRequest(apiEndpoints.TaskManager.GetTaskPriorityList)
			.then(result => {
				context.commit('setTaskPriorityList', result);
			});
	},
	getAllTaskStatusList(context) {
		customAjaxRequest(apiEndpoints.TaskManager.GetTaskStatusList)
			.then(result => {
				context.commit('setTaskStatusList', result);
			});
	},
	getAllProductTypes(context) {
		customAjaxRequest(apiEndpoints.CrmVocabularies.GetAllProductType)
			.then(result => {
				const convertedResult = result.map(e => {
					const newObj = {
						label: e.name,
						value: e.id,
					}
					return newObj;
				});
				context.commit('setProductTypes', convertedResult);
			});
	},
	getCurrenctUserInfo(context) {
		customAjaxRequest(apiEndpoints.Users.GetCurrentUserInfo)
			.then(result => {
				context.commit('setUserInfo', result);
			});
	},
	getAllCountries(context) {
		customAjaxRequest(apiEndpoints.OrganizationAddress.GetAllCountries)
			.then(result => {
				const convertedResult = result.map(e => {
					const newObj = {
						label: e.name,
						value: e.id,
					}
					return newObj;
				});
				context.commit('setCountries', convertedResult);
			});
	},
	getAllEmployees(context) {
		customAjaxRequest(apiEndpoints.Employee.GetAllEmployees)
			.then(result => {
				context.commit('setEmployees', result);
			});
	},
	getAllJobPositions(context) {
		customAjaxRequest(apiEndpoints.CrmVocabularies.GetAllJobPositions)
			.then(result => {
				const convertedResult = result.map(e => {
					const newObj = {
						label: e.name,
						value: e.id,
					}
					return newObj;
				});
				context.commit('setJobPositions', convertedResult);
			});
	},
	getAllSelectorsForOrganization(context) {
		customAjaxRequest(apiEndpoints.OrganizationHelper.GetSelectorsForOrganization)
			.then(result => {
				context.commit('setSelectorsForOrganization', result);
			});
	},
	getAllOrganizationStages(context) {
		customAjaxRequest(apiEndpoints.OrganizationHelper.GetAllOrganizationStages)
			.then(result => {
				const convertedResult = result.map(e => {
					const newObj = {
						label: e.name,
						value: e.id,
					}
					return newObj;
				});
				context.commit('setOrganizationStages', convertedResult);
			});
	},
	getAllOrganizationStagesList(context) {
		customAjaxRequest(apiEndpoints.OrganizationHelper.GetAllOrganizationStages)
			.then(result => {
				context.commit('setOrganizationStagesList', result);
			})
	},
	getAllSource(context) {
		customAjaxRequest(apiEndpoints.CrmVocabularies.GetAllSourcesAsync)
			.then(result => {
				const convertedResult = result.map(e => {
					const newObj = {
						label: e.name,
						value: e.id,
					}
					return newObj;
				});
				context.commit('setSources', convertedResult);
			});
	},
	getAllProductServices(context) {
		customAjaxRequest(apiEndpoints.CrmVocabularies.GetAllProductsAndServices)
			.then(result => {
				const convertedResult = result.map(e => {
					const newObj = {
						label: e.name,
						value: e.id,
					}
					return newObj;
				});
				context.commit('setProductServices', convertedResult);
			});
	},
	getAllServiceTypes(context) {
		customAjaxRequest(apiEndpoints.CrmVocabularies.GetAllServiceType)
			.then(result => {
				const convertedResult = result.map(e => {
					const newObj = {
						label: e.name,
						value: e.id,
					}
					return newObj;
				});
				context.commit('setServicesTypes', convertedResult);
			});
	},
	getAllDevelopementServices(context) {
		customAjaxRequest(apiEndpoints.CrmVocabularies.GetAllDevelopementServices)
			.then(result => {
				const convertedResult = result.map(e => {
					const newObj = {
						label: e.name,
						value: e.id,
					}
					return newObj;
				});
				context.commit('setDevelopementServices', convertedResult);
			});
	},
	getAllDesigneServices(context) {
		customAjaxRequest(apiEndpoints.CrmVocabularies.GetAllDesigneServices)
			.then(result => {
				const convertedResult = result.map(e => {
					const newObj = {
						label: e.name,
						value: e.id,
					}
					return newObj;
				});
				context.commit('setDesigneServices', convertedResult);
			});
	},
	getAllQaServices(context) {
		customAjaxRequest(apiEndpoints.CrmVocabularies.GetAllQAServices)
			.then(result => {
				const convertedResult = result.map(e => {
					const newObj = {
						label: e.name,
						value: e.id,
					}
					return newObj;
				});
				context.commit('setQaServices', convertedResult);
			});
	},
	getAllConsultancyServices(context) {
		customAjaxRequest(apiEndpoints.CrmVocabularies.GetAllConsultancyServices)
			.then(result => {
				const convertedResult = result.map(e => {
					const newObj = {
						label: e.name,
						value: e.id,
					}
					return newObj;
				});
				context.commit('setConsultancyServices', convertedResult);
			});
	},
	getAllPMFrameWorks(context) {
		customAjaxRequest(apiEndpoints.CrmVocabularies.GetAllPMFrameworks)
			.then(result => {
				const convertedResult = result.map(e => {
					const newObj = {
						label: e.name,
						value: e.id,
					}
					return newObj;
				});
				context.commit('setPMFrameWorks', convertedResult);
			});
	},
	getAllDevelopmentFrameWorks(context) {
		customAjaxRequest(apiEndpoints.CrmVocabularies.GetAllDevelopementFrameworks)
			.then(result => {
				const convertedResult = result.map(e => {
					const newObj = {
						label: e.name,
						value: e.id,
					}
					return newObj;
				});
				context.commit('setDevelopementFrameWorks', convertedResult);
			});
	},
	getAllEmailLabels(context) {
		customAjaxRequest(apiEndpoints.Email.GetAllEmailLabel).then(result => {
			let index = 0;
			const convertedResult = result.map(e => {
				const newObj = {
					label: e,
					value: index,
				}
				index++;
				return newObj;
			});
			context.commit('setEmailLabels', convertedResult);
		});
	},
	getAllPhoneLabels(context) {
		customAjaxRequest(apiEndpoints.Phone.GetAllPhoneLabels).then(result => {
			let index = 0;
			const convertedResult = result.map(e => {
				const newObj = {
					label: e,
					value: index,
				}
				index++;
				return newObj;
			});
			context.commit('setPhoneLabels', convertedResult);
		})
	},
	getAllTechnologyTypes(context) {
		customAjaxRequest(apiEndpoints.CrmVocabularies.GetAllTechnologyType)
			.then(result => {
				const convertedResult = result.map(e => {
					const newObj = {
						label: e.name,
						value: e.id,
					}
					return newObj;
				});
				context.commit('setTechnologyType', convertedResult);
			});
	},
	getAllSolutionTypes(context) {
		customAjaxRequest(apiEndpoints.CrmVocabularies.GetAllSolutionType)
			.then(result => {
				const convertedResult = result.map(e => {
					const newObj = {
						label: e.name,
						value: e.id,
					}
					return newObj;
				});
				context.commit('setSolutionTypes', convertedResult);
			});
	},
	getAllTaskTypes(context) {
		customAjaxRequest(apiEndpoints.TaskType.GetAllTaskType)
			.then(result => {
				const convertedResult = result.map(e => {
					const newObj = {
						label: e.name,
						value: e.id,
					}
					return newObj;
				});
				context.commit('setTaskTypes', convertedResult);
			})
	},
	getAllTechnologyTypes(context) {
		customAjaxRequest(apiEndpoints.CrmVocabularies.GetAllTechnologyType)
			.then(result => {
				const convertedResult = result.map(e => {
					const newObj = {
						label: e.name,
						value: e.id,
					}
					return newObj;
				});
				context.commit('setTechnologyTypes', convertedResult);
			})
	},
	getPipeLineStages(context, requestData) {
		customAjaxRequest(apiEndpoints.PipeLines.GetPipeLineStages, 'GET', requestData)
			.then(result => {
				context.commit('setPipeLineStages', result)
			})
	},
	getContactsByOrganizationId(context, requestData) {
		customAjaxRequest(apiEndpoints.Contact.GetContactByOrganizationId, 'GET', requestData)
			.then(result => {
				context.commit('setContactsByOrganizationId', result)
			})
	},
	getUserNotifications(context, userId) {
		customAjaxRequest(apiEndpoints.Notifications.GetNotificationsByUserId, 'GET', { userId: userId }).then(result => {
			context.commit('setUserNotififications', result)
		}).catch(() => {
			context.commit('setUserNotififications', [])
		});
	},
	getAllRoles(context) {
		return customAjaxRequest(apiEndpoints.Roles.GetAllRolesAsync).then(roles => {
			context.commit('setAllRoles', roles)
		})
	},
	getRequestedUserRoles(context, userId) {
		return customAjaxRequest(apiEndpoints.Users.GetUserRolesByUserId, "GET", { UserId: userId }).then(roles => {
			context.commit('setRequestedUserRoles', roles);
		})
	},
	modifyRoles(context, roles) {
		context.commit('setRequestedUserRoles', roles.map(r => ({ id: r })));
	},
	resetRoles(context) {
		context.commit('unsetRoles');
	}
};
const getters = {
	convertToSelectList: () => (array) => {
		return array.map(e => {
			return {
				label: e.name,
				value: e.id
			}
		})
	},
	convertToValues: () => (array) => {
		return array.map(e => e.id)
	},
	convertContactsToSelectList: () => (array) => {
		return array.map(e => {
			return {
				label: `${e.firstName} ${e.lastName}`,
				value: e.id
			}
		})
	},
	groupBy: () => (array, key) => {
		return array.reduce(function (result, x) {
			(result[x[key]] = result[x[key]] || []).push(x);
			return result;
		}, {});
	},
	pipelineStagesSelectList(state, getters) {
		return getters.convertToSelectList(state.pipelineStages);
	},
	contactsByOrganizationIdSelectList(state, getters) {
		return getters.convertContactsToSelectList(state.contactsByOrganizationId);
	},
	allOrganizationsSelect(state, getters) {
		return getters.convertToSelectList(state.allOrganizations);
	},
	allOrganizationsGroupedByStageId(state, getters) {
		return getters.groupBy(state.allOrganizations, 'stageId')
	},
	allRolesForSelect(state, getters) {
		return getters.convertToSelectList(state.allRoles);
	},
	requestedUserRolesForSelect(state, getters) {
		return getters.convertToValues(state.requestedUserRoles);
	}
};

const store = new Vuex.Store({
	state,
	mutations,
	actions,
	getters,
	plugins: [createPersistedState({
		key: "bizon360crmkey",
		reducer: state => ({
			allMenus: state.allMenus,
			userInfo: state.userInfo,
			menuLoaded: state.menuLoaded,
			customBreadcrumbs: state.customBreadcrumbs,
			sidebarOpen: state.sidebarOpen,
			allCurrencies: state.allCurrencies,
			allTaskPriorityList: state.allTaskPriorityList,
			allTaskStatusList: state.allTaskStatusList,
			allProductTypes: state.allProductTypes,
			allCountries: state.allCountries,
			allSources: state.allSources,
			allTechnologyTypes: state.allTechnologyTypes,
			allSolutionTypes: state.allSolutionTypes,
			allEmailLabels: state.allEmailLabels,
			allPhoneLabels: state.allPhoneLabels,
			allOrganizationStages: state.allOrganizationStages
		}),
	})],
	modules: [
	]
});
