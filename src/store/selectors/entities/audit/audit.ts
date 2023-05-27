import { RootState } from "~/store";

const getRoot = (state: RootState) => state.ENTITIES.AUDIT;

export const AuditSelectors = {
  getRoot,
};
