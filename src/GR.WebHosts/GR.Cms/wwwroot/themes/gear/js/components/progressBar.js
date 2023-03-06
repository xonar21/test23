Vue.component('ProgressBar', {
	template: `
		<div class="bg-white border rounded py-15 px-20 mb-2">
			<div class="stages"
					v-bind:class="{ 'stage-min-mod': stages.length <= 4 }">
				<div v-for="(stage, index) in stages"
						class="stage-main-container">
					<div class="stage-container"
							v-if="stageIndex != -1 && index < stageIndex">
						<div class="stage-status-block">
							<hr v-show="index !== 0"
								v-bind:class="['active-solid-line']" />
							<span class="stage-number-active"></span>
							<hr v-show="index + 1 !== stages.length"
								v-bind:class="['active-solid-line']" />
						</div>
						<span class="stage-inactive" v-bind:class="'stage-active'">
							{{stage.label}}
						</span>
					</div>
					<div v-else
							class="stage-container">
						<div class="stage-status-block">
							<hr v-show="index !== 0"
								v-bind:class="{ 'active-solid-line': index - 1 < stageIndex }" />
							<span v-bind:class="checkNextIndex(index)">{{index + 1}}</span>
							<hr v-show="index + 1 !== stages.length" />
						</div>
						<span class="stage-inactive">
							{{stage.label}}
						</span>
					</div>
				</div>
			</div>
		</div>
	`,
	props: {
		stages: Array,
		stageIndex: Number,
		stageId: String
	},
	methods: {
		checkNextIndex(index) {
			return this.stages[index].value == this.stageId ? 'stage-number-next' : 'stage-number';
		},
	}
});