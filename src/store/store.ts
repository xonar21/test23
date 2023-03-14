import { configureStore, combineReducers, ThunkAction, Action } from "@reduxjs/toolkit";
import { createWrapper } from "next-redux-wrapper";
import { postListReducer, apiMiddleware, dialogReducer, selectListsReducer, tagsReducer, authReducer } from "~/store";

export const reducer = {
  AUTH: authReducer,
  ENTITIES: combineReducers({
    POSTS: postListReducer,
    TAGS: tagsReducer,
  }),
  SELECT_LISTS: selectListsReducer,
  UI: combineReducers({
    DIALOG: dialogReducer,
  }),
};

export const store = configureStore({
  reducer,
  middleware: getDefaultMiddleware => getDefaultMiddleware().concat(apiMiddleware),
});

/* istanbul ignore next */
const makeStore = () => store;

export type AppStore = ReturnType<typeof makeStore>;
export type RootState = ReturnType<AppStore["getState"]>;
export type AppThunk<ReturnType = void> = ThunkAction<ReturnType, RootState, unknown, Action<string>>;

export const reduxWrapper = createWrapper<AppStore>(makeStore);
