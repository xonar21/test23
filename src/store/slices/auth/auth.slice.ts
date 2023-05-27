import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { webApi } from "~/api";
import { IAxiosErrorPayload, Nullable, NullablePartial } from "~/core";
import { IAuthError, IAuthModel, IAuthResponse, IMpassModel, IRefreshModel, ISaiseModel } from "~/models";
import { apiCall } from "~/store";

const name = "AUTH";

interface IAuthSliceState {
  loading: boolean;
  isAuthenticated: boolean;
  token: Nullable<string>;
  refreshToken: Nullable<string>;
  SaiseToken: Nullable<string>;
  fullName: Nullable<string>;
  error: Nullable<IAxiosErrorPayload<IAuthError>>;
  timeToRefresh: number;
}

export const authSliceInitialState: IAuthSliceState = {
  loading: false,
  isAuthenticated: false,
  token: null,
  refreshToken: null,
  fullName: null,
  error: null,
  SaiseToken: null,
  timeToRefresh: 0,
};

enum ActionType {
  AUTH_REQUEST_STARTED = "AUTH_REQUEST_STARTED",
  AUTH_REQUEST_SUCCEEDED = "AUTH_REQUEST_SUCCEEDED",
  AUTH_REQUEST_FAILED = "AUTH_REQUEST_FAILED",
  AUTH_REQUEST_ENDED = "AUTH_REQUEST_ENDED",
  AUTH_STATE_CHANGED = "AUTH_STATE_CHANGED",
  TIME_REFRESH_SET = "TIME_REFRESH_SET",
}

const slice = createSlice({
  name,
  initialState: authSliceInitialState,
  reducers: {
    [ActionType.AUTH_REQUEST_STARTED]: state => {
      state.loading = true;
    },
    [ActionType.AUTH_REQUEST_SUCCEEDED]: (state, action: PayloadAction<IAuthResponse>) => {
      if (!action.payload) {
        return;
      }
      const { token, refreshToken } = action.payload;
      if (token && refreshToken) {
        state.token = token;
        state.refreshToken = refreshToken;
        state.isAuthenticated = true;
        state.error = null;
      }
    },
    [ActionType.AUTH_REQUEST_FAILED]: (state, action: PayloadAction<IAxiosErrorPayload<IAuthError>>) => {
      state.error = action.payload;
    },
    [ActionType.AUTH_REQUEST_ENDED]: state => {
      state.loading = false;
    },
    [ActionType.AUTH_STATE_CHANGED]: (state, action: PayloadAction<NullablePartial<IAuthResponse>>) => {
      const { token, SaiseToken, refreshToken } = action.payload;
      state.SaiseToken = SaiseToken as string;
      state.token = token;
      state.refreshToken = refreshToken;
      state.fullName = token ? "John Doe" : null;
      state.isAuthenticated = token ? true : false;
    },
    [ActionType.TIME_REFRESH_SET]: (state, action) => {
      const { interDate } = action.payload;
      state.timeToRefresh = interDate;
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

const refresh = (model: IRefreshModel) => {
  return apiCall(webApi.Auth.refreshToken)({
    args: [model],
    ...actions,
  });
};

const invalidate = (model: IRefreshModel) => {
  return apiCall(webApi.Auth.invalidToken)({
    args: [model],
    ...actions,
  });
};

const loginMpass = (model: IMpassModel) => {
  return apiCall(webApi.Auth.loginMpass)({
    args: [model],
    ...actions,
  });
};

const loginSaise = (model: ISaiseModel) => {
  return apiCall(webApi.Auth.loginSaise)({
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
  loginSaise,
  loginMpass,
  refresh,
  invalidate,
};

export default slice.reducer;
