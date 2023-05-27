import { RootState } from "~/store";

const getRoot = (state: RootState) => state.ENTITIES.ELECTION_STATUS;

export const electionStatusSelectors = {
  getRoot,
};
