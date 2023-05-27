import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { webApi } from "~/api";
import { IElectionImport, IElectionPreview } from "~/models";
import { apiCall } from "~/store";

const name = "ELECTION";

export const electionSliceInitialState: IElectionPreview = {
  data: [],
  elections: [],
};

enum ActionType {
  ELECTION_REQUEST_SUCCEEDED = "ELECTION_REQUEST_SUCCEEDED",
  ELECTIONS_REQUEST_SUCCEEDED = "ELECTIONS_REQUEST_SUCCEEDED",
}

const slice = createSlice({
  name,
  initialState: electionSliceInitialState,
  reducers: {
    [ActionType.ELECTION_REQUEST_SUCCEEDED]: (state, action: PayloadAction<IElectionPreview>) => {
      state.data = action.payload as IElectionPreview;
    },
    [ActionType.ELECTIONS_REQUEST_SUCCEEDED]: (state, action: PayloadAction<IElectionPreview>) => {
      state.elections = action.payload.items as IElectionPreview;
    },
  },
});

const { ELECTION_REQUEST_SUCCEEDED, ELECTIONS_REQUEST_SUCCEEDED } = slice.actions;

const getElections = (paramsPayload?: any) => {
  return apiCall(webApi.Elections.getList)({
    args: [
      paramsPayload
        ? {
            PageNumber: paramsPayload.number,
            PageSize: paramsPayload.size,
            Filters: paramsPayload.filters,
            SortField: paramsPayload.sortField,
            SortOrder: paramsPayload.sortOrder,
          }
        : {},
    ],
    success: ELECTIONS_REQUEST_SUCCEEDED.type,
  });
};

const getElectionsFromSaise = () => {
  return apiCall(webApi.Elections.getListFromSaise)({
    args: [{}],
    success: ELECTION_REQUEST_SUCCEEDED.type,
  });
};

const importElection = (election: IElectionImport) => {
  return apiCall(webApi.Elections.importElection)({
    args: [election],
  });
};

const editElection = (election: IElectionImport) => {
  return apiCall(webApi.Elections.updateElection)({
    args: [election],
  });
};

export const ElectionActions = {
  getElectionsFromSaise,
  importElection,
  getElections,
  editElection,
};

export default slice.reducer;
