Vue.component('OrganizationModalForm', {
	template: `<div>
			<Modal :modalProps="organizationModalProps" @newValue="emitValueOrganization" @resetValue="resetValue" :refreshInputs="refreshInputs" :inputsKey="modalKey"/>
			<Modal :modalProps="modalOrgContactProps" @newValue="emitValueContact" @newPhone="emitPhoneValue" :refreshInputs="refreshInputs" :inputsKey="modalOrgContactKey"/>
			<Modal :modalProps="modalAddIndustryProps" @newValue="emitValueIndustry" :refreshInputs="refreshInputs" :inputsKey="modalIndustryKey"/>
			<Modal :modalProps="orgLeadModalProps" @newValue="emitValueLead"  :refreshInputs="refreshInputs" :inputsKey="modalLeadKey"/>
	</div>`,
	props: {
		editable: false
	},
	data() {
		return {
			modalOrgContactKey: 0,
			modalIndustryKey: 0,
			modalLeadKey: 0,
			listIndustrySelect: [],
			regionSelectList: [],
			countries: [],
			cities: [],
			orgSelect: [],
			employeesSelect: [],
			jobPositionsListSelect: [],
			orgValues: {},
			orgAddressValues: {},
			editOrgId: null,
			newOrgId: null,
			pageRequestFilters: [],
			modalKey: 0,
			refreshInputs: 0,
			waitAddressButton: false,
			contactIsAdded: false,
			newContactValues: {},
			industryModalValues: {},
			pipelineStagesListSelect: [],
			organisationContactsSelect: [],
			newLeadValues: {},
			newContactValues: {
				organizationId: '',
				email: '',
				phone: '',
				requiredPhone: true,
				phoneList: [],
				firstName: '',
				lastName: '',
				description: '',
				jobPositionId: '',
			},
		};
	},
	computed: {
		selectedRegion() {
			return this.regionSelectList[0] ? this.regionSelectList[0].value : '';
		},
		modalOrgContactProps() {
			return {
				id: 'addContactsOrgModal',
				label: 'Add contact',
				formInputs: [
					{
						name: 'Input',
						props: {
							id: 'contactOrg-firstName',
							type: 'text',
							label: 'First name',
							required: true,
							value: this.newContactValues.firstName,
							className: 'col-12 col-md-6',
							validator: value => fieldValidationFunc(value, 'nameLetters'),
							validatorInput: value => fieldValidationInputFunc(value, 'nameLetters')
						}
					},
					{
						name: 'Input',
						props: {
							id: 'contactOrg-lastName',
							type: 'text',
							label: 'Last name',
							required: true,
							value: this.newContactValues.lastName,
							className: 'col-12 col-md-6',
							validator: value => fieldValidationFunc(value, 'nameLetters'),
							validatorInput: value => fieldValidationInputFunc(value, 'nameLetters')
						}
					},
					{
						name: 'Select',
						props: {
							id: 'contactOrg-organizationId',
							label: 'Organization',
							required: true,
							disabled: true,
							options: this.orgSelect,
							value: this.newContactValues.organizationId,
							className: 'col-12 col-md-6'
						}
					},
					{
						name: 'Input',
						props: {
							id: 'contactOrg-email',
							type: 'email',
							label: 'Email',
							required: true,
							className: 'col-12 col-md-6',
							value: this.newContactValues.email,
							validator: value => fieldValidationFunc(value, 'email'),
							validatorInput: value => fieldValidationInputFunc(value, 'email')
						}
					},
					{
						name: 'Input',
						props: {
							id: 'contactOrg-phone',
							type: 'tel',
							label: 'Phone',
							required: this.newContactValues.requiredPhone,
							inputPrefix: '(+373)',
							inputSuffix: '<span>&#43</span>',
							suffixClass: 'inputSuffixButton',
							className: 'col-12 col-md-6',
							value: this.newContactValues.phone,
							validator: value => fieldValidationFunc(value, 'phone'),
							validatorInput: value => fieldValidationInputFunc(value, 'phone')
						}
					},
					{
						name: 'Select',
						props: {
							id: 'contactOrg-phoneListIds',
							label: 'phoneList',
							options: this.convertPhoneToSelectList(this.newContactValues.phoneList),
							value: this.newContactValues.phoneList,
							className: 'col-12 col-md-6'
						}
					},
					{
						name: 'Select',
						props: {
							id: 'contactOrg-jobPositionId',
							label: 'Job position',
							searchable: true,
							options: this.jobPositionsListSelect,
							value: this.newContactValues.jobPositionId,
							required: true,
							className: 'col-12'
						}
					},
					{
						name: 'Textarea',
						props: {
							id: 'contactOrg-description',
							label: 'Description',
							value: this.newContactValues.description,
							className: 'col-12'
						}
					}
				],
				formSubmits: [
					{
						name: 'Button',
						props: {
							label: 'Cancel',
							btnType: 'outline-secondary',
							onClick: () => {
								this.openWarningWindow();
							}
						}
					},
					{
						name: 'Button',
						props: {
							label: 'Add',
							btnType: 'success',
							btnDOMType: 'submit',
							onMouseDown: () => {
								this.addLead = false;
								this.contactIsAdded = true;
							}
						}
					},
					{
						name: 'Button',
						props: {
							label: 'Save and add lead',
							btnType: 'success',
							btnDOMType: 'submit',
							onClick: () => {
								this.addLead = true;
								this.contactIsAdded = true;
							}
						}
					}
				],
				onSubmit: () => {
					if (this.newContactValues.phoneList)
						this.newContactValues.phoneList = this.newContactValues.phoneList.map(e => e.value);
					if (this.newContactValues.phone)
						this.newContactValues.phoneList.push(this.newContactValues.phone);
					this.addNewContact().then(() => {
						$(`#${this.modalOrgContactProps.id}`).modal("hide");
						if (this.addLead) {
							this.resetLeadModalValues();
							this.newLeadValues.organizationId = this.newOrgId;
							this.loadOrganizationContacts(this.newLeadValues.organizationId);
							this.modalLeadKey++;
							$(`#${this.orgLeadModalProps.id}`).modal("show");
						}

					});

				},
				onHide: e => {
					if (!this.contactIsAdded) {
						this.openWarningWindow();
						e.preventDefault();
						e.stopImmediatePropagation();
						return false;
					} else {
						setTimeout(function () { $('.modal-backdrop').remove(); }, 1000);
					}
				}
			}
		},
		orgFormSubmits() {
			const resultArray = [
				{
					name: 'Button',
					props: {
						label: 'Cancel',
						btnType: 'outline-secondary',
						onClick: () => {
							this.resetOrgModalValues;
							$(`#${this.organizationModalProps.id}`).modal("hide");
						}
					}
				}];
			resultArray.push({
				name: 'Button',
				props: {
					label: 'Save and add contacts',
					btnType: 'success',
					btnDOMType: 'submit',
					onMouseDown: () => {
						this.addContacts = true;
					}
				}
			});
			return resultArray;
		},
		modalOrglabel() {
			return t('org_add_organization');
		},
		organizationModalProps() {
			return {
				id: 'addOrgModal',
				modalSize: 'lg',
				label: this.modalOrglabel,
				formInputs: [
					{
						name: 'Input',
						props: {
							id: 'org-name',
							type: 'text',
							label: 'Organization Name',
							required: true,
							value: this.orgValues.name,
							className: 'col-12 col-md-6',
							validator: value => fieldValidationFunc(value, 'varChar128'),
							validatorInput: value => fieldValidationInputFunc(value, 'varChar128')
						}
					},
					{
						name: 'Input',
						props: {
							id: 'org-fiscalCode',
							label: 'Fiscal code',
							type: 'text',
							required: false,
							value: this.orgValues.fiscalCode,
							className: 'col-12 col-md-6',
							validator: value => fieldValidationFunc(value, 'fiscalCodeMd'),
							validatorInput: value => fieldValidationInputFunc(value, 'fiscalCodeMd')
						}
					},
					{
						name: 'Select',
						props: {
							id: 'org-industryId',
							label: 'Industry',
							options: this.listIndustrySelect,
							size: 10,
							searchable: true,
							value: this.orgValues.industryId,
							className: 'col-12 col-md-5 col-lg-3'
						}
					},
					{
						name: 'Button',
						props: {
							label: '<span>&#43;<span/>',
							className: "wrapper",
							wrapper: "div",
							wrapperClass: "col-1",
							btnType: 'outline-secondary',
							onClick: () => {
								$(`#${this.organizationModalProps.id}`).modal("hide");
								this.resetIndustryModalValues();
								this.modalIndustryKey++;
								$(`#${this.modalAddIndustryProps.id}`).modal("show");
							}
						}
					},
					{
						name: 'Select',
						props: {
							id: 'org-employeeId',
							label: 'Nr. of Employees',
							value: this.orgValues.employeeId,
							options: this.employeesSelect,
							className: 'col-12 col-md-6 col-lg-4'
						}
					},
					{
						name: 'Input',
						props: {
							id: 'org-vitCode',
							type: 'text',
							label: 'VAT code',
							value: this.orgValues.vitCode,
							className: 'col-12 col-md-6 col-lg-4',
							validator: value => fieldValidationFunc(value, 'varChar50'),
							validatorInput: value => fieldValidationInputFunc(value, 'varChar50')
						}
					},
					{
						name: 'Input',
						props: {
							id: 'org-email',
							type: 'text',
							label: 'Email',
							required: true,
							value: this.orgValues.email,
							className: 'col-12 col-md-6 col-lg-4',
							validator: value => fieldValidationFunc(value, 'email')
						}
					},

					{
						name: 'Input',
						props: {
							id: 'org-phone',
							label: 'Phone',
							type: 'tel',
							required: true,
							inputPrefix: '(+373)',
							value: this.orgValues.phone,
							className: 'col-12 col-md-6 col-lg-4',
							validator: value => fieldValidationFunc(value, 'phone'),
							validatorInput: value => fieldValidationInputFunc(value, 'phone')
						}
					},
					{
						name: 'Input',
						props: {
							id: 'org-responsibleForPhoneNumber',
							type: 'text',
							label: 'Responsible',
							required: false,
							value: this.orgValues.responsibleForPhoneNumber,
							className: 'col-12 col-md-6 col-lg-4',
							validator: value => fieldValidationFunc(value, 'varChar128'),
							validatorInput: value => fieldValidationInputFunc(value, 'varChar128')
						}
					},
					{
						name: 'Input',
						props: {
							id: 'org-webSite',
							label: 'Web site',
							type: 'text',
							value: this.orgValues.webSite,
							className: 'col-12 col-md-6 col-lg-4',
							validator: value => fieldValidationFunc(value, 'varChar50'),
							validatorInput: value => fieldValidationInputFunc(value, 'varChar50')
						}
					},
					{
						name: 'Yearpicker',
						props: {
							id: 'org-dateOfFounding',
							label: 'Year of Founding',
							required: false,
							className: 'col-12 col-md-6 col-lg-4',
							value: this.orgValues.dateOfFounding
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
							id: 'org-bank',
							label: 'Bank',
							type: 'text',
							value: this.orgValues.bank,
							className: 'col-12 col-md-6 col-lg-4',
							validator: value => fieldValidationFunc(value, 'varChar500'),
							validatorInput: value => fieldValidationInputFunc(value, 'varChar500')
						}
					},
					{
						name: 'Input',
						props: {
							id: 'org-ibanCode',
							label: 'Code IBAN',
							type: 'text',
							value: this.orgValues.ibanCode,
							className: 'col-12 col-md-6 col-lg-4',
							validator: value => fieldValidationFunc(value, 'varChar128'),
							validatorInput: value => fieldValidationInputFunc(value, 'varChar128')
						}
					},

					{
						name: 'Input',
						props: {
							id: 'org-codSwift',
							type: 'text',
							label: 'Swift code',
							value: this.orgValues.codSwift,
							className: 'col-12 col-md-6 col-lg-4',
							validator: value => fieldValidationFunc(value, 'varChar50'),
							validatorInput: value => fieldValidationInputFunc(value, 'varChar50')
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
							id: 'org-clientType',
							label: 'Type of Organization',
							value: this.orgValues.clientType,
							required: true,
							options: [
								{
									label: 'Prospect',
									value: 0,
								},
								{
									label: 'Client',
									value: 1
								},
								{
									label: 'Lead',
									value: 2
								},
							],
							className: 'col-12 col-md-6'
						}
					},
					{
						name: 'Select',
						props: {
							id: 'org-isDeleted',
							label: 'Status of Organization',
							required: true,
							disabled: true,
							value: this.orgValues.isDeleted,
							options: [
								{
									label: 'Active',
									value: 'false',
								},
								{
									label: 'Inactive',
									value: 'true'
								}
							],
							className: 'col-12 col-md-6'
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
							id: 'addressOrg-countryId',
							label: 'Country',
							size: 10,
							searchable: true,
							options: this.countries,
							value: this.orgAddressValues.countryId,
							className: 'col-12 col-md-6 col-lg-3'
						}
					},
					{
						name: 'Select',
						props: {
							id: 'addressOrg-regionId',
							label: 'Region',
							noneSelectedText: 'Select country first',
							size: 10,
							required: true,
							searchable: true,
							options: this.regionSelectList,
							value: this.orgAddressValues.regionId,
							className: 'col-12 col-md-6 col-lg-3'
						}
					},
					{
						name: 'Select',
						props: {
							id: 'addressOrg-cityId',
							label: 'City',
							noneSelectedText: 'Select region first',
							options: this.cities,
							required: true,
							size: 10,
							searchable: true,
							value: this.orgAddressValues.cityId,
							className: 'col-12 col-md-6 col-lg-3'
						}
					},
					{
						name: 'Input',
						props: {
							id: 'addressOrg-street',
							label: 'Street',
							type: 'text',
							value: this.orgAddressValues.street,
							className: 'col-12 col-md-6 col-lg-3',
							validator: value => fieldValidationFunc(value, 'varChar128'),
							validatorInput: value => fieldValidationInputFunc(value, 'varChar128')
						}
					},
					{
						name: 'Input',
						props: {
							id: 'addressOrg-zip',
							label: 'Zip Code',
							type: 'text',
							value: this.orgAddressValues.zip,
							className: 'col-12 col-md-6 col-lg-3',
							validator: value => fieldValidationFunc(value, 'zip'),
							validatorInput: value => fieldValidationInputFunc(value, 'zip')
						}
					},
					{
						name: 'hr',
						props: {
							class: 'line-between-inputs'
						}
					},
					{
						name: 'Textarea',
						props: {
							id: 'org-description',
							label: 'Description',
							value: this.orgValues.description,
							className: 'col-12'
						}
					}
				],
				formSubmits: this.orgFormSubmits,
				onSubmit: () => {
					if (this.orgValues.dateOfFounding) {
						this.orgValues.dateOfFounding = moment(this.orgValues.dateOfFounding).format("YYYY/MM/DD");
					}
					this.addNewOrganization().then(() => {;
						this.addOrganizationAddress(this.newOrgId).then(() => {
							this.resetContactValues();
							this.newContactValues.organizationId = this.newOrgId;
							this.modalOrgContactKey++;
							if (this.addContacts) {
								$(`#${this.organizationModalProps.id}`).modal("hide");
								$(`#${this.modalOrgContactProps.id}`).modal("show");
							} else {
								$(`#${this.organizationModalProps.id}`).modal("hide");
							}
						});
					});
				}
			}
		},
		modalAddIndustryProps() {
			return {
				id: 'addIndustryModal',
				label: 'Add industry',
				formInputs: [
					{
						name: 'Input',
						props: {
							id: 'orgIndustry-name',
							type: 'text',
							label: 'Name',
							required: true,
							className: 'col-12',
							value: this.industryModalValues.name,
							validator: value => fieldValidationFunc(value, 'name'),
							validatorInput: value => fieldValidationInputFunc(value, 'name')
						}
					},
				],
				formSubmits: [
					{
						name: 'Button',
						props: {
							label: 'Cancel',
							btnType: 'outline-secondary',
							onClick: () => {
								$(`#${this.modalAddIndustryProps.id}`).modal("hide");
								$(`#${this.organizationModalProps.id}`).modal("show");
							}
						}
					},
					{
						name: 'Button',
						props: {
							label: 'Save',
							btnType: 'success',
							waiting: this.waitAddButton,
							btnDOMType: 'submit'
						}
					}
				],
				onSubmit: () => {
					this.addNewEntity().then(() => {
						this.refreshIndustries().then(() => {
							$(`#${this.modalAddIndustryProps.id}`).modal("hide");
							$(`#${this.organizationModalProps.id}`).modal("show");
						});
					});
				}
			}
		},
		orgLeadModalProps() {
			return {
				id: 'addLeadModalFromOrg',
				modalSize: 'lg',
				label: 'Add lead',
				formInputs: [
					{
						name: 'Select',
						props: {
							id: 'leadOrg-pipeLineId',
							label: 'PipeLine',
							required: true,
							options: this.$store.state.allPipelines || [],
							className: 'col-12',
							value: this.newLeadValues.pipeLineId
						}
					},
					{
						name: 'Input',
						props: {
							id: 'leadOrg-name',
							type: 'text',
							label: 'Name',
							required: true,
							className: 'col-12',
							value: this.newLeadValues.name,
							validator: value => fieldValidationFunc(value, 'name'),
							validatorInput: value => fieldValidationInputFunc(value, 'name')
						}
					},
					{
						name: 'Datepicker',
						props: {
							id: 'leadOrg-created',
							label: 'Start date',
							disabled: true,
							className: 'col-12 col-md-6',
							format: datePickerFormatTask,
							value: this.newLeadValues.created
						}
					},
					{
						name: 'Datepicker',
						props: {
							id: 'leadOrg-deadLine',
							label: 'End date',
							required: true,
							className: 'col-12 col-md-6',
							format: datePickerFormatTask,
							value: this.newLeadValues.deadLine
						}
					},
					{
						name: 'Datepicker',
						props: {
							id: 'leadOrg-clarificationDeadline',
							label: 'Clarification deadline',
							required: true,
							className: 'col-12',
							format: datePickerFormatTask,
							value: this.newLeadValues.clarificationDeadline
						}
					},
					{
						name: 'Select',
						props: {
							id: 'leadOrg-organizationId',
							label: 'Organization',
							required: true,
							disabled: true,
							options: this.orgSelect,
							value: this.newLeadValues.organizationId,
							size: 10,
							searchable: true,
							className: 'col-12 col-md-6'
						}
					},
					{
						name: 'Select',
						props: {
							id: 'leadOrg-contactId',
							label: 'Contact',
							options: this.organisationContactsSelect,
							className: 'col-12 col-md-6',
							size: 10,
							searchable: true,
							value: this.newLeadValues.contactId
						}
					},
					{
						name: 'Select',
						props: {
							id: 'leadOrg-stageId',
							label: 'Stage',
							required: true,
							disabled: this.isAddByStage,
							options: this.pipelineStagesListSelect,
							className: 'col-12 col-md-6',
							value: this.newLeadValues.stageId
						}
					},
					{
						name: 'Select',
						props: {
							id: 'leadOrg-leadStateId',
							label: 'State',
							required: true,
							options: this.leadStatesSelect,
							className: 'col-12 col-md-6',
							value: this.newLeadValues.leadStateId
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
							id: 'leadOrg-productTypeId',
							label: 'Product type',
							options: this.productTypesListSelect,
							className: 'col-12 col-md-6',
							value: this.newLeadValues.productTypeId
						}
					},
					{
						name: 'Select',
						props: {
							id: 'leadOrg-serviceTypeId',
							label: 'Service type',
							options: this.serviceTypesListSelect,
							className: 'col-12 col-md-6',
							value: this.newLeadValues.serviceTypeId
						}
					},
					{
						name: 'Select',
						props: {
							id: 'leadOrg-solutionTypeId',
							label: 'Solution type',
							options: this.solutionTypesListSelect,
							className: 'col-12 col-md-6',
							value: this.newLeadValues.solutionTypeId
						}
					},
					{
						name: 'Select',
						props: {
							id: 'leadOrg-sourceId',
							label: 'Source type',
							options: this.sourceTypesListSelect,
							className: 'col-12 col-md-6',
							value: this.newLeadValues.sourceId
						}
					},
					{
						name: 'Select',
						props: {
							id: 'leadOrg-technologyTypeId',
							label: 'Technology type',
							options: this.technologyTypesListSelect,
							className: 'col-12',
							value: this.newLeadValues.technologyTypeId
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
							id: 'leadOrg-value',
							label: 'Value',
							type: 'text',
							className: 'col-12 col-md-6',
							value: this.newLeadValues.value,
							validator: value => fieldValidationFunc(value, 'naturalNum'),
							validatorInput: value => fieldValidationInputFunc(value, 'naturalNum')
						}
					},
					{
						name: 'Select',
						props: {
							id: 'leadOrg-currencyCode',
							label: 'Currency',
							options: this.currenciesListSelect,
							size: 10,
							searchable: true,
							className: 'col-12 col-md-6',
							value: this.newLeadValues.currencyCode
						}
					},
					{
						name: 'Select',
						props: {
							id: 'leadOrg-owner',
							label: 'Owner',
							required: true,
							options: this.usersListSelect,
							value: this.newLeadValues.owner,
							className: 'col-12'
						}
					},
					{
						name: 'Select',
						props: {
							id: 'leadOrg-members',
							label: 'Members',
							multiple: true,
							searchable: true,
							options: this.membersUsers,
							value: this.newLeadValues.members,
							className: 'col-12'
						}
					},
				],
				formSubmits: [
					{
						name: 'Button',
						props: {
							label: 'Cancel',
							btnType: 'outline-secondary',
							onClick: () => {
								$(`#${this.orgLeadModalProps.id}`).modal("hide");
							}
						}
					},
					{
						name: 'Button',
						props: {
							label: 'Add',
							btnType: 'success',
							btnDOMType: 'submit',
							waiting: this.waitAddButton,
							onMouseDown: () => {

							}
						}
					},
				],
				onSubmit: () => {
					this.addNewLead().then(() => {
						$(`#${this.orgLeadModalProps.id}`).modal("hide");
						this.resetLeadModalValues();
						this.refreshInputs++;
					});

				}
			}
		}
	},
	beforeCreate() {
		this.$store.dispatch('getAllPipelines');
	},
	created: async function () {
		this.resetOrgModalValues();
		const promises = [
			// customAjaxRequest(apiEndpoints.PipeLines.GetAll),
			customAjaxRequest(apiEndpoints.Leads.GetAllLeadStates),
			customAjaxRequest(apiEndpoints.CrmCommon.GetAllCurrencies),
			customAjaxRequest(apiEndpoints.Users.GetUsers),
			customAjaxRequest(apiEndpoints.CrmVocabularies.GetAllProductType),
			customAjaxRequest(apiEndpoints.CrmVocabularies.GetAllServiceType),
			customAjaxRequest(apiEndpoints.CrmVocabularies.GetAllSolutionType),
			customAjaxRequest(apiEndpoints.CrmVocabularies.GetAllSourcesAsync),
			customAjaxRequest(apiEndpoints.CrmVocabularies.GetAllTechnologyType),
			customAjaxRequest(apiEndpoints.OrganizationHelper.GetSelectorsForOrganization),
			customAjaxRequest(apiEndpoints.JobPosition.GetAllJobPositions),
			customAjaxRequest(apiEndpoints.OrganizationAddress.GetAllCountries),
			customAjaxRequest(apiEndpoints.Organization.GetAllOrganization),
			customAjaxRequest(apiEndpoints.Employee.GetAllEmployees)

		];
		Promise.all(promises).then(result => {
			// this.pipelinesListSelect = this.convertToSelectList(result[0]);
			this.leadStatesSelect = this.convertToSelectList(result[0]);
			this.currenciesListSelect = this.convertCurrenciesToSelectList(result[1]);
			this.usersListSelect = this.convertUsersToSelectList(result[2]);
			this.membersUsers = this.convertUsersToSelectList(result[2]);
			this.productTypesListSelect = this.convertArrayToSelectList(result[3], 'name', 'id');
			this.serviceTypesListSelect = this.convertArrayToSelectList(result[4], 'name', 'id');
			this.solutionTypesListSelect = this.convertArrayToSelectList(result[5], 'name', 'id');
			this.sourceTypesListSelect = this.convertArrayToSelectList(result[6], 'name', 'id');
			this.technologyTypesListSelect = this.convertArrayToSelectList(result[7], 'name', 'id');
			this.listIndustrySelect = this.convertHelperToSelectList(result[8].listIndustry);
			this.jobPositionsListSelect = this.convertArrayToSelectList(result[9], 'name', 'id');
			this.countries = this.convertToSelectList(result[10]);
			this.orgSelect = this.convertToSelectList(result[11]);
			this.employeesSelect = this.convertEmployeeToSelectList(result[12]);
		});
	},
	methods: {
		async addNewOrganization() {
			return new Promise((resolve, reject) => {
				customAjaxRequest(apiEndpoints.Organization.AddNewOrganization, 'PUT', this.orgValues).then(result => {
					this.loadOrganizationsforSelect();
					this.newOrgId = result;
					this.newContactValues.organizationId = result;
					this.tableKey++;
					resolve(true);
				}).catch(e => {
					toast.notifyErrorList(e);
					reject(false);
				});
			});
		},
		async addOrganizationAddress(orgId) {
			this.waitAddressButton = true;
			this.orgAddressValues.organizationId = orgId;
			return new Promise((resolve, reject) => {
				customAjaxRequest(apiEndpoints.OrganizationAddress.AddOrganizationAddress, 'PUT', this.orgAddressValues).then(result => {
					this.waitAddressButton = false;
					this.resetOrgAddressValues();
					resolve(true);
				}).catch(e => {
					this.waitAddressButton = false;
					toast.notifyErrorList(e);
					reject(false);
				});
			});
		},
		async addNewEntity() {
			this.waitAddButton = true;
			return new Promise((resolve, reject) => {
				customAjaxRequest(apiEndpoints.Industry.AddNewIndustry, 'PUT', this.industryModalValues).then(() => {
					this.tableKey++;
					resolve(true);
				}).catch(e => {
					toast.notifyErrorList(e);
					reject(false);
				}).finally(() => {
					this.waitAddButton = false;
				});
			});
		},
		async updateOrganization() {
			return new Promise((resolve, reject) => {
				customAjaxRequest(apiEndpoints.Organization.UpdateOrganization, 'POST', this.orgValues).then(() => {
					resolve(true);
					this.updateOrganizationAddress();
				}).catch(e => {
					toast.notifyErrorList(e);
					reject(false);
				});
			});
		},
		async loadOrganization(organizationId) {
			return new Promise((resolve, reject) => {
				customAjaxRequest(apiEndpoints.Organization.GetOrganizationById, 'GET', { organizationId }).then(result => {
					resolve(result);
				}).catch(e => {
					toast.notifyErrorList(e);
				});
			});
		},
		async loadOrganizationsforSelect() {
			customAjaxRequest(apiEndpoints.Organization.GetAllOrganization, 'GET', { includeDeleted: false }).then(result => {
				this.orgSelect = this.convertToSelectList(result);
			});
		},
		async addNewContact() {
			return new Promise((resolve, reject) => {
				customAjaxRequest(apiEndpoints.Contact.AddNewContact, 'PUT', this.newContactValues).then(() => {
					resolve(true);
					this.$emit('action', 'refreshContactsPage');
				}).catch(e => {
					toast.notifyErrorList(e);
					reject(false);
				});
			});
		},
		async addNewLead() {
			this.waitAddressButton = true;
			for (prop in this.newLeadValues) {
				if (this.newLeadValues[prop] === null) {
					delete this.newLeadValues[prop];
				}
			}
			return new Promise((resolve, reject) => {
				customAjaxRequest(apiEndpoints.Leads.AddLead, 'PUT', this.newLeadValues).then(leadId => {
					if (this.newLeadValues.owner) {
						this.setLeadOwner(this.newLeadValues.owner, leadId, this.newLeadValues.members).then(() => {
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
		async refreshIndustries() {
			const promise = [customAjaxRequest(apiEndpoints.OrganizationHelper.GetSelectorsForOrganization, 'GET')];
			Promise.all(promise).then(result => {
				this.listIndustrySelect = this.convertHelperToSelectList(result[0].listIndustry);
			});
		},
		async loadRegionCities(regionId) {
			customAjaxRequest(apiEndpoints.OrganizationAddress.GetAllCitiesByRegionId, 'get', { regionId }).then(result => {
				this.cities = this.convertToSelectList(result);
			});
		},
		async loadCountryRegions(countryId) {
			customAjaxRequest(apiEndpoints.OrganizationAddress.GetAllRegionsByCountryId, 'get', { countryId }).then(result => {
				this.regionSelectList = this.convertToSelectList(result);
			});
		},
		async loadPipelineStages(pipeLineId) {
			customAjaxRequest(apiEndpoints.PipeLines.GetPipeLineStages, 'GET', { pipeLineId }).then(result => {
				this.pipelineStagesListSelect = this.convertToSelectList(result);
				this.modalKey++;
			});
		},
		loadOrganizationContacts(organizationId) {
			customAjaxRequest(apiEndpoints.Contact.GetContactByOrganizationId, 'GET', { organizationId }).then(result => {
				this.organisationContactsSelect = this.convertContactsToSelectList(result);
			});
		},
		updateOrganizationAddress() {
			customAjaxRequest(apiEndpoints.OrganizationAddress.UpdateOrganizationAddress, 'POST', this.orgAddressValues).then(() => {
				this.$emit('action', 'reload');
			});
		},
		resetOrgModalValues() {
			this.orgValues = {
				name: null,
				ResponsibleForPhoneNumber: null,
				clientType: 0,
				bank: null,
				email: null,
				phone: null,
				webSite: null,
				fiscalCode: null,
				ibanCode: null,
				industryId: null,
				employeeId: null,
				description: null,
				vitCode: null,
				codSwift: null,
				dateOfFounding: null,
				isDeleted: false
			}
			this.editOrgId = '';
			this.refreshInputs++;
		},
		resetIndustryModalValues() {
			this.industryModalValues = {
				name: null
			}
		},
		emitPhoneValue(val) {
			this.newContactValues.phone = '';
			this.newContactValues.requiredPhone = false;
			this.newContactValues.phoneList.push(val);
		},
		resetContactValues() {
			this.newContactValues = {
				organizationId: '',
				email: '',
				phone: '',
				requiredPhone: true,
				phoneList: [],
				firstName: '',
				lastName: '',
				description: '',
				jobPositionId: ''
			}
		},
		resetLeadModalValues() {
			this.newLeadValues = {
				name: null,
				organizationId: null,
				pipeLineId: null,
				stageId: null,
				leadStateId: null,
				value: null,
				currencyCode: null,
				created: moment().format(dateFormatTask).toString(),
				deadLine: moment().add(5, 'd').format(dateFormatTask).toString(),
				clarificationDeadline: moment().add(5, 'd').format(dateFormatTask).toString(),
				contactId: null,
				productTypeId: null,
				serviceTypeId: null,
				solutionTypeId: null,
				sourceId: null,
				technologyTypeId: null,
				members: [],
				owner: null,
			}
			this.modalLeadKey++;
			this.refreshInputs++;
		},
		resetOrgAddressValues() {
			this.orgAddressValues = {
				organizationId: null,
				countryId: null,
				regionId: null,
				cityId: null,
				street: null,
				zipCode: null
			}
			this.editOrgId = '';
			this.refreshInputs++;
		},
		addOrganization() {
			this.resetOrgModalValues();
			this.refreshInputs++;
			this.modalKey++;
			$(`#${this.organizationModalProps.id}`).modal("show");
		},
		emitValueOrganization(val) {
			const newVal = val.value === 'true' ? true : val.value === 'false' ? false : val.value;
			if (val.id.includes('addressOrg')) {
				this.orgAddressValues[val.id.replace('addressOrg-', '')] = newVal;
				if (val.id === 'addressOrg-countryId') {
					this.loadCountryRegions(val.value);
					this.cities = [];
				}
				if (val.id === 'addressOrg-regionId') {
					this.loadRegionCities(val.value);
				}
			} else {
				this.orgValues[val.id.replace('org-', '')] = newVal;
			}
		},
		emitValueIndustry(val) {
			const newVal = val.value === 'true' ? true : val.value === 'false' ? false : val.value;
			this.industryModalValues[val.id.replace('orgIndustry-', '')] = newVal;
		},
		emitValueLead(val) {
			const newVal = val.value === 'true' ? true : val.value === 'false' ? false : val.value;
			this.newLeadValues[val.id.replace('leadOrg-', '')] = newVal;

			if (val.id == 'leadOrg-pipeLineId') {
				this.loadPipelineStages(val.value);
			}
			if (val.id == 'leadOrg-owner') {
				this.membersUsers = this.membersUsers.map(m => {
					m.disabled = false;
					return m;
				});
				this.changeObjPropInArrayById(this.membersUsers, true, 'disabled', newVal, 'value');
				this.newLeadValues.members = this.newLeadValues.members.filter(m => {
					return m != newVal;
				});
			}
		},
		resetValue(val) {
			if (val === 'org-adresses') {
				resetOrgAddressValues();
			}
		},
		emitValueContact(val) {
			const newVal = val.value === 'true' ? true : val.value === 'false' ? false : val.value;
			this.newContactValues[val.id.replace('contactOrg-', '')] = newVal;
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
		convertToSelectList(array) {
			return array.map(e => {
				const newObj = {
					label: e.name,
					value: e.id
				}
				return newObj;
			});
		},
		convertEmployeeToSelectList(array) {
			return array.map(e => {
				const newObj = {
					label: e.interval,
					value: e.id
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
		openContactModal(orgId) {
			this.resetContactValues();
			this.newContactValues.organizationId = this.newOrgId ? this.newOrgId : orgId;
			this.modalOrgContactKey++;
			$(`#${this.modalOrgContactProps.id}`).modal("show");
		},
		convertHelperToSelectList(array) {
			return array.map(e => {
				const newObj = {
					label: e.text,
					value: e.value
				}
				return newObj;
			});
		},
		convertArrayToSelectList(array, textProp, valueProp, translatePrefix = null) {
			return array.map(e => {
				const text = e[textProp];
				const newObj = {
					label: translatePrefix ? window.translate(translatePrefix + text.toLowerCase()) : text,
					value: e[valueProp]
				}
				return newObj;
			});
		},
		convertPhoneToSelectList(array) {
			if (array) {
				return array.map(e => {
					const newObj = {
						label: e.value,
						value: e.value,
						disabled: true,
					}
					return newObj;
				});
			} else return [];
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
		changeObjPropInArrayById: (array, value, prop, id, idProp) => {
			for (var i in array) {
				if (array[i][idProp] == id) {
					array[i][prop] = value;
					break;
				}
			}
		},
		findObjectByPropValue(array, value, prop) {
			return array.find(x => x[prop] === value);
		},
		getRegionName(regionId) {
			return this.findObjectByPropValue(this.regionSelectList, regionId, 'value').label;
		},
		openWarningWindow() {
			alert('Organization should have at least one active contact');
		},
	}
});