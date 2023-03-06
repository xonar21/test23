Vue.component('EditLeadModalForm', {
	template: `<div>
			<Modal :modalProps="editLeadModalProps" @newValue="emitValueLead"  :refreshInputs="refreshInputs" :inputsKey="modalKey"/>
	</div>`,
	props: {
	},
	data() {
		return {
			dateFormatTask: 'YYYY/MM/DD',
			datePickerFormatTask: 'yyyy/mm/dd',
			addAndNew: false,
			orgSelect: [],
			leadValues: {},
			editOrgId: null,
			pageRequestFilters: [],
			modalKey: 0,
			refreshInputs: 0,
			waitAddressButton: false,
			leadStatesSelect: [],
			currenciesListSelect: [],
			usersListSelect: [],
			pipelineStagesListSelect: [],
			organisationContactsSelect: [],
			productTypesListSelect: [],
			membersUsers: [],
			productsListSelect: [],
			//Default role id
			defaultOwnerRoleId: '11447a80-eff9-4f68-b24f-353ab2d5ee92',
			defaultTeamRoleId: 'c92e023d-6804-43e8-88a8-ec807427d850',
		};
	},
	computed: {
		modalLeadSubmits() {
			const resultArray = [
				{
					name: 'Button',
					props: {
						label: 'Cancel',
						btnType: 'outline-secondary',
						onClick: () => {
							$(`#${this.editLeadModalProps.id}`).modal("hide");
						}
					}
				}];
			resultArray.push(
				{
					name: 'Button',
					props: {
						label: 'Save',
						btnType: 'success',
						btnDOMType: 'submit',
						waiting: this.waitAddButton,
						onMouseDown: () => {
							this.addAndNew = false;
						}
					}
				}
			);
			return resultArray;
		},
		modalAddressLabel() {
			return "Edit lead";
		},
		editLeadModalProps() {
			return {
				id: 'editLeadModal',
				modalSize: 'lg',
				label: this.modalAddressLabel,
				formInputs: [
					{
						name: 'Select',
						props: {
							id: 'editLead-pipeLineId',
							label: 'PipeLine',
							required: true,
							disabled: true,
							options: this.$store.state.allPipelines || [],
							className: 'col-12',
							value: this.leadValues.pipeLineId ? this.leadValues.pipeLineId : ''
						}
					},
					{
						name: 'Input',
						props: {
							id: 'editLead-name',
							type: 'text',
							label: 'Name',
							required: true,
							className: 'col-12',
							value: this.leadValues.name,
							validator: value => fieldValidationFunc(value, 'varChar300'),
							validatorInput: value => fieldValidationInputFunc(value, 'varChar300')
						}
					},
					{
						name: 'Datepicker',
						props: {
							id: 'editLead-created',
							label: 'Start date',
							className: 'col-12 col-md-6',
							format: this.datePickerFormatTask,
							value: this.leadValues.created
						}
					},
					{
						name: 'Datepicker',
						props: {
							id: 'editLead-deadLine',
							label: 'End date',
							required: true,
							className: 'col-12 col-md-6',
							format: this.datePickerFormatTask,
							value: this.leadValues.deadLine
						}
					},
					{
						name: 'Datepicker',
						props: {
							id: 'editLead-clarificationDeadline',
							label: 'Clarification deadline',
							required: true,
							className: 'col-12',
							format: this.datePickerFormatTask,
							value: this.leadValues.clarificationDeadline
						}
					},
					{
						name: 'Select',
						props: {
							id: 'editLead-organizationId',
							label: 'Organization',
							required: true,
							options: this.orgSelect,
							value: this.leadValues.organizationId ? this.leadValues.organizationId : '',
							size: 10,
							searchable: true,
							className: 'col-12 col-md-6'
						}
					},
					{
						name: 'Select',
						props: {
							id: 'editLead-contactId',
							label: 'Contact',
							options: this.organisationContactsSelect,
							noneSelectedText: this.organisationContactsSelect.length > 0 ? 'Nothing selected' : 'Plese select organization with contacts',
							className: 'col-12 col-md-6',
							size: 10,
							searchable: true,
							value: this.leadValues.contactId ? this.leadValues.contactId : ''
						}
					},
					{
						name: 'Select',
						props: {
							id: 'editLead-stageId',
							label: 'Stage',
							required: true,
							options: this.pipelineStagesListSelect,
							className: 'col-12 col-md-6',
							value: this.leadValues.stageId ? this.leadValues.stageId : ''
						}
					},
					{
						name: 'Select',
						props: {
							id: 'editLead-leadStateId',
							label: 'State',
							required: true,
							options: this.leadStatesSelect,
							className: 'col-12 col-md-6',
							value: this.leadValues.leadStateId ? this.leadValues.leadStateId : ''
						}
					},
					{
						name: 'hr',
						props: {
							class: 'line-between-inputs'
						}
					},
					{
						name: 'Select',
						props: {
							id: 'lead-productType',
							label: 'Type of Product(Product/Service)',
							options: [
								{
									label: 'Product',
									value: 0,
								},
								{
									label: 'Service',
									value: 1
								},
							],
							className: 'col-12 col-md-6',
							value: this.leadValues.productType?.id ? this.leadValues.productType.id : ''
						}
					},
					{
						name: 'Select',
						props: {
							id: 'editLead-productId',
							label: 'Product',
							noneSelectedText: this.leadValues.productTypeId ? 'Nothing selected' : 'Plese select a product type first',
							options: this.productsListSelect,
							className: 'col-12 col-md-6',
							value: this.leadValues.productId ? this.leadValues.productId : ''
						}
					},
					{
						name: 'Select',
						props: {
							id: 'editLead-serviceTypeId',
							label: 'Service type',
							options: this.serviceTypesListSelect,
							className: 'col-12 col-md-6',
							value: this.leadValues.serviceTypeId ? this.leadValues.serviceTypeId : ''
						}
					},
					{
						name: 'Select',
						props: {
							id: 'editLead-solutionTypeId',
							label: 'Solution type',
							options: this.solutionTypesListSelect,
							className: 'col-12 col-md-6',
							value: this.leadValues.solutionTypeId ? this.leadValues.solutionTypeId : ''
						}
					},
					{
						name: 'Select',
						props: {
							id: 'editLead-sourceId',
							label: 'Source type',
							options: this.sourceTypesListSelect,
							className: 'col-12 col-md-6',
							value: this.leadValues.sourceId ? this.leadValues.sourceId : ''
						}
					},
					{
						name: 'Select',
						props: {
							id: 'editLead-technologyTypeId',
							label: 'Technology type',
							options: this.technologyTypesListSelect,
							className: 'col-12 col-md-6',
							value: this.leadValues.technologyTypeId ? this.leadValues.technologyTypeId : ''
						}
					},
					{
						name: 'hr',
						props: {
							class: 'line-between-inputs'
						}
					},
					{
						name: 'Input',
						props: {
							id: 'editLead-value',
							label: 'Value',
							type: 'text',
							className: 'col-12 col-md-6',
							value: this.leadValues.value,
							validator: value => fieldValidationFunc(value, 'decimal'),
							validatorInput: value => fieldValidationInputFunc(value, 'decimal')
						}
					},
					{
						name: 'Select',
						props: {
							id: 'editLead-currencyCode',
							label: 'Currency',
							options: this.currenciesListSelect,
							size: 10,
							searchable: true,
							className: 'col-12 col-md-6',
							value: this.leadValues.currencyCode ? this.leadValues.currencyCode : ''
						}
					},
					{
						name: 'Select',
						props: {
							id: 'editLead-owner',
							label: 'Owner',
							required: true,
							options: this.usersListSelect,
							value: this.leadValues.owner ? this.leadValues.owner : '',
							className: 'col-12'
						}
					},
					{
						name: 'Select',
						props: {
							id: 'editLead-members',
							label: 'Members',
							multiple: true,
							searchable: true,
							options: this.membersUsers,
							value: this.leadValues.members,
							className: 'col-12'
						}
					},
					{
						name: 'Textarea',
						props: {
							id: 'editLead-description',
							label: 'Description',
							value: this.leadValues.description,
							className: 'col-12'
						}
					}
				],
				formSubmits: this.modalLeadSubmits,
				onSubmit: () => {
					if (this.leadValues.value != null && this.validateValue(this.leadValues.value)) {
						this.updateLead().then(() => {
							$(`#${this.editLeadModalProps.id}`).modal("hide");
						});
					}
					else {
						toast.notifyErrorList('Make sure your input lead value is right');
					}
				}
			}
		},
	},
	beforeCreate() {
		this.$store.dispatch('getAllPipelines');
	},
	created: async function () {
		this.resetLeadModalValues();
		const promises = [
			customAjaxRequest(apiEndpoints.Organization.GetAllOrganization),
			// customAjaxRequest(apiEndpoints.PipeLines.GetAll),
			customAjaxRequest(apiEndpoints.Leads.GetAllLeadStates),
			customAjaxRequest(apiEndpoints.CrmCommon.GetAllCurrencies),
			customAjaxRequest(apiEndpoints.Users.GetUsers),
			customAjaxRequest(apiEndpoints.CrmVocabularies.GetAllProductType),
			customAjaxRequest(apiEndpoints.CrmVocabularies.GetAllServiceType),
			customAjaxRequest(apiEndpoints.CrmVocabularies.GetAllSolutionType),
			customAjaxRequest(apiEndpoints.CrmVocabularies.GetAllSourcesAsync),
			customAjaxRequest(apiEndpoints.CrmVocabularies.GetAllTechnologyType),
		];
		Promise.all(promises).then(result => {
			this.orgSelect = this.convertToSelectList(result[0]);
			// this.pipelinesListSelect = this.convertToSelectList(result[1]);
			this.leadStatesSelect = this.convertToSelectList(result[1]);
			this.currenciesListSelect = this.convertCurrenciesToSelectList(result[2]);
			this.usersListSelect = this.convertUsersToSelectList(result[3]);
			this.membersUsers = this.convertUsersToSelectList(result[3]);
			this.productTypesListSelect = this.convertArrayToSelectList(result[4], 'name', 'id');
			this.serviceTypesListSelect = this.convertArrayToSelectList(result[5], 'name', 'id');
			this.solutionTypesListSelect = this.convertArrayToSelectList(result[6], 'name', 'id');
			this.sourceTypesListSelect = this.convertArrayToSelectList(result[7], 'name', 'id');
			this.technologyTypesListSelect = this.convertArrayToSelectList(result[8], 'name', 'id');
		});
	},
	methods: {
		async updateLead() {
			this.waitAddButton = true;
			return new Promise((resolve, reject) => {
				customAjaxRequest(apiEndpoints.Leads.UpdateLead, 'POST', this.leadValues).then(() => {
					if (this.leadValues.owner) {
						this.setLeadOwner(this.leadValues.owner, this.leadValues.id, this.leadValues.members);
					} else {
						this.tableKey++;
					}
					resolve(true);
					this.$emit('action');
					this.waitAddButton = false;
				}).catch(e => {
					toast.notifyErrorList(e);
					reject(false);
					this.waitAddButton = false;
				});
			});
		},
		loadOrganizationContacts(organizationId) {
			customAjaxRequest(apiEndpoints.Contact.GetContactByOrganizationId, 'GET', { organizationId }).then(result => {
				this.organisationContactsSelect = this.convertContactsToSelectList(result);
				this.modalKey++;
			});
		},
		async loadLead(leadId) {
			return new Promise((resolve, reject) => {
				customAjaxRequest(apiEndpoints.Leads.GetLeadById, 'GET', { leadId }).then(lead => {
					resolve(lead);
				}).catch(e => {
					toast.notifyErrorList(e);
					reject(false);
				});
			});
		},
		async loadPipelineStages(pipeLineId) {
			customAjaxRequest(apiEndpoints.PipeLines.GetPipeLineStages, 'GET', { pipeLineId }).then(result => {
				this.pipelineStagesListSelect = this.convertToSelectList(result);
				this.modalKey++;
			});
		},
		async setLeadOwner(ownerId, leadId, listMembersId = []) {
			return new Promise((resolve, reject) => {
				customAjaxRequest(apiEndpoints.Leads.SetLeadMembers, 'POST', { ownerId, leadId, listMembersId }).then(() => {
					this.tableKey++;
					resolve(true);
				}).catch(e => {
					toast.notifyErrorList(e);
					reject(false);
				});
			});
		},
		loadProducts(productType) {
			productType == 0 ? this.loadAllProduct() : this.loadAllService();
		},
		loadAllProduct() {
			customAjaxRequest(apiEndpoints.Product.GetAllProductsOfTypeProduct, 'GET').then(result => {
				this.productsListSelect = this.convertToSelectList(result);
				this.modalLeadKey++;
			});
		},
		loadAllService() {
			customAjaxRequest(apiEndpoints.Product.GetAllProductsOfTypeService, 'GET').then(result => {
				this.productsListSelect = this.convertToSelectList(result);
				this.modalLeadKey++;
			});
		},
		editLead(leadId) {
			this.resetLeadModalValues;
			this.refreshInputs++;
			this.modalKey++;
			this.loadLead(leadId).then(value => {
				this.loadPipelineStages(value.pipeLineId);
				this.leadValues = value;

				if (String(this.leadValues.value).length > 1) {
					this.leadValues.value = this.leadValues.value.toLocaleString();
				}
				this.leadValues.name = value.name;
				const owners = this.extractTeamIds(this.leadValues.leadMembers, this.defaultOwnerRoleId);
				if (owners) {
					this.leadValues.owner = owners[0];
					this.changeObjPropInArrayById(this.membersUsers, true, 'disabled', owners[0], 'value');
				}
				this.leadValues.members = this.extractTeamIds(this.leadValues.leadMembers, this.defaultTeamRoleId);
				this.leadValues.created = moment(this.leadValues.created, 'DD.MM.YYYY').format(this.dateFormatTask);
				this.leadValues.deadLine = moment(this.leadValues.deadLine, 'DD.MM.YYYY').format(this.dateFormatTask);
				this.leadValues.clarificationDeadline = moment(this.leadValues.clarificationDeadline, 'DD.MM.YYYY').format(this.dateFormatTask);
				this.loadOrganizationContacts(value.organizationId);
				if (value.product)
				{
					this.loadProducts(value.product.type);
				}
	
				customAjaxRequest(apiEndpoints.Organization.GetOrganizationById, 'GET', { organizationId: value.organizationId }).then(res => {
					this.modalKey++;
					$(`#${this.editLeadModalProps.id}`).modal("show");
				});
				this.modalKey++;
			});
		},
		resetLeadModalValues() {
			this.leadValues = {
				name: '',
				organizationId: null,
				pipeLineId: null,
				stageId: null,
				leadStateId: null,
				value: null,
				currencyCode: null,
				created: moment().format(this.dateFormatTask).toString(),
				deadLine: moment().add(5, 'd').format(this.dateFormatTask).toString(),
				members: [],
				owner: null,
				clarificationDeadline: moment().add(5, 'd').format(this.dateFormatTask).toString(),
				contactId: null,
				productTypeId: null,
				productId: null,
				serviceTypeId: null,
				solutionTypeId: null,
				sourceId: null,
				technologyTypeId: null,
				description: null
			}
			this.modalKey++;
		},
		extractTeamIds(team, roleId) {
			let returnArray = [];
			if (team) {
				team.forEach(m => {
					if (m.teamRoleId == roleId) {
						returnArray.push(m.userId);
					}
				});
			}
			return returnArray;
		},
		emitValueLead(val) {
			const newVal = val.value === 'true' ? true : val.value === 'false' ? false : val.value;
			this.leadValues[val.id.replace('editLead-', '')] = newVal;
			if (val.id == 'editLead-owner') {
				this.membersUsers = this.membersUsers.map(m => {
					m.disabled = false;
					return m;
				});
				this.changeObjPropInArrayById(this.membersUsers, true, 'disabled', newVal, 'value');
				this.leadValues.members = this.leadValues.members.filter(m => {
					return m != newVal;
				});
			}
			if (val.id == 'editLead-organizationId') {
				this.loadOrganizationContacts(val.value);
			}
			if (val.id == 'lead-productType') {
				this.loadProducts(val.value);
			}
		},
		convertToSelectList(array) {
			return array.map(e => {
				const newObj = {
					label: e.name,
					value: e.id
				}
				return newObj;
			});
		},
		convertArrayToSelectList(array, textProp, valueProp, translatePrefix = null) {
			return array.map(e => {
				const text = e[textProp];
				const newObj = {
					label: translatePrefix ? window.translate(translatePrefix + text.toLowerCase()) : text,
					disabled: false,
					value: e[valueProp]
				}
				return newObj;
			});
		},
		convertContactsToSelectList(array) {
			return array.map(e => {
				const newObj = {
					label: `${e.firstName} ${e.lastName}`,
					value: e.id
				}
				return newObj;
			});
		},
		changeObjPropInArrayById: (array, value, prop, id, idProp) => {
			for (var i in array) {
				if (array[i][idProp] == id) {
					array[i][prop] = value;
					break;
				}
			}
		},
		convertCurrenciesToSelectList(array) {
			return array.map(e => {
				const newObj = {
					label: `${e.name}(${e.symbol})`,
					value: e.code
				}
				return newObj;
			});
		},
		convertUsersToSelectList(array) {
			return array.map(e => {
				const newObj = {
					label: e.userFirstName && e.userFirstName.trim() ? `${e.userFirstName} ${e.userLastName}` : e.userName,
					value: e.id
				}
				return newObj;
			});
		},
		validateValue(value) {
			var regex = /(?=.*?\d)^\$?(([1-9]\d{0,2}(,\d{3})*)|\d+)?(\.\d{1,2})?$/;
			return String(value).match(regex) ? true : false;
		},
	}
});