import { RootState } from "~/store";

const getRoot = (state: RootState) => state.ENTITIES.POLITICAL_PARTIES;

export const PoliticalPartiesSelectors = {
  getRoot,
};
