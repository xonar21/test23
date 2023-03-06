Vue.component('EditOrganizationModalForm', {
	template: `<div>
			<Modal :modalProps="modalOrgProps" @newValue="emitValueOrg" @resetValue="resetValue" :refreshInputs="refreshInputs" :inputsKey="modalOrgKey"/>
			<Modal :modalProps="addIndustryModal" @newValue="emitValueIndustry" :refreshInputs="refreshInputs" :inputsKey="modalIndustryKey"/>
	</div>`,
	props: {
		editable: true
	},
	data() {
		return {
			modalOrgKey: 0,
			modalIndustryKey: 0,
			regions: [],
			cities: [],
			states: [],
			organizationsListSelect: [],
			geoPositionsSelect: [],
			addOrgAddr: false,
			orgValues: {},
			orgAddress: {},
			editOrgId: null,
			newOrgId: null,
			pageRequestFilters: [],
			modalKey: 0,
			refreshInputs: 0,
			waitAddressButton: false,
			orgAddresses: [],
			industryModalValues: {},
			jobPositionsListSelect: [],
			employeesListSelect: [],
			hideEmailInputs: ['', 'hide-field', 'hide-field', 'hide-field', 'hide-field'],
			emailLabels: [],
			emailList: [
				{
					email: '',
					label: ''
				},
				{
					email: '',
					label: ''
				},
				{
					email: '',
					label: ''
				},
				{
					email: '',
					label: ''
				},
				{
					email: '',
					label: ''
				},
			],
			phoneList: [
				{
					phone: '',
					countryCode: 'MD',
					dialCode: '373',
					label: ''
				},
				{
					phone: '',
					countryCode: 'MD',
					dialCode: '373',
					label: ''
				},
				{
					phone: '',
					countryCode: 'MD',
					dialCode: '373',
					label: ''
				}],
			labels: [],
			contactEmailList: [
				{
					email: '',
					label: ''
				},
				{
					email: '',
					label: ''
				},
				{
					email: '',
					label: ''
				},
				{
					email: '',
					label: ''
				},
				{
					email: '',
					label: ''
				},
			],
			hidePhoneInputs: ['', 'hide-field', 'hide-field'],
			contactHideEmailInputs: ['', 'hide-field', 'hide-field', 'hide-field', 'hide-field'],
			hideInput: {},
		};
	},
	computed: {
		emailLabelsFromStore() {
			return this.$store.state.allEmailLabels;
		},
		phoneLabels() {
			return this.$store.state.allPhoneLabels;
		},
		currenciesListSelect() {
			return this.$store.state.allCurrencies;
		},
		countries() {
			return this.$store.state.allCountries;
		},
		organizationStages() {
			return this.$store.state.allOrganizationStages;
		},
		employeesSelect() {
			return this.$store.state.allEmployees.map(e => {
				const newObj = {
					label: e.interval,
					value: e.id,
				}
				return newObj;
			});;;
		},
		listIndustry() {
			let listOfIndustries = [];
			listOfIndustries = this.$store.state.selectorsForOrganization.listIndustry?.map(e => {
				const newObj = {
					label: e.text,
					value: e.value,
				}
				return newObj;
			});
			return listOfIndustries ? listOfIndustries : [];
		},
		selectedRegion() {
			return this.regions[0] ? this.regions[0].value : '';
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
							$(`#${this.modalOrgProps.id}`).modal("hide");
						}
					}
				}];
			resultArray.push({
				name: 'Button',
				props: {
					label: 'Save',
					btnType: 'success',
					btnDOMType: 'submit',
					onMouseDown: () => {
					}
				},
			});
			return resultArray;
		},
		modalOrglabel() {
			return t('org_edit_organization');
		},
		modalOrgProps() {
			return {
				id: 'organizationModal',
				modalSize: 'lg',
				label: this.modalOrglabel,
				formInputs: [
					{
						name: 'Input',
						props: {
							id: 'company-name',
							type: 'text',
							label: 'Name Organization',
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
							id: 'company-fiscalCode',
							label: 'Code Fiscal',
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
							id: 'company-industryId',
							label: 'Industry',
							options: this.listIndustry,
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
								$(`#${this.modalOrgProps.id}`).modal("hide");
								this.resetIndustryModalValues();
								this.modalIndustryKey++;
								$(`#${this.addIndustryModal.id}`).modal("show");
							}
						}
					},
					{
						name: 'Select',
						props: {
							id: 'company-employeeId',
							label: 'Nr. of Employees',
							value: this.orgValues.employeeId,
							options: this.employeesListSelect,
							className: 'col-12 col-md-6 col-lg-4'
						}
					},
					{
						name: 'Input',
						props: {
							id: 'company-vitCode',
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
							id: 'org-email1',
							type: 'email',
							label: t('email'),
							className: 'col-6',
							value: this.emailList[0].email,
							validator: value => fieldValidationFunc(value, 'email')
						}
					},
					{
						name: 'Select',
						props: {
							id: 'org_EmailLabel1',
							label: 'Label',
							noneSelectedText: 'Select label',
							options: this.emailLabels,
							value: this.emailList[0].label,
							className: 'col-6'
						}
					},
					{
						name: 'AddHideInput',
						props: {
							id: 'HideEmailInput1',
							className: 'col-12 ' + this.hideEmailInputs[0],
							hideDelete: true,
							hidePlus: this.hideEmailInputs[1] == '' ? true : false,
							hideLabel: false,
						}
					},
					{
						name: 'Input',
						props: {
							id: 'org-email2',
							type: 'email',
							label: t('email'),
							className: 'col-6 ' + this.hideEmailInputs[1],
							value: this.emailList[1].email,
							validator: value => fieldValidationFunc(value, 'email')
						}
					},
					{
						name: 'Select',
						props: {
							id: 'org_EmailLabel2',
							label: 'Label',
							noneSelectedText: 'Select label',
							options: this.emailLabels,
							value: this.emailList[1].label,
							className: 'col-6 ' + this.hideEmailInputs[1]
						}
					},
					{
						name: 'AddHideInput',
						props: {
							id: 'HideEmailInput2',
							className: 'col-12 ' + this.hideEmailInputs[1],
							hidePlus: this.hideEmailInputs[2] == '' ? true : false,
							hideLabel: false,
						}
					},
					{
						name: 'Input',
						props: {
							id: 'org-email3',
							type: 'email',
							label: t('email'),
							className: 'col-6 ' + this.hideEmailInputs[2],
							value: this.emailList[2].email,
							validator: value => fieldValidationFunc(value, 'email')
						}
					},
					{
						name: 'Select',
						props: {
							id: 'org_EmailLabel3',
							label: 'Label',
							noneSelectedText: 'Select label',
							options: this.emailLabels,
							value: this.emailList[2].label,
							className: 'col-6 ' + this.hideEmailInputs[2]
						}
					},
					{
						name: 'AddHideInput',
						props: {
							id: 'HideEmailInput3',
							className: 'col-12 ' + this.hideEmailInputs[2],
							hidePlus: this.hideEmailInputs[3] == '' ? true : false,
							hideLabel: false,
						}
					},
					{
						name: 'Input',
						props: {
							id: 'org-email4',
							type: 'email',
							label: t('email'),
							className: 'col-6 ' + this.hideEmailInputs[3],
							value: this.emailList[3].email,
							validator: value => fieldValidationFunc(value, 'email')
						}
					},
					{
						name: 'Select',
						props: {
							id: 'org_EmailLabel4',
							label: 'Label',
							noneSelectedText: 'Select label',
							options: this.emailLabels,
							value: this.emailList[3].label,
							className: 'col-6 ' + this.hideEmailInputs[3]
						}
					},
					{
						name: 'AddHideInput',
						props: {
							id: 'HideEmailInput4',
							className: 'col-12 ' + this.hideEmailInputs[3],
							hidePlus: this.hideEmailInputs[4] == '' ? true : false,
							hideLabel: false,
						}
					},
					{
						name: 'Input',
						props: {
							id: 'org-email5',
							type: 'email',
							label: t('email'),
							className: 'col-6 ' + this.hideEmailInputs[4],
							value: this.emailList[4].email,
							validator: value => fieldValidationFunc(value, 'email')
						}
					},
					{
						name: 'Select',
						props: {
							id: 'org_EmailLabel5',
							label: 'Label',
							noneSelectedText: 'Select label',
							options: this.emailLabels,
							value: this.emailList[4].label,
							className: 'col-6 ' + this.hideEmailInputs[4]
						}
					},
					{
						name: 'AddHideInput',
						props: {
							id: 'HideEmailInput5',
							className: 'col-12 ' + this.hideEmailInputs[4],
							hidePlus: true,
							hideLabel: false,

						}
					},
					/*{
						name: 'Input',
						props: {
							id: 'company-phone',
							label: 'Phone',
							type: 'tel',
							inputPrefix: '(+373)',
							value: this.orgValues.phone,
							className: 'col-12 col-md-6 col-lg-4',
							validator: value => fieldValidationFunc(value, 'phone'),
							validatorInput: value => fieldValidationInputFunc(value, 'phone')
						}
					},*/
					{
						name: 'PhoneInput',
						props: {
							id: 'company-phone',
							type: 'text',
							label: 'Phone',
							className: 'col-12 col-md-6 col-lg-4',
							dialCode: this.orgValues.dialCode,
							defaultCountry: 'MD',
							value: this.orgValues.phone,
						}
					},
					{
						name: 'Input',
						props: {
							id: 'company-responsibleForPhoneNumber',
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
							id: 'company-webSite',
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
							id: 'company-dateOfFounding',
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
							id: 'company-bank',
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
							id: 'company-ibanCode',
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
							id: 'company-codSwift',
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
							id: 'company-stageId',
							label: 'Stage',
							options: this.organizationStages,
							required: true,
							value: this.orgValues.stageId,
							className: 'col-12 col-md-4'
						}
					},
					{
						name: 'Select',
						props: {
							id: 'company-stateId',
							label: 'State',
							options: this.states,
							size: 10,
							value: this.orgValues.stateId,
							className: 'col-12 col-md-4'
						}
					},
					{
						name: 'Select',
						props: {
							id: 'company-isDeleted',
							label: 'Status of Organization',
							required: true,
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
							className: 'col-12 col-md-4'
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
							id: 'addressC-countryId',
							label: 'Country',
							size: 10,
							required: true,
							searchable: true,
							options: this.countries,
							value: this.orgAddress.countryId,
							className: 'col-12 col-md-6 col-lg-3'
						}
					},
					{
						name: 'Select',
						props: {
							id: 'addressC-regionId',
							label: 'Region',
							noneSelectedText: 'Select country first',
							size: 10,
							searchable: true,
							options: this.regions,
							value: this.orgAddress.regionId,
							className: 'col-12 col-md-6 col-lg-3'
						}
					},
					{
						name: 'Select',
						props: {
							id: 'addressC-cityId',
							label: 'City',
							noneSelectedText: 'Select region first',
							options: this.cities,
							required: true,
							size: 10,
							searchable: true,
							value: this.orgAddress.cityId,
							className: 'col-12 col-md-6 col-lg-3'
						}
					},
					{
						name: 'Input',
						props: {
							id: 'addressC-street',
							label: 'Street',
							type: 'text',
							value: this.orgAddress.street,
							className: 'col-12 col-md-6 col-lg-3',
							validator: value => fieldValidationFunc(value, 'varChar128'),
							validatorInput: value => fieldValidationInputFunc(value, 'varChar128')
						}
					},
					{
						name: 'Input',
						props: {
							id: 'addressC-zip',
							label: 'Zip Code',
							type: 'text',
							value: this.orgAddress.zip,
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
							id: 'company-description',
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
					};
					if (this.orgValues.phone) {
						this.orgValues.dialCode = `+${this.orgValues.dialCode}`;
					}
					this.updateOrganization().then(() => {
						this.$emit('action', 'reload');
						$(`#${this.modalOrgProps.id}`).modal("hide");
					});

				}
			}
		},
		addIndustryModal() {
			return {
				id: 'addIndustry',
				label: 'Add industry',
				formInputs: [
					{
						name: 'Input',
						props: {
							id: 'industry-name',
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
								$(`#${this.addIndustryModal.id}`).modal("hide");
								$(`#${this.modalOrgProps.id}`).modal("show");
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
						$(`#${this.addIndustryModal.id}`).modal("hide");
						$(`#${this.modalOrgProps.id}`).modal("show");
					});
				}
			}
		},
		getEmailList() {
			return this.hideEmailInputs;
		},
		getContactEmailList() {
			return this.contactHideEmailInputs;
		}
	},
	created() {
		this.$store.dispatch('getAllEmailLabels');
		this.$store.dispatch('getAllSelectorsForOrganization');
		this.$store.dispatch('getAllEmployees');
		this.$store.dispatch('getAllCountries');
		this.$store.dispatch('getAllCurrencies');
		this.resetOrgModalValues();
		this.emailLabels = this.emailLabelsFromStore;
	},
	methods: {

		async addOrganizationAddress(orgId) {
			this.waitAddressButton = true;
			this.orgAddress.organizationId = orgId;
			return new Promise((resolve, reject) => {
				customAjaxRequest(apiEndpoints.OrganizationAddress.AddOrganizationAddress, 'PUT', this.orgAddress).then(result => {
					this.appendAddressToListing(result).then(() => {
						this.waitAddressButton = false;
					});
					this.resetOrgAddressValues();
					this.addOrgAddr = false;
					resolve(true);
				}).catch(e => {
					this.waitAddressButton = false;
					toast.notifyErrorList(e);
					reject(false);
				});
			});
		},
		async updateOrganization() {
			return new Promise((resolve, reject) => {
				customAjaxRequest(apiEndpoints.Organization.UpdateOrganization, 'POST', this.orgValues).then(result => {
					resolve(true);
					let EmailListUpdate = [];
					let EmailListAdd = [];
					for (i = 0; i < 5; i++) {
						if (this.emailList[i].email != '') {
							if (this.hideEmailInputs[i] == '') {
								if (this.emailList[i].id) {
									EmailListUpdate.push(this.emailList[i]);
								}
								else {
									EmailListAdd.push({
										email: this.emailList[i].email,
										label: this.emailList[i].label,
										organizationId: result
									});
								}
							}
						}
					}
					if (EmailListUpdate.length > 0)
						this.updateEmailList(EmailListUpdate);
					if (EmailListAdd.length > 0) {
						this.addEmailList(EmailListAdd);
					}
					if (this.addOrgAddr) {
						this.addOrganizationAddress(this.orgValues.id);
						this.addOrgAddr = false;
					}
					else
						this.updateOrganizationAddress();
					resolve(true);
					this.tableKey++;
				}).catch(e => {
					toast.notifyErrorList(e);
					reject(false);
				});
			});
		},
		async addEmailList(ListToAdd) {
			console.log(ListToAdd, 'add');
			return new Promise((resolve, reject) => {
				customAjaxRequest(apiEndpoints.Email.AddEmailRange, 'PUT', { model: ListToAdd }).then(() => {
					this.tableKey++;
					resolve(true);
				}).catch(e => {
					toast.notifyErrorList(e);
					reject(false);
				}).finally(() => {
					this.waitAddButton = false;
				});
			})
		},
		async updateEmailList(ListToUpdate) {
			return new Promise((resolve, reject) => {
				customAjaxRequest(apiEndpoints.Email.UpdateRangeEmail, 'POST', { model: ListToUpdate }).then(() => {
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
		async deleteEmail(emailId) {
			customAjaxRequest(apiEndpoints.Email.DeleteEmailById, 'DELETE', { emailId }).then(() => {
				this.tableKey++;
			}).catch(e => {
				toast.notifyErrorList(e);
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
		async deleteEmail(emailId) {
			customAjaxRequest(apiEndpoints.Email.DeleteEmailById, 'DELETE', { emailId }).then(() => {
				this.tableKey++;
			}).catch(e => {
				toast.notifyErrorList(e);
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
		async loadCountryCities(countryId) {
			customAjaxRequest(apiEndpoints.OrganizationAddress.GetAllCitiesByCountryId, 'get', { countryId }).then(result => {
				this.cities = this.convertToSelectList(result);
			})
		},
		async loadRegionCities(regionId) {
			customAjaxRequest(apiEndpoints.OrganizationAddress.GetAllCitiesByRegionId, 'get', { regionId }).then(result => {
				this.cities = this.convertToSelectList(result);
			});
		},
		async loadCountryRegions(countryId) {
			customAjaxRequest(apiEndpoints.OrganizationAddress.GetAllRegionsByCountryId, 'get', { countryId }).then(result => {
				this.regions = this.convertToSelectList(result);
			});
		},
		updateOrganizationAddress() {
			customAjaxRequest(apiEndpoints.OrganizationAddress.UpdateOrganizationAddress, 'POST', this.orgAddress).then(() => {
			});
		},
		async loadStatesByStage(stageId) {
			return new Promise((resolve, reject) => {
				customAjaxRequest(apiEndpoints.OrganizationHelper.GetOrganizationStatesByStage, 'GET', { stageId }).then(result => {
					resolve(result);
				}).catch(e => {
					toast.notifyErrorList(e);
					reject(false);
				});
			})
		},
		resetEmail(index) {
			this.emailList[index] = {
				email: '',
				label: ''
			};
			this.emailList.push({
				email: '',
				label: ''
			});
			this.emailList.pop();
		},
		resetOrgModalValues() {
			this.orgValues = {
				name: null,
				geoPosition: null,
				clientType: 0,
				bank: null,
				email: null,
				phone: '',
				dialCode: '373',
				webSite: null,
				responsibleForPhoneNumber: null,
				fiscalCode: null,
				ibanCode: null,
				industryId: null,
				employeeId: null,
				description: null,
				codTva: null,
				codSwift: null,
				isDeleted: false,
				dateOfFounding: null,
			}
			this.emailList = [
				{
					email: '',
					label: ''
				},
				{
					email: '',
					label: ''
				},
				{
					email: '',
					label: ''
				},
				{
					email: '',
					label: ''
				},
				{
					email: '',
					label: ''
				},
			];
			this.hideEmailInputs = ['', 'hide-field', 'hide-field', 'hide-field', 'hide-field'];
			this.editOrgId = '';
			this.refreshInputs++;
		},
		editOrganization(orgId) {
			this.loadOrganization(orgId).then(result => {
				this.orgValues = result;
				if (result.addresses[0]) {
					this.orgAddress = {
						id: result.addresses[0].id,
						organizationId: result.id,
						countryId: result.addresses[0].city.crmCountryId,
						regionId: result.addresses[0].regionId,
						cityId: result.addresses[0].cityId,
						street: result.addresses[0].street,
						zip: result.addresses[0].zip
					}
					this.loadCountryRegions(result.addresses[0].city.crmCountryId);
					this.loadCountryCities(result.addresses[0].city.crmCountryId);
					if (result.addresses[0].regionId) this.loadRegionCities(result.addresses[0].regionId);
				}
				else this.addOrgAddr = true;
				this.editOrgId = orgId;
				if (this.orgValues.dateOfFounding) {
					this.orgValues.dateOfFounding = moment(this.orgValues.dateOfFounding).format("YYYY");
				}
				this.hideEmailInputs = [];
				this.emailList = result.emailList.length > 0 ? result.emailList : [];
				for (i = 0; i < this.emailList.length; i++) {
					this.hideEmailInputs.push('');
				}
				for (i = this.emailList.length; i < 5; i++) {
					this.hideEmailInputs.push('hide-field');
					this.emailList.push(
						{
							email: '',
							label: '',
						}
					);
				}
				this.loadStatesByStage(this.orgValues.stageId).then(result => {
					this.states = this.convertToSelectList(result);
				});
				this.hideEmailInputs[0] = '';
				this.refreshInputs++;
				this.modalOrgKey++;
				$(`#${this.modalOrgProps.id}`).modal("show");
			});
		},
		emitValueOrg(val) {
			const newVal = val.value === 'true' ? true : val.value === 'false' ? false : val.value;
			if (val.id.includes('addressC')) {
				this.orgAddress[val.id.replace('addressC-', '')] = newVal;
				if (val.id === 'addressC-countryId') {
					this.loadCountryRegions(val.value);
					this.cities = [];
					this.loadCountryCities(val.value);
				}
				if (val.id === 'addressC-regionId') {
					this.cities = [];
					this.loadRegionCities(val.value);
				}
			} else {
				this.orgValues[val.id.replace('company-', '')] = newVal;
			}
			if (val.id.includes('HideEmailInput')) {
				let index = val.id[14] - '0';
				if (val.value == '') this.hideEmailInputs[index] = val.value;
				else {
					this.hideEmailInputs[index - 1] = val.value;
					if (val.value != '') {
						if (this.emailList[index - 1].id) {
							this.deleteEmail(this.emailList[index - 1].id);
						}
						this.resetEmail(index - 1);
					}
				}
				this.hideEmailInputs.push(val.value);
				this.hideEmailInputs.pop();
			}
			if (val.id.includes('org-email')) {
				let index = val.id[9] - '0';
				this.emailList[index - 1].email = val.value;
			}
			if (val.id.includes('org_EmailLabel')) {
				let index = val.id[14] - '0';
				this.emailList[index - 1].label = val.value;
			};
			if (val.id === 'company-stageId') {
				this.states = [];
				this.loadStatesByStage(val.value).then(result => {
					this.states = this.convertToSelectList(result);
				});
			}
			if (val.id === 'company-phone') {
				this.orgValues.phone = val.value.number;
				this.orgValues.dialCode = val.value.dialCode;
			}
		},
		emitValueIndustry(val) {
			const newVal = val.value === 'true' ? true : val.value === 'false' ? false : val.value;
			this.industryModalValues[val.id.replace('industry-', '')] = newVal;
		},
		resetValue(val) {
			if (val === 'company-adresses') {
				resetOrgAddressValues();
			}
		},
		resetIndustryModalValues() {
			this.industryModalValues = {
				name: null
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
		convertHelperToSelectList(array) {
			return array.map(e => {
				const newObj = {
					label: e.text,
					value: e.value
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
		convertEmployeeToSelectList(array) {
			return array.map(e => {
				const newObj = {
					label: e.interval,
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
					value: e[valueProp]
				}
				return newObj;
			});
		},
		findObjectByPropValue(array, value, prop) {
			return array.find(x => x[prop] === value);
		},
		getRegionName(regionId) {
			return this.findObjectByPropValue(this.regions, regionId, 'value').label;
		},
		resetEmail(index) {
			this.emailList[index] = {
				email: '',
				label: ''
			};
			this.emailList.push({
				email: '',
				label: ''
			});
			this.emailList.pop();
		},
	}
});
