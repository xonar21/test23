import { HYDRATE } from "next-redux-wrapper";
import { IAxiosErrorPayload } from "~/core";
import { IAuthModel, IAuthResponse } from "~/models";
import { AuthActions, authReducer, authSliceInitialState } from "~/store";

describe("Auth Slice", () => {
  const apiCallSpy = jest.spyOn(require("~/store/actions/api.actions"), "apiCall");

  it("should return the initial state", () => {
    expect(authReducer(authSliceInitialState, { type: HYDRATE, payload: { AUTH: authSliceInitialState } })).toEqual(
      authSliceInitialState,
    );
  });

  it("should handle AUTH_REQUEST_STARTED", () => {
    expect(authReducer(authSliceInitialState, { type: AuthActions.AUTH_REQUEST_STARTED })).toEqual({
      ...authSliceInitialState,
      loading: true,
    });
  });

  it("should handle AUTH_REQUEST_SUCCEEDED", () => {
    const payload: IAuthResponse = {
      token: "test",
    };
    expect(authReducer(authSliceInitialState, { type: AuthActions.AUTH_REQUEST_SUCCEEDED, payload })).toEqual({
      ...authSliceInitialState,
      isAuthenticated: true,
      token: payload.token,
    });
  });

  it("should handle AUTH_REQUEST_FAILED", () => {
    const payload: IAxiosErrorPayload = {
      message: "Test Error",
      status: 500,
      statusText: "Server Error",
    };
    expect(authReducer(authSliceInitialState, { type: AuthActions.AUTH_REQUEST_FAILED, payload })).toEqual({
      ...authSliceInitialState,
      error: payload,
    });
  });

  it("should handle AUTH_REQUEST_ENDED", () => {
    expect(authReducer({ ...authSliceInitialState, loading: true }, { type: AuthActions.AUTH_REQUEST_ENDED })).toEqual(
      authSliceInitialState,
    );
  });

  it("should handle AUTH_STATE_CHANGED", () => {
    const payload: IAuthResponse = {
      token: "test",
    };
    expect(authReducer(authSliceInitialState, { type: AuthActions.AUTH_STATE_CHANGED, payload })).toEqual({
      ...authSliceInitialState,
      isAuthenticated: true,
      token: payload.token,
    });
  });

  const model: IAuthModel = {
    email: "johndoe@domain.com",
    password: "Pa$$w0rd",
  };

  it("should login", () => {
    AuthActions.login(model);
    expect(apiCallSpy).toBeCalled();
  });

  it("should register", () => {
    AuthActions.register(model);
    expect(apiCallSpy).toBeCalled();
  });
});
