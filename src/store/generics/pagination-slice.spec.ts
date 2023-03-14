import { HYDRATE } from "next-redux-wrapper";
import { IAxiosErrorPayload } from "~/core";
import { IResponse } from "~/models";
import { createPaginationSlice, getInitialPaginationSliceState } from "~/store";

const initialState = getInitialPaginationSliceState<string>();
const { slice, paginationActions } = createPaginationSlice({
  name: "TEST",
  initialState,
  reducers: {},
  api: jest.fn(),
});
const reducer = slice.reducer;

describe("Generic Pagination Slice", () => {
  const apiCallSpy = jest.spyOn(require("~/store/actions/api.actions"), "apiCall");

  it("should return the initial state", () => {
    expect(reducer(initialState, { type: "TEST" })).toEqual(initialState);
  });

  it("should handle LIST_REQUEST_STARTED", () => {
    expect(reducer(initialState, { type: slice.actions.LIST_REQUEST_STARTED })).toEqual({
      ...initialState,
      loading: true,
    });
  });

  it("should handle LIST_REQUEST_SUCCEEDED", () => {
    const payload: IResponse<string> = {
      data: ["test"],
      limit: 10,
      page: 0,
      total: 1,
    };
    expect(reducer(initialState, { type: slice.actions.LIST_REQUEST_SUCCEEDED, payload })).toEqual({
      ...initialState,
      ...payload,
    });
  });

  it("should handle LIST_REQUEST_FAILED", () => {
    const payload: IAxiosErrorPayload = {
      message: "Test Error",
      status: 500,
      statusText: "Server Error",
    };
    expect(reducer(initialState, { type: slice.actions.LIST_REQUEST_FAILED, payload })).toEqual({
      ...initialState,
      error: payload,
    });
  });

  it("should handle LIST_REQUEST_ENDED", () => {
    expect(reducer({ ...initialState, loading: true }, { type: slice.actions.LIST_REQUEST_ENDED })).toEqual(
      initialState,
    );
  });

  it("should handle PAGE_SIZE_CHANGED", () => {
    const limit = 5;
    expect(reducer(initialState, { type: slice.actions.PAGE_SIZE_CHANGED, payload: limit })).toEqual({
      ...initialState,
      limit,
    });
  });

  it("should handle PAGE_CHANGED", () => {
    const page = 1;
    expect(reducer(initialState, { type: slice.actions.PAGE_CHANGED, payload: page })).toEqual({
      ...initialState,
      page,
    });
  });

  it("should handle SELECTED_ROWS_CHANGED", () => {
    const selectedRows = [0, 1];
    expect(reducer(initialState, { type: slice.actions.SELECTED_ROWS_CHANGED, payload: selectedRows })).toEqual({
      ...initialState,
      selectedRows,
    });
  });

  it("should handle HYDRATE", () => {
    expect(reducer(initialState, { type: HYDRATE, payload: { ENTITIES: { TEST: {} } } })).toEqual(initialState);
  });

  it("should get the list", () => {
    paginationActions.getList({ page: 0, limit: 10 });
    expect(apiCallSpy).toBeCalled();
  });
});
