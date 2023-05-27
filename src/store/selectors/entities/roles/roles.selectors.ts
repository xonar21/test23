import { RootState } from "~/store";

const getRoot = (state: RootState) => state.ENTITIES.ROLES;

export const RolesSelectors = {
  getRoot,
};
