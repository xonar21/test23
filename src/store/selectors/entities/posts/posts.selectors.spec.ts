/* eslint-disable @typescript-eslint/no-explicit-any */
import { postsSliceInitialState, PostsSelectors } from "~/store";

describe("Posts selectors", () => {
  it("getRoot", () => {
    const root = PostsSelectors.getRoot({ ENTITIES: { POSTS: postsSliceInitialState } } as any);
    expect(root).toEqual(postsSliceInitialState);
  });
});
