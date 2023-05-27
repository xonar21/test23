import { RootState } from "~/store";

const getRoot = (state: RootState) => state.ENTITIES.SUBSCRIPTION_LIST_STATUS;

export const SubscriptionListStatusSelectors = {
  getRoot,
};
