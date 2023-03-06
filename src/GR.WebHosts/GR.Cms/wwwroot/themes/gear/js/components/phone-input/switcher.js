Vue.component('Switcher', {
	template: `<component :is="wrapper" v-if="wrapper" :class="wrapperClass">
		<label class="switch">
			<input type="checkbox" v-model="inputCheck" :id="id">
			<span class="slider round"></span>
		</label>
		<span class="font-size-14">{{ label }}</span>
	</component>
	<label class="switch" v-else>
		<input type="checkbox" v-model="inputCheck" :id="${this.id}">
		<span class="slider round"></span>
	</label>`,
	data() {
		return {
			inputCheck: false
		}
	},
	props: {
		value: {
			type: Boolean,
			default: false
		},
		id: String,
		wrapper: String,
		wrapperClass: String,
		label: String
	},
	mounted() {
		this.inputCheck = this.value;
	},
	watch: {
		inputCheck() {
			if (this.inputCheck) {
				this.$emit('newValue', { value: { class: 'hide-field', checked: this.inputCheck }, id: 'N/ALabel' })
			} else {
				this.$emit('newValue', { value: { class: '', checked: this.inputCheck }, id: 'N/ALabel' });
			}
		}
	},
});