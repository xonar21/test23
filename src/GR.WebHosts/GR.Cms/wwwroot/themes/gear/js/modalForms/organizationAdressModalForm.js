Vue.component('AddressModalForm', {
	template: `<div>
			<Modal :modalProps="addressModalProps" @newValue="emitValueAddress"  :refreshInputs="refreshInputs" :inputsKey="modalKey"/>
	</div>`,
	props: {
		editable: false
	},
	data() {
		return {
			regionSelectList: [],
			countries: [],
			cities: [],
			addAndNew: false,
			orgSelect: [],
			addressValues: {},
			editOrgId: null,
			pageRequestFilters: [],
			modalKey: 0,
			refreshInputs: 0,
			waitAddressButton: false,
		};
	},
	computed: {
		selectedRegion() {
			return this.regionSelectList[0] ? this.regionSelectList[0].value : '';
		},
		modalAddressSubmits() {
			const resultArray = [
				{
					name: 'Button',
					props: {
						label: 'Cancel',
						btnType: 'outline-secondary',
						onClick: () => {
							$(`#${this.addressModalProps.id}`).modal("hide");
						}
					}
				}];
			if (!this.editableAddressModal) {
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
		modalAddressLabel() {
			return "Add address";
		},
		addressModalProps() {
			return {
				id: 'addAddressModal',
				modalSize: 'lg',
				label: this.modalAddressLabel,
				formInputs: [
					{
						name: 'Select',
						props: {
							id: 'addressInput-organizationId',
							label: 'Organization',
							options: this.orgSelect,
							value: this.addressValues.organizationId,
							size: 10,
							searchable: true,
							className: 'col-12 col-md-6'
						}
					},
					{
						name: 'Select',
						props: {
							id: 'addressInput-countryId',
							label: 'Country',
							size: 10,
							searchable: true,
							options: this.countries,
							value: this.addressValues.countryId,
							className: 'col-12 col-md-6'
						}
					},
					{
						name: 'Select',
						props: {
							id: 'addressInput-regionId',
							label: 'Region',
							size: 10,
							required: true,
							searchable: true,
							options: this.regionSelectList,
							value: this.addressValues.regionId,
							className: 'col-12 col-md-6'
						}
					},
					{
						name: 'Select',
						props: {
							id: 'addressInput-cityId',
							label: 'City',
							noneSelectedText: 'Select region first',
							options: this.cities,
							required: true,
							size: 10,
							searchable: true,
							value: this.addressValues.cityId,
							className: 'col-12 col-md-6'
						}
					},
					{
						name: 'Input',
						props: {
							id: 'addressInput-street',
							label: 'Street',
							type: 'text',
							value: this.addressValues.street,
							className: 'col-12 col-md-6',
							validator: value => fieldValidationFunc(value, 'varChar128'),
							validatorInput: value => fieldValidationInputFunc(value, 'varChar128')
						}
					},
					{
						name: 'Input',
						props: {
							id: 'addressInput-zip',
							label: 'Zip Code',
							type: 'text',
							value: this.addressValues.zip,
							className: 'col-12 col-md-6',
							validator: value => fieldValidationFunc(value, 'zip'),
							validatorInput: value => fieldValidationInputFunc(value, 'zip')
						}
					},
				],
				formSubmits: this.modalAddressSubmits,
				onSubmit: () => {
						if (this.addAndNew) {
							this.addNewAddress().then(() => {
								this.resetAddressModalValues();
								this.refreshInputs++;
							});
						} else {
							this.addNewAddress().then(() => {
								$(`#${this.addressModalProps.id}`).modal("hide");
								this.resetAddressModalValues();
								this.refreshInputs++;
							});
						}
				}
			}
		}
	},
	created: async function () {
		this.resetAddressModalValues();
		customAjaxRequest(apiEndpoints.OrganizationAddress.GetAllCountries).then(result => {
			this.countries = this.convertToSelectList(result);
		});
		customAjaxRequest(apiEndpoints.Organization.GetAllOrganization).then(result => {
			this.orgSelect = this.convertToSelectList(result);
		});
	},
	methods: {
		async addNewAddress() {
			this.waitAddButton = true;
			return new Promise((resolve, reject) => {
				customAjaxRequest(apiEndpoints.OrganizationAddress.AddOrganizationAddress, 'PUT', this.addressValues).then(() => {
					this.waitAddButton = false;
					this.tableKey++;
					resolve(true);
				}).catch(e => {
					this.waitAddButton = false;
					toast.notifyErrorList(e);
					reject(false);
				});
			});
		},
		async updateAddress() {
			this.waitAddButton = true;
			return new Promise((resolve, reject) => {
				customAjaxRequest(apiEndpoints.OrganizationAddress.UpdateOrganizationAddress, 'POST', this.addressValues).then(() => {
					this.waitAddButton = false;
					this.tableKey++;
					resolve(true);
				}).catch(e => {
					this.waitAddButton = false;
					toast.notifyErrorList(e);
					reject(false);
				});
			});
		},
		async loadAddress(id) {
			return new Promise((resolve, reject) => {
				customAjaxRequest(apiEndpoints.OrganizationAddress.GetAddressById, 'GET', { id }).then(result => {
					resolve(result);
				}).catch(e => {
					toast.notifyErrorList(e);
					reject(e);
				});
			});
		},
		async loadRegionCities(regionId) {
			customAjaxRequest(apiEndpoints.OrganizationAddress.GetAllCitiesByRegionId, 'GET', { regionId }).then(result => {
				this.cities = this.convertToSelectList(result);
			});
		},
		async loadCountryRegions(countryId) {
			customAjaxRequest(apiEndpoints.OrganizationAddress.GetAllRegionsByCountryId, 'get', { countryId }).then(result => {
				this.regionSelectList = this.convertToSelectList(result);
			});
		},
		resetAddressModalValues() {
			this.addressValues = {
				organizationId: null,
				countryId: null,
				regionId: null,
				cityId: null,
				street: null,
				zipCode: null
			}
			this.modalKey++;
			this.refreshInputs++;
		},
		addAddress() {
			this.resetAddressModalValues();
			this.refreshInputs++;
			this.modalKey++;
			$(`#${this.addressModalProps.id}`).modal("show");
		},
		emitValueAddress(val) {
			const newVal = val.value === 'true' ? true : val.value === 'false' ? false : val.value;
			this.addressValues[val.id.replace('addressInput-', '')] = newVal;
			if (val.id === 'addressInput-countryId') {
				this.loadCountryRegions(val.value);
				this.cities = [];
			}
			if (val.id === 'addressInput-regionId') {
				this.loadRegionCities(val.value);
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
	}
});