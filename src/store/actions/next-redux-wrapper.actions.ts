import { createAction } from "@reduxjs/toolkit";
import { HYDRATE } from "next-redux-wrapper";
import { RootState } from "~/store";

export const hydrate = createAction<RootState>(HYDRATE);
