import { RootState } from "~/store";

const getRoot = (state: RootState) => state.ENTITIES.GENDERS;

export const GendersSelectors = {
  getRoot,
};
