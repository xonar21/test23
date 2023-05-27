import { createSelector } from "@reduxjs/toolkit";
import { RootState } from "~/store";

const getRoot = (state: RootState) => state.SELECT_LISTS;

const getLoading = createSelector([getRoot], state => state.loading);

const getError = createSelector([getRoot], state => state.error);

export const SelectListsSelectors = {
  getRoot,
  getLoading,
  getError,
};
