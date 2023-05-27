import { RootState } from "~/store";

const getRoot = (state: RootState) => state.ENTITIES.NOTIFICATION_PROFILE;

export const NotificationProfileSelectors = {
  getRoot,
};
