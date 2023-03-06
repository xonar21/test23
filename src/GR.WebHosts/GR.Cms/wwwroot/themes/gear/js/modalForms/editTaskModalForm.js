const dateFormatEditTask = 'YYYY/MM/DD';
const datePickerFormatEditTask = 'yyyy/mm/dd';

Vue.component('EditTaskModalForm', {
	template: `<div>
			<Modal :modalProps="editTaskModalProps" @newValue="emitValueTask"  :refreshInputs="refreshInputs" :inputsKey="modalKey"/>
	</div>`,
	props: {
		editable: true
	},
	data() {
		return {
			addAndNew: false,
			orgSelect: [],
			taskValues: {},
			editOrgId: null,
			pageRequestFilters: [],
			modalKey: 0,
			refreshInputs: 0,
			waitAddressButton: false,
			statuses: [],
			priorities: [],
			taskPriorities: [],
			taskStatuses: [],
			usersSelectList: [],
			listLeadsSelect: [],
			taskTypes: []
		};
	},
	computed: {
		modalTaskSubmits() {
			const resultArray = [
				{
					name: 'Button',
					props: {
						label: 'Cancel',
						btnType: 'outline-secondary',
						onClick: () => {
							$(`#${this.editTaskModalProps.id}`).modal("hide");
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
		modalTaskLabel() {
			return "Edit task";
		},
		editTaskModalProps() {
			return {
				id: 'editTaskModal',
				label: this.modalTaskLabel,
				formInputs: [
					{
						name: 'Input',
						props: {
							id: 'editTask-name',
							type: 'text',
							label: 'Name',
							required: true,
							value: this.taskValues.name,
							className: 'col-12',
							validator: value => fieldValidationFunc(value, 'name'),
							validatorInput: value => fieldValidationInputFunc(value, 'name')
						}
					},
					{
						name: 'Select',
						props: {
							id: 'editTask-organizationId',
							label: 'Organization',
							options: this.orgSelect,
							value: this.taskValues.organizationId,
							size: 10,
							searchable: true,
							className: 'col-12'
						}
					},
					{
						name: 'Select',
						props: {
							id: 'editTask-leadId',
							label: 'Lead',
							options: this.listLeadsSelect,
							value: this.taskValues.leadId,
							size: 10,
							searchable: true,
							className: 'col-12'
						}
					},
					{
						name: 'Datepicker',
						props: {
							id: 'editTask-startDate',
							label: 'Start date',
							required: true,
							format: datePickerFormatEditTask,
							value: this.taskValues.startDate,
							className: 'col-12 col-md-6'
						}
					},
					{
						name: 'Datepicker',
						props: {
							id: 'editTask-endDate',
							label: 'End date',
							required: true,
							format: datePickerFormatEditTask,
							value: this.taskValues.endDate,
							className: 'col-12 col-md-6'
						}
					},
					{
						name: 'Select',
						props: {
							id: 'editTask-status',
							label: 'Status',
							options: this.taskStatuses,
							value: this.taskValues.status,
							className: 'col-12 col-md-6'
						}
					},
					{
						name: 'Select',
						props: {
							id: 'editTask-taskPriority',
							label: 'Priority',
							options: this.taskPriorities,
							value: this.taskValues.taskPriority,
							className: 'col-12 col-md-6'
						}
					},
					{
						name: 'Select',
						props: {
							id: 'task-taskTypeId',
							label: 'Task type',
							options: this.taskTypes,
							value: this.taskValues.taskTypeId,
							className: 'col-12'
						}
					},
					{
						name: 'Select',
						props: {
							id: 'editTask-userTeam',
							label: 'Assigne',
							searchable: true,
							required: true,
							multiple: true,
							options: this.usersSelectList,
							value: this.taskValues.userTeam,
							className: 'col-12'
						}
					},
					{
						name: 'Textarea',
						props: {
							id: 'editTask-description',
							label: 'Description',
							required: true,
							value: this.taskValues.description,
							className: 'col-12'
						}
					}
				],
				formSubmits: this.modalTaskSubmits,
				onSubmit: () => {
						this.updateTask().then(() => {
							$(`#${this.editTaskModalProps.id}`).modal("hide");
						});
				}
			}
		}
	},
	created: async function () {
		this.resetTaskModalValues();
		this.refreshInputs++;
		const promises = [
			customAjaxRequest(apiEndpoints.TaskManager.GetTaskPriorityList),
			customAjaxRequest(apiEndpoints.TaskManager.GetTaskStatusList),
			customAjaxRequest(apiEndpoints.Users.GetUsers),
			customAjaxRequest(apiEndpoints.Leads.GetAllLeads),
			customAjaxRequest(apiEndpoints.Organization.GetAllOrganization),
			customAjaxRequest(apiEndpoints.TaskType.GetAllTaskType)
		];
		Promise.all(promises).then(result => {
			this.taskPriorities = this.convertArrayToSelectList(result[0], 'text', 'value', 'system_taskmanager_');
			this.priorities = result[0];
			this.taskStatuses = this.convertArrayToSelectList(result[1], 'text', 'value', 'system_taskmanager_');
			this.statuses = result[1];
			this.usersSelectList = this.convertUsersToSelectList(result[2]);
			this.allLeadsSelect = this.convertArrayToSelectList(result[3], 'name', 'id');
			this.listLeadsSelect = this.allLeadsSelect;
			this.orgSelect = this.convertToSelectList(result[4]);
			this.taskTypes = this.convertArrayToSelectList(result[5], 'name', 'id');
		});
	},
	methods: {
		async updateTask() {
			this.waitAddButton = true;
			return new Promise((resolve, reject) => {
				customAjaxRequest(apiEndpoints.TaskManager.UpdateTask, 'POST', this.taskValues).then(() => {
					this.waitAddButton = false;
					this.tableKey++;
					this.$emit('action');
					resolve(true);
				}).catch(e => {
					this.waitAddButton = false;
					toast.notifyErrorList(e);
					reject(false);
				});
			});
		},
		async loadTask(id) {
			return new Promise((resolve, reject) => {
				customAjaxRequest(apiEndpoints.TaskManager.GetTask, 'GET', { id }).then(result => {
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
		resetTaskModalValues() {
			this.taskValues = {
				name: '',
				description: '',
				startDate: moment().format(dateFormatEditTask).toString(),
				endDate: moment().add(1, 'h').format(dateFormatEditTask).toString(),
				leadId: null,
				organizationId: null,
				userTeam: [],
				taskPriority: '0',
				status: '0',
				taskTypeId: null,
			}

			this.listLeadsSelect = this.allLeadsSelect;
		},
		editTask(taskId) {
			this.waitAddButton = false;
			this.loadTask(taskId).then(result => {
				this.taskValues = {
					id: result.id,
					name: result.name,
					description: result.description,
					startDate: moment(result.startDate, 'DD.MM.YYYY').format(dateFormatEditTask),
					endDate: moment(result.endDate, 'DD.MM.YYYY').format(dateFormatEditTask),
					userTeam: result.userTeam,
					organizationId: result.organizationId,
					leadId: result.leadId,
					taskPriority: result.taskPriority,
					status: result.status,
					taskTypeId: result.taskTypeId
				};
				if (this.taskValues.leadId)
					this.taskValues.organizationId = null;
				this.modalKey++;
				this.refreshInputs++;
				$(`#${this.editTaskModalProps.id}`).modal("show");
			});
		},
		emitValueTask(val) {
			const newVal = val.value === 'true' ? true : val.value === 'false' ? false : val.value;
			this.taskValues[val.id.replace('editTask-', '')] = newVal;
			if (val.id == 'editTask-organizationId') {
				this.listLeadsSelect = [];
				this.taskValues.leadId = null;
				customAjaxRequest(apiEndpoints.Leads.GetLeadsByOrganizationId, 'GET', { organizationId: val.value }).then(result => {
					this.listLeadsSelect = this.convertArrayToSelectList(result, 'name', 'id');
				});
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
					value: e[valueProp]
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
		getPriorityBadge(val) {
			let badgeClass = 'badge-outline-default';
			switch (val.value) {
				case '0':
					badgeClass = 'badge-outline-info';
					break;
				case '1':
					badgeClass = 'badge-outline-primary';
					break;
				case '2':
					badgeClass = 'badge-outline-warning';
					break;
				case '3':
					badgeClass = 'badge-outline-danger';
					break;
			}
			return `<span class="badge ${badgeClass}">${val.label}</span>`;
		},
	}
});