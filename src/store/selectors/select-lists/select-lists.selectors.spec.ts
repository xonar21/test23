/* eslint-disable @typescript-eslint/no-explicit-any */
import { SelectListsSelectors, selectListsSliceInitialState } from "~/store";

describe("Select Lists selectors", () => {
  const { getRoot, getLoading, getError, getUsersSelectList } = SelectListsSelectors;

  const initialState = { SELECT_LISTS: selectListsSliceInitialState } as any;

  it("getRoot", () => {
    const root = getRoot(initialState);
    expect(root).toEqual(selectListsSliceInitialState);
  });

  it("getLoading", () => {
    const loading = getLoading(initialState);
    expect(loading).toEqual(selectListsSliceInitialState.loading);
  });

  it("getError", () => {
    const error = getError(initialState);
    expect(error).toEqual(selectListsSliceInitialState.error);
  });

  it("getUsersSelectList", () => {
    const users = getUsersSelectList(initialState);
    expect(users).toEqual(selectListsSliceInitialState.users);
  });
});
