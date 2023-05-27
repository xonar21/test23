import { RootState } from "~/store";

const getRoot = (state: RootState) => state.ENTITIES.WORK_FLOWS;

export const WorkFlowsSelectors = {
  getRoot,
};
