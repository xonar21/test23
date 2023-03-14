import { PayloadAction } from "@reduxjs/toolkit";
import { webApi } from "~/api";
import { Nullable } from "~/core";
import { IPost, IPostCreate, IPostPreview } from "~/models";
import { apiCall, createPaginationSlice, IPaginationSliceState } from "~/store";

const name = "POSTS";

interface IPostsSliceState extends IPaginationSliceState<IPostPreview> {
  form: Nullable<IPostPreview>;
  submitting: boolean;
}

export const postsSliceInitialState: IPostsSliceState = {
  data: [],
  page: 0,
  limit: 0,
  total: 0,
  loading: false,
  error: null,
  selectedRows: [],

  form: null,
  submitting: false,
};

enum ActionType {
  FORM_INITIALIZED = "FORM_INITIALIZED",
  FORM_RESET = "FORM_RESET",
  POST_SUBMIT_STARTED = "POST_SUBMIT_STARTED",
  POST_SUBMIT_ENDED = "POST_SUBMIT_ENDED",
}

const { slice, paginationActions } = createPaginationSlice({
  name,
  initialState: postsSliceInitialState,
  reducers: {
    [ActionType.FORM_INITIALIZED]: (state, action: PayloadAction<IPostPreview | undefined>) => {
      state.form = action.payload || {
        id: "",
        image: "",
        likes: 0,
        owner: null,
        publishDate: "",
        tags: [],
        text: "",
      };
    },
    [ActionType.FORM_RESET]: state => {
      state.form = null;
    },
    [ActionType.POST_SUBMIT_STARTED]: state => {
      state.submitting = true;
    },
    [ActionType.POST_SUBMIT_ENDED]: state => {
      state.submitting = false;
    },
  },
  api: webApi.Posts.getList,
});

const { POST_SUBMIT_STARTED, POST_SUBMIT_ENDED } = slice.actions;

const createPost = (post: IPostCreate) => {
  return apiCall(webApi.Posts.createPost)({
    args: [post],
    start: POST_SUBMIT_STARTED.type,
    end: POST_SUBMIT_ENDED.type,
  });
};

const updatePost = (post: IPost) => {
  return apiCall(webApi.Posts.updatePost)({
    args: [post],
    start: POST_SUBMIT_STARTED.type,
    end: POST_SUBMIT_ENDED.type,
  });
};

const deletePost = (id: string) => {
  return apiCall(webApi.Posts.deletePost)({
    args: [id],
    start: POST_SUBMIT_STARTED.type,
    end: POST_SUBMIT_ENDED.type,
  });
};

export const PostsActions = {
  ...slice.actions,
  ...paginationActions,
  createPost,
  updatePost,
  deletePost,
};

export default slice.reducer;
