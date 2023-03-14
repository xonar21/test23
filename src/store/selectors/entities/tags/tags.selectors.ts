import { RootState } from "~/store";

const getRoot = (state: RootState) => state.ENTITIES.TAGS;

export const TagsSelectors = {
  getRoot,
};
