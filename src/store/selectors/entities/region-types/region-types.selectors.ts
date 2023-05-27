import { RootState } from "~/store";

const getRoot = (state: RootState) => state.ENTITIES.REGION_TYPES;

export const RegionTypesSelectors = {
  getRoot,
};
