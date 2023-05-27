import { RootState } from "~/store";

const getRoot = (state: RootState) => state.ENTITIES.ELECTION_TYPES;

export const ElectionTypesSelectors = {
  getRoot,
};
