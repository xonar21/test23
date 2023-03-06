const dateFormat = 'YYYY/MM/DD';
const datePickerFormat = 'yyyy/mm/dd';

Vue.component('EditProductModalForm', {
	template: `<div>
			<Modal :modalProps="modalProductProps" @newValue="emitValueProduct" :refreshInputs="refreshInputs" :inputsKey="modalKey"/>
	</div>`,
	props: {
		editable: true
	},
	data() {
		return {
			modalKey: 0,
			modalIndustryKey: 0,
			refreshInputs: 0,
			waitAddressButton: false,
			modalValues: {},
			categoryListSelect: [],
			productManufactoriesListSelect: [],
			CurrencyListSelect: [],
		};
	},
	computed: {
		modalProductProps() {
			return {
				id: 'editProductModal',
				label: 'Edit product',
				formInputs: [
					{
						name: 'Input',
						props: {
							id: 'entity-name',
							type: 'text',
							label: 'Name',
							required: true,
							className: 'col-12 col-md-6',
							value: this.modalValues.name,
						}
					},
					{
						name: 'Select',
						props: {
							id: 'entity-productManufactoriesId',
							label: 'Manufacturies',
							required: true,
							searchable: true,
							options: this.productManufactoriesListSelect,
							value: this.modalValues.productManufactoriesId,
							className: 'col-12 col-md-6'
						}
					},
					{
						name: 'Select',
						props: {
							id: 'entity-type',
							label: 'Product type',
							value: this.modalValues.type,
							required: true,
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
							className: 'col-12 col-md-6'
						}
					},
					{
						name: 'Select',
						props: {
							id: 'entity-categoryId',
							label: 'Category',
							required: true,
							searchable: true,
							options: this.categoryListSelect,
							value: this.modalValues.categoryId,
							className: 'col-12 col-md-6'
						}
					},
					{
						name: 'ShortTextBlock',
						props: {
							value: 'Can be sold:',
							className: 'col-3 ml-2'
						}
					},
					{
						name: 'Radio',
						props: {
							id: 'entity-canBeSold',
							label: '',
							required: true,
							className: 'row ml-1',
							options: [{ value: true, label: 'Yes' }, { value: false, label: 'No' }],
							value: this.modalValues.canBeSold,
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
							id: 'entity-ean',
							type: 'text',
							label: 'EAN',
							required: true,
							className: 'col-12 col-md-6',
							value: this.modalValues.ean,
							disabled: this.editableModal ? true : false
						}
					},
					{
						name: 'Input',
						props: {
							id: 'entity-warranty',
							type: 'text',
							label: 'Warranty',
							className: 'col-12 col-md-6',
							value: this.modalValues.warranty,
						}
					},
					{
						name: 'Input',
						props: {
							id: 'entity-sellingPrice',
							type: 'decimal',
							required: true,
							label: 'SellingPrice',
							className: 'col-12 col-md-6',
							value: this.modalValues.sellingPrice,
						}
					},
					{
						name: 'Select',
						props: {
							id: 'entity-currencyCode',
							label: 'Currency',
							options: this.CurrencyListSelect,
							disabled: false,
							size: 10,
							searchable: true,
							className: 'col-12 col-md-6',
							value: this.modalValues.currencyCode
						}
					},
					{
						name: 'Textarea',
						props: {
							id: 'entity-description',
							label: 'Description',
							required: true,
							className: 'col-12',
							value: this.modalValues.description,
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
								$(`#${this.modalProductProps.id}`).modal("hide");
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
							$(`#${this.modalProductProps.id}`).modal("hide");
					}
				)}
			}
		},
	},
	created: async function () {
		customAjaxRequest(apiEndpoints.Category.GetAllCategories).then(result => {
			this.categoryListSelect = this.convertToSelectList(result);
		});
		customAjaxRequest(apiEndpoints.Manufactory.GetAllManufactories).then(result => {
			this.productManufactoriesListSelect = this.convertToSelectList(result);
		});
		customAjaxRequest(apiEndpoints.CrmCommon.GetAllCurrencies).then(result => {
			this.CurrencyListSelect = this.convertCurrenciesToSelectList(result);
		});
		this.resetModalValues();
	},
	methods: {
		async updateEntity() {
			this.waitAddButton = true;
			return new Promise((resolve, reject) => {
				customAjaxRequest(apiEndpoints.Product.UpdateProduct, 'POST', this.modalValues).then(() => {
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
				customAjaxRequest(apiEndpoints.Product.GetProductById, 'GET', { productId: entityId }).then(result => {
					resolve(result);
				}).catch(e => {
					toast.notifyErrorList(e);
				});
			});
		},
		resetModalValues() {
			this.modalValues = {
				name: null,
				type: null,
				categoryId: null,
				description: null,
				ean: null,
				warranty: null,
				canBeSold: null,
				productManufactoriesId: null,
				sellingPrice: null,
				currencyCode: null,
			}
		},
		editProduct(entityId) {
			this.loadEntity(entityId).then(result => { true;
				this.modalValues = result;
				this.modalKey++;
				this.refreshInputs++;
				$(`#${this.modalProductProps.id}`).modal("show");
			});
		},
		emitValueProduct(val) {
			const newVal = val.value === 'true' ? true : val.value === 'false' ? false : val.value;
			this.modalValues[val.id.replace('entity-', '')] = newVal;
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
		convertCurrenciesToSelectList(array) {
			return array.map(e => {
				const newObj = {
					label: `${e.name}(${e.symbol})`,
					value: e.code
				}
				return newObj;
			});
		},
	}
});
