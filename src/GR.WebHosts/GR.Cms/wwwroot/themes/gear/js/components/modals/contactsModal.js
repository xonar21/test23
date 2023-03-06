Vue.component('ContactsModal', {
	template: `<Modal :refreshInputs="refreshInputs"
					  :modalProps="modalContactProps"
					  @newValue="emitValueContact"
					  :inputsKey="localModalKey"/>`,
	props: {
		modalAddJobPositionId: String,
		editableContactModal: Boolean,
		contactIdForEditModal: String,
		modalKey: Number,
	},
	data() {
		return {
			currentHiddenPhoneIndex: 1,
			currentHiddenEmailIndex: 1,
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
			refreshInputs: 0,
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
			hideEmailInputs: ['', 'hide-field', 'hide-field', 'hide-field', 'hide-field'],
			hidePhoneInputs: ['', 'hide-field', 'hide-field'],
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
				}
			],
			hideInput: {
				class: '',
				checked: false,
			},
			waitAddButton: false,
			addAndNew: false,
			localModalKey: 0
		}
	},
	created() {
		this.$store.dispatch('getAllOrganizationsForSelect');
		this.$store.dispatch('getAllPhoneLabels');
		this.$store.dispatch('getAllEmailLabels');
		this.$store.dispatch('getAllJobPositions');
	},
	computed: {
		jobPositionsListSelect() {
			return this.$store.state.allJobPositions;
		},
		emailLabels() {
			return this.$store.state.allEmailLabels;
		},
		phoneLabels() {
			return this.$store.state.allPhoneLabels;
		},
		getEmailList() {
			return this.hideEmailInputs;
		},
		organizationsListSelect() {
			return this.$store.state.allOrganizationsForSelect;
		},
		modalContactLabel() {
			return this.editableContactModal ? t('contacts_edit_contact') : t('contacts_add_contact');
		},
		contactFormSubmits() {
			const resultArray = [
				{
					name: 'Button',
					props: {
						label: 'Cancel',
						btnType: 'outline-secondary',
						onClick: () => {
							$(`#${this.modalContactProps.id}`).modal("hide");
						}
					}
				}
			];
			if (!this.editableContactModal) {
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
							waiting: this.waitAddButton,
							btnDOMType: 'submit'
						}
					}
				);
			}
			return resultArray;
		},
		modalContactProps() {
			return {
				id: 'contactModal',
				label: this.modalContactLabel,
				formInputs: [
					{
						name: 'Input',
						props: {
							id: 'contact-firstName',
							type: 'text',
							label: t('system_first_name'),
							required: true,
							className: 'col-12 col-md-6',
							value: this.contactValues.firstName,
							validator: value => fieldValidationFunc(value, 'nameLetters'),
							validatorInput: value => fieldValidationInputFunc(value, 'nameLetters')
						}
					},
					{
						name: 'Input',
						props: {
							id: 'contact-lastName',
							type: 'text',
							label: t('system_last_name'),
							required: true,
							className: 'col-12 col-md-6',
							value: this.contactValues.lastName,
							validator: value => fieldValidationFunc(value, 'nameLetters'),
							validatorInput: value => fieldValidationInputFunc(value, 'nameLetters')
						}
					},
					{
						name: 'Select',
						props: {
							id: 'contact-organizationId',
							label: t('organization'),
							required: true,
							searchable: true,
							options: this.organizationsListSelect,
							value: this.contactValues.organizationId,
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
							className: 'col-5'
						}
					},
					{
						name: 'Button',
						props: {
							label: '&#43;',
							className: 'btn btn-outline-secondary btn-rounded mx-auto',
							wrapper: 'Div',
							wrapperClass: 'text-center',
							disabled: !this.getEmailList[4],
							onClick: () => this.emitValueContact({ value: '', id: `HideEmailInput${this.currentHiddenEmailIndex++}` })
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
							className: 'col-5 ' + this.getEmailList[1]
						}
					},
					{
						name: 'Button',
						props: {
							label: '-',
							className: 'btn btn-outline-danger btn-rounded mx-auto',
							wrapper: 'Div',
							wrapperClass: `text-center ${this.getEmailList[1]}`,
							onClick: () => this.emitValueContact({ value: 'hide-field', id: `HideEmailInput${this.currentHiddenEmailIndex--}` })
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
							className: 'col-5 ' + this.getEmailList[2]
						}
					},
					{
						name: 'Button',
						props: {
							label: '-',
							className: 'btn btn-outline-danger btn-rounded mx-auto',
							wrapper: 'Div',
							wrapperClass: `text-center ${this.getEmailList[2]}`,
							onClick: () => this.emitValueContact({ value: 'hide-field', id: `HideEmailInput${this.currentHiddenEmailIndex--}` })
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
							className: 'col-5 ' + this.getEmailList[3]
						}
					},
					{
						name: 'Button',
						props: {
							label: '-',
							className: 'btn btn-outline-danger btn-rounded mx-auto',
							wrapper: 'Div',
							wrapperClass: `text-center ${this.getEmailList[3]}`,
							onClick: () => this.emitValueContact({ value: 'hide-field', id: `HideEmailInput${this.currentHiddenEmailIndex--}` })
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
							className: 'col-5 ' + this.getEmailList[4]
						}
					},
					{
						name: 'Button',
						props: {
							label: '-',
							className: 'btn btn-outline-danger btn-rounded mx-auto',
							wrapper: 'Div',
							wrapperClass: `text-center ${this.getEmailList[4]}`,
							onClick: () => this.emitValueContact({ value: 'hide-field', id: `HideEmailInput${this.currentHiddenEmailIndex--}` })
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
							options: this.phoneLabels,
							value: this.phoneList[0].label,
							className: 'col-5'
						}
					},
					{
						name: 'Button',
						props: {
							label: '&#43;',
							className: 'btn btn-outline-secondary btn-rounded mx-auto',
							wrapper: 'Div',
							wrapperClass: 'text-center',
							disabled: Boolean(this.hideInput.class || this.hidePhoneInputs[0] || !this.hidePhoneInputs[2]),
							onClick: () => this.emitValueContact({ value: '', id: `HideInput${this.currentHiddenPhoneIndex++}` })
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
							options: this.phoneLabels,
							value: this.phoneList[1].label,
							className: 'col-5 ' + this.hidePhoneInputs[1] + ' ' + this.hideInput.class
						}
					},
					{
						name: 'Button',
						props: {
							label: '-',
							className: 'btn btn-outline-danger btn-rounded mx-auto',
							wrapper: 'Div',
							wrapperClass: `text-center ${this.hidePhoneInputs[1]} ${this.hideInput.class}`,
							onClick: () => this.emitValueContact({ value: 'hide-field', id: `HideInput${this.currentHiddenPhoneIndex--}` })
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
							options: this.phoneLabels,
							value: this.phoneList[2].label,
							className: 'col-5 ' + this.hidePhoneInputs[2] + ' ' + this.hideInput.class
						}
					},
					{
						name: 'Button',
						props: {
							label: '-',
							className: 'btn btn-outline-danger btn-rounded mx-auto',
							wrapper: 'Div',
							wrapperClass: `text-center ${this.hidePhoneInputs[2]} ${this.hideInput.class}`,
							onClick: () => this.emitValueContact({ value: 'hide-field', id: `HideInput${this.currentHiddenPhoneIndex--}` })
						}
					},
					{
						name: 'Switcher',
						props: {
							id: 'N/ALabel',
							value: this.contactValues.notAvailable,
							wrapper: 'Div',
							wrapperClass: 'col-12 d-flex align-items-center mb-3',
							label: 'N/A'
						}
					},
					{
						name: 'Select',
						props: {
							id: 'contact-jobPositionId',
							label: t('details_job_position'),
							options: this.jobPositionsListSelect,
							value: this.contactValues.jobPositionId,
							className: 'col-12',
							addBtn: true,
							openModal: () => {
								$(`#${this.modalContactProps.id}`).modal("hide");
								this.$emit('resetJobPostion');
								this.$emit('updateModalKey');
								$(`#${this.modalAddJobPositionId}`).modal("show");
							}
						}
					},
					{
						name: 'Textarea',
						props: {
							id: 'contact-description',
							label: t('description'),
							className: 'col-12',
							value: this.contactValues.description,
						}
					}
				],
				formSubmits: this.contactFormSubmits,
				onSubmit: () => {
					if (!this.editableContactModal) {
						this.addNewContact().then(() => {
							if (this.addAndNew) {
								this.$emit('updateModalKey');
								this.refreshInputs++;
								this.resetContactModalValues();
							} else {
								$(`#${this.modalContactProps.id}`).modal("hide");
							}
						});
					} else {
						this.updateContact().then(() => {
							$(`#${this.modalContactProps.id}`).modal("hide");
						});
					}
				}
			}
		},
	},
	methods: {
		emitValueContact(val) {
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
			if (val.id.includes('contact-email')) {
				let index = val.id[13] - '0';
				this.emailList[index - 1].email = val.value;
			}
			if (val.id.includes('contact_EmailLabel')) {
				let index = val.id[18] - '0';
				this.emailList[index - 1].label = val.value;
			}

			const newVal = val.value === 'true' ? true : val.value === 'false' ? false : val.value;
			this.contactValues[val.id.replace('contact-', '')] = newVal;

			if (val.id == 'contact-organizationId') {
				this.newOrgId == newVal;
			}

			if (val.id.includes('contact-Phone')) {
				let length = val.id.length;
				let index = (val.id[length - 1] - '0') - 1;
				this.phoneList[index].phone = val.value.number;
				this.phoneList[index].countryCode = val.value.countryCode;
				this.phoneList[index].dialCode = val.value.dialCode;
			}
			if (val.id.includes('contact_PhoneLabel')) {
				let length = val.id.length;
				let index = (val.id[length - 1] - '0') - 1;
				this.phoneList[index].label = val.value;
			}

			if (val.id.includes('HideInput')) {
				let index = val.id[9] - '0';
				if (val.value == '') this.hidePhoneInputs[index] = val.value;
				else {
					this.hidePhoneInputs[index - 1] = val.value;
					if (val.value != '') {
						if (this.phoneList[index - 1].id) {
							this.deletePhone(this.phoneList[index - 1].id);
						}
						this.resetPhone(index - 1);
					}
				}
				this.hidePhoneInputs.push(val.value);
				this.hidePhoneInputs.pop();
			}
			if (val.id == 'N/ALabel') {
				this.hideInput = val.value;
				this.contactValues.notAvailable = val.value.checked;
				if (this.phoneList[0].id) {
					this.deletePhone(this.phoneList[0].id);
				}
				this.resetPhone(0);
			}
		},
		convertLabelsToSelectList(array) {
			if (array) {
				let index = 0;
				return array.map(e => {
					const newObj = {
						label: e,
						value: index,
					}
					index++;
					return newObj;
				});
			} else return [];
		},
		convertToSelectList(array) {
			if (array.length === 0) return [];
			return array.map(e => {
				const newObj = {
					label: e.name,
					value: e.id
				}
				return newObj;
			});
		},
		async addNewContact() {
			this.waitAddButton = true;
			return new Promise((resolve, reject) => {
				customAjaxRequest(apiEndpoints.Contact.AddNewContact, 'PUT', this.contactValues).then(result => {
					if (this.hideInput.class == '') {
						let addList = [];
						for (i = 0; i < 3; i++) {
							if (this.hidePhoneInputs[i] == '') {
								addList.push({
									phone: this.phoneList[i].phone,
									countryCode: this.phoneList[i].countryCode,
									dialCode: this.phoneList[i].dialCode,
									label: this.phoneList[i].label,
									contactId: result
								});
							}
						}
						this.addPhoneList(addList);
					}

					let EmailListAdd = [];
					for (i = 0; i < 5; i++) {
						if (this.hideEmailInputs[i] == '') {
							EmailListAdd.push({
								email: this.emailList[i].email,
								label: this.emailList[i].label,
								contactId: result
							});
						}
					}

					if (EmailListAdd.length > 0)
						this.addEmailList(EmailListAdd);

					this.$emit('updateTableKey');
					resolve(true);
				}).catch(e => {
					toast.notifyErrorList(e);
					reject(false);
				}).finally(() => {
					this.waitAddButton = false;
				});
			});
		},
		async addPhoneList(ListToAdd) {
			return new Promise((resolve, reject) => {
				customAjaxRequest(apiEndpoints.Phone.AddPhoneRange, 'PUT', { model: ListToAdd }).then(() => {
					this.$emit('updateTableKey');
					resolve(true);
				}).catch(e => {
					toast.notifyErrorList(e);
					reject(false);
				}).finally(() => {
					this.waitAddButton = false;
				});
			})
		},
		async addEmailList(ListToAdd) {
			return new Promise((resolve, reject) => {
				customAjaxRequest(apiEndpoints.Email.AddEmailRange, 'PUT', { model: ListToAdd }).then(() => {
					this.$emit('updateTableKey');
					resolve(true);
				}).catch(e => {
					toast.notifyErrorList(e);
					reject(false);
				}).finally(() => {
					this.waitAddButton = false;
				});
			})
		},
		resetContactModalValues() {
			this.contactValues = {
				organizationId: '',
				email: '',
				requiredPhone: true,
				phoneList: [],
				firstName: '',
				lastName: '',
				description: '',
				jobPositionId: '',
				notAvailable: false
			}
			this.phoneList = [
				{
					phone: '',
					countryCode: 'MD',
					dialCode: '373',
					label: '',
					contactId: ''
				},
				{
					phone: '',
					countryCode: 'MD',
					dialCode: '373',
					label: '',
					contactId: ''
				},
				{
					phone: '',
					countryCode: 'MD',
					dialCode: '373',
					label: '',
					contactId: ''
				}
			];
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
			this.hideInput = {
				class: '',
				checked: false,
			};
			this.hidePhoneInputs = ['', 'hide-field', 'hide-field'];
			this.currentHiddenEmailIndex = 1;
			this.currentHiddenPhoneIndex = 1;
		},
		async updateContact() {
			this.waitAddButton = true;
			return new Promise((resolve, reject) => {
				customAjaxRequest(apiEndpoints.Contact.UpdateContact, 'POST', this.contactValues).then(result => {
					if (this.hideInput.class == '') {
						//construct add and update list for phones
						let updateList = [];
						let addList = [];
						for (i = 0; i < 3; i++) {
							if (this.hidePhoneInputs[i] == '') {
								if (this.phoneList[i].id) {
									updateList.push(this.phoneList[i]);
								}
								else {
									addList.push({
										phone: this.phoneList[i].phone,
										countryCode: this.phoneList[i].countryCode,
										dialCode: this.phoneList[i].dialCode,
										label: this.phoneList[i].label,
										contactId: result
									});
								}
							}
						}
						if (updateList.length > 0)
							this.updatePhoneList(updateList);
						if (addList.length > 0)
							this.addPhoneList(addList);
					}

					//construct add and update list for emails
					let EmailListUpdate = [];
					let EmailListAdd = [];
					for (i = 0; i < 5; i++) {
						if (this.hideEmailInputs[i] == '') {
							if (this.emailList[i].id) {
								EmailListUpdate.push(this.emailList[i]);
							}
							else {
								EmailListAdd.push({
									email: this.emailList[i].email,
									label: this.emailList[i].label,
									contactId: result
								});
							}
						}
					}
					if (EmailListUpdate.length > 0)
						this.updateEmailList(EmailListUpdate);
					if (EmailListAdd.length > 0) {
						this.addEmailList(EmailListAdd);
					}

					this.$emit('updateTableKey');
					resolve(true);
				}).catch(e => {
					toast.notifyErrorList(e);
					reject(false);
				}).finally(() => {
					this.waitAddButton = false;
				});
			});
		},
		async updatePhoneList(ListToUpdate) {
			return new Promise((resolve, reject) => {
				customAjaxRequest(apiEndpoints.Phone.UpdateRangePhone, 'POST', { model: ListToUpdate }).then(() => {
					this.$emit('updateTableKey');
					resolve(true);
				}).catch(e => {
					toast.notifyErrorList(e);
					reject(false);
				}).finally(() => {
					this.waitAddButton = false;
				});
			});
		},
		async updateEmailList(ListToUpdate) {
			return new Promise((resolve, reject) => {
				customAjaxRequest(apiEndpoints.Email.UpdateRangeEmail, 'POST', { model: ListToUpdate }).then(() => {
					this.$emit('updateTableKey');
					resolve(true);
				}).catch(e => {
					toast.notifyErrorList(e);
					reject(false);
				}).finally(() => {
					this.waitAddButton = false;
				});
			});
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
		resetPhone(index) {
			this.phoneList[index] = {
				phone: '',
				countryCode: 'MD',
				dialCode: '373',
				label: '',
				contactId: ''
			};
		},
		async deletePhone(phoneId) {
			customAjaxRequest(apiEndpoints.Phone.DeletePhoneById, 'DELETE', { phoneId }).then(() => {
				this.$emit('updateTableKey');
			}).catch(e => {
				toast.notifyErrorList(e);
			});
		},
		async deleteEmail(emailId) {
			customAjaxRequest(apiEndpoints.Email.DeleteEmailById, 'DELETE', { emailId }).then(() => {
				this.$emit('updateTableKey');
			}).catch(e => {
				toast.notifyErrorList(e);
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
	},
	watch: {
		modalKey() {
			if (this.editableContactModal) {
				this.hideEmailInputs = [];
				this.hidePhoneInputs = [];
				this.loadContact(this.contactIdForEditModal).then(result => {
					this.contactValues = result;
					this.phoneList = result.phoneList.length > 0 ? result.phoneList : [];

					this.emailList = result.emailList.length > 0 ? result.emailList : [];

					for (i = 0; i < this.phoneList.length; i++) {
						this.hidePhoneInputs.push('');
					}
					for (i = this.phoneList.length; i < 3; i++) {
						this.hidePhoneInputs.push('hide-field');
						this.phoneList.push(
							{
								phone: '',
								countryCode: 'MD',
								dialCode: '373',
								label: '',
								contactId: this.contactIdForEditModal
							}
						);
					}
					this.hidePhoneInputs[0] = '';
					if (this.contactValues.notAvailable) {
						this.hideInput = {
							class: 'hide-field',
							checked: true
						}
					}
					else {
						this.hideInput = {
							class: '',
							checked: false
						}
					}
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
					this.hideEmailInputs[0] = '';
					this.refreshInputs++;
					this.localModalKey++;
					$(`#${this.modalContactProps.id}`).modal("show");
				});
			} else {
				this.resetContactModalValues();
				this.refreshInputs++;
				this.localModalKey++;
				$(`#${this.modalContactPropsId}`).modal("show");
			}
		}
	}
});