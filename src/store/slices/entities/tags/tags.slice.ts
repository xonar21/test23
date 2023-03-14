import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { webApi } from "~/api";
import { IAxiosErrorPayload, Nullable } from "~/core";
import { IResponse } from "~/models";
import { apiCall, hydrate } from "~/store";

const name = "TAGS";

interface ITagsSliceState {
  loading: boolean;
  tags: string[];
  error: Nullable<IAxiosErrorPayload>;
}

export const tagsSliceInitialState: ITagsSliceState = {
  loading: false,
  tags: [],
  error: null,
};

enum ActionType {
  TAGS_REQUEST_STARTED = "TAGS_REQUEST_STARTED",
  TAGS_REQUEST_SUCCEEDED = "TAGS_REQUEST_SUCCEEDED",
  TAGS_REQUEST_FAILED = "TAGS_REQUEST_FAILED",
  TAGS_REQUEST_ENDED = "TAGS_REQUEST_ENDED",
}

const slice = createSlice({
  name,
  initialState: tagsSliceInitialState,
  reducers: {
    [ActionType.TAGS_REQUEST_STARTED]: state => {
      state.loading = true;
    },
    [ActionType.TAGS_REQUEST_SUCCEEDED]: (state, action: PayloadAction<IResponse<string>>) => {
      const trimmed = action.payload.data.filter(tag => !!tag).map(tag => tag.trim());
      state.tags = trimmed.filter((tag, index) => !!tag && trimmed.indexOf(tag) === index);
      state.error = null;
    },
    [ActionType.TAGS_REQUEST_FAILED]: (state, action: PayloadAction<IAxiosErrorPayload>) => {
      state.error = action.payload;
    },
    [ActionType.TAGS_REQUEST_ENDED]: state => {
      state.loading = false;
    },
  },
  extraReducers: builder => {
    builder.addCase(hydrate, (state, action) => {
      return {
        ...state,
        ...action.payload.ENTITIES[name],
      };
    });
  },
});

const { TAGS_REQUEST_STARTED, TAGS_REQUEST_SUCCEEDED, TAGS_REQUEST_FAILED, TAGS_REQUEST_ENDED } = slice.actions;

const getTags = () => {
  return apiCall(webApi.Tags.getList)({
    args: [null],
    start: TAGS_REQUEST_STARTED.type,
    success: TAGS_REQUEST_SUCCEEDED.type,
    failure: TAGS_REQUEST_FAILED.type,
    end: TAGS_REQUEST_ENDED.type,
  });
};

export const TagsActions = {
  ...slice.actions,
  getTags,
};

export default slice.reducer;
