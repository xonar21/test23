Vue.component('EditAgreementModalForm', {
	template: `
	<div>
		<Modal :refreshInputs="refreshInputs" :modalProps="editAgreementModalProps" @newValue="emitModalValue" :inputsKey="modalKey"/>
	</div>
	`,
	props: {
		editable: true
	},
	data() {
		return {
			modalKey: 0,
			contractTemplatesSelect: [],
			leadsSelect: [],
			organisationsSelect: [],
			organisationAddressesSelect: [],
			organisationContactsSelect: [],
			usersSelect: [],
			productsSelect: [],
			workCategoriesSelect: [],
			modalValues: {},
			isOrgSelectDisabled: false,
			waitAddButton: false,
			refreshInputs: 0
		};
	},
	computed: {
		modalLabel() {
			return 'Edit agreement';
		},
		editAgreementModalProps() {
			return {
				id: 'editAgreement',
				label: this.modalLabel,
				formInputs: [
					{
						name: 'Input',
						props: {
							id: 'editAgreement-name',
							type: 'text',
							label: 'Name',
							required: true,
							className: 'col-12',
							value: this.modalValues.name,
							validator: value => fieldValidationFunc(value, 'varChar128'),
							validatorInput: value => fieldValidationInputFunc(value, 'varChar128')
						}
					},
					{
						name: 'Select',
						props: {
							id: 'editAgreement-contractTemplateId',
							label: 'Template Contract',
							required: true,
							options: this.contractTemplatesSelect,
							className: 'col-12',
							value: this.modalValues.contractTemplateId
						}
					},
					{
						name: 'Select',
						props: {
							id: 'editAgreement-organizationId',
							label: 'Organization',
							required: true,
							options: this.organisationsSelect,
							size: 10,
							className: 'col-12',
							value: this.modalValues.organizationId
						}
					},
					{
						name: 'Select',
						props: {
							id: 'editAgreement-leadId',
							label: 'Lead',
							required: true,
							size: 10,
							searchable: true,
							options: this.leadsSelect,
							className: 'col-12',
							value: this.modalValues.leadId,
							noneSelectedText: this.modalValues.organizationId && this.leadsSelect.length == 0 ? 'Please add at least one lead to this organization' : 'Nothing selected'
						}
					},
					{
						name: 'Select',
						props: {
							id: 'editAgreement-organizationAddressId',
							label: 'Organization address',
							required: true,
							options: this.organisationAddressesSelect,
							size: 10,
							searchable: true,
							className: 'col-12',
							value: this.modalValues.organizationAddressId,
							noneSelectedText: this.modalValues.organizationId && this.organisationAddressesSelect.length == 0 ? 'Please add at least one address to this organization' : 'Nothing selected'
						}
					},
					{
						name: 'Select',
						props: {
							id: 'editAgreement-contactId',
							label: 'Contact',
							required: true,
							size: 10,
							searchable: true,
							options: this.organisationContactsSelect,
							className: 'col-12 col-md-6',
							value: this.modalValues.contactId,
							noneSelectedText: this.modalValues.organizationId && this.organisationContactsSelect.length == 0 ? 'Please add at least one contact to this organization' : 'Nothing selected'
						}
					},
					{
						name: 'Select',
						props: {
							id: 'editAgreement-productId',
							label: 'Product',
							required: false,
							options: this.productsSelect,
							size: 10,
							className: 'col-12 col-md-6',
							value: this.modalValues.productId
						}
					},
					{
						name: 'Input',
						props: {
							id: 'editAgreement-values',
							label: 'Values',
							type: 'text',
							className: 'col-12 col-md-6',
							value: this.modalValues.values,
							validator: value => fieldValidationFunc(value, 'naturalNum'),
							validatorInput: value => fieldValidationInputFunc(value, 'naturalNum')
						}
					},
					{
						name: 'Input',
						props: {
							id: 'editAgreement-commission',
							label: 'Commission',
							type: 'text',
							required: true,
							className: 'col-12 col-md-6',
							inputSuffix: '%',
							value: this.modalValues.commission,
							validator: value => fieldValidationFunc(value, 'percentage'),
							validatorInput: value => fieldValidationInputFunc(value, 'percentage')
						}
					},
					{
						name: 'Select',
						props: {
							id: 'editAgreement-userId',
							label: 'Responsible',
							required: true,
							size: 10,
							searchable: true,
							options: this.usersSelect,
							className: 'col-12',
							value: this.modalValues.userId
						}
					},
					{
						name: 'Textarea',
						props: {
							id: 'editAgreement-description',
							label: 'Description',
							required: true,
							value: this.modalValues.description,
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
								$(`#${this.editAgreementModalProps.id}`).modal("hide");
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
					this.updateEntity().then(() => {
						$(`#${this.editAgreementModalProps.id}`).modal("hide");
						this.$emit('action');
					});
				}
			}
		},
	},
	created() {
		this.resetModalValues();
		const promises = [
			customAjaxRequest(apiEndpoints.Contract.GetAllContractTemplate),
			customAjaxRequest(apiEndpoints.Organization.GetAllOrganization),
			customAjaxRequest(apiEndpoints.Users.GetUsers),
			customAjaxRequest(apiEndpoints.Product.GetAllProducts)
		];
		Promise.all(promises).then(result => {
			this.contractTemplatesSelect = this.convertToSelectList(result[0]);
			this.organisationsSelect = this.convertToSelectList(result[1]);
			this.usersSelect = this.convertUsersToSelectList(result[2]);
			this.productsSelect = this.convertToSelectList(result[3]);
		});
	},
	methods: {
		async updateEntity() {
			console.log(this.modalValues);
			this.waitAddButton = true;
			return new Promise((resolve, reject) => {
				customAjaxRequest(apiEndpoints.Agreement.UpdateAgreement, 'POST', this.modalValues).then(() => {
					this.tableKey++;
					this.$emit('action');
					resolve(true);
				}).catch(e => {
					toast.notifyErrorList(e);
					reject(false);
				}).finally(() => {
					this.waitAddButton = false;
				});
			});
		},
		async loadEntity(entityId) {
			return new Promise((resolve, reject) => {
				customAjaxRequest(apiEndpoints.Agreement.GetAgreementById, 'GET', { agreementId: entityId }).then(result => {
					resolve(result);
					console.log(result);
				}).catch(e => {
					toast.notifyErrorList(e);
				});
			});
		},
		loadOrganizationAdditionalData(organizationId) {
			const promises = [
				customAjaxRequest(apiEndpoints.Leads.GetLeadsByOrganizationId, 'GET', { organizationId }),
				customAjaxRequest(apiEndpoints.Contact.GetContactByOrganizationId, 'GET', { organizationId }),
				customAjaxRequest(apiEndpoints.OrganizationAddress.GetAddressesByOrganizationId, 'GET', { organizationId })
			];
			Promise.all(promises).then(result => {
				this.leadsSelect = this.convertToSelectList(result[0]);
				this.organisationContactsSelect = this.convertContactsToSelectList(result[1]);
				this.organisationAddressesSelect = this.convertAdddressToSelectList(result[2]);
				this.modalKey++;
			});
		},
		editEntity(entityId) {
			this.loadEntity(entityId).then(result => {
				this.modalValues = {
					id: entityId,
					name: result.name,
					leadId: result.leadId,
					organizationId: result.organizationId,
					productId: result.productId,
					contactId: result.contactId,
					organizationAddressId: result.organizationAddressId,
					userId: result.userId,
					contractTemplateId: result.contractTemplateId,
					commission: result.commission,
					description: result.description,
					values: result.values
				}
				this.updateEntityId = entityId;
				if (result.organizationId) {
					this.loadOrganizationAdditionalData(result.organizationId);
				}
				this.modalKey++;
				this.refreshInputs++;
				$(`#${this.editAgreementModalProps.id}`).modal("show");
			});
		},
		generateContract(agreementId) {
			window.open(`${window.location.origin}${apiEndpoints.Agreement.GenerateFileContractForAgreement}?agreementId=${agreementId}`, '_blank');
		},
		resetModalValues() {
			this.modalValues = {
				name: null,
				leadId: null,
				organizationId: null,
				contactId: null,
				organizationAddressId: null,
				userId: null,
				contractTemplateId: null,
				commission: null,
				values: 0
			}
		},
		emitModalValue(val) {
			const newVal = val.value === 'true' ? true : val.value === 'false' ? false : val.value;
			this.modalValues[val.id.replace('editAgreement-', '')] = newVal;
			if (val.id == 'editAgreement-organizationId') {
				this.loadOrganizationAdditionalData(val.value);
				this.modalKey++;
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
		convertContactsToSelectList(array) {
			return array.map(e => {
				const newObj = {
					label: `${e.firstName} ${e.lastName}`,
					value: e.id
				}
				return newObj;
			});
		},
		convertUsersToSelectList(array) {
			return array.map(e => {
				return {
					label: e.userFirstName && e.userFirstName.trim() ? `${e.userFirstName} ${e.userLastName}` : e.userName,
					value: e.id
				}
			});
		},
		convertAdddressToSelectList(array) {
			return array.map(e => {
				const newObj = {
					label: `r.${e.city.region.name}, ${e.city.name} ${e.street ? ', ' + e.street : ''}`,
					value: e.id
				}
				return newObj;
			});
		},
		getUserName(userId) {
			const user = this.findObjectByPropValue(this.usersSelect, userId, 'value');
			return user.label;
		},
		findObjectByPropValue: (array, value, prop) => {
			return array.find(x => x[prop] === value);
		}
	}
});