import { RootState } from "~/store";

const getRoot = (state: RootState) => state.ENTITIES.VOTER_PROFILE;

export const VoterSelectors = {
  getRoot,
};
