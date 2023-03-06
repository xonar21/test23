Vue.component('PhoneInput', {
	template: `<div :class="className">
					<div class="form-group">
						<label :for="name">
							{{ label }} <span class="required-input-label" v-if="required">*</span>
						</label>
					<vue-tel-input
						v-model="number"
						:placeholder="placeholder"
						:required="required"
						:validCharactersOnly="validCharactersOnly"
						:enabledCountryCode="enabledCountryCode"
						:preferredCountries="preferredCountries"
						:inputClasses="inputClasses"
						:inputId="id"
						:disabled="disabled"
						:disabledFetchingCountry="disabled"
						:dynamicPlaceholder="dynamicPlaceholder"
						:maxLen="maxLength"
						:defaultCountry="defaultCountry"
						:wrapperClasses="wrapClass"
						@input="inputChange"
						@validate="inputIsValid"
						@country-changed="countryChanged"></vue-tel-input>
					</div>
				</div>`,
	data() {
		return {
			number: '',
			wrapClass: '',
			counter: 0,
			maxLength: 15,
			phoneDetails: {
				number: '',
				countryCode: '',
				dialCode: ''
			},
			bindProps: {
				autocomplete: "off",
			}
		}
	},
	mounted() {
		this.wrapClass = this.wrapperClasses;
		this.maxLength = this.maxLen;
		if(this.value != '')
			this.number += this.value + ' ';
		this.phoneDetails.number += this.value;
		this.phoneDetails.countryCode = this.defaultCountry;
		this.phoneDetails.dialCode = this.dialCode;
		this.$emit("newValue", { value: this.phoneDetails, id: this.id });
	},
	props: {
		className: String,
		label: String,
		name: String,
		dialCode: String,
		value: {
			type: String,
			default: ''
		},
		mode: {
			type: String,
			default: ''
		},
		defaultCountry: {
			type: String,
			default: ''
		},
		disabledFetchingCountry: {
			type: Boolean,
			default: true
		},
		disabled: {
			type: Boolean,
			default: false
		},
		disabledFormatting: {
			type: Boolean,
			default: false
		},
		placeholder: {
			type: String,
			default: 'Set phone number'
		},
		required: {
			type: Boolean,
			default: false
		},
		validCharactersOnly: {
			type: Boolean,
			default: true
		},
		enabledCountryCode: {
			type: Boolean,
			default: true
		},
		enabledFlags: {
			type: Boolean,
			default: true
		},
		preferredCountries: {
			type: Array,
			default: function () {
				return ['MD', 'RO', 'US', 'RU'];
			}
		},
		onlyCountries: {
			type: Array,
			default: function () {
				return [];
			}
		},
		ignoredCountries: {
			type: Array,
			default: function () {
				return [];
			}
		},
		autofocus: {
			type: Boolean,
			default: false
		},
		name: {
			type: String,
			default: 'telephone'
		},
		maxLen: {
			type: Number,
			default: 8
		},
		wrapperClasses: {
			type: String, Array, Object,
			default: function () {
				return '';
			}
		},
		customValidate: {
			type: Boolean,
			default: true
		},
		inputClasses: {
			type: String, Array, Object,
			default: function () {
				return 'form-control';
			}
		},
		id: { type: String, required: true },
		inputOptions: {
			type: Object,
			default: function () {
				return { showDialCode: false, tabindex: 0 };
			}
		},
		dropdownOptions: {
			type: Object,
			default: function () {
				return { showDialCode: false, tabindex: 0 };
			}
		},
		dynamicPlaceholder: {
			type: Boolean,
			default: false
		},
	},
	watch: {
		number: function (val, old) {
			this.$emit("newValue", { value: this.phoneDetails, id: this.id });
		},
		dialCode: function (val, old) {
			this.phoneDetails.dialCode = val;
		},
		value: function (val, old) {
			if (val === '') this.wrapClass = '';
			this.number = val;
			this.phoneDetails.number = val;
		},
		defaultCountry: function (val, old) {
			this.phoneDetails.countryCode = val;
		}
	},
	methods: {
		inputChange(val) {

			this.phoneDetails.number = val;
			if (val != '') {
				if (this.wrapClass === '' && this.phoneDetails.countryCode === "MD")
					this.wrapClass = 'vue-tel-input-wrapper-invalid';
				if (this.phoneDetails.countryCode === "MD" && val.length === 8)
					this.wrapClass = 'vue-tel-input-wrapper-valid';
			} else this.wrapClass = '';
		},
		inputIsValid(val) {
			if (val.isValid) {
				this.wrapClass = 'vue-tel-input-wrapper-valid';
				this.number = val.number.significant;
				this.phoneDetails.number = this.number;
				this.$emit("newValue", { value: this.phoneDetails, id: this.id });
				this.maxLength = this.number.length;
			}
			else {
				if (this.number != '' && val.regionCode === "MD") {
					if (val.number.significant.length == 8) {
						this.wrapClass = 'vue-tel-input-wrapper-valid';
						this.number = val.number.significant;
						this.phoneDetails.number = this.number;
						this.$emit("newValue", { value: this.phoneDetails, id: this.id });
						this.maxLength = this.number.length;
					}
					else {
						this.wrapClass = 'vue-tel-input-wrapper-invalid';
					}
				}
			}
		},
		countryChanged(val) {
			if (this.number != '' && this.counter > 0) this.number = '';
			this.counter++;
			this.phoneDetails.countryCode = val.iso2;
			this.phoneDetails.dialCode = val.dialCode;
			this.phoneDetails.number = this.number;
			this.wrapClass = '';
			if (val.iso2 === 'MD')
				this.maxLength = 8;
			else this.maxLength = 15;
		}
	}
});