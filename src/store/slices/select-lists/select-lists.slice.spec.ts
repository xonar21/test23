import { HYDRATE } from "next-redux-wrapper";
import { IAxiosErrorPayload, IExtendedApiPayload } from "~/core";
import { IResponse } from "~/models";
import { selectListsReducer, selectListsSliceInitialState, SelectListsActions } from "~/store";

describe("Select Lists Slice", () => {
  const apiCallSpy = jest.spyOn(require("~/store/actions/api.actions"), "apiCall");

  it("should return the initial state", () => {
    expect(
      selectListsReducer(selectListsSliceInitialState, {
        type: HYDRATE,
        payload: { SELECT_LISTS: selectListsSliceInitialState },
      }),
    ).toEqual(selectListsSliceInitialState);
  });

  it("should handle LIST_REQUEST_STARTED", () => {
    expect(
      selectListsReducer(selectListsSliceInitialState, {
        type: SelectListsActions.LIST_REQUEST_STARTED,
        payload: "users",
      }),
    ).toEqual({
      ...selectListsSliceInitialState,
      loading: { ...selectListsSliceInitialState.loading, users: true },
    });
  });

  it("should handle LIST_REQUEST_SUCCEEDED", () => {
    const payload: IExtendedApiPayload<IResponse<string>, string> = {
      responsePayload: { data: ["test"], limit: 10, page: 0, total: 1 },
      optionalPayload: "users",
    };
    expect(
      selectListsReducer(selectListsSliceInitialState, { type: SelectListsActions.LIST_REQUEST_SUCCEEDED, payload }),
    ).toEqual({
      ...selectListsSliceInitialState,
      users: payload.responsePayload.data,
    });
  });

  it("should handle LIST_REQUEST_FAILED", () => {
    const payload: IExtendedApiPayload<IAxiosErrorPayload, string> = {
      responsePayload: { message: "Test Error", status: 500, statusText: "Server Error" },
      optionalPayload: "users",
    };
    expect(
      selectListsReducer(selectListsSliceInitialState, { type: SelectListsActions.LIST_REQUEST_FAILED, payload }),
    ).toEqual({
      ...selectListsSliceInitialState,
      error: { ...selectListsSliceInitialState.error, users: payload.responsePayload },
    });
  });

  it("should handle LIST_REQUEST_ENDED", () => {
    expect(
      selectListsReducer(
        { ...selectListsSliceInitialState, loading: { ...selectListsSliceInitialState.loading, users: true } },
        { type: SelectListsActions.LIST_REQUEST_ENDED, payload: "users" },
      ),
    ).toEqual(selectListsSliceInitialState);
  });

  it("should get users", () => {
    SelectListsActions.getUsersSelectList();
    expect(apiCallSpy).toBeCalled();
  });
});
