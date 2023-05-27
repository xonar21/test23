import { RootState } from "~/store";

const getRoot = (state: RootState) => state.ENTITIES.NOTIFICATIONS;

export const NotificationsSelectors = {
  getRoot,
};
