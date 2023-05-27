import { RootState } from "~/store";

const getRoot = (state: RootState) => state.ENTITIES.PERMISSIONS;

export const PermissionsSelectors = {
  getRoot,
};
