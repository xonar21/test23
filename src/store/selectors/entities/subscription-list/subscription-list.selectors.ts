import { RootState } from "~/store";

const getRoot = (state: RootState) => state.ENTITIES.SUBSCRIPTION_LIST;

export const SubscriptionListSelectors = {
  getRoot,
};
