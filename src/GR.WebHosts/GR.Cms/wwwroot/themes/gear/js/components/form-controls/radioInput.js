Vue.component('Radio', {
	template: `<div :class="className">
			<div class="custom-control custom-radio" v-for="item in options" :key="item.value">
				<input type="radio" class="custom-control-input ml-1"
					:name="'radio-' + id" :id="id + '-' + item.value"
					:value="item.value" v-model="inputValue" :required="required"
					@change="onChange" checked="item.value === value ? checked : ''"/>
				<label class="custom-control-label mr-2" :for="id + '-' + item.value">{{ item.label }}</label>
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
	data() { return { inputValue: this.value } },
	watch: {
		value: function () {
			this.inputValue = this.value;
		},
	},
	methods: {
		onChange() {
			this.$emit("newValue", { value: this.inputValue, id: this.id });
		}
	}
	
});
