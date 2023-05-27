import { RootState } from "~/store";

const getRoot = (state: RootState) => state.ENTITIES.NOTIFICATION_EVENT;

export const NotificationEventSelectors = {
  getRoot,
};
