Vue.component('Dropdown', {
	template: `<div class="dropdown datatables-dropdown ml-auto">
		<div class="more-vertical" data-toggle="dropdown"></div>
		<ul class="dropdown-menu dropdown-menu-right" x-placement="bottom-end">
			<li v-for="(action, index, i) in actions"
				:key="i"
				class="position-relative pl-30px dropdown-item context-menu-icon"
				:class="'context-menu-icon-' + action.icon"
				@click.stop.prevent="emitAction({ key: index, id })"
				data-toggle="dropdown">
				<span>{{ action.name }}</span>
			</li>
		</ul>
	</div>`,
	props: {
		actions: Object,
		id: String
	},
	methods: {
		emitAction({ key, id }) {
			this.$emit('actionEmit', { key, value: [id] });
		}
	}
});