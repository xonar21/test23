Vue.component('Comment', {
	template: `<div>
			<div class="d-flex"
					data-toggle="collapse"
					:data-target="'#' + comment.id"
					aria-expanded="false"
					:aria-controls="comment.id">
			<a class="btn btn-outline-primary user-rectangle audit__user-img"
				href="#"
				:title="comment.author">
				{{ comment.author.charAt(0) }}
			</a>
			<div>
				<h5 class="mb-0 line-height-12">{{ comment.author }}</h5>
				<span class="font-size-12">{{ comment.changed }}</span>
			</div>
			<div class="ml-auto">
				<svg v-if="comment.commentReply?.length"
					 xmlns="http://www.w3.org/2000/svg"
					 width="17"
					 height="17"
					 viewBox="0 0 24 24"
					 fill="none"
					 stroke="currentColor"
					 stroke-width="2"
					 stroke-linecap="round"
				     stroke-linejoin="round"
					 class="feather feather-chevron-down ml-auto comment__chevron">
					<polyline points="6 9 12 15 18 9"></polyline>
				</svg>
				<a href="#"
					data-toggle="dropdown"
					class="dotted-link comment__dotted-link"
					v-if="comment.author === currentUserName">
					<svg xmlns="http://www.w3.org/2000/svg"
						 width="15"
						 height="15"
						 viewBox="0 0 24 24"
						 fill="none"
						 stroke="currentColor"
						 stroke-width="2"
						 stroke-linecap="round"
						 stroke-linejoin="round"
						 class="feather feather-more-vertical">
						<circle cx="12" cy="12" r="1"></circle>
						<circle cx="12" cy="5" r="1"></circle>
						<circle cx="12" cy="19" r="1"></circle>
					</svg>
				</a>
				<div class="dropdown-menu">
					<a class="dropdown-item"
					   href="#"
					   @click="emitResetEditModal(comment.id)">
						Edit
					</a>
					<a class="dropdown-item"
					   href="#"
					   @click="emitResetReplyModal({ author: comment.author, commentId: comment.id })">
						Reply
					</a>
					<div class="dropdown-divider"></div>
					<a class="dropdown-item"
					   href="#"
					   @click="emitResetDeleteModal({ author: comment.author, commentId: comment.id })">
						Delete
					</a>
				</div>
			</div>
		</div>
		<p class="mb-0 font-size-14 word-wrap">{{ assignedUsers }}</p>
		<p class="color-black mb-0 font-size-14 word-wrap">
			{{ comment.message }}
		</p>
		<hr class="hr-description hr_audit" />
	</div>
	`,
	props: {
		comment: Object,
		currentUserName: String
	},
	methods: {
		emitResetEditModal(id) {
			this.$emit('resetEditModal', id);
		},
		emitResetReplyModal({ author, commentId }) {
			this.$emit('resetReplyModal', { author, commentId });
		},
		emitResetDeleteModal({ author, commentId }) {
			this.$emit('resetDeleteModal', { author, commentId });
		}
	},
	computed: {
		assignedUsers() {
			return this.comment.assignedUsers ? this.comment.assignedUsers.map(u => u.userEmail).join(", ") : '';
		}
	}
});