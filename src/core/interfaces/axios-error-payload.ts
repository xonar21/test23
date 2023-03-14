import { HttpStatusCode } from "~/api";

export interface IAxiosErrorPayload<T = unknown> {
  message: string;
  status?: HttpStatusCode;
  statusText?: string;
  data?: T;
}
