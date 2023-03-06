Vue.component('TextBlock', {
	template: `
		<div class="w-100">
			{{ value }}
		</div>
	`,
	props: {
		value: String
	}
});
Vue.component('ShortTextBlock', {
	template: `
		<div :class="className">
			<div v-if="hidden">{{ value }} </div>
		</div>
	`,
	props: {
		value: String,
		className: String,
		hidden: {
			type: Boolean,
			default: true
		}
	}
});
