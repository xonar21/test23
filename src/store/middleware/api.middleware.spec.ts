/* eslint-disable @typescript-eslint/no-empty-function */
/* eslint-disable @typescript-eslint/no-explicit-any */
import { MiddlewareAPI } from "redux";
import { ERROR_MESSAGES } from "~/shared";
import { apiCall, apiCallFailureAction, apiCallSuccessAction, apiMiddleware } from "~/store";

describe("API Middleware", () => {
  const actionTypes = {
    start: "START",
    success: "SUCCESS",
    failure: "FAILURE",
    end: "END",
  };

  const errorPayload = {
    message: ERROR_MESSAGES.API_MIDDLEWARE_PROMISE_NOT_FOUND,
    status: undefined,
    statusText: undefined,
  };

  const mockedWebApi = {
    Namespace: { get: jest.fn() },
  };

  const mockedStore: MiddlewareAPI = { dispatch: jest.fn(), getState: jest.fn() };
  const mockedNext = jest.fn();
  const getValueByPathSpy = jest.spyOn(require("~/shared/utils/object-utils"), "getValueByPath");

  beforeEach(() => {
    jest.clearAllMocks();
  });

  it("dispatches default success action on passing no success action type", async () => {
    getValueByPathSpy.mockImplementationOnce(() => () => {});
    const action = apiCall(mockedWebApi.Namespace.get)({
      args: [null],
    });

    await apiMiddleware(mockedStore)(mockedNext)(action);

    expect(mockedStore.dispatch).toHaveBeenLastCalledWith({
      payload: undefined,
      type: apiCallSuccessAction.type,
    });
  });

  it("dispatches success action type", async () => {
    getValueByPathSpy.mockImplementationOnce(() => () => {});
    const action = apiCall(mockedWebApi.Namespace.get)({
      args: [null],
      start: actionTypes.start,
      success: actionTypes.success,
      successPayload: "test",
      end: actionTypes.end,
    });

    await apiMiddleware(mockedStore)(mockedNext)(action);

    expect(mockedStore.dispatch).toHaveBeenCalledTimes(3);
    expect(mockedStore.dispatch).toHaveBeenNthCalledWith(2, {
      payload: { requestPayload: undefined, optionalPayload: "test" },
      type: actionTypes.success,
    });
  });

  it("dispatches default failure action on passing no failure action type", async () => {
    getValueByPathSpy.mockImplementationOnce(() => undefined);
    const action = apiCall(mockedWebApi.Namespace.get)({
      args: [null],
    });

    await apiMiddleware(mockedStore)(mockedNext)(action);

    expect(apiMiddleware).toThrowError();
    expect(mockedStore.dispatch).toHaveBeenLastCalledWith({
      type: apiCallFailureAction.type,
      payload: errorPayload,
    });
  });

  it("dispatches failure action type", async () => {
    getValueByPathSpy.mockImplementationOnce(() => undefined);
    const action = apiCall(mockedWebApi.Namespace.get)({
      args: [null],
      start: actionTypes.start,
      failure: actionTypes.failure,
      failurePayload: "test",
      end: actionTypes.end,
    });

    await apiMiddleware(mockedStore)(mockedNext)(action);

    expect(apiMiddleware).toThrowError();
    expect(mockedStore.dispatch).toHaveBeenCalledTimes(3);
    expect(mockedStore.dispatch).toHaveBeenNthCalledWith(2, {
      type: actionTypes.failure,
      payload: { responsePayload: errorPayload, optionalPayload: "test" },
    });
  });

  it("dispatches apiCallSuccessAction", async () => {
    await apiMiddleware(mockedStore)(mockedNext)(apiCallSuccessAction);
    expect(mockedNext).toHaveBeenCalled();
  });

  it("dispatches apiCallFailureAction", async () => {
    await apiMiddleware(mockedStore)(mockedNext)(apiCallFailureAction);
    expect(mockedNext).toHaveBeenCalled();
  });

  it("covers default path", async () => {
    await apiMiddleware(mockedStore)(mockedNext)({ type: "TEST" });
    expect(mockedNext).toHaveBeenCalled();
  });
});
