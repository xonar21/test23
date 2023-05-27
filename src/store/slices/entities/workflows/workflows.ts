import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { webApi } from "~/api";
import { IWorkFlowsPreview, IWorkFlowUpdate } from "~/models";
import { apiCall } from "~/store";

const name = "WORK_FLOWS";

export const workFlowsSliceInitialState: IWorkFlowsPreview = {
  data: [],
  workflowsStatus: {},
  workflowTransitions: {},
};

enum ActionType {
  WORK_FLOWS_REQUEST_SUCCEEDED = "WORK_FLOWS_REQUEST_SUCCEEDED",
  STATUS_SUBSCRIPTON_LIST_REQUEST_SUCCEEDED = "STATUS_SUBSCRIPTON_LIST_REQUEST_SUCCEEDED",
  STATUS_WORK_FLOWS_REQUEST_SUCCEEDED = "STATUS_WORK_FLOWS_REQUEST_SUCCEEDED",
  WORK_FLOWS_TRANSITIONS_REQUEST_SUCCEEDED = "WORK_FLOWS_TRANSITIONS_REQUEST_SUCCEEDED",
}

const slice = createSlice({
  name,
  initialState: workFlowsSliceInitialState,
  reducers: {
    [ActionType.WORK_FLOWS_REQUEST_SUCCEEDED]: (state, action: PayloadAction<IWorkFlowsPreview>) => {
      state.data = action.payload as IWorkFlowsPreview;
    },
    [ActionType.STATUS_WORK_FLOWS_REQUEST_SUCCEEDED]: (state, action: PayloadAction<IWorkFlowsPreview>) => {
      const { responsePayload, optionalPayload } = action.payload;
      state.workflowsStatus[optionalPayload] = responsePayload;
    },
    [ActionType.WORK_FLOWS_TRANSITIONS_REQUEST_SUCCEEDED]: (state, action: PayloadAction<IWorkFlowsPreview>) => {
      state.workflowTransitions = action.payload as IWorkFlowsPreview;
    },
  },
});

const { WORK_FLOWS_REQUEST_SUCCEEDED, WORK_FLOWS_TRANSITIONS_REQUEST_SUCCEEDED, STATUS_WORK_FLOWS_REQUEST_SUCCEEDED } =
  slice.actions;

const getWorkflows = () => {
  return apiCall(webApi.WorkFlows.getWorkFlows)({
    args: [{ IncludeStates: true, IncludeEntities: true, IncludeTransitions: true }],
    success: WORK_FLOWS_REQUEST_SUCCEEDED.type,
  });
};

const getWorkflowStatus = (params?: any) => {
  return apiCall(webApi.WorkFlows.getWorkFlowsStatus)({
    args: [params.id],
    success: STATUS_WORK_FLOWS_REQUEST_SUCCEEDED.type,
    successPayload: params.entityType,
  });
};

const getWorkFlowStateTransitions = (workFlowId: string, stateId: string) => {
  return apiCall(webApi.WorkFlows.getWorkFlowStateTransitions)({
    args: [{ workFlowId: workFlowId, stateId: stateId }],
  });
};

const getWorkFlowTransitions = (paramsPayload?: any) => {
  return apiCall(webApi.WorkFlows.getWorkFlowTransitions)({
    args: [{ workFlowId: paramsPayload.workFlowId }],
    success: WORK_FLOWS_TRANSITIONS_REQUEST_SUCCEEDED.type,
  });
};

const updateWorkflow = (workFlow: IWorkFlowUpdate) => {
  return apiCall(webApi.WorkFlows.updateWorkFlow)({
    args: [workFlow],
  });
};

const updateWorkflowTransitions = (workFlow: IWorkFlowUpdate) => {
  return apiCall(webApi.WorkFlows.updateWorkFlowTransitions)({
    args: [workFlow],
  });
};

const updateWorkflowStatus = (workFlow: IWorkFlowUpdate) => {
  return apiCall(webApi.WorkFlows.updateWorkFlowStatus)({
    args: [workFlow],
  });
};

export const WorkFlowsActions = {
  getWorkflows,
  getWorkFlowStateTransitions,
  updateWorkflow,
  getWorkFlowTransitions,
  updateWorkflowTransitions,
  updateWorkflowStatus,
  getWorkflowStatus,
};

export default slice.reducer;
