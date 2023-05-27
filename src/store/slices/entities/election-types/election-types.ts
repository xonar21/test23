import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { webApi } from "~/api";
import { IElectionTypesCreate, IElectionTypesPreview } from "~/models";
import { apiCall } from "~/store";

const name = "ELECTION_TYPES";

export const electionTypesSliceInitialState: IElectionTypesPreview = {
  data: [],
};

enum ActionType {
  ELECTION_TYPES_REQUEST_SUCCEEDED = "ELECTION_TYPES_REQUEST_SUCCEEDED",
}

const slice = createSlice({
  name,
  initialState: electionTypesSliceInitialState,
  reducers: {
    [ActionType.ELECTION_TYPES_REQUEST_SUCCEEDED]: (state, action: PayloadAction<IElectionTypesPreview>) => {
      state.data = action.payload.items as IElectionTypesPreview;
    },
  },
});

const { ELECTION_TYPES_REQUEST_SUCCEEDED } = slice.actions;

const getElectionTypes = (paramsPayload: any) => {
  return apiCall(webApi.ElectionTypes.getList)({
    args: [
      {
        PageNumber: paramsPayload.number,
        PageSize: paramsPayload.size,
        Filters: paramsPayload.filters,
        SortField: paramsPayload.sortField,
        SortOrder: paramsPayload.sortOrder,
      },
    ],
    success: ELECTION_TYPES_REQUEST_SUCCEEDED.type,
  });
};
const deleteElectionType = (id: string) => {
  return apiCall(webApi.ElectionTypes.deleteElectionType)({
    args: [id],
  });
};

const deactivateElectionType = (ElectionType: any) => {
  return apiCall(webApi.ElectionTypes.deactivateElectionType)({
    args: [ElectionType],
  });
};

const activateElectionType = (ElectionType: any) => {
  return apiCall(webApi.ElectionTypes.activateElectionType)({
    args: [ElectionType],
  });
};

const updateElectionType = (ElectionType: IElectionTypesCreate) => {
  return apiCall(webApi.ElectionTypes.updateElectionType)({
    args: [ElectionType],
  });
};

const createElectionType = (ElectionType: IElectionTypesCreate) => {
  return apiCall(webApi.ElectionTypes.createElectionType)({
    args: [ElectionType],
  });
};

export const ElectionTypesActions = {
  getElectionTypes,
  deleteElectionType,
  deactivateElectionType,
  activateElectionType,
  updateElectionType,
  createElectionType,
};

export default slice.reducer;
