import { RootState } from "~/store";

const getRoot = (state: RootState) => state.ENTITIES.ELECTION_FUNCTION;

export const ElectionFunctionSelectors = {
  getRoot,
};
