import { WebApi } from "~/api";
import { Paths } from "~/core";

export interface IApiMiddlewarePayload extends IApiPayload<unknown[]> {
  promisePath: Paths<WebApi>;
}

export interface IApiPayload<TArgs> {
  /** WebApi promise function arguments */
  args: TArgs;

  /** The action type that will be dispatched before the API call */
  start?: string;

  /** Optional payload for the start action type */
  startPayload?: unknown;

  /** The action type that will be dispatched if the API call succeeded */
  success?: string;

  /** Optional payload for the success action type */
  successPayload?: unknown;

  /** The action type that will be dispatched if the API call failed */
  failure?: string;

  /** Optional payload for the failure action type */
  failurePayload?: unknown;

  /** The action type that will be dispatched after the API call, regardless of the response status */
  end?: string;

  /** Optional payload for the end action type */
  endPayload?: unknown;
}

export interface IExtendedApiPayload<TResponsePayload, TOptionalPayload> {
  responsePayload: TResponsePayload;
  optionalPayload: TOptionalPayload;
}
