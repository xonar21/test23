import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { webApi } from "~/api";
import { IUsersPreview } from "~/models";
import { apiCall } from "~/store";

const name = "USERS";

export const usersSliceInitialState: IUsersPreview = {
  data: {
    users: [],
    internalUsers: [],
    externalUsers: [],
  },
};

enum ActionType {
  USERS_REQUEST_SUCCEEDED = "USERS_REQUEST_SUCCEEDED",
  USERS_INTERNAL_REQUEST_SUCCEEDED = "USERS_INTERNAL_REQUEST_SUCCEEDED",
  USERS_EXTERNAL_REQUEST_SUCCEEDED = "USERS_EXTERNAL_REQUEST_SUCCEEDED",
}

const slice = createSlice({
  name,
  initialState: usersSliceInitialState,
  reducers: {
    [ActionType.USERS_REQUEST_SUCCEEDED]: (state, action: PayloadAction<IUsersPreview>) => {
      state.data.users = action.payload;
    },

    [ActionType.USERS_INTERNAL_REQUEST_SUCCEEDED]: (state, action: PayloadAction<IUsersPreview>) => {
      state.data.internalUsers = action.payload;
    },

    [ActionType.USERS_EXTERNAL_REQUEST_SUCCEEDED]: (state, action: PayloadAction<IUsersPreview>) => {
      state.data.externalUsers = action.payload;
    },
  },
});

const { USERS_INTERNAL_REQUEST_SUCCEEDED, USERS_EXTERNAL_REQUEST_SUCCEEDED, USERS_REQUEST_SUCCEEDED } = slice.actions;

const getUsers = (paramsPayload: any) => {
  return apiCall(webApi.Users.getList)({
    args: [
      {
        PageNumber: paramsPayload.number,
        PageSize: paramsPayload.size,
        Filters: paramsPayload.filters,
        SortField: paramsPayload.sortField,
        SortOrder: paramsPayload.sortOrder,
      },
    ],
    success: USERS_REQUEST_SUCCEEDED.type,
  });
};

const getInternalUsers = (paramsPayload: any) => {
  return apiCall(webApi.Users.getInternalUsers)({
    args: [
      {
        PageNumber: paramsPayload.number,
        PageSize: paramsPayload.size,
        Filters: paramsPayload.filters,
        SortField: paramsPayload.sortField,
        SortOrder: paramsPayload.sortOrder,
      },
    ],
    success: USERS_INTERNAL_REQUEST_SUCCEEDED.type,
  });
};

const getExternalUsers = (paramsPayload: any) => {
  return apiCall(webApi.Users.getExternalUsers)({
    args: [
      {
        PageNumber: paramsPayload.number,
        PageSize: paramsPayload.size,
        Filters: paramsPayload.filters,
        SortField: paramsPayload.sortField,
        SortOrder: paramsPayload.sortOrder,
      },
    ],
    success: USERS_EXTERNAL_REQUEST_SUCCEEDED.type,
  });
};

export const UsersActions = {
  ...slice.actions,
  getUsers,
  getInternalUsers,
  getExternalUsers,
};

export default slice.reducer;
