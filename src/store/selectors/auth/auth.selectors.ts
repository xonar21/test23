import { RootState } from "~/store";

const getRoot = (state: RootState) => state.AUTH;

export const AuthSelectors = {
  getRoot,
};
