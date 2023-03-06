const dateFormat = 'YYYY/MM/DD';
const datePickerFormat = 'yyyy/mm/dd';

Vue.component('EditCampaignModalForm', {
	template: `<div>
			<Modal :modalProps="modalCampaignProps" @newValue="emitValueCampaign" :refreshInputs="refreshInputs" :inputsKey="modalKey"/>
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
			campaignTypeListSelect: [],
			allMarketingLists: [],
			campaignList: [],
			campaignValues: {
				name: '',
				campaignCost: '',
				campaignBudget: '',
				startDate: moment().format(dateFormat).toString(),
				endDate: moment().add(5, 'd').format(dateFormat).toString(),
				description: '',
				marketingLists: []
			},

		};
	},
	computed: {
		campaignFormSubmits() {
			const resultArray = [
				{
					name: 'Button',
					props: {
						label: 'Cancel',
						btnType: 'outline-secondary',
						onClick: () => {
							this.resetCampaignModalValues;
							$(`#${this.modalCampaignProps.id}`).modal("hide");
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
		modalCampaignProps() {
			return {
				id: 'campaignModal',
				modalSize: 'lg',
				label: 'Edit Campaign',
				formInputs: [
					{
						name: 'Input',
						props: {
							id: 'campaign-name',
							type: 'text',
							label: 'Name',
							required: true,
							className: 'col-12',
							value: this.campaignValues.name,
							validator: value => fieldValidationFunc(value, 'varChar128'),
							validatorInput: value => fieldValidationInputFunc(value, 'varChar128')
						}
					},
					{
						name: 'Input',
						props: {
							id: 'campaign-campaignCost',
							type: 'text',
							label: 'Cost',
							required: true,
							className: 'col-12 col-md-6',
							className: 'col-12 col-md-6',
							value: this.campaignValues.campaignCost
						}
					},
					{
						name: 'Input',
						props: {
							id: 'campaign-campaignBudget',
							type: 'text',
							label: 'Budget',
							required: true,
							value: this.campaignValues.campaignBudget,
							className: 'col-12 col-md-6'
						}
					},
					{
						name: 'Datepicker',
						props: {
							id: 'campaign-startDate',
							label: 'Start date',
							required: true,
							className: 'col-12 col-md-6',
							format: datePickerFormat,
							value: this.campaignValues.startDate
						}
					},
					{
						name: 'Datepicker',
						props: {
							id: 'campaign-endDate',
							label: 'End date',
							required: true,
							className: 'col-12 col-md-6',
							format: datePickerFormat,
							value: this.campaignValues.endDate
						}
					},
					{
						name: 'Select',
						props: {
							id: 'campaign-campaignTypeId',
							label: 'Campaign Type',
							options: this.campaignTypeListSelect,
							value: this.campaignValues.campaignTypeId,
							className: 'col-12'
						}
					},
					{
						name: 'Select',
						props: {
							id: 'campaign-marketingLists',
							label: 'Members',
							options: this.allMarketingLists,
							multiple: true,
							size: 10,
							searchable: true,
							value: this.campaignValues.marketingLists,
							className: 'col-12'
						}
					},
					{
						name: 'Textarea',
						props: {
							id: 'campaign-description',
							label: 'Description',
							className: 'col-12',
							value: this.campaignValues.description,
						}
					}
				],
				formSubmits: this.campaignFormSubmits,
				onSubmit: () => {
					this.updateCampaign().then(() => {
						$(`#${this.modalCampaignProps.id}`).modal("hide");
					});

				}
			}
		},
	},
	created: async function () {
		this.resetCampaignModalValues();
		customAjaxRequest(apiEndpoints.CrmVocabularies.GetAllCampaignType).then(result => {
			this.campaignTypeListSelect = this.convertToSelectList(result);
		});
		customAjaxRequest(apiEndpoints.MarketingList.GetAllMarketingLists).then(result => {
			this.allMarketingLists = this.convertToSelectList(result);
		});
		customAjaxRequest(apiEndpoints.Campaign.GetAllCampaigns).then(result => {
			this.campaignList = result;
		});
	},
	methods: {
		async updateCampaign() {
			this.waitAddButton = true;
			return new Promise((resolve, reject) => {
				const { marketingLists, ...newObj } = this.campaignValues;
				let newList = [];
				if (marketingLists.length > 0) {
					marketingLists.forEach(marketingListId => {
						newList.push({
							marketingListId
						});
					});
				}
				newObj.marketingLists = newList;
				customAjaxRequest(apiEndpoints.Campaign.UpdateCampaign, 'POST', newObj).then(() => {
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
		async loadCampaign(campaignId) {
			return new Promise((resolve, reject) => {
				customAjaxRequest(apiEndpoints.Campaign.GetCampaignById, 'GET', { campaignId }).then(result => {
					resolve(result);
				}).catch(e => {
					toast.notifyErrorList(e);
					reject(e);
				});
			});
		},
		resetCampaignModalValues() {
			this.campaignValues = {
				name: '',
				campaignCost: '',
				campaignBudget: '',
				startDate: moment().format(dateFormat).toString(),
				endDate: moment().add(1, 'd').format(dateFormat).toString(),
				description: '',
			}
			this.refreshInputs++;
		},
		editCampaign(campaignId) {
			this.loadCampaign(campaignId).then(result => {
				this.campaignValues = result;
				let newList = [];
				result.marketingLists.forEach(x => {
					newList.push(x.marketingListId);
				});
				this.campaignValues.marketingLists = newList;
				this.campaignValues.startDate = moment(result.startDate, 'DD.MM.YYYY').format(dateFormat);
				this.campaignValues.endDate = moment(result.endDate, 'DD.MM.YYYY').format(dateFormat);
				this.modalKey++;
				this.refreshInputs++;
				$(`#${this.modalCampaignProps.id}`).modal("show");
			});
		},
		emitValueCampaign(val) {
			const newVal = val.value === 'true' ? true : val.value === 'false' ? false : val.value;
			this.campaignValues[val.id.replace('campaign-', '')] = newVal;
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
	}
});
