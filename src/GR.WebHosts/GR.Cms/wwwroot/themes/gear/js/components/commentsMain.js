Vue.component('CommentsMain', {
	template: `<div>
		<CommentCreate :message="model.message"
					   :disableMainPostButton="disableMainPostButton"
					   :usersListSelect="usersSelectList"
					   :userListValue="assignesUsersIdsValue"
					   @setTaggedUsers="setTaggedUsers"
					   @createComment="CreateComment"
					   @emitNewMessage="emitNewMessage" />
		<div class="bg-white border rounded py-15 px-20 mb-2">
			<div class="d-flex"
				 data-toggle="collapse"
				 data-target="#comments-container"
				 aria-expanded="false"
				 aria-controls="comments-container"
				 @click="makeInitialRequests">
				<h6 class="mb-0">Comments</h6>
				<i class="ml-auto comment__chevron"
					data-feather="chevron-down">
				</i>
			</div>
			<div class="collapse" id="comments-container">
				<hr class="hr-description my-3" />
				<div v-if="!dataLoaded" class="group-loader"><Loader /></div>
				<template v-else>
					<template v-for="(comment, index) in comments">
						<Comment :comment="comment"
									:key="index"
									:currentUserName="currentUserName"
									@resetEditModal="resetEditModal"
									@resetReplyModal="resetReplyModal"
									@resetDeleteModal="resetDeleteModal" />
						<div class="comment__subcomment collapse"
								v-if="comment.commentReply.length"
								:id="comment.id">
							<template v-for="(reply, index) in comment.commentReply">
								<Comment :comment="reply"
											:key="index"
											:currentUserName="currentUserName"
											@resetEditModal="resetEditModal"
											@resetReplyModal="resetReplyModal"
											@resetDeleteModal="resetDeleteModal" />
							</template>
						</div>
					</template>
				</template>
			</div>
		</div>

		<template v-if="dataLoaded">
			<Modal :refreshInputs="refreshInputs" :modalProps="modalDeleteProps" :inputsKey="modalKey" />
			<Modal :refreshInputs="refreshInputs" :modalProps="modalEditProps" @newValue="emitModalValue" :inputsKey="modalKey" />
		</template>
	</div>`,
	props: {
		apiUrl: String,
		entityField: String,
		entityId: String,
		usersListSelect: Array
	},
	data() {
		return {
			comments: [],
			model: {},
			disableModalPostButton: true,
			disableMainPostButton: true,
			modalKey: 0,
			refreshInputs: 0,
			deleteCommentModal: {},
			waitButton: false,
			editModal: {},
			replyModal: {},
			assignedUsers: [],
			replyToComment: false,
			scroll: false,
			dataLoaded: false,
		}
	},
	computed: {
		usersSelectList() {
			return this.$store.state.allUsers;
		},
		getCurrentUserInfo() {
			return this.$store.state.userInfo;
		},
		currentUserName() {
			return this.getCurrentUserInfo.userName;
		},
		modalDeleteProps() {
			return {
				id: 'deleteCommentModal',
				label: 'Remove comment',
				formInputs: [
					{
						name: 'TextBlock',
						props: {
							value: this.deleteCommentModal.text
						}
					}
				],
				formSubmits: [
					{
						name: 'Button',
						props: {
							label: 'Cancel',
							btnType: 'outline-secondary',
							onClick: () => {
								$(`#${this.modalDeleteProps.id}`).modal("hide");
							}
						}
					},
					{
						name: 'Button',
						props: {
							label: 'Remove',
							btnType: 'danger',
							waiting: this.waitButton,
							btnDOMType: 'submit'
						}
					}
				],
				onSubmit: async () => {
					await this.deleteComment(this.deleteCommentModal.id)
					$(`#${this.modalDeleteProps.id}`).modal("hide");
					toast.notify({ icon: "success", heading: "Comment was deleted." });
					this.GetCommentsByEntityId();
				}
			}
		},
		modalEditProps() {
			return {
				id: 'editCommentModal',
				label: this.replyToComment ? 'Reply to comment' : 'Edit comment',
				formInputs: [
					{
						name: 'Select',
						props: {
							id: 'entity-selectedUsers',
							label: 'Users',
							className: 'col-12',
							multiple: true,
							options: this.usersSelectList,
							value: this.assignedUsers
						}
					},
					{
						name: 'PrimitiveTextArea',
						props: {
							id: 'entity-message',
							label: 'Message',
							className: 'col-12',
							value: this.replyToComment ? this.replyModal.message : this.editModal.message,
						}
					}
				],
				formSubmits: [
					{
						name: 'Button',
						props: {
							label: 'Cancel',
							btnType: 'outline-secondary',
							onClick: () => {
								$(`#${this.modalEditProps.id}`).modal("hide");
							}
						}
					},
					{
						name: 'Button',
						props: {
							label: 'Save',
							btnType: 'success',
							disabled: this.disableModalPostButton,
							waiting: this.waitButton,
							btnDOMType: 'submit'
						}
					}
				],
				onSubmit: () => {
					if (this.replyToComment) {
						$(`#${this.modalEditProps.id}`).modal("hide");
						this.CreateReply();
						this.replyToComment = false;
					}
					else {
						this.updateEntity().then(() => {
							$(`#${this.modalEditProps.id}`).modal("hide");
							this.GetCommentsByEntityId();
						});
					}
				}
			}
		},
		assignesUsersIdsValue() {
			return this.model.assignedUsersIds;
		}
	},
	created() {
		this.resetComment();
		this.resetDeleteModal({ author: '', commentId: '' });
	},
	methods: {
		setTaggedUsers(value) {
			this.model.assignedUsersIds = [...value.value];
		},
		async makeInitialRequests() {
			if (!this.dataLoaded) {
				this.$store.dispatch('getCurrenctUserInfo');
				const result = await customAjaxRequest(this.apiUrl, 'GET', { [this.entityField]: this.entityId });
				this.comments = result;
				this.dataLoaded = true;
			}
		},
		resetComment() {
			this.model = {
				message: '',
				[this.entityField]: this.entityId,
				assignedUsersIds: [],
			},
			$("#commentsUserList").selectpicker("val", "");
			this.disableMainPostButton = true;
			this.disableModalPostButton = true;
		},
		resetDeleteModal({ author, commentId }) {
			this.deleteCommentModal = {
				text: `Delete ${author} comment?`,
				id: commentId
			}
			$(`#${this.modalDeleteProps.id}`).modal("show");
		},
		async resetEditModal(commentId) {
			this.disableModalPostButton = true;
			const result = await customAjaxRequest(apiEndpoints.Comments.GetCommentByIdAsync, 'GET', { id: commentId });
			this.editModal = result;
			this.assignedUsers = [];
			result.assignedUsers.forEach(user => {
				this.assignedUsers.push(user.userId);
			});
			this.modalKey++;
			$(`#${this.modalEditProps.id}`).modal("show");
		},
		resetReplyModal({ author, commentId }) {
			this.replyModal = {
				commentId: commentId,
				message: '',
				[this.entityField]: this.entityId,
				assignedUsersIds: [],
			}
			this.assignedUsers = [];
			this.modalKey++;
			let index = this.usersSelectList.findIndex(user => user.label == author);
			if (index != -1) {
				this.replyModal.assignedUsersIds.push(this.usersSelectList[index].value);
			}
			this.disableModalPostButton = true;
			this.replyToComment = true;
			$(`#${this.modalEditProps.id}`).modal("show");
		},
		async CreateReply() {
			this.disableMainPostButton = true;
			await customAjaxRequest(apiEndpoints.Comments.AddCommentAsync, 'PUT', this.replyModal)
			this.GetCommentsByEntityId();
		},
		async CreateComment() {
			this.disableMainPostButton = true;
			await customAjaxRequest(apiEndpoints.Comments.AddCommentAsync, 'PUT', this.model);
			if (this.dataLoaded) {
				this.GetCommentsByEntityId();
			}
			this.resetComment();
		},
		async GetCommentsByEntityId() {
			const result = await customAjaxRequest(this.apiUrl, 'GET', { [this.entityField]: this.entityId });
			this.comments = result;
		},
		async updateEntity() {
			this.waitButton = true;
			let unassignedUsers = [];
			this.editModal.assignedUsers.forEach(user => {
				let index = this.assignedUsers.findIndex(aUser => aUser == user.userId);
				if (index == -1) {
					unassignedUsers.push(user.userId);
				} else {
					this.assignedUsers.splice(index, 1);
				}
			});

			const data = {
				assignedUsers: this.assignedUsers,
				unassignedUsers,
				comment: this.editModal
			}
			try {
				await customAjaxRequest(apiEndpoints.Comments.UpdateComment, 'POST', data);
			} catch (err) {
				toast.notifyErrorList(err);
			} finally {
				this.waitButton = false;
			}

		},
		async deleteComment(commentId) {
			this.waitButton = true;
			try {
				await customAjaxRequest(apiEndpoints.Comments.DeleteCommentAsync, 'DELETE', { id: commentId });
			} catch (err) {
				toast.notifyErrorList(err);
			} finally {
				this.waitButton = false;
			}
		},
		emitNewMessage(val) {
			this.disableMainPostButton = true;
			this.model.message = val.value;
			if (this.model.message.length > 0) {
				this.disableMainPostButton = false;
			}
		},
		emitModalValue(val) {
			this.disableModalPostButton = false;
			if (val.id == 'entity-selectedUsers') {
				if (this.replyToComment)
					this.replyModal.assignedUsersIds = val.value;
				else this.assignedUsers = val.value;
			}
			else {
				const newVal = val.value === 'true' ? true : val.value === 'false' ? false : val.value;
				if (this.replyToComment)
					this.replyModal[val.id.replace('entity-', '')] = newVal;
				else this.editModal[val.id.replace('entity-', '')] = newVal;
			}
		}
	}
});