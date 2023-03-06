Vue.component('Yearpicker', {
	template: `
		<div :class="className">
			<div class="form-group" :class="{ disabled : disabled }">
				<label class="float-label" :for="id">{{ label }}<span class="required-input-label" v-if="required">*</span></label>
				<input
					type="text"
					class="form-control"
					:id="id"
					:disabled="disabled"
					v-model="newValue"
					:required="required"
				>
			</div>
		</div>
	`,
	props: {
		id: String,
		className: String,
		disabled: {
			type: Boolean,
			default: false
		},
		label: String,
		required: {
			type: Boolean,
			default: false
		},
		value: {
			type: [String, Number, Array],
			default: null
		},
		format: String,
	},
	mounted() {
		$(`#${this.id}`).datepicker({
			format: "yyyy",
			viewMode: "years",
			minViewMode: "years",
			autoclose: true
		}).on('changeDate', e => {
			this.newValue = moment(e.date).format('YYYY');
		});
	},
	computed: {
		newValue: {
			get() {
				return this.value;
			},
			set(val) {
				this.$emit("newValue", { value: val, id: this.id });
			}
		}
	},
});

