import { AnyAction } from "@reduxjs/toolkit";

export interface IActionResult<T> extends AnyAction {
  succeeded: boolean;
  payload: T;
}
