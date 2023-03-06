Vue.component('AddHideInput', {
	template: `
		<div :id="id" class="col-12 form-componet" :class="className">
			<div class="row pl-3">
				<a><span class="icon-plus" @click="addInput" v-bind:style="{display: hidePlus ? 'none' : 'block'}"></span></a>
				<div class="n-a-container" v-bind:style="{display: hideLabel ? 'flex' : 'none'}">
					<input type="checkbox" :id="createId" name="'N/ACheck' + id"
						:value="value"
						v-model="inputCheck">
					<label class="n-a-label" for="'N/ACheck' + id">N/A</label><br>
				</div>
				<a class="ml-auto mr-3"><span class="icon-delete ml-auto" @click="deleteInput" v-bind:style="{display: hideDelete ? 'none' : 'block'}"></span></a>
			</div>	
		</div>`,
	data() {
		return {
			inputCheck: false
		}
	},
	computed: {
		createId() {
			return this.id + 'N/ACheck';
		}
	},
	props: {
		id: { type: String, required: true },
		className: String,
		value: {
			type: Boolean,
			default: false
		},
		hidePlus: {
			type: Boolean,
			default: false
		},
		hideDelete: {
			type: Boolean,
			default: false
		},
		hideLabel: {
			type: Boolean,
			default: true
		}
	},
	mounted() {
		this.inputCheck = this.value;
	},
	watch: {
		inputCheck() {
			var checkBox = document.getElementById(this.createId);
			if (this.inputCheck) {
				this.$emit('newValue', { value: { class: 'hide-field', checked: this.inputCheck }, id: 'N/ALabel' })
			}
			else this.$emit('newValue', { value: { class: '', checked: this.inputCheck }, id: 'N/ALabel' });
		}
	},
	methods: {
		addInput() {
			this.$emit('newValue', { value: '', id: this.id });
		},
		deleteInput() {
			this.$emit('newValue', { value: ' hide-field', id: this.id })
		},
	}
})