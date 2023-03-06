Vue.component('CommentCreate', {
	template: `<div class="bg-white border rounded py-15 px-20 d-flex flex-column mb-2 pt-4">
		<Select :options="usersListSelect"
				:multiple="true"
				:searchable="true"
				label="Users"
				id="commentsUserList"
				:value="userListValue"
				@newValue="(value) => $emit('setTaggedUsers', value)" />
		<PrimitiveTextArea id="CommentMesasage"
							:value="message"
							placeholder="Leave your comment here"
							textareaClassName="form-control_small"
							:hasLabel="true"
							formGroupClassName="mb-0"
							:isResizableOnType="true"
							initMinHeight="42"
							@newValue="val => $emit('emitNewMessage', val)" />
		<button type="button"
				class="btn btn-primary align-self-end mt-2"
				@click="$emit('createComment')"
				v-if="!disableMainPostButton">
			Add
		</button>
	</div>`,
	props: {
		message: String,
		disableMainPostButton: Boolean,
		usersListSelect: Array,
		userListValue: Array,
	}
});