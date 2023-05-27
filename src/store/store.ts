import { configureStore, combineReducers, ThunkAction, Action } from "@reduxjs/toolkit";
import { createWrapper } from "next-redux-wrapper";
import {
  postListReducer,
  apiMiddleware,
  dialogReducer,
  selectListsReducer,
  tagsReducer,
  authReducer,
  userReducer,
  regionsReducer,
  regionTypesReducer,
  voterReducer,
  rolesReducer,
  permissionsReducer,
  usersReducer,
  electionFunctionReducer,
  subscriptionListStatusReducer,
  gendersReducer,
  subscriptionListReducer,
  electionReducer,
  workFlowsReducer,
  politicalPartiesReducer,
  electionTypesReducer,
  electionStatusReducer,
  auditReducer,
  notificationEventReducer,
  notificationProfileReducer,
  notificationsReducer,
} from "~/store";

export const reducer = {
  AUTH: authReducer,
  USER: userReducer,
  ENTITIES: combineReducers({
    POSTS: postListReducer,
    TAGS: tagsReducer,
    REGIONS: regionsReducer,
    REGION_TYPES: regionTypesReducer,
    VOTER_PROFILE: voterReducer,
    ROLES: rolesReducer,
    PERMISSIONS: permissionsReducer,
    USERS: usersReducer,
    ELECTION_FUNCTION: electionFunctionReducer,
    SUBSCRIPTION_LIST_STATUS: subscriptionListStatusReducer,
    GENDERS: gendersReducer,
    SUBSCRIPTION_LIST: subscriptionListReducer,
    ELECTIONS: electionReducer,
    WORK_FLOWS: workFlowsReducer,
    POLITICAL_PARTIES: politicalPartiesReducer,
    ELECTION_TYPES: electionTypesReducer,
    ELECTION_STATUS: electionStatusReducer,
    AUDIT: auditReducer,
    NOTIFICATION_EVENT: notificationEventReducer,
    NOTIFICATION_PROFILE: notificationProfileReducer,
    NOTIFICATIONS: notificationsReducer,
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
