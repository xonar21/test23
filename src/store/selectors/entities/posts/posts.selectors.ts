import { RootState } from "~/store";

const getRoot = (state: RootState) => state.ENTITIES.POSTS;

export const PostsSelectors = {
  getRoot,
};
