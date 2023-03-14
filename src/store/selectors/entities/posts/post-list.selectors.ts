import { createPaginationSliceSelectors } from "~/store";

/* istanbul ignore next */
export const PostListSelectors = createPaginationSliceSelectors(state => state.ENTITIES.POSTS);
