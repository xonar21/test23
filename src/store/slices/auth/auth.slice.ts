import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { webApi } from "~/api";
import { IAxiosErrorPayload, Nullable, NullablePartial } from "~/core";
import { IAuthError, IAuthModel, IAuthResponse } from "~/models";
import { apiCall } from "~/store";

const name = "AUTH";

interface IAuthSliceState {
  loading: boolean;
  isAuthenticated: boolean;
  token: Nullable<string>;
  error: Nullable<IAxiosErrorPayload<IAuthError>>;
}

export const authSliceInitialState: IAuthSliceState = {
  loading: false,
  isAuthenticated: false,
  token: null,
  error: null,
};

enum ActionType {
  AUTH_REQUEST_STARTED = "AUTH_REQUEST_STARTED",
  AUTH_REQUEST_SUCCEEDED = "AUTH_REQUEST_SUCCEEDED",
  AUTH_REQUEST_FAILED = "AUTH_REQUEST_FAILED",
  AUTH_REQUEST_ENDED = "AUTH_REQUEST_ENDED",
  AUTH_STATE_CHANGED = "AUTH_STATE_CHANGED",
}

const slice = createSlice({
  name,
  initialState: authSliceInitialState,
  reducers: {
    [ActionType.AUTH_REQUEST_STARTED]: state => {
      state.loading = true;
    },
    [ActionType.AUTH_REQUEST_SUCCEEDED]: (state, action: PayloadAction<IAuthResponse>) => {
      state.token = action.payload.token;
      state.isAuthenticated = true;
      state.error = null;
    },
    [ActionType.AUTH_REQUEST_FAILED]: (state, action: PayloadAction<IAxiosErrorPayload<IAuthError>>) => {
      state.error = action.payload;
    },
    [ActionType.AUTH_REQUEST_ENDED]: state => {
      state.loading = false;
    },
    [ActionType.AUTH_STATE_CHANGED]: (state, action: PayloadAction<NullablePartial<IAuthResponse>>) => {
      const { token } = action.payload;
      state.token = token;
      state.isAuthenticated = !!token;
    },
  },
});

const { AUTH_REQUEST_STARTED, AUTH_REQUEST_SUCCEEDED, AUTH_REQUEST_FAILED, AUTH_REQUEST_ENDED } = slice.actions;

const actions = {
  start: AUTH_REQUEST_STARTED.type,
  success: AUTH_REQUEST_SUCCEEDED.type,
  failure: AUTH_REQUEST_FAILED.type,
  end: AUTH_REQUEST_ENDED.type,
};

const login = (model: IAuthModel) => {
  return apiCall(webApi.Auth.login)({
    args: [model],
    ...actions,
  });
};

const register = (model: IAuthModel) => {
  return apiCall(webApi.Auth.register)({
    args: [model],
    ...actions,
  });
};

export const AuthActions = {
  ...slice.actions,
  login,
  register,
};

export default slice.reducer;
