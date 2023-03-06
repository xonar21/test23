Vue.component('CheckBox', {
	template: `<div :class="className">
		<div class="custom-control custom-checkbox" v-for="item in options" :key="item.value">
			<input type="checkbox" class="custom-control-input"
			:name="'checkbox-' + id" :id="id + '-' + item.value" :value="item.value" v-model="inputValue"
			@change="onChange" :required="required"/>
			<label class="custom-control-label" :for="id + '-' + item.value">{{ item.label }}</label>
		</div>
	</div>`,
	props: {
		id: { type: String, required: true },
		className: String,
		label: String,
		required: { type: Boolean, default: false },
		options: Array,
		value: [String, Boolean]
	},
	data() { return { inputValue: [] } },
	methods: {
		onChange() {
			this.$emit("newValue", { value: this.inputValue, id: this.id });
		}
	}

});