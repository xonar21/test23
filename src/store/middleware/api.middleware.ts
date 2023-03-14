import { Middleware, PayloadAction } from "@reduxjs/toolkit";
import { AxiosError } from "axios";
import { webApi } from "~/api";
import { IActionResult, IApiMiddlewarePayload, IAxiosErrorPayload, IExtendedApiPayload } from "~/core";
import { ERROR_MESSAGES, getValueByPath } from "~/shared";
import { apiCallInitAction, apiCallSuccessAction, apiCallFailureAction } from "~/store";

// eslint-disable-next-line @typescript-eslint/ban-types
const apiMiddleware: Middleware<{}, unknown> =
  ({ dispatch }) =>
  next =>
  async (action: PayloadAction<IApiMiddlewarePayload | string | unknown>) => {
    switch (action.type) {
      case apiCallInitAction.type: {
        const {
          promisePath,
          args,
          start,
          startPayload,
          success,
          successPayload,
          failure,
          failurePayload,
          end,
          endPayload,
        } = action.payload as IApiMiddlewarePayload;

        next(action);
        if (start) dispatch({ type: start, payload: startPayload });

        try {
          const promise: (...args: unknown[]) => Promise<unknown> = getValueByPath(webApi, promisePath);

          if (promise) {
            const response = await promise(...args);
            const successActionPayload = successPayload
              ? ({ responsePayload: response, optionalPayload: successPayload } as IExtendedApiPayload<
                  unknown,
                  unknown
                >)
              : response;

            if (success) dispatch({ type: success, payload: successActionPayload });
            else dispatch(apiCallSuccessAction(successActionPayload));

            return { succeeded: true, payload: successActionPayload } as IActionResult<unknown>;
          } else {
            throw new Error(ERROR_MESSAGES.API_MIDDLEWARE_PROMISE_NOT_FOUND);
          }
        } catch (error) {
          const axiosError = error as AxiosError;
          const errorPayload: IAxiosErrorPayload = {
            message: axiosError.message,
            status: axiosError.response?.status,
            statusText: axiosError.response?.statusText,
            data: axiosError.response?.data,
          };
          const errorActionPayload = failurePayload
            ? ({ responsePayload: errorPayload, optionalPayload: failurePayload } as IExtendedApiPayload<
                unknown,
                unknown
              >)
            : errorPayload;

          if (failure) dispatch({ type: failure, payload: errorActionPayload });
          else dispatch(apiCallFailureAction(errorActionPayload as IAxiosErrorPayload));

          return { succeeded: false, payload: errorActionPayload } as IActionResult<IAxiosErrorPayload>;
        } finally {
          if (end) dispatch({ type: end, payload: endPayload });
        }
      }
      case apiCallSuccessAction.type:
        // Default success behavior callback
        next(action);
        break;
      case apiCallFailureAction.type:
        // Default failure behavior callback
        next(action);
        break;
      default:
        return next(action);
    }
  };

export default apiMiddleware;
