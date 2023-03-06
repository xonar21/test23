Vue.component('Input', {
	template: `<div :class="className">
		<div class="form-group">
			<label
				:for="name"
				:class="classNames"
				>{{ label }}<span class="required-input-label" v-if="required">*</span></label
			>
			<div class="d-flex align-items-center">
				<div class="input-prefix" v-if="inputPrefix" v-html="inputPrefix">
				</div>
				<input
					:type="type"
					v-set-value="value"
					class="form-control"
					:step="step"
					:class="validClass"
					:id="id"
					:placeholder="placeholder"
					:required="required"
					:disabled="disabled"
					@blur="removeValid"
					v-model="inputValue"
				/>
				<div class="input-suffix" :class="suffixClass" v-html="inputSuffix" v-if="inputSuffix">
				</div>
			</div>
			<div class="invalid-feedback" v-if="error.length > 0">{{ error }}</div>
		</div>
    </div>`,
	data() {
		return {
			error: "",
			isValid: "",
			inputValue: '',
		};
	},
	props: {
		id: { type: String, required: true },
		className: String,
		name: String,
		suffixClass: String,
		step: String,
		type: {
			type: String,
			required: true,
			validator: function (value) {
				return (
					[
						"text",
						"password",
						"email",
						"search",
						"tel",
						"text",
						"number",
						"url"
					].indexOf(value) !== -1
				);
			}
		},
		label: String,
		disabled: Boolean,
		validator: Function,
		validatorInput: Function,
		required: { type: Boolean, default: false },
		placeholder: String,
		value: [String, Number],
		refresh: Number,
		inputPrefix: String,
		inputSuffix: String
	},
	computed: {
		validClass: {
			get() {
				let className = this.inputPrefix ? 'with-prefix ' : '';
				className += this.inputSuffix ? 'with-suffix ' : '';
				className += this.isValid == "valid"
					? "is-valid"
					: this.isValid == "invalid"
						? "is-invalid"
						: "";
				return className;
			}
		},
		classNames() {
			const c = this.disabled ? 'disabled ' : this.validClass;
			return c + ' float-label';
		}
	},
	methods: {
		removeValid() {
			if (this.isValid != "invalid") {
				this.resetValidity();
			}
		},
		resetValidity() {
			this.isValid = '';
			this.error = '';
		},
	},
	watch: {
		refresh: function () {
			this.resetValidity();
		},
		inputValue: function (newVal, oldVal) {
			if (typeof this.validatorInput == "function" && newVal) {
				const validatorInputResponse = this.validatorInput(newVal);
				if (validatorInputResponse === false) {
					this.inputValue = oldVal;
				}
			}
			if (typeof this.validator == "function" && this.inputValue) {
				const validatorResponse = this.validator(this.inputValue);
				if (validatorResponse == true) {
					this.isValid = "valid";
					this.error = "";
				} else {
					this.isValid = "invalid";
					this.error = validatorResponse;
				}
			} else {
				this.removeValid();
			}
			if (newVal && newVal.length == 0) {
				this.removeValid();
			}
			this.$emit("newValue", { value: this.inputValue, id: this.id });
		}
	}
});