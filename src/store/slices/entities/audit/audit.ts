import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { webApi } from "~/api";
import { IAuditPreview } from "~/models";
import { apiCall } from "~/store";

const name = "AUDIT";

export const auditSliceInitialState: IAuditPreview = {
  data: [],
};

enum ActionType {
  AUDIT_REQUEST_SUCCEEDED = "AUDIT_REQUEST_SUCCEEDED",
}

const slice = createSlice({
  name,
  initialState: auditSliceInitialState,
  reducers: {
    [ActionType.AUDIT_REQUEST_SUCCEEDED]: (state, action: PayloadAction<IAuditPreview>) => {
      state.data = action.payload as IAuditPreview;
    },
  },
});

const { AUDIT_REQUEST_SUCCEEDED } = slice.actions;

const getAudit = (paramsPayload?: any) => {
  return apiCall(webApi.Audit.getList)({
    args: [
      paramsPayload
        ? {
            Skip: paramsPayload.number,
            Take: paramsPayload.size,
            UserId: paramsPayload.filters ? paramsPayload.filters : "",
          }
        : {},
    ],
    success: AUDIT_REQUEST_SUCCEEDED.type,
  });
};

export const AuditActions = {
  getAudit,
};

export default slice.reducer;
