/* eslint-disable @typescript-eslint/no-explicit-any */
import { TagsSelectors, tagsSliceInitialState } from "~/store";

describe("Posts selectors", () => {
  it("getRoot", () => {
    const root = TagsSelectors.getRoot({ ENTITIES: { TAGS: tagsSliceInitialState } } as any);
    expect(root).toEqual(tagsSliceInitialState);
  });
});
