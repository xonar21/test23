Vue.component('Audit', {
	template: `<div class="bg-white border rounded py-15 px-20">
		<div class="d-flex"
				data-toggle="collapse"
				data-target="#audit"
				aria-expanded="false"
				aria-controls="audit"
				@click="makeInitialRequests">
			<h6 class="mb-0">Timeline</h6>
			<i class="ml-auto comment__chevron"
				data-feather="chevron-down">
			</i>
		</div>
		<div class="collapse" id="audit">
			<hr class="hr-description my-3" />
			<div v-if="!auditLoaded" class="group-loader">
				<Loader />
			</div>
			<template v-if="auditLoaded">
				<div v-for="(auditDetail, index) in auditDetails">
					<div v-for="(event, index) in auditDetail.auditDetailses">
						<span class="font-size-12">{{ event.changed }}</span>
						<p class="color-black mb-0 font-size-14">
							{{ event.modifiedBy }} updated <span class="font-weight-600">{{ event.propertyName }}</span> to <span class="font-weight-600">{{ isStageId(event.propertyName) ? mapStageIdToLabel(event.value) : event.value }}</span>
						</p>
						<hr class="hr-description hr_audit" />
					</div>
				</div>
				<div v-if="!moreAuditLoaded" class="group-loader">
					<Loader />
				</div>
				<button class="btn btn-sm btn-primary"
						@click="loadMoreAudit"
						v-if="isMoreData">
					Load more
				</button>
			</template>
		</div>
	</div>`,
	data() {
		return {
			auditDetails: [],
			auditLoaded: false,
			moreAuditLoaded: true,
			isMoreData: true
		}
	},
	props: {
		pageRequest: Object,
		stages: Array
	},
	methods: {
		makeInitialRequests() {
			if (!this.auditLoaded) {
				this.requestAudit().then(response => {
					this.auditDetails = response.result;
					this.auditLoaded = true;
					if (response.pageCount === this.pageRequest.page || response.pageCount === 0) {
						this.isMoreData = false;
					}
				});
			}
		},
		requestAudit() {
			return customAjaxRequest(apiEndpoints.CrmCommon.GetPaginatedAudit, 'GET', this.pageRequest);
		},
		isStageId(propName) {
			return propName === 'StageId'
		},
		mapStageIdToLabel(stageId) {
			const stage = this.stages.find(stage => stage.value === stageId);
			return stage ? stage.label : stageId;
		},
		loadMoreAudit() {
			this.$emit('incrementPage');
			this.moreAuditLoaded = false;
			this.requestAudit().then(response => {
				this.auditDetails.push(response.result);
				this.moreAuditLoaded = true;
				if (response.pageCount === this.pageRequest.page || response.pageCount === 0) {
					this.isMoreData = false;
				}
			});;
		}
	}
});