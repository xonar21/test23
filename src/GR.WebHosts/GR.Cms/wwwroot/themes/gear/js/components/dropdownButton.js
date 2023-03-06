Vue.component('dropdownButton', {
	template: `
       <div>
			<div class="dropdown">
				<button class="btn btn-success btn-lg dropdown-toggle" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
						Add new
				</button>
				<ul class="dropdown-menu">
					<li>
						<a tabindex="-1" @click="openTaskModal" class="pl-2" href="#">Task</a>
					</li>
					<li>
						<a tabindex="-1" @click="openLeadModal" class="pl-2" href="#" >Lead</a>
					</li>
					<li>
						<a tabindex="-1" @click="openAgreementModal" class="pl-2" href="#">Agreement</a>
					</li>
					</li>
						<a tabindex="-1" class="dropdown-toggle pl-2" type="button" id="dropdownSubmenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
							Client
						</a>
					  <li class="dropdown-submenu" style="position:relative; left:100%; top:-8rem;" >
						<ul class="dropdown-menu" >
						<li>
							<a @click="openOrganizationModal" class="pl-2" href="#">Organization</a>
						</li>
						 <li>
							<a  @click="openAddressModal" class="pl-2" href="#">Address</a>
						</li>
						<li>
							<a @click="openContactModal" class="pl-2" href="#">Contact</a>
						</li>
						</ul>
					  </li>
					</ul>
			</div>

			<ContactModalForm ref="ContactModal" />
			<OrganizationModalForm ref="OrganizationModal" />
			<AddressModalForm ref="AddressModal" />
			<TaskModalForm ref="TaskModal" />
			<AgreementModalForm ref="AgreementModal" />
			<LeadModalForm ref="LeadModal" />

		</div>
`,
	props: {
		label: String,
		className: String,
		onClick: {
			type: Function,
			default: () => { }
		},
		onMouseDown: {
			type: Function,
			default: () => { }
		},
		disabled: Boolean,
		waiting: Boolean,
		btnDOMType: {
			type: String,
			default: 'button',
			validator: function (value) {
				return [
					'submit',
					'button',
					'reset'
				].indexOf(value) !== -1
			}
		},
		btnType: {
			type: String,
			validator: function (value) {
				return [
					'success',
					'warning',
					'danger',
					'info',
					'primary',
					'purple',
					'dark',
					'secondary',
					'light',
					'outline-success',
					'outline-warning',
					'outline-danger',
					'outline-info',
					'outline-primary',
					'outline-purple',
					'outline-dark',
					'outline-secondary',
					'outline-light'
				].indexOf(value) !== -1
			},
			default: 'info'
		},
		btnSize: {
			type: String,
			validator: function (value) {
				return ['lg', 'md', 'sm', ''].indexOf(value) !== -1
			},
			default: ''
		},
	},
	computed: {
		isAdmin() {
			customAjaxRequest(apiEndpoints.Roles.CurrentUserIsAdministrator, 'GET').then(result => {
				return result;
			});
		},
		buttonSize() {
			return this.btnSize !== '' ? `btn-${this.btnSize}` : '';
		},
		waitingClass() {
			return this.waiting ? 'waiting' : '';
		}
	},
	methods: {
		openOrganizationModal() {
			this.$refs['OrganizationModal'].addOrganization();
		},
		openTaskModal() {
			this.$refs['TaskModal'].addTask();
		},
		openLeadModal() {
			this.$refs['LeadModal'].addLead();
		},
		openContactModal() {
			this.$refs['ContactModal'].addContact();
		},
		openAddressModal() {
			this.$refs['AddressModal'].addAddress();
		},
		openAgreementModal() {
			this.$refs['AgreementModal'].addAgreement();
		},
	}
});