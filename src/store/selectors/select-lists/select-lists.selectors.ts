import { createSelector } from "@reduxjs/toolkit";
import { RootState } from "~/store";

const getRoot = (state: RootState) => state.SELECT_LISTS;

const getLoading = createSelector([getRoot], state => state.loading);

const getError = createSelector([getRoot], state => state.error);

const getUsersSelectList = createSelector([getRoot], state => state.users);

export const SelectListsSelectors = {
  getRoot,
  getLoading,
  getError,
  getUsersSelectList,
};
