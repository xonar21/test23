import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { webApi } from "~/api";
import { IElectionStatusPreview, IElectionStatusUpdate } from "~/models";
import { apiCall } from "~/store";

const name = "ELECTION_STATUS";

export const electionStatusSliceInitialState: IElectionStatusPreview = {
  data: [],
};

enum ActionType {
  ELECTION_STATUS_REQUEST_SUCCEEDED = "ELECTION_STATUS_REQUEST_SUCCEEDED",
}

const slice = createSlice({
  name,
  initialState: electionStatusSliceInitialState,
  reducers: {
    [ActionType.ELECTION_STATUS_REQUEST_SUCCEEDED]: (state, action: PayloadAction<IElectionStatusPreview>) => {
      state.data = action.payload.items as IElectionStatusPreview;
    },
  },
});

const { ELECTION_STATUS_REQUEST_SUCCEEDED } = slice.actions;

const getElectionStatus = (paramsPayload: any) => {
  return apiCall(webApi.ElectionStatus.getList)({
    args: [
      {
        PageNumber: paramsPayload.number,
        PageSize: paramsPayload.size,
        Filters: paramsPayload.filters,
        SortField: paramsPayload.sortField,
        SortOrder: paramsPayload.sortOrder,
      },
    ],
    success: ELECTION_STATUS_REQUEST_SUCCEEDED.type,
  });
};

const updateElectionStatus = (electionStatus: IElectionStatusUpdate) => {
  return apiCall(webApi.ElectionStatus.updateElectionStatus)({
    args: [electionStatus],
  });
};

export const ElectionStatusActions = {
  getElectionStatus,
  updateElectionStatus,
};

export default slice.reducer;
