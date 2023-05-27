import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { webApi } from "~/api";
import { Nullable } from "~/core";
import { IUserPreview } from "~/models";
import { apiCall } from "~/store";

const name = "USERS";

interface IUserSliceState {
  data: Nullable<IUserPreview>;
  submitting: boolean;
}

export const userSliceInitialState: IUserSliceState = {
  data: null,
  submitting: false,
};

enum ActionType {
  AUTH_REQUEST_SUCCEEDED = "AUTH_REQUEST_SUCCEEDED",
}

const slice = createSlice({
  name,
  initialState: userSliceInitialState,
  reducers: {
    [ActionType.AUTH_REQUEST_SUCCEEDED]: (state, action: PayloadAction<IUserPreview>) => {
      state.data = action.payload;
    },
  },
});

const { AUTH_REQUEST_SUCCEEDED } = slice.actions;

const actions = {
  success: AUTH_REQUEST_SUCCEEDED.type,
};

const getUserById = (id: string) => {
  return apiCall(webApi.Users.getUserById)({
    args: [id],
    ...actions,
  });
};

const getCurrentUser = () => {
  return apiCall(webApi.Auth.getCurrentUser)({
    args: [{}],
    ...actions,
  });
};

export const UserActions = {
  ...slice.actions,
  getUserById,
  getCurrentUser,
};

export default slice.reducer;
