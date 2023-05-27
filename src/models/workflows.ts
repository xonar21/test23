export interface IWorkFlowUpdate {
  entities: any[];
  workflowId?: string;
}

export interface IWorkFlowsPreview {
  data: any;
  items?: any;
  workflowTransitions?: any;
  workflowsStatus?: any;
  responsePayload?: any;
  optionalPayload?: any;
}
