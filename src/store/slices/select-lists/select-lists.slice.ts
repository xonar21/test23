/* eslint-disable @typescript-eslint/no-explicit-any */
import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { webApi } from "~/api";
import { IAxiosErrorPayload, IExtendedApiPayload, Nullable } from "~/core";
import { IResponse, IUserPreview } from "~/models";
import { apiCall, hydrate } from "~/store/actions";

const name = "SELECT_LISTS";

interface ISelectLists {
  users: IUserPreview[];
}

const initialSelectLists: ISelectLists = {
  users: [],
};

type SelectListKey = keyof typeof initialSelectLists;

type SelectListsSliceState = ISelectLists & {
  loading: Record<SelectListKey, boolean>;
  error: Record<SelectListKey, Nullable<IAxiosErrorPayload>>;
};

export const selectListsSliceInitialState: SelectListsSliceState = {
  ...initialSelectLists,
  loading: {
    users: false,
  },
  error: {
    users: null,
  },
};

enum ActionType {
  LIST_REQUEST_STARTED = "LIST_REQUEST_STARTED",
  LIST_REQUEST_SUCCEEDED = "LIST_REQUEST_SUCCEEDED",
  LIST_REQUEST_FAILED = "LIST_REQUEST_FAILED",
  LIST_REQUEST_ENDED = "LIST_REQUEST_ENDED",
}

const slice = createSlice({
  name,
  initialState: selectListsSliceInitialState,
  reducers: {
    [ActionType.LIST_REQUEST_STARTED]: (state, action: PayloadAction<SelectListKey>) => {
      state.loading[action.payload] = true;
    },
    [ActionType.LIST_REQUEST_SUCCEEDED]: (
      state,
      action: PayloadAction<IExtendedApiPayload<IResponse<any>, SelectListKey>>,
    ) => {
      const { responsePayload, optionalPayload } = action.payload;
      state[optionalPayload] = responsePayload.data;
      state.error[optionalPayload] = null;
    },
    [ActionType.LIST_REQUEST_FAILED]: (
      state,
      action: PayloadAction<IExtendedApiPayload<IAxiosErrorPayload, SelectListKey>>,
    ) => {
      const { responsePayload, optionalPayload } = action.payload;
      state.error[optionalPayload] = responsePayload;
    },
    [ActionType.LIST_REQUEST_ENDED]: (state, action: PayloadAction<SelectListKey>) => {
      state.loading[action.payload] = false;
    },
  },
  extraReducers: builder => {
    builder.addCase(hydrate, (state, action) => {
      return {
        ...state,
        ...action.payload[name],
      };
    });
  },
});

const { LIST_REQUEST_STARTED, LIST_REQUEST_SUCCEEDED, LIST_REQUEST_FAILED, LIST_REQUEST_ENDED } = slice.actions;

const getSelectList = (api: (args: any) => Promise<unknown>, key: SelectListKey) => {
  return apiCall(api)({
    args: [{ page: 0, limit: 50 }],
    start: LIST_REQUEST_STARTED.type,
    startPayload: key,
    success: LIST_REQUEST_SUCCEEDED.type,
    successPayload: key,
    failure: LIST_REQUEST_FAILED.type,
    failurePayload: key,
    end: LIST_REQUEST_ENDED.type,
    endPayload: key,
  });
};

const getUsersSelectList = () => {
  return getSelectList(webApi.Users.getList, "users");
};

export const SelectListsActions = {
  ...slice.actions,
  getUsersSelectList,
};

export default slice.reducer;
