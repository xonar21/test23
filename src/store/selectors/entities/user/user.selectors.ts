import { createSelector } from "@reduxjs/toolkit";
import { RootState } from "~/store";

const getRoot = (state: RootState) => state.USER;

const getClaims = createSelector([getRoot], state => state.data?.claims || []);

export const UserSelectors = {
  getRoot,
  getClaims,
};
