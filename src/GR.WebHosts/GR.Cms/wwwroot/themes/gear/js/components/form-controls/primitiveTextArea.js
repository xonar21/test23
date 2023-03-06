Vue.component('PrimitiveTextArea', {
	template: `<div :class="className">
		<div class="form-group"
			 :class="formGroupClassName">
			<label
				v-show="hasLabel"
				:for="name">
				{{ label }}
				<span class="required-input-label" v-if="required">*</span>
			</label>
			<div class="d-flex flex-column">
				<textarea
					class="form-control"
					:class="textareaClassName"
					:id="id"
					:placeholder="placeholder"
					:required="required"
					:disabled="disabled"
					:style="isResizableOnType && textareaStyle"
					@blur="removeValid"
					v-model="newValue">
				</textarea>
				<textarea
					v-if="isResizableOnType"
					v-model="newValue"
					ref="textareaShadow"
					tabindex="0"
					class="textarea_shadow">
				</textarea>
			</div>
			<div class="invalid-feedback" v-if="error.length > 0">{{ error }}</div>
		</div>
    </div>`,
	data() {
		return {
			error: "",
			isValid: "",
			inputHeight: "0"
		};
	},
	props: {
		id: { type: String, required: true },
		className: String,
		textareaClassName: { type: String, default: "form-control_default" },
		name: String,
		formGroupClassName: String,
		label: String,
		hasLabel: { type: Boolean, default: true },
		disabled: Boolean,
		required: { type: Boolean, default: false },
		placeholder: String,
		value: [String, Number],
		isResizableOnType: { type: Boolean, default: false },
		initMinHeight: String
	},
	created() {
		this.inputHeight = this.initMinHeight;
	},
	methods: {
		resize() {
			const scrollHeight = this.$refs.textareaShadow.scrollHeight;
			this.inputHeight = scrollHeight > 42 ? `${this.$refs.textareaShadow.scrollHeight + 18}px` : 42
		},
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
	computed: {
		newValue: {
			get() {
				return this.value;
			},
			set(val) {
				if (this.isResizableOnType) {
					this.resize();
				}
				this.$emit("newValue", { value: val, id: this.id });
			}
		},
		textareaStyle() {
			return {
				'min-height': this.inputHeight
			}
		}
	},
});