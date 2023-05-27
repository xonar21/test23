import { RootState } from "~/store";

const getRoot = (state: RootState) => state.ENTITIES.USERS;

export const UsersSelectors = {
  getRoot,
};
