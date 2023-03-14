import { RootState } from "~/store";

const getRoot = (state: RootState) => state.UI.DIALOG;

export const DialogSelectors = {
  getRoot,
};
