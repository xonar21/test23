import { RootState } from "~/store";

const getRoot = (state: RootState) => state.ENTITIES.ELECTIONS;

export const ElectionSelectors = {
  getRoot,
};
