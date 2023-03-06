Vue.component('LeadModalForm', {
	template: `<div>
			<Modal :modalProps="leadModalProps" @newValue="emitValueLead"  :refreshInputs="refreshInputs" :inputsKey="modalKey"/>
	</div>`,
	props: {
		editable: false
	},
	data() {
		return {
			addAndNew: false,
			orgSelect: [],
			leadValues: {},
			editOrgId: null,
			pageRequestFilters: [],
			modalKey: 0,
			refreshInputs: 0,
			waitAddressButton: false,
			isAddByStage: false,
			leadStatesSelect: [],
			currenciesListSelect: [],
			usersListSelect: [],
			organisationContactsSelect: [],
			pipelineStagesListSelect: [],
			productsListSelect: []
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
							$(`#${this.leadModalProps.id}`).modal("hide");
						}
					}
				}];
			if (!this.editableLeadModal) {
				resultArray.push(
					{
						name: 'Button',
						props: {
							label: 'Add',
							btnType: 'success',
							btnDOMType: 'submit',
							waiting: this.waitAddButton,
							onMouseDown: () => {
								this.addAndNew = false;
							}
						}
					},
					{
						name: 'Button',
						props: {
							label: 'Add & new',
							btnType: 'success',
							btnDOMType: 'submit',
							waiting: this.waitAddButton,
							onMouseDown: () => {
								this.addAndNew = true;
							}
						}
					}
				);
			} else {
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
			}
			return resultArray;
		},
		modalLeadLabel() {
			return "Add lead";
		},
		leadModalProps() {
			return {
				id: 'addLeadModal',
				modalSize: 'lg',
				label: this.modalLeadLabel,
				formInputs: [
					{
						name: 'Select',
						props: {
							id: 'leadInput-pipeLineId',
							label: 'PipeLine',
							required: true,
							//disabled: true,
							options: this.$store.state.allPipelines || [],
							className: 'col-12',
							value: this.leadValues.pipeLineId
						}
					},
					{
						name: 'Input',
						props: {
							id: 'leadInput-name',
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
							id: 'leadInput-created',
							label: 'Start date',
							disabled: true,
							className: 'col-12 col-md-6',
							format: datePickerFormatTask,
							value: this.leadValues.created
						}
					},
					{
						name: 'Datepicker',
						props: {
							id: 'leadInput-deadLine',
							label: 'End date',
							required: true,
							className: 'col-12 col-md-6',
							format: datePickerFormatTask,
							value: this.leadValues.deadLine
						}
					},
					{
						name: 'Datepicker',
						props: {
							id: 'leadInput-clarificationDeadline',
							label: 'Clarification deadline',
							required: true,
							className: 'col-12',
							format: datePickerFormatTask,
							value: this.leadValues.clarificationDeadline
						}
					},
					{
						name: 'Select',
						props: {
							id: 'leadInput-organizationId',
							label: 'Organization',
							required: true,
							options: this.orgSelect,
							value: this.leadValues.organizationId,
							size: 10,
							searchable: true,
							className: 'col-12 col-md-6'
						}
					},
					{
						name: 'Select',
						props: {
							id: 'leadInput-contactId',
							label: 'Contact',
							options: this.organisationContactsSelect,
							noneSelectedText: this.organisationContactsSelect.length > 0 ? 'Nothing selected' : 'Plese select organization with contacts',
							className: 'col-12 col-md-6',
							size: 10,
							searchable: true,
							value: this.leadValues.contactId
						}
					},
					{
						name: 'Select',
						props: {
							id: 'leadInput-stageId',
							label: 'Stage',
							required: true,
							disabled: this.isAddByStage,
							options: this.pipelineStagesListSelect,
							className: 'col-12 col-md-6',
							value: this.leadValues.stageId
						}
					},
					{
						name: 'Select',
						props: {
							id: 'leadInput-leadStateId',
							label: 'State',
							required: true,
							options: this.leadStatesSelect,
							className: 'col-12 col-md-6',
							value: this.leadValues.leadStateId
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
							id: 'leadInput-productType',
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
							value: this.leadValues.productType
						}
					},
					{
						name: 'Select',
						props: {
							id: 'leadInput-productId',
							label: 'Product',
							noneSelectedText: this.leadValues.productTypeId ? 'Nothing selected' : 'Plese select a product type first',
							options: this.productsListSelect,
							className: 'col-12 col-md-6',
							value: this.leadValues.productId
						}
					},
					{
						name: 'Select',
						props: {
							id: 'leadInput-serviceTypeId',
							label: 'Service type',
							options: this.serviceTypesListSelect,
							className: 'col-12 col-md-6',
							value: this.leadValues.serviceTypeId
						}
					},
					{
						name: 'Select',
						props: {
							id: 'leadInput-solutionTypeId',
							label: 'Solution type',
							options: this.solutionTypesListSelect,
							className: 'col-12 col-md-6',
							value: this.leadValues.solutionTypeId
						}
					},
					{
						name: 'Select',
						props: {
							id: 'leadInput-sourceId',
							label: 'Source type',
							options: this.sourceTypesListSelect,
							className: 'col-12 col-md-6',
							value: this.leadValues.sourceId
						}
					},
					{
						name: 'Select',
						props: {
							id: 'leadInput-technologyTypeId',
							label: 'Technology type',
							options: this.technologyTypesListSelect,
							className: 'col-12 col-md-6',
							value: this.leadValues.technologyTypeId
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
							id: 'leadInput-value',
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
							id: 'leadInput-currencyCode',
							label: 'Currency',
							options: this.currenciesListSelect,
							size: 10,
							searchable: true,
							className: 'col-12 col-md-6',
							value: this.leadValues.currencyCode
						}
					},
					{
						name: 'Select',
						props: {
							id: 'leadInput-owner',
							label: 'Owner',
							required: true,
							options: this.usersListSelect,
							value: this.leadValues.owner,
							className: 'col-12'
						}
					},
					{
						name: 'Select',
						props: {
							id: 'leadInput-members',
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
							id: 'leadInput-description',
							label: 'Description',
							value: this.leadValues.description,
							className: 'col-12'
						}
					}
				],
				formSubmits: this.modalLeadSubmits,
				onSubmit: () => {
					if (this.validateValue(this.leadValues.value)) {
						if (this.addAndNew) {
							this.addNewLead().then(() => {
								this.resetLeadModalValues();
								this.refreshInputs++;
							});
						} else {
							this.addNewLead().then(() => {
								$(`#${this.leadModalProps.id}`).modal("hide");
								this.resetLeadModalValues();
								this.refreshInputs++;
							});
						}
					}
					else toast.notifyErrorList('Make sure your input lead value is right');
				}
			}
		}
	},
	beforeCreate() {
		this.$store.dispatch('getAllPipelines');
	},
	created: async function () {
		this.resetLeadModalValues();
		const promises = [
			/*customAjaxRequest(apiEndpoints.PipeLines.GetPipeLineStages, 'get', { pipeLineId: this.pipelineId }),*/
			customAjaxRequest(apiEndpoints.Organization.GetAllOrganization),
			// customAjaxRequest(apiEndpoints.PipeLines.GetAll),
			customAjaxRequest(apiEndpoints.Leads.GetAllLeadStates),
			customAjaxRequest(apiEndpoints.CrmCommon.GetAllCurrencies),
			customAjaxRequest(apiEndpoints.Users.GetUsers),
			customAjaxRequest(apiEndpoints.CrmVocabularies.GetAllProductType),
			customAjaxRequest(apiEndpoints.CrmVocabularies.GetAllServiceType),
			customAjaxRequest(apiEndpoints.CrmVocabularies.GetAllSolutionType),
			customAjaxRequest(apiEndpoints.CrmVocabularies.GetAllSourcesAsync),
			customAjaxRequest(apiEndpoints.CrmVocabularies.GetAllTechnologyType)

		];
		Promise.all(promises).then(result => {
			/*this.pipelineStagesListSelect = modalLeadUtils.convertToSelectList(result[0]);*/
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
		async addNewLead() {
			this.waitAddButton = true;
			for (prop in this.leadValues) {
				if (this.leadValues[prop] === null) {
					delete this.leadValues[prop];
				}
			}
			return new Promise((resolve, reject) => {
				customAjaxRequest(apiEndpoints.Leads.AddLead, 'PUT', this.leadValues).then(leadId => {
					if (this.leadValues.owner) {
						this.setLeadOwner(this.leadValues.owner, leadId, this.leadValues.members).then(() => {
							resolve(true);
							this.waitAddButton = false;
						});
					} else {
						this.tableKey++;
						resolve(true);
						this.waitAddButton = false;
					}
				}).catch(e => {
					toast.notifyErrorList(e);
					reject(false);
					this.waitAddButton = false;
				});
			});
		},
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
					this.waitAddButton = false;
				}).catch(e => {
					toast.notifyErrorList(e);
					reject(false);
					this.waitAddButton = false;
				});
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
		loadOrganizationContacts(organizationId) {
			customAjaxRequest(apiEndpoints.Contact.GetContactByOrganizationId, 'GET', { organizationId }).then(result => {
				this.organisationContactsSelect = this.convertContactsToSelectList(result);
				this.modalLeadKey++;
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
		async loadPipelineStages(pipeLineId) {
			customAjaxRequest(apiEndpoints.PipeLines.GetPipeLineStages, 'GET', { pipeLineId }).then(result => {
				this.pipelineStagesListSelect = this.convertToSelectList(result);
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
				created: moment().format(dateFormatTask).toString(),
				deadLine: moment().add(5, 'd').format(dateFormatTask).toString(),
				members: [],
				owner: null,
				clarificationDeadline: moment().add(5, 'd').format(dateFormatTask).toString(),
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
		addLead() {
			this.resetLeadModalValues();
			this.refreshInputs++;
			this.modalKey++;
			$(`#${this.leadModalProps.id}`).modal("show");
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
			this.leadValues[val.id.replace('leadInput-', '')] = newVal;
			if (val.id == 'leadInput-pipeLineId') {
				this.loadPipelineStages(val.value);
			}
			if (val.id == 'leadInput-owner') {
				this.membersUsers = this.membersUsers.map(m => {
					m.disabled = false;
					return m;
				});
				this.changeObjPropInArrayById(this.membersUsers, true, 'disabled', newVal, 'value');
				this.leadValues.members = this.leadValues.members.filter(m => {
					return m != newVal;
				});
			}
			if (val.id == 'leadInput-organizationId') {
				this.loadOrganizationContacts(val.value);
			}
			if (val.id == 'leadInput-productType') {
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