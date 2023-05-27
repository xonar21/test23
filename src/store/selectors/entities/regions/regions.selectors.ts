import { RootState } from "~/store";

const getRoot = (state: RootState) => state.ENTITIES.REGIONS;

export const RegionsSelectors = {
  getRoot,
};
