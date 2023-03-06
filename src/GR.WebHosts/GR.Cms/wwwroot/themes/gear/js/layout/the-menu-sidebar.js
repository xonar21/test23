Vue.component('TheMenuSidebar', {
	template: `
		<div class="left-sidebar"
			:class="{ open: isSidebarOpen }"
			v-click-outside="closeModal">
			<div class="left-sidebar-content">
				<div class="btn-new-place d-none pl-4 align-items-center">
					<transition name="fade">
						<button class="btn btn-success btn-success align-items-center py-1 px-2" type="button" v-show="$store.state.sidebarOpen">
							<i data-feather="plus"></i>
							<span class="btn-label">New</span>
						</button>
					</transition>
				</div>
				<Menu />
			</div>
		</div>
	`,
	mounted() {
		feather.replace({
			width: 15,
			height: 15
		});
	},
	computed: {
		isSidebarOpen() {
			return this.$store.state.sidebarOpen;
		},
	},
	methods: {
		closeModal() {
			this.$store.dispatch("closeSidebarAction");
		},
	}
});