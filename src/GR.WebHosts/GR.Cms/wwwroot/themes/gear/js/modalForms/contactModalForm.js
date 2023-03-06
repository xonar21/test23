Vue.component('ContactModalForm', {
	template: `<div>
			<Modal :modalProps="contactModalProps" @newValue="emitContactValue"  @newPhone="emitPhone" :refreshInputs="refreshInputs" :inputsKey="modalKey"/>
			<Modal :refreshInputs="refreshInputs" :modalProps="modalAddJobPositionProps" @newValue="emitJobPositionValue" :inputsKey="modalKey"/>
	</div>`,
	props: {
		editable: false
	},
	data() {
		return {
			jobPositionsListSelect: [],
			jobPosition: {},
			addAndNew: false,
			waitAddButton: false,
			orgSelect: [],
			pageRequestFilters: [],
			modalKey: 0,
			refreshInputs: 0,
			waitAddressButton: false,
			contactValues: {
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
		modalAddJobPositionProps() {
			return {
				id: 'addJobPositionModal',
				label: 'Add job position',
				formInputs: [
					{
						name: 'Input',
						props: {
							id: 'job-name',
							type: 'text',
							label: 'Name',
							required: true,
							className: 'col-12',
							value: this.jobPosition.name,
							validator: value => fieldValidationFunc(value, 'name'),
							validatorInput: value => fieldValidationInputFunc(value, 'name')
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
								$(`#${this.modalAddJobPositionProps.id}`).modal("hide");
								$(`#${this.contactModalProps.id}`).modal("show");
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
					console.log('submit');
					this.addNewJobPosition().then(() => {
						$(`#${this.modalAddJobPositionProps.id}`).modal("hide");
						$(`#${this.contactModalProps.id}`).modal("show");
					});
				}
			}
		},
		modalContactSubmits() {
			const resultArray = [
				{
					name: 'Button',
					props: {
						label: 'Cancel',
						btnType: 'outline-secondary',
						onClick: () => {
							$(`#${this.contactModalProps.id}`).modal("hide");
						}
					}
				}];
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
			return resultArray;
		},
		modalContactLabel() {
			return "Add contact";
		},
		contactModalProps() {
			return {
				id: 'addContactModal',
				label: this.modalContactLabel,
				formInputs: [
					{
						name: 'Input',
						props: {
							id: 'contactInput-firstName',
							type: 'text',
							label: 'First Name',
							required: true,
							value: this.contactValues.firstName,
							className: 'col-12 col-md-6',
							validator: value => fieldValidationFunc(value, 'name'),
							validatorInput: value => fieldValidationInputFunc(value, 'name')
						}
					},
					{
						name: 'Input',
						props: {
							id: 'contactInput-lastName',
							type: 'text',
							label: 'Last Name',
							required: true,
							value: this.contactValues.lastName,
							className: 'col-12',
							validator: value => fieldValidationFunc(value, 'name'),
							validatorInput: value => fieldValidationInputFunc(value, 'name')
						}
					},
					{
						name: 'Select',
						props: {
							id: 'contactInput-organizationId',
							label: 'Organization',
							options: this.orgSelect,
							required: true,
							value: this.contactValues.organizationId,
							size: 10,
							searchable: true,
							className: 'col-12'
						}
					},
					{
						name: 'Input',
						props: {
							id: 'contact-email1',
							type: 'email',
							label: t('email'),
							required: true,
							className: 'col-6',
							value: this.emailList[0].email,
							validator: value => fieldValidationFunc(value, 'email')
						}
					},
					{
						name: 'Select',
						props: {
							id: 'contact_EmailLabel1',
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
							className: 'col-12 ' + this.getEmailList[0],
							hideDelete: true,
							hidePlus: this.getEmailList[1] == '' ? true : false,
							hideLabel: false,
						}
					},
					{
						name: 'Input',
						props: {
							id: 'contact-email2',
							type: 'email',
							label: t('email'),
							className: 'col-6 ' + this.getEmailList[1],
							value: this.emailList[1].email,
							validator: value => fieldValidationFunc(value, 'email')
						}
					},
					{
						name: 'Select',
						props: {
							id: 'contact_EmailLabel2',
							label: 'Label',
							noneSelectedText: 'Select label',
							options: this.emailLabels,
							value: this.emailList[1].label,
							className: 'col-6 ' + this.getEmailList[1]
						}
					},
					{
						name: 'AddHideInput',
						props: {
							id: 'HideEmailInput2',
							className: 'col-12 ' + this.getEmailList[1],
							hidePlus: this.getEmailList[2] == '' ? true : false,
							hideLabel: false,
						}
					},
					{
						name: 'Input',
						props: {
							id: 'contact-email3',
							type: 'email',
							label: t('email'),
							className: 'col-6 ' + this.getEmailList[2],
							value: this.emailList[2].email,
							validator: value => fieldValidationFunc(value, 'email')
						}
					},
					{
						name: 'Select',
						props: {
							id: 'contact_EmailLabel3',
							label: 'Label',
							noneSelectedText: 'Select label',
							options: this.emailLabels,
							value: this.emailList[2].label,
							className: 'col-6 ' + this.getEmailList[2]
						}
					},
					{
						name: 'AddHideInput',
						props: {
							id: 'HideEmailInput3',
							className: 'col-12 ' + this.getEmailList[2],
							hidePlus: this.getEmailList[3] == '' ? true : false,
							hideLabel: false,
						}
					},
					{
						name: 'Input',
						props: {
							id: 'contact-email4',
							type: 'email',
							label: t('email'),
							className: 'col-6 ' + this.getEmailList[3],
							value: this.emailList[3].email,
							validator: value => fieldValidationFunc(value, 'email')
						}
					},
					{
						name: 'Select',
						props: {
							id: 'contact_EmailLabel4',
							label: 'Label',
							noneSelectedText: 'Select label',
							options: this.emailLabels,
							value: this.emailList[3].label,
							className: 'col-6 ' + this.getEmailList[3]
						}
					},
					{
						name: 'AddHideInput',
						props: {
							id: 'HideEmailInput4',
							className: 'col-12 ' + this.getEmailList[3],
							hidePlus: this.getEmailList[4] == '' ? true : false,
							hideLabel: false,
						}
					},
					{
						name: 'Input',
						props: {
							id: 'contact-email5',
							type: 'email',
							label: t('email'),
							className: 'col-6 ' + this.getEmailList[4],
							value: this.emailList[4].email,
							validator: value => fieldValidationFunc(value, 'email')
						}
					},
					{
						name: 'Select',
						props: {
							id: 'contact_EmailLabel5',
							label: 'Label',
							noneSelectedText: 'Select label',
							options: this.emailLabels,
							value: this.emailList[4].label,
							className: 'col-6 ' + this.getEmailList[4]
						}
					},
					{
						name: 'AddHideInput',
						props: {
							id: 'HideEmailInput5',
							className: 'col-12 ' + this.getEmailList[4],
							hidePlus: true,
							hideLabel: false,

						}
					},
					{
						name: 'PhoneInput',
						props: {
							id: 'contact-Phone1',
							type: 'text',
							label: 'Phone',
							required: this.hideInput.class == '' ? true : false,
							className: 'col-6',
							disabled: this.hideInput.class == '' ? false : true,
							dialCode: this.phoneList[0].dialCode,
							defaultCountry: this.phoneList[0].countryCode,
							value: this.phoneList[0].phone,
						}
					},
					{
						name: 'Select',
						props: {
							id: 'contact_PhoneLabel1',
							label: 'Label',
							disabled: this.hideInput.class == '' ? false : true,
							noneSelectedText: 'Select label',
							options: this.labels,
							value: this.phoneList[0].label,
							className: 'col-6'
						}
					},
					{
						name: 'AddHideInput',
						props: {
							id: 'HideInput1',
							className: 'col-12 ' + this.hideInput1,
							hideDelete: true,
							hidePlus: this.hideInput.class != '' || this.hidePhoneInputs[0] || this.hidePhoneInputs[1] == '' ? true : false,
							hideLabel: this.hidePhoneInputs[1] == '' || this.hidePhoneInputs[2] == '' ? false : true,
							value: this.contactValues.notAvailable
						}
					},
					{
						name: 'PhoneInput',
						props: {
							id: 'contact-Phone2',
							type: 'text',
							label: 'Phone',
							className: 'col-6 ' + this.hidePhoneInputs[1] + ' ' + this.hideInput.class,
							dialCode: this.phoneList[1].dialCode,
							defaultCountry: this.phoneList[1].countryCode,
							value: this.phoneList[1].phone,
						}
					},
					{
						name: 'Select',
						props: {
							id: 'contact_PhoneLabel2',
							label: 'Label',
							noneSelectedText: 'Select label',
							options: this.labels,
							value: this.phoneList[1].label,
							className: 'col-6 ' + this.hidePhoneInputs[1] + ' ' + this.hideInput.class
						}
					},
					{
						name: 'AddHideInput',
						props: {
							id: 'HideInput2',
							className: 'col-12 ' + this.hidePhoneInputs[1] + ' ' + this.hideInput.class,
							hidePlus: this.hidePhoneInputs[2] == '' ? true : false,
							hideLabel: false,
						}
					},
					{
						name: 'PhoneInput',
						props: {
							id: 'contact-Phone3',
							type: 'text',
							label: 'Phone',
							className: 'col-6 ' + this.hidePhoneInputs[2] + ' ' + this.hideInput.class,
							dialCode: this.phoneList[2].dialCode,
							defaultCountry: this.phoneList[2].countryCode,
							value: this.phoneList[2].phone,
						}
					},
					{
						name: 'Select',
						props: {
							id: 'contact_PhoneLabel3',
							label: 'Label',
							noneSelectedText: 'Select label',
							options: this.labels,
							value: this.phoneList[2].label,
							className: 'col-6 ' + this.hidePhoneInputs[2] + ' ' + this.hideInput.class
						}
					},
					{
						name: 'AddHideInput',
						props: {
							id: 'HideInput3',
							className: 'col-12 ' + this.hidePhoneInputs[2] + ' ' + this.hideInput.class,
							hidePlus: true,
							hideLabel: false,
						}
					},
					{
						name: 'Select',
						props: {
							id: 'contactInput-jobPositionId',
							label: 'Job position',
							options: this.jobPositionsListSelect,
							value: this.contactValues.jobPositionId,
							className: 'col-11'
						}
					},
					{
						name: 'Button',
						props: {
							label: '<span>&#43;<span/>',
							className: "allwaysWrapped",
							wrapper: "div",
							wrapperClass: "col-1",
							btnType: 'outline-secondary',
							onClick: () => {
								$(`#${this.contactModalProps.id}`).modal("hide");
								this.resetJobPostion();
								this.modalKey++;
								$(`#${this.modalAddJobPositionProps.id}`).modal("show");
							}
						}
					},
					{
						name: 'Textarea',
						props: {
							id: 'contactInput-description',
							label: 'Description',
							className: 'col-12',
							value: this.contactValues.description,
						}
					}
				],
				formSubmits: this.modalContactSubmits,
				onSubmit: () => {
					if (this.contactValues.phoneList)
						this.contactValues.phoneList = this.contactValues.phoneList.map(e => e.value);
					if (this.contactValues.phone)
						this.contactValues.phoneList.push(this.contactValues.phone);

					this.addNewContact().then(() => {
						if (!this.addAndNew) {
							$(`#${this.contactModalProps.id}`).modal("hide");
						} else {
							this.resetContactModalValues();
							this.refreshInputs++;
						}
					});
				}
			}
		}
	},
	created: async function () {
		this.resetContactModalValues();
		customAjaxRequest(apiEndpoints.JobPosition.GetAllJobPositions).then(result => {
			this.jobPositionsListSelect = this.convertArrayToSelectList(result, 'name', 'id');
		});
		customAjaxRequest(apiEndpoints.Organization.GetAllOrganization).then(result => {
			this.orgSelect = this.convertToSelectList(result);
		});
	},
	methods: {
		async addNewJobPosition() {
			this.waitAddButton = true
			return new Promise((resolve, reject) => {
				customAjaxRequest(apiEndpoints.JobPosition.AddNewJobPosition, 'PUT', this.jobPosition).then(result => {
					this.jobPositionsListSelect.push({ label: this.jobPosition.name, value: result });
					resolve(true);
				}).catch(e => {
					toast.notifyErrorList(e);
					reject(false);
				}).finally(() => {
					this.waitAddButton = false;
				});
			});
		},
		async addNewContact() {
			this.waitAddButton = true;
			return new Promise((resolve, reject) => {
				customAjaxRequest(apiEndpoints.Contact.AddNewContact, 'PUT', this.contactValues).then(() => {
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
		async updateContact() {
			this.waitAddButton = true;
			return new Promise((resolve, reject) => {
				customAjaxRequest(apiEndpoints.Contact.UpdateContact, 'POST', this.contactValues).then(() => {
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
		async loadContact(contactId) {
			return new Promise((resolve, reject) => {
				$.ajax({
					url: '/api/Contact/GetContactById',
					type: 'GET',
					contentType: "application/x-www-form-urlencoded",
					data: { contactId },
					success: data => {
						if (Array.isArray(data)) {
							resolve(data);
						}
						else {
							if (data.is_success) {
								resolve(data.result);
							} else if (!data.is_success) {
								reject(data.error_keys);
							} else {
								resolve(data);
							}
						}
					},
					error: e => {
						reject(e);
					}
				});
			});
		},
		async loadRegionCities(regionId) {
			customAjaxRequest(apiEndpoints.OrganizationAddress.GetAllCitiesByRegionId, 'GET', { regionId }).then(result => {
				this.cities = this.convertToSelectList(result);
			});
		},
		resetContactModalValues() {
			this.contactValues = {
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
		addContact() {
			this.resetContactModalValues();
			this.refreshInputs++;
			this.modalKey++;
			$(`#${this.contactModalProps.id}`).modal("show");
		},
		resetJobPostion() {
			this.jobPosition.name = null;
		},
		emitPhone(val) {
			console.log(val);
			this.contactValues.phone = '';
			this.contactValues.requiredPhone = false;
			this.contactValues.phoneList.push(val);
		},
		emitContactValue(val) {
			const newVal = val.value === 'true' ? true : val.value === 'false' ? false : val.value;
			this.contactValues[val.id.replace('contactInput-', '')] = newVal;
		},
		emitJobPositionValue(val) {
			this.jobPosition.name = val.value
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
	}
});