Vue.component('ModifyUserRolesModalForm', {
	template: `<Modal :modalProps="userRolesModalProps" @newValue="emitNewValue"  :refreshInputs="refreshInputs" :inputsKey="modalKey"/>`,
	data() {
		return {
			refreshInputs: 0,
			modalKey: 0,
			waitAddButton: false
		}
	},
	computed: {
		selectedRoles() {
			return this.$store.getters.requestedUserRolesForSelect;
		},
		modalValues() {
			return {
				userId: this.userId,
				roleIds: this.selectedRoles
			}
		},
		roles() {
			return this.$store.getters.allRolesForSelect;
		},
		userRolesModalProps() {
			return {
				id: 'modifyUserRoles',
				label: 'Manage Roles',
				formInputs: [
					{
						name: 'Select',
						props: {
							id: 'mofifyRoles-roleIds',
							label: 'Roles',
							options: this.roles,
							value: this.modalValues.roleIds,
							multiple: true,
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
								$(`#${this.userRolesModalProps.id}`).modal("hide");
							}
						}
					},
					{
						name: 'Button',
						props: {
							label: 'Save',
							btnType: 'success',
							btnDOMType: 'submit',
							waiting: this.waitAddButton,
						}
					},
				],
				onSubmit: () => {
					this.updateRoles().then(() => {
						$(`#${this.userRolesModalProps.id}`).modal("hide");
						this.resetModalValues();
						this.$emit("updateTableKey");
					});
				}
			}
		}
	},
	methods: {
		emitNewValue(value) {
			if (value.id === "mofifyRoles-roleIds") {
				this.$store.dispatch('modifyRoles', value.value);
			}
		},
		updateRoles() {
			this.waitAddButton = true;
			return customAjaxRequest(apiEndpoints.Users.UpdateUserRolesByUserId, 'POST', this.modalValues);
		},
		resetModalValues() {
			this.waitAddButton = false;
			this.$store.dispatch('resetRoles');
		},
		async openModal(userId) {
			this.userId = userId;
			await this.$store.dispatch('getAllRoles');
			await this.$store.dispatch('getRequestedUserRoles', userId);
			$(`#${this.userRolesModalProps.id}`).modal("show");
			this.modalKey++;
			this.refreshInputs++;
		}
	}
});