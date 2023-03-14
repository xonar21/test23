/* eslint-disable @typescript-eslint/no-explicit-any */
import { createAction } from "@reduxjs/toolkit";
import { getPromisePath } from "~/api";
import { IActionResult, IApiMiddlewarePayload, IApiPayload, IAxiosErrorPayload } from "~/core";

enum ActionType {
  API_CALL_INIT = "API/API_CALL_INIT",
  API_CALL_SUCCESS = "API/API_CALL_SUCCESS",
  API_CALL_FAILURE = "API/API_CALL_FAILURE",
}

export const apiCallInitAction = createAction<IApiMiddlewarePayload>(ActionType.API_CALL_INIT);
export const apiCallSuccessAction = createAction<unknown>(ActionType.API_CALL_SUCCESS);
export const apiCallFailureAction = createAction<IAxiosErrorPayload>(ActionType.API_CALL_FAILURE);

/**
 * Creates a Redux action that handles an API call
 * @param api WebApi promise function selector
 */
export const apiCall =
  <TArgs, TResponse>(api: (args: TArgs) => Promise<TResponse>) =>
  (payload: IApiPayload<Parameters<typeof api>>): IActionResult<TResponse | IAxiosErrorPayload> =>
    apiCallInitAction({
      promisePath: getPromisePath(api),
      ...payload,
    }) as any;
