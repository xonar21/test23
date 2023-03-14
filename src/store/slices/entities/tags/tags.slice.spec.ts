import { HYDRATE } from "next-redux-wrapper";
import { IAxiosErrorPayload } from "~/core";
import { IResponse } from "~/models";
import { tagsReducer, tagsSliceInitialState, TagsActions } from "~/store";

describe("Tags Slice", () => {
  const apiCallSpy = jest.spyOn(require("~/store/actions/api.actions"), "apiCall");

  it("should return the initial state", () => {
    expect(
      tagsReducer(tagsSliceInitialState, { type: HYDRATE, payload: { ENTITIES: { TAGS: tagsSliceInitialState } } }),
    ).toEqual(tagsSliceInitialState);
  });

  it("should handle TAGS_REQUEST_STARTED", () => {
    expect(tagsReducer(tagsSliceInitialState, { type: TagsActions.TAGS_REQUEST_STARTED })).toEqual({
      ...tagsSliceInitialState,
      loading: true,
    });
  });

  it("should handle TAGS_REQUEST_SUCCEEDED", () => {
    const payload: IResponse<string> = {
      data: ["test"],
      limit: 10,
      page: 0,
      total: 1,
    };
    expect(tagsReducer(tagsSliceInitialState, { type: TagsActions.TAGS_REQUEST_SUCCEEDED, payload })).toEqual({
      ...tagsSliceInitialState,
      tags: payload.data,
    });
  });

  it("should handle TAGS_REQUEST_FAILED", () => {
    const payload: IAxiosErrorPayload = {
      message: "Test Error",
      status: 500,
      statusText: "Server Error",
    };
    expect(tagsReducer(tagsSliceInitialState, { type: TagsActions.TAGS_REQUEST_FAILED, payload })).toEqual({
      ...tagsSliceInitialState,
      error: payload,
    });
  });

  it("should handle TAGS_REQUEST_ENDED", () => {
    expect(tagsReducer({ ...tagsSliceInitialState, loading: true }, { type: TagsActions.TAGS_REQUEST_ENDED })).toEqual(
      tagsSliceInitialState,
    );
  });

  it("should get the tags", () => {
    TagsActions.getTags();
    expect(apiCallSpy).toBeCalled();
  });
});
