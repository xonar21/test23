Vue.component('Link', {
	template: `<a :href="href"
				  :class="className"
				  v-if="display">
				{{ text }}
			   </a>`,
	props: {
		href: String,
		display: { type: Boolean, default: true },
		className: String,
		text: String
	},
	computed: {

	},
	methods: {

	}
});

